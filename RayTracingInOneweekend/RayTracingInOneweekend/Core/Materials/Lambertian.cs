using RayTracingInOneweekend.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Materials
{
    public class Lambertian : Material
    {
        public Vec3 Albedo { get; set; }

        public Lambertian(Vec3 albedo)
        {
            Albedo = albedo;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 scatterDirection = rec.Normal + Vec3.RandomUnitVector();
            scattered = new Ray(rec.P, scatterDirection);
            attenuation = Albedo;
            return true;
        }
    }
}
