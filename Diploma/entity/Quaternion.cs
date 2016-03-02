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





    }
}
