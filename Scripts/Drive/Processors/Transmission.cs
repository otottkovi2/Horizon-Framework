using System.Collections.Generic;
using Godot;

namespace Horizon.Drive.Processors
{
    public partial class Transmission : Node, IForceProcessor
    {
        public IForceProcessor NextProcessor { get; set; }
        public sbyte SelectedGear
        {
            get => selectedGear;
            set
            {
                if(!gears.ContainsKey(value)) return;
                selectedGear = value;
                GD.Print("Changed gear to " + value);
            }
        }

        private readonly Dictionary<sbyte, float> gears = new()
        {
            {-1, -0.27f},
            {0,0},
            {1,0.27f},
            {2,0.48f},
            {3,0.73f},
            {4,0.98f},
            {5,1.2f},
            {6,1.45f}
        };
        private sbyte selectedGear;

        public void Process(RotForc currentForc)
        {
            currentForc.speed *= gears[selectedGear];
            currentForc.torque = selectedGear != 0 ? currentForc.torque / gears[selectedGear] : 0;
            GD.Print("Current forc in the transmission: " + currentForc);
            (this as IForceProcessor).PassToNext(currentForc);
        }

        public override void _Ready()
        {
            NextProcessor = GetChild<IForceProcessor>(0);
            GD.Print("Currently in gear #" + selectedGear);
            if (NextProcessor is null)
                GD.PushWarning(
                    "No next processor found. Make sure to place the next force processor as this object's child." +
                    " If this is the last processor, implement IForceOutput.");
        }

        public override void _PhysicsProcess(double delta)
        {
            if (Input.IsActionJustReleased("gear R", true)) SelectedGear = -1;
            if (Input.IsActionJustReleased("gear N", true)) SelectedGear = 0;
            for (sbyte i = 3; i <= gears.Count; i++)
            {
                if (Input.IsActionJustReleased("gear " + (i-2), true)) SelectedGear = (sbyte) (i-2);
            }
        }
    }
}