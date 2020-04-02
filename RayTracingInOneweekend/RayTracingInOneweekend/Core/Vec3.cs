using RayTracingInOneweekend.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RayTracingInOneweekend.Core
{
    public class Vec3
    {
        public double[] e = new double[3];

        public double X => e[0];
        public double Y => e[1];
        public double Z => e[2];

        public Vec3() : this(0.0, 0.0, 0.0) { }
        public Vec3(double e0, double e1, double e2)
        {
            e[0] = e0;
            e[1] = e1;
            e[2] = e2;
        }

        #region operator overload

        public static Vec3 operator -(Vec3 v)
        {
            return new Vec3(-v.e[0], -v.e[1], -v.e[2]);
        }

        public static Vec3 operator -(Vec3 u, Vec3 v)
        {
            return new Vec3(u[0] - v[0], u[1] - v[1], u[2] - v[2]);
        }

        public static Vec3 operator +(Vec3 u, Vec3 v)
        {
            return new Vec3(u[0] + v[0], u[1] + v[1], u[2] + v[2]);
        }

        public static Vec3 operator *(Vec3 u, Vec3 v)
        {
            return new Vec3(u[0] * v[0], u[1] * v[1], u[2] * v[2]);
        }

        public static Vec3 operator *(double t, Vec3 v)
        {
            return new Vec3(t * v[0], t * v[1], t * v[2]);
        }

        public static Vec3 operator /(Vec3 u, Vec3 v)
        {
            return new Vec3(u[0] / v[0], u[1] / v[1], u[2] / v[2]);
        }

        public static Vec3 operator /(Vec3 v, double t)
        {
            return new Vec3(v[0] / t, v[1] / t, v[2] / t);
        }

        public double this[int i]
        {
            get { return e[i]; }
            set { e[i] = value; }
        }


        #endregion

        public double LengthSquared => e[0] * e[0] + e[1] * e[1] + e[2] * e[2];

        public double Length => Math.Sqrt(this.LengthSquared);

        public void WriteColor(StreamWriter writer, int samplesPerPixel)
        {
            double scale = 1.0 / samplesPerPixel;

            double r = scale * e[0];
            double g = scale * e[1];
            double b = scale * e[2];
            int ri = (int)(256 * RayTracerUtils.Clamp(r, 0.0, 0.999));
            int gi = (int)(256 * RayTracerUtils.Clamp(g, 0.0, 0.999));
            int bi = (int)(256 * RayTracerUtils.Clamp(b, 0.0, 0.999));
            writer.WriteLine($"{ri} {gi} {bi}");
        }

        public static double Dot(Vec3 u, Vec3 v)
        {
            return u[0] * v[0] + u[1] * v[1] + u[2] * v[2];
        }

        public static Vec3 Cross(Vec3 u, Vec3 v)
        {
            return new Vec3(u[1] * v[2] - u[2] * v[1],
                            u[2] * v[0] - u[0] * v[2],
                            u[0] * v[1] - u[1] * v[0]);
        }

        public static Vec3 UnitVector(Vec3 v)
        {
            return v / v.Length;
        }

    }
}
