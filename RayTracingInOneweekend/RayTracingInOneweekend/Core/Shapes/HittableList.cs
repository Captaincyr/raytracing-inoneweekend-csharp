using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core.Shapes
{
    public class HittableList : Hittable
    {
        public HittableList() {}
        public HittableList(Hittable hittable)
        {
            Objects.Add(hittable);
        }

        public List<Hittable> Objects { get; set; } = new List<Hittable>();

        public override bool Hit(Ray r, double tMin, double tMax, ref HitRecord rec)
        {
            HitRecord tempRec = new HitRecord();
            bool hitAnything = false;
            double closestSoFar = tMax;

            foreach (Hittable hittable in Objects)
            {
                if (hittable.Hit(r, tMin, closestSoFar, ref tempRec))
                {
                    hitAnything = true;
                    closestSoFar = tempRec.T;
                    rec = tempRec;
                }
            }

            return hitAnything;
        }
    }
}
