using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Diploma.entity;
using Diploma.methods;

namespace Diploma
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\tStart of application");

            Quaternion Quat0 = new Quaternion(1.0, 1.0, 1.0, 1.0);
            Quaternion QuatT = new Quaternion(1.0, 1.0, 2.0, 3.0);
            Quaternion psiStart = new Quaternion(1.0, 1.0, 1.0, 1.0);

            double epsilon = 0.01;
            int n = 100;
            double T_start = 10.0;
            IVector Omega = new Vector(new double[] {-1.0, 1.0});
            

            MainProblem main = new MainProblem(Quat0, QuatT, epsilon, psiStart, T_start, n, Omega);
            main.start();
            //main.printCurrentParameters();

            Console.Write("\n\n\n\n\n\t\t\tEnter an key to exit... ");
            Console.ReadKey();
        }
    }
}
