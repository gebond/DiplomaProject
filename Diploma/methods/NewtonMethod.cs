using System;
using Diploma.entity;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        private int n; // кол-во шагов в методе РК
        private Quaternion psi_Old;
        private double T_old;

        public NewtonMethod(MainProblem CallFrom, Quaternion psiStart, double T_start)
        {
            Console.WriteLine("\n\t* NewtonMethod created!");
            callMain = CallFrom;
            psi_Old = psiStart;
            T_old = T_start;

        }

        public void RunProcess()
        {
            Vector delta;
            do
            {
                while ((double)T_old / callMain.N > 0.5) // проверяем будет ли ШАГ > 0.5
                {
                    callMain.N *= 2;
                }
                Quaternion res = RungeKutta.Run(psi_Old, T_old, callMain.Lambda0, callMain.N); // обращение к метду РК

                
                // обращение к методам подсчета невязки, и пересчета поправки

                Console.WriteLine("123123");
                delta = new Vector(5); // поправки которые мы нашли

            } while (delta.norm() > callMain.Epsilon);
        }

        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
