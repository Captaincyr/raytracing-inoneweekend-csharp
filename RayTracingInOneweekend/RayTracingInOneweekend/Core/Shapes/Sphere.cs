using RayTracingInOneweekend.Core.Materials;
using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Shapes
{
    public class Sphere : Hittable
    {
        public Vec3 Center { get; set; }
        public double Radius { get; set; }
        public Material Material { get; set; }

        public Sphere(Vec3 center, double radius, Material material)
        {
            Center = center;
            Radius = radius;
            Material = material;
        }

        public override bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            Vec3 oc = r.Origin - Center;
            double a = r.Direction.LengthSquared;
            double halfB = Vec3.Dot(oc, r.Direction);
            double c = oc.LengthSquared - Radius * Radius;
            double discriminant = halfB * halfB - a * c;
            if (discriminant > 0)
            {
                double root = Math.Sqrt(discriminant);
                double temp = (-halfB - root) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.At(rec.T);
                    Vec3 outwardNormal = (rec.P - Center) / Radius;
                    rec.SetFaceNormal(r, outwardNormal);
                    rec.Material = Material;
                    return true;
                }
                temp = (-halfB + root) / a;
                if (temp < tMax && temp > tMin)
                {
                    rec.T = temp;
                    rec.P = r.At(rec.T);
                    Vec3 outwardNormal = (rec.P - Center) / Radius;
                    rec.SetFaceNormal(r, outwardNormal);
                    rec.Material = Material;
                    return true;
                }
            }
            return false;
        }
    }
}
