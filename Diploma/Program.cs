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
            Console.WriteLine("Heelooo! 12123123123123");
            Console.WriteLine("Vasssy1");

            IVector some = new Vector();
            IVector some2 = new Vector3();

            List<IVector> list = new List<IVector>();
            list.Add(some);
            list.Add(some2);

            foreach (IVector vec in list)
            {
                vec.print();
            }

            Console.Write("\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\n\nEnter key to exit ... ");
            Console.ReadKey();
        }
    }
}
