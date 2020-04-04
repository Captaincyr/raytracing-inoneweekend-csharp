using RayTracingInOneweekend.Core.Materials;
using RayTracingInOneweekend.Core.Shapes;
using RayTracingInOneweekend.Utils;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

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
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (StreamWriter writer = new StreamWriter(path))
            {
                writer.WriteLine("P3");
                writer.WriteLine(IMAGE_WIDTH + " " + IMAGE_HEIGHT);
                writer.WriteLine("255");

                double aspectRatio = IMAGE_WIDTH / IMAGE_HEIGHT;
                Vec3 lookFrom = new Vec3(13, 2, 3);
                Vec3 lookAt = new Vec3(0, 0, 0);
                Vec3 vUp = new Vec3(0, 1, 0);
                double distanceToFocus = 10.0;
                double aperture = 0.1;

                Camera cam = new Camera(lookFrom, lookAt, vUp, 20, aspectRatio, aperture, distanceToFocus);

                HittableList world = RandomScene();
                Vec3[,] colors = new Vec3[IMAGE_HEIGHT, IMAGE_WIDTH];

                int linesRemaining = IMAGE_HEIGHT;

                Parallel.For(0, IMAGE_HEIGHT, j =>
                {
                    // We handle each pixel of the line on a different thread to speed up the process
                    Parallel.For(0, IMAGE_WIDTH, i =>
                    {
                        Vec3 color = new Vec3();
                        for (int s = 0; s < SAMPLES_PER_PIXEL; ++s)
                        {
                            double u = (i + RayTracerUtils.RandomDouble()) / IMAGE_WIDTH;
                            double v = (j + RayTracerUtils.RandomDouble()) / IMAGE_HEIGHT;
                            Ray r = cam.GetRay(u, v);
                            color += RayColor(r, world, MAX_DEPTH);
                        }

                        lock (colors)
                        {
                            colors[j, i] = color;
                        }

                    });

                    double percentage = 100.0 - (double)--linesRemaining / IMAGE_HEIGHT * 100.0;
                    Progress(this, new ProgressArgs(percentage));
                });

                // Output the colors in the file
                for (int j = IMAGE_HEIGHT - 1; j >= 0; --j)
                {
                    for (int i = 0; i < IMAGE_WIDTH; ++i)
                    {
                        colors[j, i].WriteColor(writer, SAMPLES_PER_PIXEL);
                    }
                }
            }

            sw.Stop();

            Console.WriteLine();
            Console.WriteLine($"Time: {sw.Elapsed.Minutes}:{sw.Elapsed.Seconds:00}");
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

        private HittableList RandomScene()
        {
            HittableList world = new HittableList();

            world.Objects.Add(new Sphere(new Vec3(0, -1000, 0), 1000, new Lambertian(new Vec3(0.5, 0.5, 0.5))));

            int i = 1;
            Vec3 differenceCenter = new Vec3(4, 0.2, 0);
            for (int a = -11; a < 11; a++)
            {
                for (int b = -11; b < 11; b++)
                {
                    double chooseMat = RayTracerUtils.RandomDouble();
                    Vec3 center = new Vec3(a + 0.9 * RayTracerUtils.RandomDouble(), 0.2, b + 0.9 * RayTracerUtils.RandomDouble());
                    if ((center - differenceCenter).Length > 0.9)
                    {
                        if (chooseMat < 0.8)
                        {
                            //diffuse
                            Vec3 albedo = Vec3.Random() * Vec3.Random();
                            world.Objects.Add(new Sphere(center, 0.2, new Lambertian(albedo)));
                        }
                        else if (chooseMat < 0.95)
                        {
                            //metal
                            Vec3 albedo = Vec3.Random(0.5, 1);
                            double fuzz = RayTracerUtils.RandomDouble(0, 0.5);
                            world.Objects.Add(new Sphere(center, 0.2, new Metal(albedo, fuzz)));
                        }
                        else
                        {
                            //glass
                            world.Objects.Add(new Sphere(center, 0.2, new Dielectric(1.5)));
                        }
                    }
                }
            }

            world.Objects.Add(new Sphere(new Vec3(0, 1, 0), 1.0, new Dielectric(1.5)));

            world.Objects.Add(new Sphere(new Vec3(-4, 1, 0), 1.0, new Lambertian(new Vec3(0.4, 0.2, 0.1))));

            world.Objects.Add(new Sphere(new Vec3(4, 1, 0), 1.0, new Metal(new Vec3(0.7, 0.6, 0.5), 0.0)));

            return world;
        }
    }
}
