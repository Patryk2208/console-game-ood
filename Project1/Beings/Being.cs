using Project_oob.Map;

namespace Project_oob.Beings;

public abstract class Being : MapElement
{
    public string Name { get; protected set; }
}


public abstract class UserControlledBeing : Being
{
    public abstract void TakeInput();
}


public abstract class NpcBeing : Being
{
    //later
}