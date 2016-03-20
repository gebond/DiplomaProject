using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double delT = 0.1; // дельта для T
        double delPsi = 0.01;
        PsiTime currentPsiTime;// кватернион + время текущие ИСКОМОЕ ВРЕМЯ T

        List<double> xi = new List<double> {1.0}; // коэффициенты хи


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
            while (k<3)
            {
                Console.WriteLine("\n\t\t* Newton {0} iteration:", k);
                while ((double) currentPsiTime.T / callMain.N >= 0.01) // проверяем будет ли ШАГ <= 0.01
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }

                // cоставляем первую невязку
                Vector N0 = countN(currentPsiTime);
                Console.Write("\n\t\t начальная невязка:");
                Console.Write("\n\t\t N0: "); N0.print(); Console.Write("\t\t Norm:" + N0.norm());

                // составляем СЛАУ по невязкам и решаем поправки
                SLAU slau = createSLAU(N0, callMain.Epsilon);
                Console.Write("\t\t "); slau.print();
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
            Console.WriteLine("\t\t ТЕПЕРЬ:" + Nnext.norm() + "  БЫЛО:" + N0.norm());
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
                    Console.WriteLine("\t\t Nnext.norm = " + Nnext.norm());
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
                    res.psi[i-1] += xi * corrects[i];
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
                    changedPsiTime.T += delT;
                    Nmass[i] = countN(changedPsiTime);
                }
                else
                {
                    var changedPsiTime = new PsiTime(currentPsiTime.psi, currentPsiTime.T);
                    Console.WriteLine("123123123123123");
                    changedPsiTime.psi.print();
                    changedPsiTime.psi[i-1] += delPsi;
                    Nmass[i] = countN(changedPsiTime);
                }
            }
            Matrix res = new Matrix(Nmass);
            res.print();
            return res;

        }
        private Matrix createNmatrixForSLau(Matrix N, Vector N0)
        {
            Matrix res = new Matrix((short)N0.length);
            for (int i = 0; i < res.length; i++)
            {
                for (int j = 0; j < res.length; j++)
                {
                    if (i == 0)
                    {
                        res[i, j] = (N[i, j] - N0[i]) / delT;
                    }
                    else
                    {
                        res[i, j] = (N[i, j] - N0[i]) / delPsi;
                    }
                }
            }
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
            // results = <hamilton, lambda>
            Tuple<double, Quaternion> results = RungeKutta.Run(psiTimeRequest, callMain);

            Nres[0] = results.Item1; //  это значение функции гамильтона в полученной системе РК
            Quaternion diff = results.Item2 - callMain.LambdaT;
            
            for (int i = 0; i < 4; i++)
            {
                Nres[i+1] = diff[i]; // копируем весь кватернион разницы
            }
            results.Item2.print();
            Nres.print();
            Console.WriteLine (Nres.norm());
            return Nres;
        }
        private double SummAbs(Vector vector)
        {
            double sum = 0;
            for (int i = 0; i < vector.length; i++)
            {
                sum += Math.Abs(vector[i]);
            }
            return sum;
        }
    }
}
