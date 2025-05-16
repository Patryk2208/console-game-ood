using RPG_ood.Input;
using RPG_ood.Map;
using RPG_ood.Model;
using RPG_ood.Model.Beings;
using RPG_ood.Model.Game;
using RPG_ood.Model.Game.GameState;

namespace RPG_ood.Controller.Input;

public class InputHandlingBuilder(GameState state) : IRoomBuilder
{
    private ConsoleInputHandlerLink _inputChainHead;
    
    private List<ConsoleInputHandlerLink> _inputHandlers = new List<ConsoleInputHandlerLink>();

    private List<ConsoleInputHandlerLink> _possibleInputHandlers =
    [
        new MoveUpLink(state),
        new MoveDownLink(state),
        new MoveLeftLink(state),
        new MoveRightLink(state),
        new ExitLink(state),
        new EquipLink(state),
        new ThrowLink(state),
        new ThrowAllLink(state),
        new PickUpSelectUpLink(state),
        new PickUpSelectDownLink(state),
        new EqSelectUpLink(state),
        new EqSelectDownLink(state),
        new PutInRightHand(state),
        new PutInLeftHand(state),
        new UseFromRightHandLink(state),
        new UseFromLeftHandLink(state),
        new NormalAttackRightHandLink(state),
        new NormalAttackLeftHandLink(state),
        new SneakAttackRightHandLink(state),
        new SneakAttackLeftHandLink(state),
        new MagicAttackRightHandLink(state),
        new MagicAttackLeftHandLink(state),
        new VerifyUserLink(state)
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
    public ConsoleInputHandlerLink GetResult()
    {
        _inputChainHead = new SentinelLink(state);
        _inputHandlers.Reverse();
        foreach (var handler in _inputHandlers)
        {
            handler.SetNextLink(_inputChainHead);
            _inputChainHead = handler;
        }

        var verify = _possibleInputHandlers[^1];
        verify.SetNextLink(_inputChainHead);
        _inputChainHead = verify;
        return _inputChainHead;
    }
}