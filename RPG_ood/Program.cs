using System.Collections.Concurrent;
using System.IO.Compression;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Text.Json;
using RPG_ood.App.Client;
using RPG_ood.App.Server;
using RPG_ood.Commands;
using RPG_ood.Communication.Snapshots;
using RPG_ood.Controller.Input;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Effects;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.Items;

namespace RPG_ood;

static class Program
{
    static async Task Main(string[] args)
    {
        //Snapshot serialization/deserialization
        /*var p = new Player("patryk");
        IUsable u = new Sword();
        p.Bd.BodyParts["LeftHand"].PutOn(u);
        var json = JsonSerializer.Serialize(p);
        //var cleanJson = JsonDocument.Parse(json).RootElement.GetRawText();
        var bytes = System.Text.Encoding.UTF8.GetBytes(json);
        Console.WriteLine(json);
        /*using (var file = new FileStream("./snapshot.json", FileMode.Truncate, FileAccess.Write))
        {
            using (var gzip = new GZipStream(file, CompressionMode.Compress))
            {
                gzip.Write(bytes);
            }
        }

        using (var file = new FileStream("./snapshot.json", FileMode.Open, FileAccess.Read))
        {
            using (var gzip = new GZipStream(file, CompressionMode.Decompress))
            {
                using (var memory = new MemoryStream())
                {
                    gzip.CopyTo(memory);
                    Console.WriteLine(Encoding.UTF8.GetString(memory.ToArray()));
                }
            }
        }#1#

        var deserializedGameSnapshot = JsonSerializer.Deserialize<Player>(json);
        Console.WriteLine(JsonSerializer.Serialize(deserializedGameSnapshot));
        Console.WriteLine("finished");
        //Console.WriteLine(json);*/

        //Command serialization/deserialization
        /*var k = Console.ReadKey();
        var c = new Command(k, 2234);
        var json = JsonSerializer.Serialize(c);
        Console.WriteLine(json);
        var command = JsonSerializer.Deserialize<Command>(json);
        Console.WriteLine("Done");*/

        //sockets
        /*var sc = new Task[2];

        sc[0] = Task.Run(() =>
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] is server");
            var listener = new Socket(SocketType.Stream, ProtocolType.Tcp);
            try
            {
                var endPoint = new IPEndPoint(IPAddress.Any, 5555);
                listener.Bind(endPoint);
                listener.Listen(10);
                var client = listener.Accept();
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Client {client.RemoteEndPoint} connected");

                var bufrec = new byte[1024];
                var rec = client.Receive(bufrec);
                string data = Encoding.UTF8.GetString(bufrec, 0, rec);
                Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Received: {data}");

                string resp = "Hello World! from server";
                byte[] bufsend = Encoding.UTF8.GetBytes(resp);
                rec = client.Send(bufsend);

                client.Shutdown(SocketShutdown.Both);
                client.Close();
            }
            catch (Exception e)
            {
                Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Exception: {e}");
            }
            finally
            {
                listener.Close();
            }
        });

        sc[1] = Task.Run(() =>
        {
            Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] is client");
            var client = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (true)
            {
                Thread.Sleep(300);
                try
                {
                    client.Connect(new IPEndPoint(IPAddress.Loopback, 5555));
                    Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] {client.RemoteEndPoint} connected from {client.LocalEndPoint}");
                    client.Send(Encoding.UTF8.GetBytes("Hello World! from client"));
                    var bufrec = new byte[1024];
                    client.Receive(bufrec);
                    string data = Encoding.UTF8.GetString(bufrec, 0, bufrec.Length);
                    Console.WriteLine($"[{Thread.CurrentThread.ManagedThreadId}] Received: {data}");
                    client.Shutdown(SocketShutdown.Both);
                    client.Close();
                    break;
                }
                catch (Exception e)
                {
                    Console.WriteLine($"{Thread.CurrentThread.ManagedThreadId} Exception: {e}");
                }
            }
        });

        Task.WaitAll(sc);*/

        //server-client test
        /*var option = Console.ReadKey();
        if (option.KeyChar == 's')
        {
            var server = new Server();
            await server.Run();
        }
        else if (option.KeyChar == 'c')
        {
            var client = new Client();
            await client.Run();
        }*/
        
        //command line parsing
        string server = null;
        string client = null;
        int port = 5555; // Default port

        for (int i = 0; i < args.Length; i++)
        {
            switch (args[i])
            {
                case "--server":
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                    {
                        if (int.TryParse(args[++i], out int serverPort))
                        {
                            port = serverPort;
                        }
                    }
                    server = $"0.0.0.0:{port}";
                    break;

                case "--client":
                    if (i + 1 < args.Length && !args[i + 1].StartsWith("--"))
                    {
                        client = args[++i];
                    }
                    else
                    {
                        client = $"localhost:{port}";
                    }
                    break;
            }
        }
        
        if (server != null)
        {
            Console.WriteLine($"Starting server on port {port}");
            var s = new Server(port);
            await s.Run();
        }
        else if (client != null)
        {
            var parts = client.Split(':');
            var ipAddress = parts[0];
            var clientPort = parts.Length > 1 ? int.Parse(parts[1]) : port;
            if (IPAddress.TryParse(ipAddress, out IPAddress? ip))
            {
                Console.WriteLine($"Connecting client to {ip}:{clientPort}");
                var c = new Client(ip, clientPort);
                await c.Run();
            }
        }
        else
        {
            Console.WriteLine("Usage:");
            Console.WriteLine("  Server mode: --server [port] (default: 5555)");
            Console.WriteLine("  Client mode: --client [ip:port] (default: localhost:5555)");
        }
    }
}

