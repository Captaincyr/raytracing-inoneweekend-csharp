using RayTracingInOneweekend.Core.Shapes;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Materials
{
    public class Metal : Material
    {
        public double Fuzz { get; set; }
        public Vec3 Albedo { get; set; }

        public Metal(Vec3 albedo, double f)
        {
            Albedo = albedo;
            Fuzz = f < 1 ? f : 1;
        }


        public override bool Scatter(Ray rIn, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            Vec3 reflected = Vec3.Reflect(Vec3.UnitVector(rIn.Direction), rec.Normal);
            scattered = new Ray(rec.P, reflected + Fuzz * Vec3.RandomInUnitSphere());
            attenuation = Albedo;
            return Vec3.Dot(scattered.Direction, rec.Normal) > 0;
        }
    }
}
