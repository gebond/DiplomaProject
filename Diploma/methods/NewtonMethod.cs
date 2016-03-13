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
            Vector delta = new Vector(5);
            Vector vectPsiTime = new Vector(5);
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] = Psi[0,1,2,3]
             * vect[4] == Time
             * */
            do
            {
                while ((double)T_old / callMain.N >= 0.5) // проверяем будет ли ШАГ >= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }
                Quaternion res = RungeKutta.Run(vectPsiTime, callMain); // обращение к метду РК

                
                // обращение к методам подсчета невязки, и пересчета поправки

                delta = delta; // поправки которые мы нашли
                

            } while (delta.norm() > callMain.Epsilon);
        }

        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
