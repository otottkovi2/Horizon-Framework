using System.Threading;
using Godot;

namespace Horizon.Drive.Inputs
{
    public partial class Accelerator : Node, IForceInput
    {
        public IForceProcessor FirstProcessor { get; private set; }

        private float currentThrottle;

        private bool isAccelerating;
        private readonly RotForc startForc = new (0, 0);

        public void SendToProcessor()
        {
            startForc.torque = currentThrottle;
            FirstProcessor.Process(startForc);
        }

        public override void _Ready()
        {
            FirstProcessor = GetChild<IForceProcessor>(0);
            if(FirstProcessor is null) GD.PushWarning("No processor could be found. Check if you have an IForceProcessor as this Node's child!");
        }

        public override void _PhysicsProcess(double delta)
        {
            currentThrottle = Input.GetActionStrength("accelerate", true);
            if (currentThrottle == 0 && isAccelerating)
            {
                isAccelerating = false;
                return;
            }
            isAccelerating = true;
            GD.Print("---- END OF FRAME ----");
            SendToProcessor();
        }
    }
}