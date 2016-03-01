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
        
        void IVector.print()
        {
            try
            {
                System.Console.WriteLine("Vector3: [{0}, ({1}, {2}, {3})];", x, y, z);
            }
            catch (FormatException)
            {
                Console.WriteLine("Empty vector3!");
            }
        }
        int IVector.length
        {
            get { return 3; }
        }
    }
}
