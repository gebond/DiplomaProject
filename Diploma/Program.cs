using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Diploma.entity;

namespace Diploma
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\tHello to the programm");
            Console.WriteLine("\t\t\tPlease do not huste!\n\n");

            IVector some = new Vector(3);
            IVector some2 = new Vector3();
            IVector test = new Vector(3);

            
            
            
            
            List<IVector> list = new List<IVector>();
            list.Add(some);
            list.Add(some2);
            list.Add(test);
            foreach (IVector vec in list)
            {
                vec.print();
            }

            Console.Write("\n\n\n\n\n\n\n\t\t\tEnter key to exit ... ");
            Console.ReadKey();
        }
    }
}
