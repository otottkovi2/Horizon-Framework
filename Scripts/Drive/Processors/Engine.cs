using System;
using System.Collections.Generic;
using Godot;
using Horizon.Drive;

namespace Horizon.Drive.Processors
{
    public partial class Engine : Node, IForceProcessor
    {
        [Export] private int power = 150;

        [Export]private Curve rpmTorqueCurve = new();

        public IForceProcessor NextProcessor { get; private set; }

        private float currentRpm;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private float throttlePercent = 0.5f;

        public void Process(RotForc currentForc)
        {
            GD.Print("Hello, I'm engine.");
            currentRpm += power * throttlePercent;
            var torque = rpmTorqueCurve.Sample(currentRpm);
            currentForc.speed = currentRpm;
            currentForc.torque = torque;
            GD.Print("Current forc: " + currentForc);
            (this as IForceProcessor).PassToNext(currentForc);
        }

        public override void _Ready()
        {
            var rpmValues = new float[] { 0, 2500, 3000, 3500, 4000, 4500, 5000, 5500, 6000, 6500, 7000, 7500 };
            var torqueValues = new float[] {0, 40, 70, 90, 130, 190, 225, 230, 245, 265, 262, 255 };
            var pos = new Vector2();
            for (var i = 0; i < rpmValues.Length; i++)
            {
                pos.X = rpmValues[i];
                pos.Y = torqueValues[i];
                rpmTorqueCurve.AddPoint(pos);
            }

            NextProcessor = GetChild<IForceProcessor>(0);
            if(NextProcessor is null) GD.PrintErr("This IForceProcessor has no processor after it. If this is the" +
                                                  " last processing step, please implement IForceOutput as well.");
        }
        
    }
}
