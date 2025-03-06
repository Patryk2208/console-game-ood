namespace Project_oob.Attributes;

public abstract class Attribute
{
    public string Name;
    protected int Value;
    protected static int MaxValue;

    public void SetValue(int value)
    {
        Value = value;
    }
}

public class Power : Attribute
{
    public new readonly string Name;
    public Power()
    {
        Name = "Power";
        MaxValue = 100;
    }
}

public class Agility : Attribute
{
    public new readonly string Name;
    public Agility()
    {
        Name = "Agility";
        MaxValue = 100;
    }
}

public class Health : Attribute
{
    public new readonly string Name;
    public Health()
    {
        Name = "Health";
        MaxValue = 100;
    }
}

public class Luck : Attribute
{
    public new readonly string Name;
    public Luck()
    {
        Name = "Luck";
        MaxValue = 100;
    }
}

public class Aggression : Attribute
{
    public new readonly string Name;
    public Aggression()
    {
        Name = "Aggression";
        MaxValue = 100;
    }
}

public class Wisdom : Attribute
{
    public new readonly string Name;
    public Wisdom()
    {
        Name = "Wisdom";
        MaxValue = 100;
    }
}