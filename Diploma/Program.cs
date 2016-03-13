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

            Quaternion Quat0 = new Quaternion(1,1,1,1);
            Quaternion QuatT = new Quaternion();
            Quaternion psiStart = new Quaternion();
            int stepsForRk = 100;
            double T_start = 10.0;

            MainProblem main = new MainProblem(Quat0, QuatT, 0.01, psiStart, stepsForRk, T_start);
            main.start();
            //main.printCurrentParameters();

            Console.Write("\n\n\n\n\n\t\t\tEnter an key to exit... ");
            Console.ReadKey();
        }
    }
}
