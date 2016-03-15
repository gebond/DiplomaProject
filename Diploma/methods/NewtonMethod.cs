using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double del = 0.00000001; // дельта для метода 
        PsiTime psiTime;// кватернион + время
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
            psiTime = new PsiTime(psiStart, T_start);
        }

        public void RunProcess()
        {
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Newton %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            int newtonItaration = 1;
            for (int i = 0; i < 10; i++)
            {
                Console.WriteLine("\n\t\t* Newton {0} iteration:", newtonItaration);
                while ((double) psiTime.T / callMain.N >= 0.0011) // проверяем будет ли ШАГ <= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }
                Console.WriteLine("\t\t  params: T = {0}, n = {1}, h = {2}", psiTime.T, callMain.N, psiTime.T / callMain.N);
                Console.Write("\t\t  Psi:"); psiTime.psi.print(); 

                Quaternion resLambda = RungeKutta.Run(psiTime, callMain); // обращение к метду РК
                Console.WriteLine("\t\t  norm before: {0}, norm after: {1}", callMain.Lambda0.getMagnitude(), resLambda.getMagnitude());
                
                // обращение к методам подсчета невязки

                //Quaternion res = - 1 + psiTime.psi*(0.5 * (resLambda % ));


                newtonItaration++;
                psiTime.T += 10;

            }
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }


        private void  Hamilton(){
            
        }

        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
