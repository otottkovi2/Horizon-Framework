using Godot;
using System;
using System.Collections.Generic;
using Godot.Collections;

namespace Horizon.Drive
{
    public partial class Vehicle : Node
    {
        private List<IForceProcessor> processors = new();
        private IForceInput input;
        private List<IWheel> wheels  = new();

        // Called when the node enters the scene tree for the first time.
        public override void _Ready()
        {
            input = GetChild<IForceInput>(1);
            var child = ((input as Node)!).GetChild<IForceProcessor>(0);
            GD.Print("Hello");
            while (child is not null)
            {
                processors.Add(child);
                child = ((child as Node)!).GetChild<IForceProcessor>(0);
            }

            var children = GetChildren();
            for (var i = 0; i < children.Count; i++)
            {
                if(children[i] is IWheel) wheels.Add(children[i] as IWheel);
            }
        }

        // Called every frame. 'delta' is the elapsed time since the previous frame.
        public override void _Process(double delta)
        {
        }
    }
}
