namespace RPG_ood.App;

public class App
{
    public Server.Server MainGameModelServer { get; set; }
    public Client.Client Client { get; set; }
    
    
}
/*var cts = new CancellationTokenSource();
var mtx = new Mutex();
var input = new Input(mtx, cts); 
var gs = new GameState("Player One", mtx, cts, input);
var view = new View.Display.View(gs, mtx, cts);
input.TakeInput();
view.RunDisplay();
gs.RunGame();*/