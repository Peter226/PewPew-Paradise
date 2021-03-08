using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PewPew_Paradise.Maths
{
    public struct Vector2
    {
        public double x;
        public double y;


        public Vector2(double x = 0, double y = 0)
        {
            this.x = x;
            this.y = y;
        }
        public static Vector2 One
        {
            get { return new Vector2(1, 1); }
        }
        public static Vector2 Zero
        {
            get { return new Vector2(); }
        }

        public Vector2 Round()
        {
            return new Vector2(Math.Round(this.x),Math.Round(this.y));
        }




        public double Length
        {
            get { return Math.Sqrt(x * x + y * y); }
        }

        public double DistanceTo(Vector2 vector)
        {
            Vector2 d = vector - this;
            return Math.Sqrt(d.x * d.x + d.y * d.y);
        }

        public static double Distance(Vector2 a, Vector2 b)
        {
            Vector2 d = a - b;
            return Math.Sqrt(d.x * d.x + d.y * d.y);
        }



        public static Vector2 operator +(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x + b.x,a.y + b.y);
        }
        public static Vector2 operator -(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x - b.x, a.y - b.y);
        }
        public static Vector2 operator *(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x * b.x, a.y * b.y);
        }
        public static Vector2 operator /(Vector2 a, Vector2 b)
        {
            return new Vector2(a.x / b.x, a.y / b.y);
        }


        public static Vector2 operator +(Vector2 a, double b)
        {
            return new Vector2(a.x + b, a.y + b);
        }
        public static Vector2 operator -(Vector2 a, double b)
        {
            return new Vector2(a.x - b, a.y - b);
        }
        public static Vector2 operator *(Vector2 a, double b)
        {
            return new Vector2(a.x * b, a.y * b);
        }
        public static Vector2 operator /(Vector2 a, double b)
        {
            return new Vector2(a.x / b, a.y / b);
        }


        public static Vector2 operator +(double a, Vector2 b)
        {
            return new Vector2(a + b.x, a + b.y);
        }
        public static Vector2 operator -(double a, Vector2 b)
        {
            return new Vector2(a - b.x, a - b.y);
        }
        public static Vector2 operator *(double a, Vector2 b)
        {
            return new Vector2(a * b.x, a * b.y);
        }
        public static Vector2 operator /(double a, Vector2 b)
        {
            return new Vector2(a / b.x, a / b.y);
        }

    }
}
