using RPG_ood.Model;

namespace RPG_ood.Map;

public interface IRoomDirector
{
    public IRoomBuilder _builder { get; set; }
    public void SwitchBuilders(IRoomBuilder builder);
    public void Build();
}

public class SimpleRaggedMaze(IRoomBuilder builder) : IRoomDirector
{
    public IRoomBuilder _builder { get; set; } = builder;

    public void SwitchBuilders(IRoomBuilder builder)
    {
        _builder = builder;
    }

    public void Build()
    {
        _builder.BuildFullRoom();
        _builder.AddCentralRoom(0.3f);
        var (s0, s1) = (0, 0);
        for (int i = 0; i < 30; i++)
        {
            (s0, s1) = _builder.AddRandomPath(s0, s1);
        }
        _builder.PlaceItems(3);
        _builder.PlaceWeapons(3);
        _builder.PlaceModifiedWeapons(3);
        _builder.PlaceElixirs(2);
        _builder.PlaceEnemies(2);
    }
}

public class PlayableMaze (IRoomBuilder builder) : IRoomDirector
{
    public IRoomBuilder _builder { get; set; } = builder;
    public void SwitchBuilders(IRoomBuilder builder)
    {
        _builder = builder;
    }

    public void Build()
    {
        _builder.BuildFullRoom();
        _builder.CarveMaze();
        _builder.AddCentralRoom(0.2f);
        for (int i = 0; i < 10; i++)
        {
            _builder.AddRandomChamber(2);
        }

        for (int i = 0; i < 5; i++)
        {
            _builder.AddRandomPath();
        }
        _builder.PlaceItems(8);
        _builder.PlaceWeapons(2);
        _builder.PlaceModifiedWeapons(7);
        _builder.PlaceElixirs(7);
        _builder.PlaceEnemies(5);
    }
}

public class TestMaze (IRoomBuilder builder) : IRoomDirector
{
    public IRoomBuilder _builder { get; set; } = builder;
    public void SwitchBuilders(IRoomBuilder builder)
    {
        _builder = builder;
    }

    public void Build()
    {
        _builder.BuildFullRoom();
        _builder.CarveMaze();
        _builder.PlaceEnemies(3);
        //_builder.PlaceModifiedWeapons(3);
    }
}