using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text.Json;
using System.Threading.Channels;

using Client.Controller;

using Model;
using Model.Commands;
using Model.Communication.Snapshots;
using Model.RelativeGameState;

namespace Client.App;

public class Client
{
    private long Id { get; set; }
    private RelativeGameState StateModel { get; init; }
    private View.View View { get; init; }
    private Controller.Controller Controller {get; init;}
    private Channel<Command> CommandChannel { get; init; }
    private IPAddress ServerIp { get; set; }
    private int ServerPort { get; set; }

    public Client(IPAddress? serverIp = null, int serverPort = 7777)
    {
        Id = long.MaxValue;
        var mvc = new MvcSynchronization();
        StateModel = new RelativeGameState(mvc);
        View = new View.View(StateModel, mvc);
        Controller = new Controller.Controller();
        CommandChannel = Channel.CreateUnbounded<Command>();
        ServerIp = serverIp ?? IPAddress.Loopback;
        ServerPort = serverPort;
    }

    public async Task Run()
    {
        var io = new Task[2];
        try
        {
            using (var client = new TcpClient())
            {
                await client.ConnectAsync(ServerIp, ServerPort); 
                await using (var stream = client.GetStream())
                {
                    var idBytes = new byte[8];
                    await stream.ReadExactlyAsync(idBytes, 0, sizeof(long));
                    Id = BitConverter.ToInt64(idBytes, 0);
                    if (Id == -1)
                    {
                        StateModel.Sync.ShouldAllExit = true;
                        Console.WriteLine("sth wrong exiting");
                        return;
                    }
                    
                    io[0] = ReceiveSnapshots(client, stream);
                    io[1] = SendCommands(client, stream);
                    Task.WaitAll(io);
                    stream.Close();
                    client.Close();
                }
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }

    private async Task ReceiveSnapshots(TcpClient client, NetworkStream stream)
    {
        try
        {
            View.Prepare();
            while (!StateModel.Sync.ShouldAllExit)
            {
                var receivedSnapshot = await ReceiveAndDecompressSnapshot(stream);

                StateModel.Sync.GameMutex.WaitOne();
                StateModel.LastSyncMoment = receivedSnapshot.SyncMoment;
                StateModel.Player = receivedSnapshot.Player;
                StateModel.AppliedEffects = receivedSnapshot.AppliedEffects;
                StateModel.CurrentRelativeRoom = new RelativeRoomState(receivedSnapshot.CurrentRoomSnapshot);
                StateModel.CurrentRelativeLogs = new RelativeLogs(receivedSnapshot.Logs);

                View.Refresh();
                StateModel.Sync.GameMutex.ReleaseMutex();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            StateModel.Sync.ShouldAllExit = true;
            View.Cleanup();
        }
    }

    private async Task<GameSnapshot> ReceiveAndDecompressSnapshot(NetworkStream stream)
    {
        var msgCompressedLen = new byte[4];
        var msgPreCompressedLen = new byte[4];
        await stream.ReadExactlyAsync(msgCompressedLen, 0, sizeof(int));
        var compressedLen = BitConverter.ToInt32(msgCompressedLen, 0);
        await stream.ReadExactlyAsync(msgPreCompressedLen, 0, sizeof(int));
        var preCompressedLen = BitConverter.ToInt32(msgPreCompressedLen, 0);
        
        var compressedBuffer = new byte[compressedLen];
        await stream.ReadExactlyAsync(compressedBuffer, 0, compressedLen);

        using var msOut = new MemoryStream();
        byte[] decompressedBuffer;
        using (var ms = new MemoryStream(compressedBuffer, 0, compressedLen))
        {
            await using (var gzip = new GZipStream(ms, CompressionMode.Decompress))
            {
                gzip.CopyTo(msOut, compressedLen);
                decompressedBuffer = msOut.ToArray();
            }
        }

        var recGame = JsonSerializer.Deserialize<GameSnapshot>(decompressedBuffer);

        if (recGame == null) throw new Exception("Json deserialization error");
        return recGame;
    }

    private async Task SendCommands(TcpClient client, NetworkStream stream)
    {
        try
        {
            while (!StateModel.Sync.ShouldExitController)
            {
                var key = Console.ReadKey(true);
                StateModel.Sync.GameMutex.WaitOne();
                View.Refresh();
                StateModel.Sync.GameMutex.ReleaseMutex();

                if (Id == long.MaxValue) continue;
                var command = Controller.ParseInputIntoCommand(new InputUnit(Id, key));
                if (command == null) continue;

                await PrepareAndWriteCompressedCommand(stream, command, CompressionLevel.Optimal);

                if (key.Key == ConsoleKey.Escape)
                {
                    StateModel.Sync.ShouldExitController = true;
                }
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
        }
        finally
        {
            StateModel.Sync.ShouldAllExit = true;
        }
    }
    
    private async Task PrepareAndWriteCompressedCommand(NetworkStream stream, Command command, CompressionLevel compressionLevel)
    {
        var serializedJson = JsonSerializer.SerializeToUtf8Bytes(command);
        var preCompressedLength = serializedJson.Length;
        using var ms = new MemoryStream();
        await using(var gzip = new GZipStream(ms, compressionLevel))
        {
            await gzip.WriteAsync(serializedJson); 
        }
        var compressedBuffer = ms.ToArray();
        var compressedLength = compressedBuffer.Length;
        await stream.WriteAsync(BitConverter.GetBytes(compressedLength));
        await stream.WriteAsync(BitConverter.GetBytes(preCompressedLength));
        await stream.WriteAsync(compressedBuffer, 0, compressedLength);
    }
}