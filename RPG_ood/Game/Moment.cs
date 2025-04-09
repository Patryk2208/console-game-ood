namespace RPG_ood.Game;

public abstract class CustomEvent
{
    protected abstract List<(string, IObserver)> Observers { get; set; }

    public void AddObserver(string name, IObserver observer)
    {
        Observers.Add((name, observer));
    }

    public void RemoveObserver(string name, IObserver observer)
    {
        Observers.Remove((name, observer));
    }

    public void ClearObservers()
    {
        Observers.Clear();
    }
    public void NotifyObservers(GameState? state)
    {
        foreach (var observer in Observers.ToList())
        {
            observer.Item2.Update(state);
        }
    }
}
public class MomentChangedEvent : CustomEvent
{
    protected override List<(string, IObserver)> Observers { get; set; } = new();
    public IEnumerable<string> Names => Observers.Select(x => x.Item1);
}