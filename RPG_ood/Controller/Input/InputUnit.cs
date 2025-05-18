namespace RPG_ood.Input;

public struct InputUnit
{
    public long PlayerId { get; set; }
    public ConsoleKeyInfo KeyInfo { get; set; }

    public InputUnit(long playerId, ConsoleKeyInfo keyInfo)
    {
        PlayerId = playerId;
        KeyInfo = keyInfo;
    }
}