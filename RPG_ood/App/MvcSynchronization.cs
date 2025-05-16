namespace RPG_ood.App;

public class MvcSynchronization
{
    public MvcSynchronization()
    {
        ImmediateExit = new CancellationTokenSource();
        GameMutex = new Mutex();
    }
    
    public bool ShouldAllExit
    {
        get => (ShouldExitView & ShouldExitController & ShouldExitModel);
        set
        {
            ShouldExitView = value ? value : ShouldExitView;
            ShouldExitModel = value ? value : ShouldExitModel;
            ShouldExitController = value ? value : ShouldExitController;
        }
    }
    public bool ShouldExitView  { get; set; }
    public bool ShouldExitModel { get; set; }
    public bool ShouldExitController { get; set; }
    public CancellationTokenSource ImmediateExit { get; set; }
    public Mutex GameMutex { get; set; }
    
}