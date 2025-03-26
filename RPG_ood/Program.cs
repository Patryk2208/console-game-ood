using RPG_ood.Items;
using RPG_ood.Beings;
using RPG_ood.Display;
using RPG_ood.Game;
using RPG_ood.Input;
using RPG_ood.Map;

namespace RPG_ood;

class Program
{
    static async Task Main()
    {
        var cts = new CancellationTokenSource();
        var mtx = new Mutex();
        var gs = new SinglePlayerGameState("Player One", mtx, cts);
        var input = new Input.Input(gs, mtx, cts); 
        input.TakeInput();
        await gs.RunDisplay();
    }
}