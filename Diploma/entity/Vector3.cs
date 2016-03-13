using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Vector3:IVector
    {
        // поля
        private double x;
        private double y;
        private double z;

        public Vector3()
        {
            x = 0; y = 0; z = 0;
        }
        public Vector3(double x, double y, double z)
        { 
            this.x = x; this.y = y; this.z = z; 
        }
        public Vector3(IVector v)
        {
            if (v.length == 3)
            {
                x = v[0];
                y = v[1];
                z = v[2];
            }
            else
            {
                Console.WriteLine("Error while creating Vector3: amount of params isnot 3!");
            }
        }

        double IVector.this[int index]
        {
            get 
            {
                switch(index)
                {
                    case (0): return x;
                    case (1): return y;
                    case (2): return z;
                    default : return 0;
                }
            }
            set
            {
                switch (index)
                {
                    case (0): x = value; break;
                    case (1): y = value; break;
                    case (2): z = value; break;
                    default:  break;
                }
            }
        }
        void IVector.print()
        {
            try
            {
                System.Console.WriteLine("Vector3: [{0}, {1}, {2}]", x, y, z);
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Empty Vector3!");
            }
        }
        int IVector.length
        {
            get { return 3; }
        }

        double IVector.norm()
        {
            return Math.Sqrt(x*x + y*y + z*z);
        }

        public double X{ 
            get
            {
                return x;
            }
            set
            {
                x = value;
            }
        }
        public double Y 
        {   
            get
            {
                return y;
            }
            set
            {
                y = value;
            }
        }
        public double Z
        {
            get
            {
                return z;
            }
            set
            {
                z = value;
            }
        }

        public double norm()
        {
            return Math.Sqrt(x * x + y * y + z * z);
        }

        public static Vector3 operator +(Vector3 v1, Vector3 v2)
        {
            return new Vector3(v1.x + v2.x, v1.y + v2.y, v1.z + v2.z);
        }
        public static Vector3 operator -(Vector3 a, Vector3 b)
        {
            return new Vector3(a.x - b.x, a.y - b.y, a.z - b.z);
        }
        public static Vector3 operator *(double a, Vector3 v)
        {
            return new Vector3(a * v.x, a * v.y, a * v.z);
        }
        public static Vector3 operator *(int a, Vector3 v)
        {
            return new Vector3((double)a * v.x, (double)a * v.y, (double)a * v.z);
        }
        public static double operator *(Vector3 v1, Vector3 v2)
        { //скалярное произведение
            return v1.x * v2.x + v1.y * v2.y + v1.z * v2.z;
        }
        public static Vector3 operator %(Vector3 v1, Vector3 v2)
        {
            return new Vector3((double)v1.y * v2.z - v1.z * v2.y, (double)v1.z * v2.x - v1.x * v2.z, (double)v1.x * v2.y - v1.y * v2.x);
        } //векторное произведение
        public static bool operator ==(Vector3 a, Vector3 b)
        {
            // If both are null, or both are same instance, return true.
            if (System.Object.ReferenceEquals(a, b))
            {
                return true;
            }

            // If one is null, but not both, return false.
            if (((object)a == null) || ((object)b == null))
            {
                return false;
            }

            // Return true if the fields match:
            return a.x == b.x && a.y == b.y && a.z == b.z;
        }
        public static bool operator !=(Vector3 a, Vector3 b)
        {
            return !(a == b);
        }
        public bool Equals(Vector3 a)
        {
            // Return true if the fields match:
            return x == a.x && y == a.y && z == a.z;
        }
        public override int GetHashCode()
        {
            int hashcode = 0; 
            hashcode = 31 * hashcode + (int)x.GetHashCode();
            hashcode = 31 * hashcode + (int)y.GetHashCode();
            hashcode = 31 * hashcode + (int)z.GetHashCode();
            return hashcode;
        }
    }
}
