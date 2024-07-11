using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using Godot;
using Godot.Collections;
// ReSharper disable ForCanBeConvertedToForeach

namespace Horizon.Drive.Processors
{
    public partial class Differential : Node, IForceProcessor, IForceOutput
    {
        public IForceProcessor NextProcessor => null;
        public IWheel[] Wheels { get; private set; }

        [Export] private Array<Node> wheelObjects = new();
        [Export] private float gearRatio;
        private float wheelTorque1;
        private float wheelTorque2;

        public void Process(RotForc currentForc)
        {
            var currentRatio = Mathf.Lerp(0, gearRatio, Wheels[0].Speed / Wheels[1].Speed / 2);
            wheelTorque1 = currentForc.torque * currentRatio;
            wheelTorque2 = currentForc.torque / currentRatio;
            GD.Print("Current forc sent to the wheels:\n\t1: " + wheelTorque1 + ", 2: " + wheelTorque2);
            ApplyToWheels();
        }


        public void ApplyToWheels()
        {
            Wheels[0].Move(wheelTorque1);
            Wheels[1].Move(wheelTorque2);
        }

        public override void _Ready()
        {
            Wheels = new IWheel[2];
            for (var i = 0; i < wheelObjects.Count; i++)
            {
                if (wheelObjects[i] is not IWheel) wheelObjects.RemoveAt(i);
            }
            if (wheelObjects.Count != 2)
                GD.PushWarning(
                    "There weren't exactly 2 wheels added to the differential. More wheels will be ignored, less wheels" +
                    " will throw an error. Always add 2 IWheels!");
            Wheels[0] = wheelObjects[0] as IWheel;
            Wheels[1] = wheelObjects[1] as IWheel;
        }
    }
}