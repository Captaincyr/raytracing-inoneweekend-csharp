using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core
{
    public class ProgressArgs : EventArgs
    {
        public ProgressArgs(double percentage)
        {
            Percentage = percentage;
        }

        public double Percentage { get; set; }
    }
}
