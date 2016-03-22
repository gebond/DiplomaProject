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
        static void startAllGoodPankratov()
        {
            Console.WriteLine("\t\t\tStart of application");

            Vector3 OMEGA_0 = new Vector3(0.2, 0.3, -0.4);
            Vector3 OMEGA_T = new Vector3(0, 0, 0);

            Quaternion LAMBDA_0 = new Quaternion(0.5, 0.1, 0.1, -0.1);

            double Umax = 0.001;

            var PSI = new Vector3(-0.2, 0.2, -0.2);
            double T = 10;

            int N = 100;
            double eps = 0.000000001;

            MainProblem main = new MainProblem(OMEGA_0, OMEGA_T, PSI, LAMBDA_0, T, Umax, N, eps);

            NewtonMethod newton = new NewtonMethod(main, PSI, T);
            newton.RunProcess();
        }
        static void tryExactly()
        {
            Console.WriteLine("\t\t\tStart of application");

            Vector3 OMEGA_0 = new Vector3(0.2, 0.3, -0.4);
            Vector3 OMEGA_T = new Vector3(0, 0, 0);


            Quaternion LAMBDA_0 = new Quaternion(0.5, 0.1, 0.1, -0.1);

            double Umax = 0.001;

            var PSI = new Vector3(-371.39068, -557.08601, 742.78135);
            double T = 538.51648;

            int N = 100;
            double eps = 0.000000001;

            MainProblem main = new MainProblem(OMEGA_0, OMEGA_T, PSI, LAMBDA_0, T, Umax, N, eps);

            NewtonMethod newton = new NewtonMethod(main, PSI, T);
            newton.RunProcess();
        }
        static void Main(string[] args)
        {

            //startAllGoodPankratov();
            tryExactly();

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
