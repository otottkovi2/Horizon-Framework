using Godot;

namespace Horizon.Drive.Wheels
{
    public partial class TestWheel: Node,IWheel
    {
        public float Speed => rb.AngularVelocity.Length() != 0 ? rb.AngularVelocity.Length() : 1;
        private RigidBody3D rb;
        private Vector3 torqueVector = new (0, 0, 0);
        public void Move(float torque)
        {
            torqueVector.X = torque;
            rb.ApplyTorque(torqueVector);
        }

        public override void _Ready()
        {
            rb = GetChild<RigidBody3D>(0);
        }
    }
}