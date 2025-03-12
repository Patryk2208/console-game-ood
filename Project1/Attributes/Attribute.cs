namespace Project_oob.Attributes;

public abstract class Attribute
{
    public string Name { get; protected init; }
    public int Value { get; protected set; }
    public static int MaxValue { get; protected set; }

    public void SetValue(int value)
    {
        Value = value;
    }
}

public class Power : Attribute
{
    public Power()
    {
        Name = "Power";
        MaxValue = 100;
        Value = MaxValue / 2;
    }
}

public class Agility : Attribute
{
    public Agility()
    {
        Name = "Agility";
        MaxValue = 100;
        Value = MaxValue / 2;
    }
}

public class Health : Attribute
{
    public Health()
    {
        Name = "Health";
        MaxValue = 100;
        Value = MaxValue / 2;
    }
}

public class Luck : Attribute
{
    public Luck()
    {
        Name = "Luck";
        MaxValue = 100;
        Value = MaxValue / 2;
    }
}

public class Aggression : Attribute
{
    public Aggression()
    {
        Name = "Aggression";
        MaxValue = 100;
        Value = MaxValue / 2;
    }
}

public class Wisdom : Attribute
{
    public Wisdom()
    {
        Name = "Wisdom";
        MaxValue = 100;
        Value = MaxValue / 2;
    }
}