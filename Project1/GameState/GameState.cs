using Project_oob.Beings;
using Project_oob.Items;

namespace Project_oob.GameState;

public abstract class GameState
{
    public abstract ICollection<Being> Beings { get; protected set; }
    public abstract ICollection<Item> Items { get; protected set; }
    
    
}

public class SinglePlayerGameState : GameState
{
    public override ICollection<Being> Beings { get; protected set; }
    public override ICollection<Item> Items { get; protected set; }

    public SinglePlayerGameState(Player player)
    {
        Beings = new List<Being>();
        Beings.Add(player);
        Items = new List<Item>();
    }
}