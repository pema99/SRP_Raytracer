namespace SimpleRaytracer
{
    public class Plane : Shape
    {
        public Vector3 Origin;
        public Vector3 Normal;

        public Plane(Vector3 Origin, Vector3 Normal, Vector3 Color)
            : base(Color)
        {
            this.Origin = Origin;
            this.Normal = Normal;
        }

        public override bool Intersect(Ray ray, out Vector3 hit, out Vector3 normal)
        {
            //Find prikprodukt mellem planens normal og stråles retning, hvis den er 0 (eller tæt på) er disse parallele og skærer ikke
            double cosTheta = Vector3.Dot(Normal, ray.Direction);
            if (cosTheta > 0.001 || cosTheta < -0.001)
            {
                //Find parameter til stråle
                double t = Vector3.Dot(Origin - ray.Origin, Normal) / cosTheta;

                //Tjek om skæringen er på den rigtige side af strålen
                if (t >= 0)
                {
                    hit = ray.Origin + ray.Direction * t;
                    normal = Normal;

                    //Hvis vi er på den "forkerte side" af planen, flip normalen
                    if (cosTheta > 0.001)
                    {
                        normal = -normal;
                    }

                    return true;
                }
            }

            //Hvis ingen skæring, returner falsk
            hit = Vector3.Zero;
            normal = Vector3.Zero;
            return false;
        }
    }
}
