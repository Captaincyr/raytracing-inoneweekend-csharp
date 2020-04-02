using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core
{
    public class ProgressArgs : EventArgs
    {
        public ProgressArgs(int lineRemaining)
        {
            RemainingLines = lineRemaining;
        }

        public int RemainingLines { get; set; }
    }
}
