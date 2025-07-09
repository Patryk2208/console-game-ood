using Model.Game.Map;

namespace Client.Controller;

public class InputHandlingBuilder : IRoomBuilder
{
    private ConsoleInputHandlerLink _inputChainHead;
    
    private readonly List<ConsoleInputHandlerLink> _inputHandlers = new List<ConsoleInputHandlerLink>();

    private readonly List<ConsoleInputHandlerLink> _possibleInputHandlers =
    [
        new MoveUpLink(),
        new MoveDownLink(),
        new MoveLeftLink(),
        new MoveRightLink(),
        new ExitLink(),
        new EquipLink(),
        new ThrowLink(),
        new ThrowAllLink(),
        new PickUpSelectUpLink(),
        new PickUpSelectDownLink(),
        new EqSelectUpLink(),
        new EqSelectDownLink(),
        new PutInRightHandLink(),
        new PutInLeftHandLink(),
        new UseFromRightHandLink(),
        new UseFromLeftHandLink(),
        new NormalAttackRightHandLink(),
        new NormalAttackLeftHandLink(),
        new SneakAttackRightHandLink(),
        new SneakAttackLeftHandLink(),
        new MagicAttackRightHandLink(),
        new MagicAttackLeftHandLink()
    ];

    public void BuildEmptyRoom()
    {
        for (int i = 0; i < 5; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void BuildFullRoom()
    {
        for (int i = 0; i < 5; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void CarveMaze() {}
    public (int, int) AddRandomPath(int s0 = -1, int s1 = -1)
    {
        return default;
    }
    public void AddRandomChamber(int size) {}
    public void AddCentralRoom(float size) {}
    public void PlaceItems(int maxItemsOfType)
    {
        for (int i = 5; i < 12; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void PlaceWeapons(int maxItemsOfType)
    {
        for (int i = 5; i < 22; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void PlaceModifiedWeapons(int maxItemsOfType)
    {
        for (int i = 5; i < 22; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void PlaceElixirs(int maxItemsOfType)
    {
        for (int i = 5; i < 22; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void PlaceEnemies(int maxItemsOfType) {}

    public void BuildAll()
    {
        foreach (var t in _possibleInputHandlers)
        {
            if(!_inputHandlers.Contains(t)) _inputHandlers.Add(t);
        }
    }
    public ConsoleInputHandlerLink GetResult()
    {
        _inputChainHead = new SentinelLink();
        _inputHandlers.Reverse();
        foreach (var handler in _inputHandlers)
        {
            handler.SetNextLink(_inputChainHead);
            _inputChainHead = handler;
        }
        return _inputChainHead;
    }
}