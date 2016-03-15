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

            for (int k = 0; k < 4; k++)
            {
                Console.WriteLine("\n\t\t* Newton {0} iteration:", k);
                while ((double) psiTime.T / callMain.N >= 0.0011) // проверяем будет ли ШАГ <= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }
                //Console.WriteLine("\t\t  params: T = {0}, n = {1}, h = {2}", psiTime.T, callMain.N, psiTime.T / callMain.N);
                //Console.Write("\t\t  Psi:"); psiTime.psi.print(); 
                //Console.WriteLine("\t\t  norm before: {0}, norm after: {1}", callMain.Lambda0.getMagnitude(), resLambda.getMagnitude());
                
                // обращение к методам подсчета невязки

                Vector N0 = countN(psiTime.psi, psiTime.T);
                Vector[] Nmass = new Vector[5];
                for (int i = 0; i < 5; i++)
                {
                    if (i == 0)
                    {
                        Nmass[i] = countN(psiTime.psi, psiTime.T + del);
                    }
                    else
                    {
                        psiTime.psi[i] += del;
                        Nmass[i] = countN(psiTime.psi, psiTime.T);
                        psiTime.psi[i] -= del;
                    }
                }
                Console.Write("\t\t calculated vectorN0: "); N0.print();
                Console.Write("\t\t resulted matrix of N:"); 
                Matrix Nmatr = new Matrix(Nmass);
                Nmatr.print();


            }
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }
        private Matrix createNmatrixForSLau(Matrix N, Vector N0)
        {
            Ma
            return 
        }
        private Vector countN(Quaternion psi, double T)
        {
            Vector Nres = new Vector(5);
            // обращение к РК для получения кватерниона на конце
            // results = <lambda, hamilton>
            Tuple< Quaternion, double> results = RungeKutta.Run(psiTime, callMain);
            Quaternion diff = results.Item1 - callMain.LambdaT;
            for (int i = 0; i < 4; i++)
            {
                Nres[i] = diff[i]; // копируем весь кватернион разницы
            }
            Nres[4] = results.Item2; // последняя позиция - это значение функции гамильтона в полученной системе РК
            return Nres;
        }
        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
