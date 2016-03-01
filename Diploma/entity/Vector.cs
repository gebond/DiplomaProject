using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Vector:IVector
    {
        private decimal[] vect;

        public Vector(int dim)
        {
            vect = new decimal[dim];
        }
        public Vector(decimal[] mass)
        {
            vect = new decimal[mass.Length];
            for (int i = 0; i < mass.Length; i++)
            {
                vect[i] = mass[i];
            }
        }
        public Vector(IVector a)
        {
            vect = new decimal[a.length];
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
        decimal IVector.this[int index]
        {
            get{ return vect[index]; }
            set { vect[index] = value; }
        }

    }
}
