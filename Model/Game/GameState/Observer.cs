namespace Model.Game.GameState;

public interface IObserver
{
    public void Update(Game.GameState.GameState? state, long id);
}