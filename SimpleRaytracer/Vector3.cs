using System;

namespace SimpleRaytracer
{
    public struct Vector3
    {
        private static Vector3 zero = new Vector3(0, 0, 0);
        public static Vector3 Zero
        {
            get { return zero; }
        }

        public double X;
        public double Y;
        public double Z;

        public Vector3(double x, double y, double z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public static Vector3 Cross(Vector3 vector1, Vector3 vector2)
        {
            var x = vector1.Y * vector2.Z - vector2.Y * vector1.Z;
            var y = -(vector1.X * vector2.Z - vector2.X * vector1.Z);
            var z = vector1.X * vector2.Y - vector2.X * vector1.Y;
            return new Vector3(x, y, z);
        }

        public static double Dot(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X * vector2.X + vector1.Y * vector2.Y + vector1.Z * vector2.Z;
        }

        public double Length()
        {
            return Math.Sqrt(LengthSquared());
        }

        public double LengthSquared()
        {
            return (X * X) + (Y * Y) + (Z * Z);
        }

        public void Normalize()
        {
            double length = Length();
            X /= length;
            Y /= length;
            Z /= length;
        }

        public static Vector3 Normalize(Vector3 value)
        {
            return value / value.Length();
        }

        public static bool operator ==(Vector3 vector1, Vector3 vector2)
        {
            return vector1.X == vector2.X
                && vector1.Y == vector2.Y
                && vector1.Z == vector2.Z;
        }

        public static bool operator !=(Vector3 vector1, Vector3 vector2)
        {
            return !(vector1 == vector2);
        }

        public static Vector3 operator +(Vector3 vector1, Vector3 vector2)
        {
            vector1.X += vector2.X;
            vector1.Y += vector2.Y;
            vector1.Z += vector2.Z;
            return vector1;
        }

        public static Vector3 operator -(Vector3 value)
        {
            value = new Vector3(-value.X, -value.Y, -value.Z);
            return value;
        }

        public static Vector3 operator -(Vector3 vector1, Vector3 vector2)
        {
            vector1.X -= vector2.X;
            vector1.Y -= vector2.Y;
            vector1.Z -= vector2.Z;
            return vector1;
        }

        public static Vector3 operator *(Vector3 vector1, Vector3 vector2)
        {
            vector1.X *= vector2.X;
            vector1.Y *= vector2.Y;
            vector1.Z *= vector2.Z;
            return vector1;
        }

        public static Vector3 operator *(Vector3 value, double scale)
        {
            value.X *= scale;
            value.Y *= scale;
            value.Z *= scale;
            return value;
        }

        public static Vector3 operator *(double scale, Vector3 value)
        {
            value.X *= scale;
            value.Y *= scale;
            value.Z *= scale;
            return value;
        }

        public static Vector3 operator /(Vector3 vector1, Vector3 vector2)
        {
            vector1.X /= vector2.X;
            vector1.Y /= vector2.Y;
            vector1.Z /= vector2.Z;
            return vector1;
        }

        public static Vector3 operator /(Vector3 vector1, double divider)
        {
            vector1.X /= divider;
            vector1.Y /= divider;
            vector1.Z /= divider;
            return vector1;
        }
    }
}
