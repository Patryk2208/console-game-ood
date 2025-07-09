namespace Model.Game.Beings;

[Serializable]
public class StrategyManager
{
    public MovementStrategy GetStrategy(float healthPercentage, MovementStrategy currentStrategy)
    {
        const float tolerance = 0.01f;
        if (Math.Abs(healthPercentage - 1.0f) < tolerance)
        {
            return currentStrategy;
        }
        return healthPercentage switch
        {
            < 1.0f and > 0.5f => new AggressiveMovementStrategy(),
            < 0.5f => new ShyMovementStrategy(),
            _ => currentStrategy
        };
    }
}