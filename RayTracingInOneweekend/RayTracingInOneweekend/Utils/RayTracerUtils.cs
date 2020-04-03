using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Utils
{
    public class RayTracerUtils
    {
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

        public static double Schlick(double cosine, double refIdx)
        {
            double r0 = (1 - refIdx) / (1 + refIdx);
            r0 = r0 * r0;
            return r0 + (1 - r0) * Math.Pow(1 - cosine, 5);
        }
    }
}
