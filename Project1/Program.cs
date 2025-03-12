using Project_oob.Beings;
using Project_oob.Display;
using Project_oob.Game;
using Project_oob.Items;
using Project_oob.Map;
using Project_oob.Input;

namespace Project_oob;

class Program
{
    static async Task Main(string[] args)
    {
        var pl = new Player("Player 1");
        var wrld = new World();
        wrld.Maps.Add("Map 1", new Map.Map("Map 1", 1));
        var rm = new Room("Room 1", 40, 20);
        wrld.Maps.First().Value.Rooms[0] = rm;
        var gs = new SinglePlayerGameState(pl, wrld, 3);
        var cts = new CancellationTokenSource();
        var mtx = new Mutex();
        var testDisp = new ConsoleDisplay(80, 24, gs, rm, cts, mtx);
        var input = new KeyboardInput(gs, cts, mtx);
        input.TakeInput();
        await testDisp.Run();
    }
}