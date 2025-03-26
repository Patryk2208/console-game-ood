using System.Text;

namespace RPG_ood.Game;

public class Instruction
{
    public List<string> Instructions { get; } = new();

    public Instruction()
    {
        Instructions.Add("(W, S, A, D) steering, Esc - Exit");
    }
}