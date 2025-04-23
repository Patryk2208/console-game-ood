using RPG_ood.Beings;
using RPG_ood.Game;
using RPG_ood.Map;

namespace RPG_ood.Input;

public class InputHandlingBuilder(GameState state, Logs logs) : IRoomBuilder
{
    private ConsoleInputHandlerLink _inputChainHead;
    private List<ConsoleInputHandlerLink> _rawInputHandlers = new List<ConsoleInputHandlerLink>();
    private readonly GameState _gameState = state;
    private readonly Logs _logs = logs;

    public void BuildEmptyRoom()
    {
        var up = new MoveUpLink(_gameState, _logs);
        var down = new MoveDownLink(_gameState, _logs);
        var left = new MoveLeftLink(_gameState, _logs);
        var right = new MoveRightLink(_gameState, _logs);
        var exit = new ExitLink(_gameState, _logs);
        if (!_rawInputHandlers.Contains(up)) _rawInputHandlers.Add(up);
        if (!_rawInputHandlers.Contains(down)) _rawInputHandlers.Add(down);
        if (!_rawInputHandlers.Contains(left)) _rawInputHandlers.Add(left);
        if (!_rawInputHandlers.Contains(right)) _rawInputHandlers.Add(right);
        if (!_rawInputHandlers.Contains(exit)) _rawInputHandlers.Add(exit);
    }
    public void BuildFullRoom(AnsiConsoleColor color)
    {
        var up = new MoveUpLink(_gameState, _logs);
        var down = new MoveDownLink(_gameState, _logs);
        var left = new MoveLeftLink(_gameState, _logs);
        var right = new MoveRightLink(_gameState, _logs);
        var exit = new ExitLink(_gameState, _logs);
        if (!_rawInputHandlers.Contains(up)) _rawInputHandlers.Add(up);
        if (!_rawInputHandlers.Contains(down)) _rawInputHandlers.Add(down);
        if (!_rawInputHandlers.Contains(left)) _rawInputHandlers.Add(left);
        if (!_rawInputHandlers.Contains(right)) _rawInputHandlers.Add(right);
        if (!_rawInputHandlers.Contains(exit)) _rawInputHandlers.Add(exit);
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
        var equip = new EquipLink(_gameState, _logs);
        var throwOut = new ThrowLink(_gameState, _logs);
        var throwAll = new ThrowAllLink(_gameState, _logs);
        var selectUp = new PickUpSelectUpLink(_gameState, _logs);
        var selectDown = new PickUpSelectDownLink(_gameState, _logs);
        var eqSelectUp = new EqSelectUpLink(_gameState, _logs);
        var eqSelectDown = new EqSelectDownLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(equip)) _rawInputHandlers.Add(equip);
        if(!_rawInputHandlers.Contains(throwOut)) _rawInputHandlers.Add(throwOut);
        if(!_rawInputHandlers.Contains(throwAll)) _rawInputHandlers.Add(throwAll);
        if(!_rawInputHandlers.Contains(selectUp)) _rawInputHandlers.Add(selectUp);
        if(!_rawInputHandlers.Contains(selectDown)) _rawInputHandlers.Add(selectDown);
        if(!_rawInputHandlers.Contains(eqSelectUp)) _rawInputHandlers.Add(eqSelectUp);
        if(!_rawInputHandlers.Contains(eqSelectDown)) _rawInputHandlers.Add(eqSelectDown);
    }
    public void PlaceWeapons(int maxItemsOfType)
    {
        var equip = new EquipLink(_gameState, _logs);
        var throwOut = new ThrowLink(_gameState, _logs);
        var throwAll = new ThrowAllLink(_gameState, _logs);
        var selectUp = new PickUpSelectUpLink(_gameState, _logs);
        var selectDown = new PickUpSelectDownLink(_gameState, _logs);
        var eqSelectUp = new EqSelectUpLink(_gameState, _logs);
        var eqSelectDown = new EqSelectDownLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(equip)) _rawInputHandlers.Add(equip);
        if(!_rawInputHandlers.Contains(throwOut)) _rawInputHandlers.Add(throwOut);
        if(!_rawInputHandlers.Contains(throwAll)) _rawInputHandlers.Add(throwAll);
        if(!_rawInputHandlers.Contains(selectUp)) _rawInputHandlers.Add(selectUp);
        if(!_rawInputHandlers.Contains(selectDown)) _rawInputHandlers.Add(selectDown);
        if(!_rawInputHandlers.Contains(eqSelectUp)) _rawInputHandlers.Add(eqSelectUp);
        if(!_rawInputHandlers.Contains(eqSelectDown)) _rawInputHandlers.Add(eqSelectDown);
        var rightHand = new PutInRightHand(_gameState, _logs);
        var leftHand = new PutInLeftHand(_gameState, _logs);
        if(!_rawInputHandlers.Contains(rightHand)) _rawInputHandlers.Add(rightHand);
        if(!_rawInputHandlers.Contains(leftHand)) _rawInputHandlers.Add(leftHand);
        var useLeft = new UseFromLeftHandLink(_gameState, _logs);
        var useRight = new UseFromRightHandLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(useLeft)) _rawInputHandlers.Add(useLeft);
        if(!_rawInputHandlers.Contains(useRight)) _rawInputHandlers.Add(useRight);
        var normalRight = new NormalAttackRightHandLink(_gameState, _logs);
        var normalLeft = new NormalAttackLeftHandLink(_gameState, _logs);
        var sneakRight = new SneakAttackRightHandLink(_gameState, logs);
        var sneakLeft = new SneakAttackLeftHandLink(_gameState, logs);
        var magicRight = new MagicAttackRightHandLink(_gameState, logs);
        var magicLeft = new MagicAttackLeftHandLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(normalRight)) _rawInputHandlers.Add(normalRight);
        if(!_rawInputHandlers.Contains(normalLeft)) _rawInputHandlers.Add(normalLeft);
        if(!_rawInputHandlers.Contains(sneakRight)) _rawInputHandlers.Add(sneakRight);
        if(!_rawInputHandlers.Contains(sneakLeft)) _rawInputHandlers.Add(sneakLeft);
        if(!_rawInputHandlers.Contains(magicRight)) _rawInputHandlers.Add(magicRight);
        if(!_rawInputHandlers.Contains(magicLeft)) _rawInputHandlers.Add(magicLeft);
    }
    public void PlaceModifiedWeapons(int maxItemsOfType)
    {
        var equip = new EquipLink(_gameState, _logs);
        var throwOut = new ThrowLink(_gameState, _logs);
        var throwAll = new ThrowAllLink(_gameState, _logs);
        var selectUp = new PickUpSelectUpLink(_gameState, _logs);
        var selectDown = new PickUpSelectDownLink(_gameState, _logs);
        var eqSelectUp = new EqSelectUpLink(_gameState, _logs);
        var eqSelectDown = new EqSelectDownLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(equip)) _rawInputHandlers.Add(equip);
        if(!_rawInputHandlers.Contains(throwOut)) _rawInputHandlers.Add(throwOut);
        if(!_rawInputHandlers.Contains(throwAll)) _rawInputHandlers.Add(throwAll);
        if(!_rawInputHandlers.Contains(selectUp)) _rawInputHandlers.Add(selectUp);
        if(!_rawInputHandlers.Contains(selectDown)) _rawInputHandlers.Add(selectDown);
        if(!_rawInputHandlers.Contains(eqSelectUp)) _rawInputHandlers.Add(eqSelectUp);
        if(!_rawInputHandlers.Contains(eqSelectDown)) _rawInputHandlers.Add(eqSelectDown);
        var rightHand = new PutInRightHand(_gameState, _logs);
        var leftHand = new PutInLeftHand(_gameState, _logs);
        if(!_rawInputHandlers.Contains(rightHand)) _rawInputHandlers.Add(rightHand);
        if(!_rawInputHandlers.Contains(leftHand)) _rawInputHandlers.Add(leftHand);
        var useLeft = new UseFromLeftHandLink(_gameState, _logs);
        var useRight = new UseFromRightHandLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(useLeft)) _rawInputHandlers.Add(useLeft);
        if(!_rawInputHandlers.Contains(useRight)) _rawInputHandlers.Add(useRight);
        var normalRight = new NormalAttackRightHandLink(_gameState, _logs);
        var normalLeft = new NormalAttackLeftHandLink(_gameState, _logs);
        var sneakRight = new SneakAttackRightHandLink(_gameState, logs);
        var sneakLeft = new SneakAttackLeftHandLink(_gameState, logs);
        var magicRight = new MagicAttackRightHandLink(_gameState, logs);
        var magicLeft = new MagicAttackLeftHandLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(normalRight)) _rawInputHandlers.Add(normalRight);
        if(!_rawInputHandlers.Contains(normalLeft)) _rawInputHandlers.Add(normalLeft);
        if(!_rawInputHandlers.Contains(sneakRight)) _rawInputHandlers.Add(sneakRight);
        if(!_rawInputHandlers.Contains(sneakLeft)) _rawInputHandlers.Add(sneakLeft);
        if(!_rawInputHandlers.Contains(magicRight)) _rawInputHandlers.Add(magicRight);
        if(!_rawInputHandlers.Contains(magicLeft)) _rawInputHandlers.Add(magicLeft);
    }
    public void PlaceElixirs(int maxItemsOfType)
    {
        var equip = new EquipLink(_gameState, _logs);
        var throwOut = new ThrowLink(_gameState, _logs);
        var throwAll = new ThrowAllLink(_gameState, _logs);
        var selectUp = new PickUpSelectUpLink(_gameState, _logs);
        var selectDown = new PickUpSelectDownLink(_gameState, _logs);
        var eqSelectUp = new EqSelectUpLink(_gameState, _logs);
        var eqSelectDown = new EqSelectDownLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(equip)) _rawInputHandlers.Add(equip);
        if(!_rawInputHandlers.Contains(throwOut)) _rawInputHandlers.Add(throwOut);
        if(!_rawInputHandlers.Contains(throwAll)) _rawInputHandlers.Add(throwAll);
        if(!_rawInputHandlers.Contains(selectUp)) _rawInputHandlers.Add(selectUp);
        if(!_rawInputHandlers.Contains(selectDown)) _rawInputHandlers.Add(selectDown);
        if(!_rawInputHandlers.Contains(eqSelectUp)) _rawInputHandlers.Add(eqSelectUp);
        if(!_rawInputHandlers.Contains(eqSelectDown)) _rawInputHandlers.Add(eqSelectDown);
        var useLeft = new UseFromLeftHandLink(_gameState, _logs);
        var useRight = new UseFromRightHandLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(useLeft)) _rawInputHandlers.Add(useLeft);
        if(!_rawInputHandlers.Contains(useRight)) _rawInputHandlers.Add(useRight);
        var normalRight = new NormalAttackRightHandLink(_gameState, _logs);
        var normalLeft = new NormalAttackLeftHandLink(_gameState, _logs);
        var sneakRight = new SneakAttackRightHandLink(_gameState, logs);
        var sneakLeft = new SneakAttackLeftHandLink(_gameState, logs);
        var magicRight = new MagicAttackRightHandLink(_gameState, logs);
        var magicLeft = new MagicAttackLeftHandLink(_gameState, _logs);
        if(!_rawInputHandlers.Contains(normalRight)) _rawInputHandlers.Add(normalRight);
        if(!_rawInputHandlers.Contains(normalLeft)) _rawInputHandlers.Add(normalLeft);
        if(!_rawInputHandlers.Contains(sneakRight)) _rawInputHandlers.Add(sneakRight);
        if(!_rawInputHandlers.Contains(sneakLeft)) _rawInputHandlers.Add(sneakLeft);
        if(!_rawInputHandlers.Contains(magicRight)) _rawInputHandlers.Add(magicRight);
        if(!_rawInputHandlers.Contains(magicLeft)) _rawInputHandlers.Add(magicLeft);
    }
    public void PlacePlayer(Player player) {}
    public void PlaceEnemies(int maxItemsOfType) {}
    public ConsoleInputHandlerLink GetResult()
    {
        _inputChainHead = new SentinelLink(_gameState, _logs);
        _rawInputHandlers.Reverse();
        foreach (var handler in _rawInputHandlers)
        {
            handler.NextLink = _inputChainHead;
            _inputChainHead = handler;
        }
        return _inputChainHead;
    }
}