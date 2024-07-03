namespace Horizon.Drive
{
    public interface IForceProcessor
    {
        protected IForceProcessor NextProcessor { get; }
        public void Process(RotForc currentForc);

        public void PassToNext(RotForc forcToPass)
        {
            NextProcessor.Process(forcToPass);
        }
    }
}