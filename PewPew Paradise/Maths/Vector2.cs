using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using PewPew_Paradise.GameLogic;
namespace PewPew_Paradise.Maths
{
    /// <summary>
    /// Vector2 point coordinate used for sprite positions, scaling, etc...
    /// </summary>
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


        public override string ToString()
        {
            return $"({x}; {y})";
        }

        /// <summary>
        /// Lerp.
        /// </summary>
        public static Vector2 Lerp(Vector2 A, Vector2 B, double t)
        {
            return B * t + A * (1 - t);
        }


        /// <summary>
        /// Rounded vector
        /// </summary>
        /// <returns></returns>
        public Vector2 Round()
        {
            return new Vector2(Math.Round(this.x),Math.Round(this.y));
        }

        /// <summary>
        /// Floored vector
        /// </summary>
        /// <returns></returns>
        public Vector2 Floor()
        {
            return new Vector2(Math.Floor(this.x), Math.Floor(this.y));
        }


        /// <summary>
        /// Ceiled vector
        /// </summary>
        /// <returns></returns>
        public Vector2 Ceil()
        {
            return new Vector2(Math.Ceiling(this.x), Math.Ceiling(this.y));
        }


        /// <summary>
        /// Rounded vector to the game's pixel art resolution
        /// </summary>
        public Vector2 RoundToPixels()
        {
            return ((this * GameManager.GameResolution / GameManager.GameUnitSize).Round() * GameManager.GameUnitSize / GameManager.GameResolution);
        }


        /// <summary>
        /// Normal of the vector
        /// </summary>
        /// <returns></returns>
        public Vector2 Normalize()
        {
            double length = Length();
            if (length == 0.0) {
                return Vector2.One;
            }
            return this / length;
        }

        /// <summary>
        /// Length of the Vector
        /// </summary>
        /// <returns></returns>
        public double Length()
        {
            return Math.Sqrt(x * x + y * y);
        }
        
        /// <summary>
        /// Absolute
        /// </summary>
        /// <returns></returns>
        public Vector2 Abs()
        {
            return new Vector2(Math.Abs(this.x),Math.Abs(this.y));
        }


        /// <summary>
        /// Distance to other Vector point
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        public double DistanceTo(Vector2 vector)
        {
            Vector2 d = vector - this;
            return Math.Sqrt(d.x * d.x + d.y * d.y);
        }

        /// <summary>
        /// Distance of two Vector points
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static double Distance(Vector2 a, Vector2 b)
        {
            Vector2 d = a - b;
            return Math.Sqrt(d.x * d.x + d.y * d.y);
        }

        //Cast to point and back
        public static implicit operator Point(Vector2 vec) => new Point(vec.x,vec.y);
        public static implicit operator Vector2(Point point) => new Vector2(point.X, point.Y);
        //Cast to vector and back
        public static implicit operator Vector(Vector2 vec) => new Vector(vec.x, vec.y);
        public static implicit operator Vector2(Vector vec) => new Vector2(vec.X, vec.Y);
        //Cast to size and back
        public static explicit operator Size(Vector2 vec) => new Size(vec.x,vec.y);
        public static explicit operator Vector2(Size s) => new Vector2(s.Width, s.Height);

        //Operators for doing Math
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
