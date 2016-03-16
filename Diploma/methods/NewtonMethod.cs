using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double del = 0.000001; // дельта для метода 
        PsiTime currentPsiTime;// кватернион + время - ЗАДАЧА НАХОЖДЕНИЯ!!!!
        List<double> xi = new List<double> { 1.0, 0.5, 0.25, 0.125}; // коэффициенты хи


        public NewtonMethod(MainProblem CallFrom, Quaternion psiStart, double T_start)
        {
            Console.WriteLine("\n\t* NewtonMethod created!");
            callMain = CallFrom;
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] = Psi[0,1,2,3]
             * vect[4] == Time
             * */
            currentPsiTime = new PsiTime(psiStart, T_start);
        }

        public void RunProcess()
        {
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Newton %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            var k = 1;
            while (k<2)
            {
                Console.WriteLine("\n\t\t* Newton {0} iteration:", k);
                while ((double) currentPsiTime.T / callMain.N >= 0.001) // проверяем будет ли ШАГ <= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }

                // cоставляем первую невязку
                Vector N0 = countN(currentPsiTime);
                Console.Write("\n\t\t N0: "); N0.print();

                // составляем СЛАУ по невязкам и решаем поправки
                SLAU slau = createSLAU(N0, callMain.Epsilon);
                Console.Write("\t\t slau:"); slau.print();
                Vector corrects = slau.getResult();
                Console.Write("\t\t cor: "); corrects.print();

                // получаем следуюущую невязку и в случае успеха меняем текущие psiTime
                getNextAndChangePsiT(corrects, N0);

                // если Nnext было найдено - значит можно переходить к следующей итерации метода Ньютона
                k++;
            }
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }

        private void showResults(Vector Nnext, Vector N0)
        {
            Console.WriteLine("\t%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.Write("\n\t\t OK SEE CURRENT T= {0}; Psi: ", currentPsiTime.T); currentPsiTime.psi.print();
            Console.WriteLine("\t\t NEXT:" + Nnext.norm() + "OLD:" + N0.norm());
            Console.WriteLine("\n\t%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }

        private void getNextAndChangePsiT(Vector corrects, Vector N0)
        {
            Vector Nnext = null;
            PsiTime tryPsiTime = currentPsiTime;
            var countxi = 0;
            do
            {
                try
                {
                    tryPsiTime = upgradePsiTime(corrects, xi[countxi]);
                    Nnext = countN(tryPsiTime);
                }
                catch (Exception ex)
                {
                    xi.Add(xi[countxi - 1] / 2);
                    if (countxi > 10) // сколько раз разрешается уменьшить ХИ
                    {
                        Console.Write("Can't resolve this :(");
                        Console.ReadKey();
                    }
                }
                countxi++;

            } while (Nnext.norm() > N0.norm());
            currentPsiTime = tryPsiTime;
            showResults(Nnext, N0);
        }
        private PsiTime upgradePsiTime(Vector corrects, double xi)
        {
            PsiTime res = new PsiTime(currentPsiTime.psi, currentPsiTime.T);
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    res.T += xi * corrects[i];
                }
                else
                {
                    res.psi[i] += xi * corrects[i];
                }
            }
            return res;

        }
        private Matrix createNMatrix()
        {
            Vector[] Nmass = new Vector[5];
            for (int i = 0; i < 5; i++)
            {
                if (i == 0)
                {
                    var changedPsiTime = currentPsiTime;
                    changedPsiTime.T += del;
                    Nmass[i] = countN(changedPsiTime);
                }
                else
                {
                    var changedPsiTime = currentPsiTime;
                    changedPsiTime.psi[i] += del;
                    Nmass[i] = countN(changedPsiTime);
                }
            }
            
            return new Matrix(Nmass);

        }
        private Matrix createNmatrixForSLau(Matrix N, Vector N0)
        {
            Matrix res = new Matrix((short)N0.length);
            for (int i = 0; i < res.length; i++)
            {
                for (int j = 0; j < res.length; j++)
                {
                    res[i, j] = (N[i, j] - N0[i]) / del;
                }
            }
            res.print();
            return res;
        }
        private SLAU createSLAU(Vector N0, double precision)
        {
            var Nmatrix = createNMatrix();
            var Nslau = createNmatrixForSLau(Nmatrix, N0);
            var resSlau = new SLAU(Nslau, (-1) * N0, precision);
            return resSlau;
        }
        private Vector countN(PsiTime psiTimeRequest)
        {
            Vector Nres = new Vector(5);
            // обращение к РК для получения кватерниона на конце
            // results = <lambda, hamilton>
            Tuple< Quaternion, double> results = RungeKutta.Run(psiTimeRequest, callMain);
            Console.Write("\t\t Ham: "  + results.Item2); 
            //Console.Write("\t\t result.LambdaT: "); results.Item1.print();
            Quaternion diff = results.Item1 - callMain.LambdaT;
            for (int i = 0; i < 4; i++)
            {
                Nres[i] = diff[i]; // копируем весь кватернион разницы
            }
            Nres[4] = results.Item2; // последняя позиция - это значение функции гамильтона в полученной системе РК
            Console.Write("\n\t\t find N"); Nres.print();
            return Nres;
        }
        
    }
}
