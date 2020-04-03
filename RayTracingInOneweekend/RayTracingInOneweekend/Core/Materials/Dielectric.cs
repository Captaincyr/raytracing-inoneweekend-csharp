using RayTracingInOneweekend.Core.Shapes;
using RayTracingInOneweekend.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Materials
{
    public class Dielectric : Material
    {
        public double RefIdx { get; set; }

        public Dielectric(double ri)
        {
            RefIdx = ri;
        }

        public override bool Scatter(Ray rIn, HitRecord rec, ref Vec3 attenuation, ref Ray scattered)
        {
            attenuation = new Vec3(1, 1, 1);
            double etaIOverEtaT = rec.FrontFace ? (1.0 / RefIdx) : RefIdx;

            Vec3 unitDirection = Vec3.UnitVector(rIn.Direction);
            double cosTheta = Math.Min(Vec3.Dot(-unitDirection, rec.Normal), 1.0);
            double sinTheta = Math.Sqrt(1.0 - cosTheta * cosTheta);

            if (etaIOverEtaT * sinTheta > 1.0)
            {
                Vec3 reflected = Vec3.Reflect(unitDirection, rec.Normal);
                scattered = new Ray(rec.P, reflected);
                return true;
            }

            double reflectProb = RayTracerUtils.Schlick(cosTheta, etaIOverEtaT);
            if (RayTracerUtils.RandomDouble() < reflectProb)
            {
                Vec3 reflected = Vec3.Reflect(unitDirection, rec.Normal);
                scattered = new Ray(rec.P, reflected);
                return true;
            }

            Vec3 refracted = Vec3.Refract(unitDirection, rec.Normal, etaIOverEtaT);
            scattered = new Ray(rec.P, refracted);
            return true;
        }
    }
}
