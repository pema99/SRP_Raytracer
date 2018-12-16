using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;

namespace SimpleRaytracer
{
    class Program
    {
        public static void Main(string[] args) => new Program().Start();

        int width;
        int height;
        Vector3[,] framebuffer;
        List<Shape> scene;
        List<Light> lights;

        double cameraPlaneHeight;
        double aspectRatio;

        public void Start()
        {
            //Initialiser framebuffer
            width = 600;
            height = 400;
            framebuffer = new Vector3[width, height];

            //Beregn aspektratio og den halve højde på skærmplanet
            cameraPlaneHeight = Math.Tan(75 / 2 * Math.PI / 180.0);
            aspectRatio = (double)width / (double)height;

            //Fyld scene
            scene = new List<Shape>();
            scene.Add(new Sphere(new Vector3(0, 0, 3), 0.5, new Vector3(1, 1, 1)));
            scene.Add(new Sphere(new Vector3(-1, 0, 2), 0.5, new Vector3(1, 0, 0)));
            scene.Add(new Sphere(new Vector3(1, 0, 2), 0.5, new Vector3(0, 0, 1)));
            scene.Add(new Plane(new Vector3(0, -0.5, 0), new Vector3(0, 1, 0), new Vector3(1, 1, 1)));
            scene.Add(new Plane(new Vector3(0, 2, 0), new Vector3(0, -1, 0), new Vector3(1, 1, 1)));
            scene.Add(new Plane(new Vector3(0, 0, 4.5), new Vector3(0, 0, -1), new Vector3(1, 1, 1)));
            scene.Add(new Plane(new Vector3(-2, 0, 0), new Vector3(1, 0, 0), new Vector3(0, 1, 0)));
            scene.Add(new Plane(new Vector3(2, 0, 0), new Vector3(-1, 0, 0), new Vector3(0, 1, 0)));

            //Lyskilder
            lights = new List<Light>();
            lights.Add(new Light(new Vector3(0, 1, 1), 2));

            //Loop gennem alle pixels på skærmen
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Generer primærstråle
                    Vector3 rayDirection = new Vector3(
                        (2.0 * ((double)x / width) - 1.0) * cameraPlaneHeight * aspectRatio, 
                        (1.0 - 2.0 * ((double)y / height)) * cameraPlaneHeight, 1);
                    rayDirection.Normalize();

                    //Bestem farve
                    framebuffer[x, y] = Trace(new Ray(Vector3.Zero, rayDirection));
                }
            }

            WriteToImage("render.png");
        }
        
        //Følg en primærstråles sti
        Vector3 Trace(Ray ray)
        {
            //Hvis skæring fandt sted
            if (Raycast(ray, out Shape closest, out Vector3 closestHit, out Vector3 closestNormal))
            {
                //Loop gennem alle lys og summer deres bidrag
                Vector3 finalColor = Vector3.Zero;
                foreach (Light light in lights)
                {
                    //Kast skyggestråle
                    Raycast(new Ray(light.Origin, Vector3.Normalize(closestHit - light.Origin)), out Shape shadow, out Vector3 shadowHit, out Vector3 shadowNormal);

                    //Hvis skyggestrålen rammer et andet objekt end det vi kigger på, må punktet være skyggelagt
                    if (shadow == closest)
                    {
                        //Beregn farve med shading
                        Vector3 lightDirection = light.Origin - closestHit;
                        double distanceSquared = lightDirection.LengthSquared();
                        lightDirection.Normalize();
                        finalColor += closest.Color * Math.Max(Vector3.Dot(closestNormal, lightDirection), 0) / distanceSquared * light.Intensity;
                    }
                }
                return finalColor;
            }

            //Hvis vi ramte ingenting, returner sort baggrundsfarve
            return Vector3.Zero;
        }

        //Find nærmeste skæring i scene, hvis nogen
        bool Raycast(Ray ray, out Shape closest, out Vector3 closestHit, out Vector3 closestNormal)
        {
            //Minimum distance indtil videre
            double minDistance = double.MaxValue;

            //Info om tætteste skæring
            closest = null;
            closestHit = Vector3.Zero;
            closestNormal = Vector3.Zero;

            //Loop gennem alle sceneobjekter
            foreach (Shape S in scene)
            {
                //Hvis stråler skærer objektet
                if (S.Intersect(ray, out Vector3 hit, out Vector3 normal))
                {
                    //... Og distancen er mindre end den tidligere mindste
                    if ((hit - ray.Origin).Length() < minDistance)
                    {
                        //Sæt denne skæring til at være den nye nærmeste skæring
                        minDistance = (hit - ray.Origin).Length();
                        closest = S;
                        closestHit = hit;
                        closestNormal = normal;
                    }
                }
            }
            //Return falsk hvis intet ramt
            return closest != null;
        }

        //Gem framebuffer i billede
        public void WriteToImage(string Path)
        {
            Bitmap bmp = new Bitmap(width, height);

            //Loop gennem hver pixel i framebuffer
            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    //Set pixelens farve, med tonemapping, Cout = Cin / (Cin + 1)
                    bmp.SetPixel(x, y, Color.FromArgb(
                        (int)(framebuffer[x, y].X / (framebuffer[x, y].X + 1) * 255),
                        (int)(framebuffer[x, y].Y / (framebuffer[x, y].Y + 1) * 255),
                        (int)(framebuffer[x, y].Z / (framebuffer[x, y].Z + 1) * 255))
                    );
                }
            }
            bmp.Save(Path);
            Process.Start(Path);
        }
    }
}
