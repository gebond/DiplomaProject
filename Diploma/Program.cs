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

            Vector3 OMEGA_0 = new Vector3(0.2, 0.3, -0.4);
            Vector3 OMEGA_T = new Vector3(0, 0, 0);
            double Umax = 0.001;

            var PSI = new Vector3(-0.2, 0.2, -0.2);
            double T = 10;

            int N = 100;
            double eps = 0.000000001;

            MainProblem main = new MainProblem(OMEGA_0, OMEGA_T, PSI, T, Umax, N, eps);

            NewtonMethod newton = new NewtonMethod(main, PSI, T);
            newton.RunProcess();

            //Console.WriteLine("res " + Math.Pow(2,0));

            //Tuple<double, Vector3> res = RungeKutta.Run(new PsiTime(PSI, T), main);
            //PSI[3] = 0.06;
            // T = 9;
            // res = RungeKutta.Run(new PsiTime(PSI, T), main);*/



            Console.Write("\n\n\n\n\n\t\t\tEnter an key to exit... ");
            Console.ReadKey();
        }
    }
}
