using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double del = 0.00000001; // дельта для метода 
        Vector vectPsiTime = new Vector(5); // кватернион + время
        Vector N_old = new Vector(5); // невязка на шаге метода ньютона
        HashSet<double> xi = new HashSet<double> { 1.0, 0.5, 0.25, 0.125, 0.0625, 0.03125, 0.15625, 0.0078125 }; // коэффициенты хи


        public NewtonMethod(MainProblem CallFrom, Quaternion psiStart, double T_start)
        {
            Console.WriteLine("\n\t* NewtonMethod created!");
            callMain = CallFrom;
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] = Psi[0,1,2,3]
             * vect[4] == Time
             * */
            for (int i = 0; i < 4; i++)
            {
                vectPsiTime[i] = psiStart[i];
            }
            vectPsiTime[4] = T_start;
        }

        public void RunProcess()
        {
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Newton %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            int newtonItaration = 1;
            do
            {
                Console.WriteLine("\n\t\t* Newton {0} iteration:", newtonItaration);
                while ((double) vectPsiTime[4] / callMain.N >= 0.0011) // проверяем будет ли ШАГ <= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }
                Console.WriteLine("\t\t  params: T = {0}, n = {1}, h = {2}", vectPsiTime[4], callMain.N, vectPsiTime[4] / callMain.N);
                Console.WriteLine("\t\t  Psi: [{0}, {1}, {2}, {3}]", vectPsiTime[0], vectPsiTime[1], vectPsiTime[2], vectPsiTime[3]); 

                Quaternion resLambda = RungeKutta.Run(vectPsiTime, callMain); // обращение к метду РК
                Console.WriteLine("\t\t  norm before: {0}, norm after: {1}", callMain.Lambda0.getMagnitude(), resLambda.getMagnitude());
                
                // обращение к методам подсчета невязки
                
                


                newtonItaration++;

            } while (false);
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }


        private void  Hamilton(){}

        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
