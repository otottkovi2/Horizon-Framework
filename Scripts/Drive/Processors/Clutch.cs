using Godot;

namespace Horizon.Drive.Processors
{
    public partial class Clutch : Node, IForceProcessor
    {
        public IForceProcessor NextProcessor { get; private set; }

        private bool isEngaged = true;

        public void Engage()
        {
            isEngaged = true;
        }

        public void Disengage()
        {
            isEngaged = false;
        }

        public void Process(RotForc currentForc)
        {
            if (!isEngaged)
            {
                GD.Print("Clutch pressed!");
                currentForc = RotForc.Zero;
            }
            GD.Print("Current forc in the clutch: " + currentForc);
            (this as IForceProcessor).PassToNext(currentForc);
        }

        public override void _Ready()
        {

            NextProcessor = GetChild<IForceProcessor>(0);
            if (NextProcessor is null)
                GD.PushWarning(
                    "No next processor found. Make sure to place the next force processor as this object's child." +
                    " If this is the last processor, implement IForceOutput.");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Input.IsActionJustPressed("clutch", true)) Disengage();
            if (Input.IsActionJustReleased("clutch", true)) Engage();
        }
    }
}