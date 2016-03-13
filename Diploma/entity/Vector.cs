using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Vector:IVector
    {
        private double[] vect;

        public Vector(int dim)
        {
            vect = new double[dim];
        }
        public Vector(double[] mass)
        {
            vect = new double[mass.Length];
            for (int i = 0; i < mass.Length; i++)
            {
                vect[i] = mass[i];
            }
        }
        public Vector(IVector a)
        {
            vect = new double[a.length];
            for (int i = 0; i < a.length; i++)
            {
                vect[i] = a[i];
            }
        }

        void IVector.print()
        {
            try
            {  
                for (int i = 0; i < vect.Length; i++)
                {   
                    if(i == 0)
                    {
                        System.Console.Write("Vector: [");
                    }
                    
                    if (i != vect.Length - 1)
                    {
                        Console.Write("{0}, ", vect[i]);
                    }
                    else
                    {
                        Console.WriteLine("{0}]", vect[i]);
                    }
                }
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Empty Vect massive!");
            }
        }
        int IVector.length
        {
            get { return vect.Length; }
        }
        double IVector.this[int index]
        {
            get{ return vect[index]; }
            set { vect[index] = value; }
        }
        double IVector.norm()
        {
            double res = 0;
            try
            {  
                for (int i = 0; i < vect.Length; i++)
                {
                    if (Math.Abs(vect[i]) > res)
                    {
                        res = Math.Abs(vect[i]);
                    }
                }
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Empty Vect massive!");
            }
            return res;
        }

        public double norm()
        {
            double res = 0;
            try
            {
                for (int i = 0; i < vect.Length; i++)
                {
                    if (Math.Abs(vect[i]) > res)
                    {
                        res = Math.Abs(vect[i]);
                    }
                }
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Empty Vect massive!");
            }
            return res;
        } 
        public static Vector operator +(Vector v1, Vector v2)
        {
            if(v1.vect.Length != v2.vect.Length)
            {
                throw new ArgumentException("not similar dim");
            }
            double[] res = new double[v1.vect.Length];
            for (int i = 0; i < v1.vect.Length; i++)
            {
                res[i] = v1.vect[i] + v2.vect[i];
            }
            return new Vector(res);
        }
        public static Vector operator -(Vector v1, Vector v2)
        {
            if (v1.vect.Length != v2.vect.Length)
            {
                throw new ArgumentException("not similar dim");
            }
            double[] res = new double[v1.vect.Length];
            for (int i = 0; i < v1.vect.Length; i++)
            {
                res[i] = v1.vect[i] - v2.vect[i];
            }
            return new Vector(res);
        }
        public static Vector operator *(double a, Vector v)
        {
            double[] res = new double[v.vect.Length];
            for (int i = 0; i < v.vect.Length; i++)
            {
                res[i] = a * v.vect[i];
            }
            return new Vector(res);
        }
        public static Vector operator *(int a, Vector v)
        {
            double[] res = new double[v.vect.Length];
            for (int i = 0; i < v.vect.Length; i++)
            {
                res[i] = (double) a * v.vect[i];
            }
            return new Vector(res);
        }
        public static double operator *(Vector v1, Vector v2)
        {
            if (v1.vect.Length != v2.vect.Length)
            {
                throw new ArgumentException("not similar dim");
            }
            double res = 0;
            for (int i = 0; i < v1.vect.Length; i++)
            {
                res += v1.vect[i]*v2.vect[i];
            }
            return res;
        }
        
        public static bool operator ==(Vector a, Vector b)
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
            for (int i  = 0; i  < a.vect.Length; i ++)
            {
                if (a.vect[i] != b.vect[i]) { return false; }
            }
            return true;
        }
        public static bool operator !=(Vector a, Vector b)
        {
            return !(a == b);
        }
        public bool Equals(Vector a)
        {
            // Return true if the fields match:
            for (int i = 0; i < a.vect.Length; i++)
            {
                if (a.vect[i] != vect[i]) { return false; }
            }
            return true;
        }
        public override int GetHashCode()
        {
            int hashcode = 0;
            for (int i = 0; i < vect.Length; i++)
            {
                hashcode = 31 * hashcode + (int)vect[i].GetHashCode();
            }
            return hashcode;
        }
    }
}
