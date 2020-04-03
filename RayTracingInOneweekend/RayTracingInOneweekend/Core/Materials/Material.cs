using RayTracingInOneweekend.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Materials
{
    public abstract class Material
    {
        public abstract bool Scatter(Ray rIn, HitRecord rec, ref Vec3 attenuation, ref Ray scattered);
    }
}
