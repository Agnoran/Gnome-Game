using UnityEngine;

public interface IEnemySense
{
    bool HasTarget { get; }
    //Transfrom Target { get; }
    float DistanceToTarget { get; }
    bool CanSeeTarget { get; }
}