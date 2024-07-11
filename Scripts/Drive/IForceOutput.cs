using System.Collections.Generic;

namespace Horizon.Drive
{
    public interface IForceOutput
    {
        protected IWheel[] Wheels { get; }

        public void ApplyToWheels();
    }
}