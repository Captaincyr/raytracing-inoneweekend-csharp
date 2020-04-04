using RayTracingInOneweekend.Core;
using System;
using System.IO;
using System.Linq;

namespace RayTracingInOneweekend
{
    class Program
    {
        const string OUTPUT_PATH = @"D:\Projets\raytracing-inoneweekend-csharp\Output\";

        static void Main(string[] args)
        {
            Console.WriteLine("Outputing image...");

            RayTracer image = new RayTracer();
            image.Progress += ProgressHandler;
            int numberOuptutFiles = Directory.GetFiles(OUTPUT_PATH).Count();
            image.Output(OUTPUT_PATH + "image_" + numberOuptutFiles + ".ppm");

            Console.WriteLine("Outputing image DONE");
        }

        static void ProgressHandler(object sender, ProgressArgs e)
        {
            Console.Write($"\r{e.Percentage}% ...                     ");
        } 
    }
}
