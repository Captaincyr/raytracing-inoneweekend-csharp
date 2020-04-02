using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core
{
    public class Ray
    {
        public Vec3 Origin { get; set; }
        public Vec3 Direction { get; set; }

        public Ray() { }
        public Ray(Vec3 origin, Vec3 direction)
        {
            Origin = origin;
            Direction = direction;
        }

        public Vec3 At(double t)
        {
            return Origin + t * Direction;
        }
    }
}
