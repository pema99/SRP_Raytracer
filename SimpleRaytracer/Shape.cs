namespace SimpleRaytracer
{
    public abstract class Shape
    {
        public Vector3 Color;

        public Shape(Vector3 Color)
        {
            this.Color = Color;
        }

        public abstract bool Intersect(Ray ray, out Vector3 hit, out Vector3 hormal);
    }
}
