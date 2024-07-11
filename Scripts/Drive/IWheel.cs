namespace Horizon.Drive
{
    public interface IWheel
    {
        public float Speed { get; }

        void Move(float torque);
    }
}