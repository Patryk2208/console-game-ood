using RPG_ood.Communication.Snapshots;
using RPG_ood.Map;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game.Beings;
using RPG_ood.Model.Game.Items;
using RPG_ood.Model.Items;

namespace RPG_ood.View;

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