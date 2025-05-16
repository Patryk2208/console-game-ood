using RPG_ood.Commands;

namespace RPG_ood.App.Server;

public class ClientContext
{
    public long Id { get; init; }
    public Task ReceiveCommand { get; set; }
    public Task SendSnapshot { get; set; }
    public CancellationTokenSource TokenSource { get; init; }
    public Mutex Mutex { get; init; }
}