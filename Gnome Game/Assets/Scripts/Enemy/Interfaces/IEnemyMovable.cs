using UnityEngine;

public interface IEnemyMovable
{
    Rigidbody RB { get; set; }
    Vector3 MovementDirection { get; set; }
    void MoveEnemy(Vector3 velocity);
    void CheckForDirectionFacing();
}
