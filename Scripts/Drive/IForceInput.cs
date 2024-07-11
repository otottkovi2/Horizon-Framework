namespace Horizon.Drive
{
    public interface IForceInput
    {
        protected IForceProcessor FirstProcessor { get; }

        void SendToProcessor();
    }
}