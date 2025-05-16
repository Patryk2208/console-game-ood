using System.Buffers.Binary;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Reflection.Metadata.Ecma335;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using RPG_ood.Commands;
using RPG_ood.Communication.Snapshots;
using RPG_ood.Controller.Input;
using RPG_ood.Model.GameSnapshot;
using RPG_ood.View.Display;

namespace RPG_ood.App.Client;

public class Client
{
    private long Id { get; set; }
    private RelativeGameState StateModel { get; init; }
    private View.Display.View View { get; init; }
    private Channel<Command> CommandChannel { get; init; }
    //private Task[] MvcTasks { get; init; }
    private IPAddress ServerIp { get; set; }
    private int ServerPort { get; set; }

    public Client(IPAddress? serverIp = null, int serverPort = 5555)
    {
        Id = long.MaxValue;
        var mvc = new MvcSynchronization();
        StateModel = new RelativeGameState(mvc);
        View = new View.Display.View(StateModel, mvc);
        CommandChannel = Channel.CreateUnbounded<Command>();
        ServerIp = serverIp ?? IPAddress.Loopback;
        ServerPort = serverPort;
        //MvcTasks = new Task[1];
        //todo initialize handlers

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
                StateModel.CurrentRelativeRoom = new RelativeRoomState(receivedSnapshot.CurrentRoomSnapshot);
                /*if (StateModel.Player.IsDead)
                {
                    StateModel.Sync.ShouldExitController = true;
                }*/

                View.Refresh();
                StateModel.Sync.GameMutex.ReleaseMutex();
            }

        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            StateModel.Sync.ShouldAllExit = true;
        }
        finally
        {
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
                var command = new Command(key, Id);
                if (command == null) throw new Exception("Command error");
                
                await PrepareAndWriteCompressedCommand(stream, command, CompressionLevel.Optimal);

                if (command.KeyInfo.Key == ConsoleKey.Escape)
                {
                    break;
                }
            }

        }
        catch (Exception e)
        {
            StateModel.Sync.ShouldAllExit = true;
            Console.WriteLine(e);
        }
    }
    
    /*private byte[] PrepareMessage(Command command)
    {
        var json = JsonSerializer.Serialize(command);
        byte[] msg = new byte[4 + Encoding.UTF8.GetByteCount(json)];
        BinaryPrimitives.WriteInt32LittleEndian(msg.AsSpan(0, 4), json.Length);
        Encoding.UTF8.GetBytes(json, 0, json.Length, msg, 4);
        return msg;
    }*/
    
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