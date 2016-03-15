using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double del = 0.00000001; // дельта для метода 
        PsiTime psiTime;// кватернион + время - ЗАДАЧА НАХОЖДЕНИЯ!!!!
        List<double> xi = new List<double> { 0.5, 0.25}; // коэффициенты хи


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

            var k = 1;
            while (true)
            {
                Console.WriteLine("\n\t\t* Newton {0} iteration:", k);
                while ((double) psiTime.T / callMain.N >= 0.0011) // проверяем будет ли ШАГ <= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }
                
                
                // обращение к методам подсчета невязки

                Vector N0 = countN();
                Vector[] Nmass = new Vector[5];
                for (int i = 0; i < 5; i++)
                {
                    if (i == 0)
                    {
                        psiTime.T += del;
                        Nmass[i] = countN();
                        psiTime.T -= del;
                    }
                    else
                    {
                        psiTime.psi[i] += del;
                        Nmass[i] = countN();
                        psiTime.psi[i] -= del;
                    }
                }
                Console.Write("\n\t\tN0: "); N0.print();
                //Console.Write("\n\t\t resulted matrix of N:"); 
                // составляем матрицу 5х5 - невязки
                Matrix Nmatr = new Matrix(Nmass);
                //Nmatr.print();
                // составляем СЛАУ по невязкам с учетом нулевой невязки
                SLAU slau = createSLAU(Nmatr, N0, callMain.Epsilon);
                //Console.Write("\n\t\t system to solve:"); //slau.print();
                // РЕШАЕМ ОТНОСИТЕЛЬНО поправок
                Vector corrects = slau.getResult();
                Console.Write("\n\t\tcorrects:");corrects.print();
                // составляем следующее приближение при xi = 1
                upgradePsiTime(corrects, 1);
                // необходимо проверить как изменилась невязка N next
                Vector Nnext = countN();
                var countxi = 0;
                while(Nnext.norm() > N0.norm())
                {
                    try
                    {
                        Console.Write("\n\t\tNext {0}; Pre= {1}", Nnext.norm(), N0.norm());
                        Console.Write("\n\t\t fault -> try to upgrade PsiTime with xi = " + xi[countxi]);
                        upgradePsiTime(corrects, xi[countxi]);
                        Nnext = countN();
                        countxi++;
                    }
                    catch (Exception ex)
                    {
                        xi.Add(xi[countxi-1] / 2);
                        if (countxi > 10)
                        {
                            Console.Write("Can't resolve this :(");
                            Console.ReadKey();
                        }
                    }

                }
                Console.WriteLine("\t%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                Console.Write("\n\t\t OK SEE CURRENT T= {0}; Psi=", psiTime.T); psiTime.psi.print();
                Console.WriteLine("\t\t newnorm =" + Nnext.norm() + "oldnorm = " + N0.norm());
                Console.WriteLine("\n\t%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
                k++;
            }
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }

        private void upgradePsiTime(Vector corrects, double xi)
        {
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    psiTime.T += xi * corrects[i];
                }
                else
                {
                    psiTime.psi[i] += xi * corrects[i];
                }
            }

        }
        private Matrix createNmatrixForSLau(Matrix N, Vector N0)
        {
            Matrix res = new Matrix((short)N0.length);
            for (int i = 0; i < res.length; i++)
            {
                for (int j = 0; j < res.length; j++)
                {
                    res[i, j] = N[i, j] - N0[i] / del;
                }
            }
            return res;
        }
        private SLAU createSLAU(Matrix N, Vector N0, double precision)
        {
            var Nslau = createNmatrixForSLau(N, N0);
            var resSlau = new SLAU(Nslau, (-1) * N0, precision);
            return resSlau;
        }
        private Vector countN()
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
