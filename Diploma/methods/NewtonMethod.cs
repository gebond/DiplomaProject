using System;
using Diploma.entity;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        private int steps; // кол-во шагов в методе РК
        private Quaternion psi_Old;
        private double T_old;

        public NewtonMethod(MainProblem CallFrom, Quaternion psiStart, int steps, double T_start)
        {
            Console.WriteLine("\n\t* NewtonMethod created!");
            callMain = CallFrom;
            this.steps = steps;
            psi_Old = psiStart;
            T_old = T_start;
        }

        public void RunProcess()
        {

            Quaternion r_diff;

            /*while()
            {
            *
            }*/
        }

        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
