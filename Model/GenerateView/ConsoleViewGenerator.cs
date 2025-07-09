using Model.Communication.Snapshots;
using Model.Game.Beings;
using Model.Game.Items;
using Model.Game.Map;

namespace Model.GenerateView;

public class ConsoleViewGenerator : IViewGenerator
{
    public ConsolePixel NewlyCreated { get; private set; }
    //Player
    public void VisitPlayer(Player p)
    {
        NewlyCreated = new(AnsiConsoleColor.Magenta, '¶', p.WasAttacked ? AnsiConsoleColor.BgRed : null);
    }

    public void VisitPlayerSnapshot(PlayerSnapshot p)
    {
        NewlyCreated = new (AnsiConsoleColor.Magenta, p.Name[0], p.WasAttacked ? AnsiConsoleColor.BgRed : null);
    }
    
    //Enemies
    public void VisitOrc(Orc o)
    {
        NewlyCreated = new(AnsiConsoleColor.Green, 'O', o.WasAttacked ? AnsiConsoleColor.BgRed : null);
    }

    public void VisitGiant(Giant g)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightBlue, 'G', g.WasAttacked ? AnsiConsoleColor.BgRed : null);
    }

    //Items
    //minerals
    public void VisitSand(Sand s)
    {
        NewlyCreated = new(AnsiConsoleColor.Yellow, '*');
    }

    public void VisitWood(Wood w)
    {
        NewlyCreated = new(AnsiConsoleColor.Red, '@');
    }

    public void VisitWater(Water w)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightBlue, '?');
    }
    //monies
    public void VisitCoin(Coin c)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightYellow, '\u00a9');
    }

    public void VisitGold(Gold g)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightYellow, '$');
    }
    //elixirs
    public void VisitHealingElixir(HealingElixir e)
    {
        NewlyCreated = new(AnsiConsoleColor.Cyan, 'e');
    }

    public void VisitPowerElixir(PowerElixir e)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightRed, 'e');
    }

    public void VisitPoison(Poison e)
    {
        NewlyCreated = new(AnsiConsoleColor.Green, 'e');
    }

    public void VisitAntidote(Antidote e)
    {
        NewlyCreated = new(AnsiConsoleColor.Yellow, 'e');
    }

    public void VisitHealthElixir(HealthElixir e)
    {
        NewlyCreated = new(AnsiConsoleColor.Cyan, 'e');
    }
    //weapons
    public void VisitSword(Sword s)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightMagenta, 's');
    }

    public void VisitKnife(Knife k)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightMagenta, 'k');
    }

    public void VisitBigSword(BigSword b)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightMagenta, 'b');
    }

    public void VisitShield(Shield s)
    {
        NewlyCreated = new(AnsiConsoleColor.BrightMagenta, 'd');
    }
    //map
    public void VisitBlankMapElement(BlankMapElement e)
    {
        NewlyCreated = new ConsolePixel();
    }

    public void VisitWall(Wall w)
    {
        NewlyCreated = new ConsolePixel(AnsiConsoleColor.Cyan, '\u2588');
    }
}