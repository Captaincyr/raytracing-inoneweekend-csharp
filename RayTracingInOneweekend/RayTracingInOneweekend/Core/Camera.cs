using RayTracingInOneweekend.Utils;
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
        public Vec3 U { get; set; }
        public Vec3 V { get; set; }
        public Vec3 W { get; set; }
        public double LensRadius { get; set; }

        /// <param name="vFov">Top to bottom, in degrees</param>
        public Camera(Vec3 lookFrom, Vec3 lookAt, Vec3 vUp, double vFov, double aspect, double aperture, double focusDistance)
        {
            Origin = lookFrom;
            LensRadius = aperture / 2;

            double theta = RayTracerUtils.DegreesToRadians(vFov);
            double halfHeight = Math.Tan(theta / 2);
            double halfWidth = aspect * halfHeight;
            W = Vec3.UnitVector(lookFrom - lookAt);
            U = Vec3.UnitVector(Vec3.Cross(vUp, W));
            V = Vec3.Cross(W, U);

            LowerLeftCorner = Origin - halfWidth * focusDistance * U - halfHeight * focusDistance * V - focusDistance * W;
            Horizontal = 2 * halfWidth * focusDistance * U;
            Vertical = 2 * halfHeight * focusDistance * V;
        }

        public Ray GetRay(double s, double t)
        {
            Vec3 rd = LensRadius * Vec3.RandomInUnitDisk();
            Vec3 offset = rd.X* U  +  rd.Y * V;

            return new Ray(Origin + offset, LowerLeftCorner + s * Horizontal + t * Vertical - Origin - offset);
        }
    }
}
