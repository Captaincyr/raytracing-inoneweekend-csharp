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

            double r = Math.Sqrt(scale * e[0]);
            double g = Math.Sqrt(scale * e[1]);
            double b = Math.Sqrt(scale * e[2]);
            int ri = (int)(256 * RayTracerUtils.Clamp(r, 0.0, 0.999));
            int gi = (int)(256 * RayTracerUtils.Clamp(g, 0.0, 0.999));
            int bi = (int)(256 * RayTracerUtils.Clamp(b, 0.0, 0.999));
            writer.WriteLine($"{ri} {gi} {bi}");
        }

        #region Utils Methods

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

        public static Vec3 Random()
        {
            return new Vec3(RayTracerUtils.RandomDouble(), RayTracerUtils.RandomDouble(), RayTracerUtils.RandomDouble());
        }

        public static Vec3 Random(double min, double max)
        {
            return new Vec3(RayTracerUtils.RandomDouble(min, max), RayTracerUtils.RandomDouble(min, max), RayTracerUtils.RandomDouble(min, max));
        }

        public static Vec3 RandomInUnitSphere()
        {
            Vec3 p;
            do
            {
                p = Random(-1, 1);
            } while (p.LengthSquared >= 1);

            return p;
        }

        public static Vec3 RandomInHemisphere(Vec3 normal)
        {
            Vec3 inUnitSphere = RandomInUnitSphere();
            if (Dot(inUnitSphere, normal) > 0.0)
            {
                return inUnitSphere;
            }
            else
            {
                return -inUnitSphere;
            }
        }

        public static Vec3 RandomUnitVector()
        {
            double a = RayTracerUtils.RandomDouble(0, 2 * Math.PI);
            double z = RayTracerUtils.RandomDouble(-1, 1);
            double r = Math.Sqrt(1 - z * z);
            return new Vec3(r * Math.Cos(a), r * Math.Sin(a), z);
        }

        public static Vec3 RandomInUnitDisk()
        {
            Vec3 p;
            do
            {
                p = new Vec3(RayTracerUtils.RandomDouble(-1, 1), RayTracerUtils.RandomDouble(-1, 1), 0);
            } while (p.LengthSquared >= 1);

            return p;
        }

        public static Vec3 Reflect(Vec3 v, Vec3 n)
        {
            return v - 2 * Dot(v, n) * n;
        }

        public static Vec3 Refract(Vec3 uv, Vec3 n, double etaIOverEta)
        {
            double cosTheta = Dot(-uv, n);
            Vec3 rOutParallel = etaIOverEta * (uv + cosTheta * n);
            Vec3 rOutPerp = -Math.Sqrt(1.0 - rOutParallel.LengthSquared) * n;
            return rOutParallel + rOutPerp;
        }

        #endregion

    }
}
