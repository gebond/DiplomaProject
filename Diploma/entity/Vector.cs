using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Vector:IVector
    {
        private double[] vect;

        void IVector.print()
        {
            try
            {
                System.Console.WriteLine("Vector:");
                for (int i = 0; i < vect.Length; i++)
                {
                    Console.Write("{0}\n", vect[i]);
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
    }
}
