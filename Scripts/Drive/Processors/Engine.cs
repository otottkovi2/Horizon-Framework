using System;
using System.Collections.Generic;
using Godot;
using Horizon.Drive;

namespace Horizon.Drive.Processors
{
    public class Engine : Node, IForceProcessor
    {
        private const int Power = 150;

        public IForceProcessor NextProcessor { get; }

        private readonly Curve rpmTorqueCurve;

        private float currentRpm = 0;

        // ReSharper disable once FieldCanBeMadeReadOnly.Local
        private float throttlePercent = 0.5f;

        public void Process(RotForc currentForc)
        {
            currentRpm += Power * throttlePercent;
            var torque = rpmTorqueCurve.Sample(currentRpm);
            currentForc.speed = currentRpm;
            currentForc.torque = torque;
            (this as IForceProcessor).PassToNext(currentForc);
        }
        

        public Engine(IForceProcessor nextProcessor,Curve engineCurve)
        {
            NextProcessor = nextProcessor;
            rpmTorqueCurve = engineCurve;
        }
    }
}
