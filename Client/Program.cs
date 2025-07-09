using System.Net;

namespace Client;

static class Program
{
    static async Task Main(string[] args)
    {
        try
        {
            var (ip, port) = ParseArgs(args);
            Console.WriteLine($"Connecting client to {ip}:{port}");
            var c = new App.Client(ip, port);
            await c.Run();
        }
        catch (Exception e)
        {
            Console.WriteLine("Usage: [ip] [port] (default: localhost 5555)");
            Console.WriteLine(e);
        }
    }

    static (IPAddress, int) ParseArgs(string[] args)
    {
        if (args.Length != 2)
        {
            throw new ArgumentException("Invalid number of arguments");
        }
        var ip = IPAddress.Parse(args[0]);
        var port = int.Parse(args[1]);
        return (ip, port);
    }
}