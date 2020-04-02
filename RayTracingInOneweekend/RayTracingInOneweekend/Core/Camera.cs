using System;
using System.Collections.Generic;
using System.Text;

namespace RayTracingInOneweekend.Core
{
    public class Camera
    {
        public Vec3 Origin { get; set; }
        public Vec3 LowerLeftCorner { get; set; }
        public Vec3 Horizontal { get; set; }
        public Vec3 Vertical { get; set; }

        public Camera()
        {
            LowerLeftCorner = new Vec3(-2.0, -1.0, -1.0);
            Horizontal = new Vec3(4.0, 0.0, 0.0);
            Vertical = new Vec3(0.0, 2.0, 0.0);
            Origin = new Vec3();
        }

        public Ray GetRay(double u, double v)
        {
            return new Ray(Origin, LowerLeftCorner + u * Horizontal + v * Vertical - Origin);
        }
    }
}
