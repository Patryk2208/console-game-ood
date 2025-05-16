namespace RPG_ood.Model.Game.GameState;

public class Instruction
{
    public List<string> Instructions { get; } = new();

    public Instruction()
    {
        Instructions.Add("(W, S, A, D) steering, Esc - Exit");
    }
}