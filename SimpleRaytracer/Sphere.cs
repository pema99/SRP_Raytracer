using System;

namespace SimpleRaytracer
{
    public class Sphere : Shape
    {
        public Vector3 Origin;
        public double Radius;

        public Sphere(Vector3 Position, double Radius, Vector3 Color)
            : base(Color)
        {
            this.Origin = Position;
            this.Radius = Radius;
        }

        public override bool Intersect(Ray ray, out Vector3 hit, out Vector3 normal)
        {
            normal = Vector3.Zero;
            hit = Vector3.Zero;

            double a = 1; //Dette er 1 fordi mine retningsvektorer er normaliserede
            double b = 2 * (-Vector3.Dot(Origin, ray.Direction) + Vector3.Dot(ray.Origin, ray.Direction));
            double c = -Math.Pow(Radius, 2) + Vector3.Dot(ray.Origin - Origin, ray.Origin - Origin);

            //Tjek hvis der er nogen skæring ved at løse andengradsligning
            if (SolveEquation(a, b, c, out double X0, out double X1))
            {
                //Kig kun på den nærmeste skæring
                double t;
                if (X0 < X1)
                {
                    t = X0;
                }
                else
                {
                    t = X1;
                }

                //Skæring "bag" stråle, ignorer
                if (t < 0)
                {
                    return false;
                }

                //Find skæringpunktets koordinater
                hit = ray.Origin + ray.Direction * t;
                normal = Vector3.Normalize(hit - Origin);
                return true;
            }

            //Intet ramt
            return false;
        }

        //Løs andengradsligning
        private bool SolveEquation(double a, double b, double c, out double x0, out double x1)
        {
            x0 = 0;
            x1 = 0;

            //Find diskriminant
            double discriminant = b * b - 4 * a * c;

            //1 løsning
            if (discriminant == 0)
            {
                x0 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                return true;
            }

            //2 løsning
            else if (discriminant > 0)
            {
                x0 = (-b + Math.Sqrt(discriminant)) / (2 * a);
                x1 = (-b - Math.Sqrt(discriminant)) / (2 * a);
                return true;
            }

            //Ingen løsning fundet, returner falsk
            return false;
        }
    }
}
