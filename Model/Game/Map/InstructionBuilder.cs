using Model.Game.GameState;

namespace Model.Game.Map;


public class RoomInstructionBuilder : IRoomBuilder
{
    private Instruction _instruction { get; set; } = new Instruction();
    public void BuildEmptyRoom() {}

    public void BuildFullRoom() {}

    public void CarveMaze() {}

    public (int, int) AddRandomPath(int s0 = -1, int s1 = -1)
    {
        return (0, 0);
    }

    public void AddRandomChamber(int size) {}

    public void AddCentralRoom(float size)
    {
        //no enemies in the central room?
    }

    public void PlaceItems(int maxItemsOfType)
    {
        var inst = """
                   E - pickup selected to Eq, T - throw away selected from Eq
                   ArrowUP/DOWN - go through items to pickup
                   ArrowLeft/Right - go through items in Eq
                   Items are marked with small letters
                   """;
        if (!_instruction.Instructions.Contains(inst))
        {
            _instruction.Instructions.Add(inst);
        }
    }

    public void PlaceWeapons(int maxItemsOfType)
    {
        var inst = """
                   P - Place selected weapon in right hand / Move item from right hand back to Eq
                   L - Place selected weapon in left hand / Move item from left hand back to Eq
                   Enter - Use item in right hand
                   Alt+Enter - Use item in left hand
                   Weapons are marked with small letters
                   """;
        if (!_instruction.Instructions.Contains(inst))
        {
            _instruction.Instructions.Add(inst);
        }
    }

    public void PlaceModifiedWeapons(int maxItemsOfType)
    {
        var inst = """
                   P - Place selected weapon in right hand / Move item from right hand back to Eq
                   L - Place selected weapon in left hand / Move item from left hand back to Eq
                   Enter - Use item in right hand
                   Alt+Enter - Use item in left hand
                   Weapons are marked with small letters
                   """;
        var inst2 = """
                    Modified items are colored
                    """;
        if (!_instruction.Instructions.Contains(inst))
        {
            _instruction.Instructions.Add(inst);
        }

        if (!_instruction.Instructions.Contains(inst2))
        {
            _instruction.Instructions.Add(inst2);
        }
    }

    public void PlaceElixirs(int maxItemsOfType)
    {
        var inst = """
                   Elixirs are marked with letter e
                   All used elixir effects are shown
                   """;
        if (!_instruction.Instructions.Contains(inst))
        {
            _instruction.Instructions.Add(inst);
        }
    }

    public void PlaceEnemies(int maxItemsOfType)
    {
        var inst = """
                   You can see enemies in your neighborhood, You cannot pass through them
                   Enemies are marked with Big Letters
                   """;
        if (!_instruction.Instructions.Contains(inst))
        {
            _instruction.Instructions.Add(inst);
        }
    }

    public Instruction GetResult()
    {
        return _instruction;
    }
}