public interface IEnemySense
{
    bool HasTarget { get; }
    Transform Target { get; }
    float DistanceToTarget { get; }
    bool CanSeeTarget { get; }
}