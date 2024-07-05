namespace Horizon.Drive
{
    public class RotForc
    {
        public float speed;
        public float torque;

        public RotForc(float speed, float torque)
        {
            this.speed = speed;
            this.torque = torque;
        }

        public override string ToString()
        {
            return $"(RPM: {speed}, torque: {torque})";
        }
    }
}
