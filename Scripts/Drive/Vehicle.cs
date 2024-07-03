using Godot;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Godot.Collections;

namespace Horizon.Drive
{
    public partial class Vehicle : Node
    {
        [Export] private Array<Node> processorNodes;
        private IForceProcessor[] processors;

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            processors = new IForceProcessor[processors.Length];
            for (var i = 0; i < processorNodes.Count; i++) processors[i] = processorNodes[i] as IForceProcessor;
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}
