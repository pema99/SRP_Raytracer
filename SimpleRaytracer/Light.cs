namespace SimpleRaytracer
{
    public struct Light
    {
        public Vector3 Origin;
        public double Intensity;

        public Light(Vector3 Origin, double Intensity)
        {
            this.Origin = Origin;
            this.Intensity = Intensity;
        }
    }
}
