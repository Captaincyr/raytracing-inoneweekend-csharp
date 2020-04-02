using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Utils
{
    public class RayTracerUtils
    {
        public const double RAND_MAX = Double.MaxValue;

        public static double DegreesToRadians(double degrees)
        {
            return (degrees * Math.PI) / 180;
        }

        public static double RandomDouble()
        {
            Random r = new Random();
            return r.NextDouble();
        }

        public static double RandomDouble(double min, double max)
        {
            Random r = new Random();
            return min + (max - min) * r.NextDouble();
        }

        public static double Clamp(double x, double min, double max)
        {
            if (x < min) return min;
            if (x > max) return max;
            return x;
        }
    }
}
