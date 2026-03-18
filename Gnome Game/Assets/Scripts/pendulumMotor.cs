using UnityEngine;

public class pendulumMotor : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float force;
    [SerializeField] float swingAngle;

    HingeJoint joint;
    JointMotor motor;

    bool goingRight = true;
    void Start()
    {
        joint = GetComponent<HingeJoint>();
        motor = joint.motor;

        joint.useMotor = true;
        joint.useLimits = true;

        JointLimits limits = joint.limits;
        limits.min = -swingAngle;
        limits.max = swingAngle;
        joint.limits = limits;
    }

    void Update()
    {
        motor.force = force;
        motor.targetVelocity = goingRight ? speed : -speed;
        joint.motor = motor;

        if (joint.angle >= swingAngle - 1f)
            goingRight = false;

        if (joint.angle <= -swingAngle + 1f)
            goingRight = true;
    }
}
