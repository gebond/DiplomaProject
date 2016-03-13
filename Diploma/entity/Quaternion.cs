using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Quaternion
    {   
        // поля
        private Vector3 v;
        private double w;

        // конструкторы
        public Quaternion()
        {
            this.w = 1;
            this.v = new Vector3();
        }
        public Quaternion(double w, double x, double y, double z)
        {
            this.w = w;
            this.v = new Vector3(x, y, z);

        }
        public Quaternion(double w, Vector3 v)
        {
            this.w = w;
            this.v = v;

        }
        public Quaternion(Quaternion q)
        {
            this.w = q.w;
            this.v = q.v;
        }
        public Quaternion(double angle, Vector3 v, bool withangle)
        {
            //var res = new Quaternion(Math.Cos(0.5 * angle), Math.Sin(0.5 * angle) * v);
        }

        public double getw() { return w; }
        public double getx() { return v.getx(); }
        public double gety() { return v.gety(); }
        public double getz() { return v.getz(); }

        // методы 
        public void changeToStandart()
        {
            this.w = 1;
            this.v = new Vector3();
        }
        public Quaternion getConjugateQuaternion()
        {
            return new Quaternion(w, (-1) * this.v.getx(), (-1) * this.v.gety(), (-1) * this.v.getz());
        }
        public double getNorm()
        {
            return this.v * this.v + this.w * this.w;
        }
        public double getMagnitude()
        {
            return System.Math.Sqrt(this.getNorm());
        }
        public Quaternion getInverse()
        {
            return new Quaternion(1.0 / this.getNorm() * this.getConjugateQuaternion());
        }
        public void changeValues(double neww, double newx, double newy, double newz)
        {
            this.w = neww;
            this.v = new Vector3(newx, newy, newz);
        }
        public void changeValues(Vector3 v, double w)
        {
            this.w = w;
            this.v = v;
        }
        public void changeValues(Quaternion q)
        {
            this.w = q.w;
            this.v = q.v;
        }

        // печать
        public void print()
        {
            System.Console.WriteLine("Quaternion: [{0}, ({1}, {2}, {3})];", this.w, this.v.getx(), this.v.gety(), this.v.getz());
        }
        public void printInverse()
        {

            System.Console.Write("Inverse: ");
            this.getInverse().print();
        }
        public void printNorm()
        {
            System.Console.WriteLine("Norm: " + this.getNorm() + ";");
        }
        public void printMagnitude()
        {
            System.Console.WriteLine("Magnitude: " + this.getMagnitude() + ";");
        }

        public static Quaternion Abs(Quaternion toAbs)
        {
            return new Quaternion(Math.Abs(toAbs.w), Math.Abs(toAbs.getx()), Math.Abs(toAbs.gety()), Math.Abs(toAbs.getz()));
        }

        public static Quaternion operator +(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.w + q2.w, q1.v + q2.v);
        }
        public static Quaternion operator -(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.w - q2.w, q1.v - q2.v);
        }
        public static Quaternion operator *(double a, Quaternion q)
        {
            return new Quaternion(a * q.w, a * q.v);
        }
        public static Quaternion operator *(int a, Quaternion q)
        {
            return new Quaternion(a * q.w, a * q.v);
        }
        public static Quaternion operator %(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.w * q2.w - q1.v * q2.v, q1.v % q2.v + q1.w * q2.v + q2.w * q1.v);
        }// УМНОЖЕНИЕ
        public static Quaternion operator *(Quaternion q1, Quaternion q2)
        {
            return new Quaternion(q1.getw() * q2.getw(), q1.getx() * q2.getx(), q1.gety() * q2.gety(), q1.getz() * q2.getz());
        } // и все таки скалярное произведение
        public static Quaternion operator *(Quaternion q, Vector3 v)
        {
            var q2 = new Quaternion(0.0, v.getx(), v.gety(), v.getz());
            return new Quaternion(q * q2);
        }
        public static bool operator ==(Quaternion a, Quaternion b)
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
            return a.w == b.w && a.v == b.v;
        }
        public static bool operator !=(Quaternion a, Quaternion b)
        {
            return !(a == b);
        }
        public bool Equals(Quaternion a)
        {
            // Return true if the fields match:
            return w == a.w && v == a.v;
        }
        public override int GetHashCode()
        {
            int hashcode = 0;
            hashcode = 31 * hashcode + (int)w.GetHashCode();
            hashcode = 31 * hashcode + (int)v.getx().GetHashCode();
            hashcode = 31 * hashcode + (int)v.gety().GetHashCode();
            hashcode = 31 * hashcode + (int)v.getz().GetHashCode();
            return hashcode;
        }
    }
}
