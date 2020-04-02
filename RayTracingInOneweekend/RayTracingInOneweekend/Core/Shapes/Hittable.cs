using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Shapes
{
    public abstract class Hittable
    {
        public abstract bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec);
    }

    public struct HitRecord
    {
        public double T;
        public Vec3 P;
        public Vec3 Normal;
        public bool FrontFace;

        public void SetFaceNormal(Ray r, Vec3 outwardNormal)
        {
            FrontFace = Vec3.Dot(r.Direction, outwardNormal) < 0;
            Normal = FrontFace ? outwardNormal : -outwardNormal;
        }
    }
}
