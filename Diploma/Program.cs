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

            Quaternion LAMBDA_0 = new Quaternion(1, 0, 0, 0);
            Quaternion LAMBDA_T = new Quaternion(1, 0.5, 0, 1);

            var PSI = new Vector3(0, 0, 0);
            double T = 10;

            int N = 100;
            double eps = 0.001;


            MainProblem main = new MainProblem(LAMBDA_0, LAMBDA_T, PSI, T, N, new Vector(new double[2] { -1, 1 }), eps);

            NewtonMethod newton = new NewtonMethod(main, PSI, T);
            newton.RunProcess();




            Tuple<double, Quaternion> res = RungeKutta.Run(new PsiTime(PSI, T), main);
            //PSI[3] = 0.06;
            // T = 9;
            // res = RungeKutta.Run(new PsiTime(PSI, T), main);*/



            Console.Write("\n\n\n\n\n\t\t\tEnter an key to exit... ");
            Console.ReadKey();
        }
    }
}
