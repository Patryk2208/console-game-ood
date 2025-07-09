using Model.Communication.Snapshots;
using Model.Game.Beings;
using Model.Game.Items;
using Model.Game.Map;

namespace Model.GenerateView;

public interface IViewGenerator
{
    public void VisitPlayer(Player p);
    public void VisitPlayerSnapshot(PlayerSnapshot p);
    public void VisitOrc(Orc o);
    public void VisitGiant(Giant g);
    public void VisitSand(Sand s);
    public void VisitWood(Wood w);
    public void VisitWater(Water w);
    public void VisitCoin(Coin c);
    public void VisitGold(Gold g);
    public void VisitHealingElixir(HealingElixir e);
    public void VisitPowerElixir(PowerElixir e);
    public void VisitPoison(Poison e);
    public void VisitAntidote(Antidote e);
    public void VisitHealthElixir(HealthElixir e);
    public void VisitSword(Sword s);
    public void VisitKnife(Knife k);
    public void VisitBigSword(BigSword b);
    public void VisitShield(Shield s);
    public void VisitBlankMapElement(BlankMapElement e);
    public void VisitWall(Wall w);

}