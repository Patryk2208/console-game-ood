using RPG_ood.Beings;
using RPG_ood.Game;
using RPG_ood.Map;

namespace RPG_ood.Input;

public class InputHandlingBuilder(GameState state, Logs logs) : IRoomBuilder
{
    private ConsoleInputHandlerLink _inputChainHead;
    
    private List<ConsoleInputHandlerLink> _inputHandlers = new List<ConsoleInputHandlerLink>();

    private List<ConsoleInputHandlerLink> _possibleInputHandlers =
    [
        new MoveUpLink(state, logs),
        new MoveDownLink(state, logs),
        new MoveLeftLink(state, logs),
        new MoveRightLink(state, logs),
        new ExitLink(state, logs),
        new EquipLink(state, logs),
        new ThrowLink(state, logs),
        new ThrowAllLink(state, logs),
        new PickUpSelectUpLink(state, logs),
        new PickUpSelectDownLink(state, logs),
        new EqSelectUpLink(state, logs),
        new EqSelectDownLink(state, logs),
        new PutInRightHand(state, logs),
        new PutInLeftHand(state, logs),
        new UseFromRightHandLink(state, logs),
        new UseFromLeftHandLink(state, logs),
        new NormalAttackRightHandLink(state, logs),
        new NormalAttackLeftHandLink(state, logs),
        new SneakAttackRightHandLink(state, logs),
        new SneakAttackLeftHandLink(state, logs),
        new MagicAttackRightHandLink(state, logs),
        new MagicAttackLeftHandLink(state, logs)
    ];
    private readonly GameState _gameState = state;
    private readonly Logs _logs = logs;

    public void BuildEmptyRoom()
    {
        for (int i = 0; i < 5; i++)
        {
            if(!_inputHandlers.Contains(_possibleInputHandlers[i])) _inputHandlers.Add(_possibleInputHandlers[i]);
        }
    }
    public void BuildFullRoom(AnsiConsoleColor color)
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
    public void PlacePlayer(Player player) {}
    public void PlaceEnemies(int maxItemsOfType) {}
    public ConsoleInputHandlerLink GetResult()
    {
        _inputChainHead = new SentinelLink(_gameState, _logs);
        _inputHandlers.Reverse();
        foreach (var handler in _inputHandlers)
        {
            handler.NextLink = _inputChainHead;
            _inputChainHead = handler;
        }
        return _inputChainHead;
    }
}