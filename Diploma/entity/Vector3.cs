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
    }
}
