namespace RPG_ood.Beings;

public interface IEnemy : INpc
{
    public int Damage { get; set; }
    public void Attack();
}

public class Orc : IEnemy
{
    public Position Pos { get; set; }
    public string Name { get; init; }
    public int Color { get; init; }
    public int Hp { get; set; }

    public Orc()
    {
        Name = "Orc";
        Color = 33;
        Hp = 100;
    }
    
    public void TakeDamage(int damage)
    {
        throw new NotImplementedException();
    }

    public int Damage { get; set; }
    public void Attack()
    {
        throw new NotImplementedException();
    }

    public override string ToString()
    {
        return "O";
    }
}