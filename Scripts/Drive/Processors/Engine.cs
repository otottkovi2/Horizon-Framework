using System;
using System.Collections.Generic;
using System.Runtime.ExceptionServices;
using Godot;
using Horizon.Drive;

namespace Horizon.Drive.Processors
{
    public partial class Engine : Node, IForceProcessor
    {
        [Export] private int power = 150;
        [Export] private int redline = 6600;

        public IForceProcessor NextProcessor { get; private set; }

        private float currentRpm;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private float throttlePercent = 0.5f;
        private float[] rpmValues;
        private float[] torqueValues;

        private const int RpmIncreaseRate = 10;

        public void Process(RotForc currentForc)
        {
            GD.Print("Hello, I'm engine.");
            throttlePercent = currentForc.torque;
            currentRpm += RpmIncreaseRate * throttlePercent;
            float torque = 0;
            if (currentRpm > 0 && currentRpm < redline)
            {
                for (var i = 0; i < rpmValues.Length; i++)
                {
                    if (rpmValues[i] < Mathf.Abs(currentRpm)) continue;
                    var weight = Mathf.InverseLerp(rpmValues[i - 1], rpmValues[i], currentRpm);
                    torque = Mathf.Lerp(torqueValues[i - 1], torqueValues[i], weight);
                    break;
                }
            }
            else if (currentRpm > redline) currentRpm = redline;
            currentForc.speed = currentRpm;
            currentForc.torque = torque;
            GD.Print("Current forc: " + currentForc);
            (this as IForceProcessor).PassToNext(currentForc);
        }

        public override void _Ready()
        {
            rpmValues = new float[]
            {
                0, 1500, 1750, 2000, 2250, 2500, 2750, 3000, 3250, 3500, 3750, 4000, 4250, 4500, 4750, 5000, 5250, 5500,
                5750, 6000, 6250, 6500
            };
            torqueValues = new float[]
            {
                0, 128, 169, 216, 257, 271, 264, 257, 253, 253, 253, 254, 257, 253, 252, 252, 252, 250, 230, 216, 199,
                189
            };
            NextProcessor = GetChild<IForceProcessor>(0);
            if (NextProcessor is null) GD.PrintErr("This IForceProcessor has no processor after it. If this is the" +
                                                  " last processing step, please implement IForceOutput as well.");
        }

    }
}
