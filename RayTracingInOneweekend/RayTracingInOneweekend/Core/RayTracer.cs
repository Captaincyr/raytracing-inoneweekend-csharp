using RayTracingInOneweekend.Core.Materials;
using RayTracingInOneweekend.Core.Shapes;
using RayTracingInOneweekend.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace RayTracingInOneweekend.Core
{
    public class RayTracer
    {
        const int IMAGE_WIDTH = 200;
        const int IMAGE_HEIGHT = 100;
        const int SAMPLES_PER_PIXEL = 100;
        const int MAX_DEPTH = 50;

        public event EventHandler<ProgressArgs> Progress;


        public void Output(string path)
        {
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("P3");
                writer.WriteLine(IMAGE_WIDTH + " " + IMAGE_HEIGHT);
                writer.WriteLine("255");

                Camera cam = new Camera();

                HittableList world = new HittableList();
                world.Objects.Add(new Sphere(new Vec3(0, 0, -1), 0.5, new Lambertian(new Vec3(0.1, 0.2, 0.5))));
                world.Objects.Add(new Sphere(new Vec3(0, -100.5, -1), 100, new Lambertian(new Vec3(0.8, 0.8, 0.0))));
                world.Objects.Add(new Sphere(new Vec3(1, 0, -1), 0.5, new Metal(new Vec3(0.8, 0.6, 0.2), 0.3)));
                world.Objects.Add(new Sphere(new Vec3(-1, 0, -1), 0.5, new Dielectric(1.5)));
                world.Objects.Add(new Sphere(new Vec3(-1, 0, -1), -0.45, new Dielectric(1.5)));


                for (int j = IMAGE_HEIGHT - 1; j >= 0 ; --j)
                {
                    Progress(this, new ProgressArgs(j + 1));
                    for (int i = 0; i < IMAGE_WIDTH; ++i)
                    {
                        Vec3 color = new Vec3();
                        for (int s = 0; s < SAMPLES_PER_PIXEL; ++s)
                        {
                            double u = (i + RayTracerUtils.RandomDouble()) / IMAGE_WIDTH;
                            double v = (j + RayTracerUtils.RandomDouble()) / IMAGE_HEIGHT;
                            Ray r = cam.GetRay(u, v);
                            color += RayColor(r, world, MAX_DEPTH);
                        }
                        color.WriteColor(writer, SAMPLES_PER_PIXEL);
                    }
                }
            }
        }

        private Vec3 RayColor(Ray r, Hittable world, int depth)
        {
            HitRecord rec = new HitRecord();

            // If we've exceeded the ray bounce limit, no more light is gathered
            if (depth <= 0)
                return new Vec3();

            if (world.Hit(r, 0.001, Double.PositiveInfinity, ref rec))
            {
                Ray scattered = new Ray();
                Vec3 attenuation = new Vec3();
                if (rec.Material.Scatter(r, rec, ref attenuation, ref scattered))
                {
                    return attenuation * RayColor(scattered, world, depth - 1);
                }
                return new Vec3();
            }
            Vec3 unitDirection = Vec3.UnitVector(r.Direction);
            double t = 0.5 * (unitDirection.Y + 1.0);
            return (1.0 - t) * new Vec3(1.0, 1.0, 1.0) + t * new Vec3(0.5, 0.7, 1.0);
        }
    }
}
