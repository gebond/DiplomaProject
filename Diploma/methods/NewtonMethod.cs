using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double delT = 1; // дельта для T
        double delPsi = 0.001;
        PsiTime currentPsiTime;// кватернион + время текущие ИСКОМОЕ ВРЕМЯ T

        List<double> xi = new List<double> {1.0}; // коэффициенты хи


        public NewtonMethod(MainProblem CallFrom, Vector3 psiStart, double T_start)
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
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% МЕТОД НЬЮТОНА %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            var k = 1;
            while (k<3)
            {
                Console.WriteLine("\n\t\tМЕТОДА НЬЮТОНА {0} ИТЕРАЦИЯ:", k);
                while ((double) currentPsiTime.T / callMain.N >= 0.01) // проверяем будет ли ШАГ <= 0.01
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }

                //Console.WriteLine("\n\t\tНАЧАЛЬНАЯ НЕВЯЗКА N0:");
                // cоставляем первую невязку
                Vector N0 = countN(currentPsiTime);

                // составляем СЛАУ по невязкам и решаем поправки
                SLAU slau = createSLAU(N0, callMain.Epsilon);
                Vector corrects = slau.getResult();
                Console.WriteLine("\t\tПОЛУЧЕННЫЕ ПОПРАВКИ: " + corrects.ToString());

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
            PsiTime tryPsiTime = new PsiTime(null);
            Console.WriteLine("\n\t\tПРИМЕНЯНЕМ ПОПРАВКИ:");
            var countxi = 0;
            do
            {
                try
                {
                    Console.Write("\t\t -> XI={0} ", xi[countxi]);
                }
                catch (Exception ex)
                {
                    xi.Add(xi[countxi - 1] / 2);
                    if (countxi > 15) // сколько раз разрешается уменьшить ХИ
                    {
                        Console.WriteLine("\t\tНЕЛЬЗЯ УМЕНЬШИТЬ НОРМУ НЕВЯЗКИ!");
                        Console.ReadKey();
                    }
                    Console.Write("\t\t -> XI={0} ", xi[countxi]);
                }
                tryPsiTime = upgradePsiTime(corrects, xi[countxi]);
                Nnext = countN(tryPsiTime);
                countxi++;

            } while (Nnext.norm() > N0.norm());
            currentPsiTime = new PsiTime(tryPsiTime);
            Console.WriteLine("\t\tOK! ТЕПЕРЬ: " + currentPsiTime.ToString() + "\n\t\tНОРМА НЕВЯЗКИ ПОСЛЕ ИТЕРАЦИИ: " + Nnext.norm());
            Console.WriteLine("\t\tНОРМА УМЕНЬШИЛАСЬ НА " + (N0.norm() - Nnext.norm()));
            //showResults(Nnext, N0);
        }
        private PsiTime upgradePsiTime(Vector corrects, double xi)
        {
            PsiTime res = new PsiTime(currentPsiTime);
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
                    changedPsiTime.psi[i-1] += delPsi;
                    Nmass[i] = countN(changedPsiTime);
                }
            }
            Matrix res = new Matrix(Nmass);
            Console.WriteLine("\n\t\tМАТРИЦА НЕВЯЗОК:\n" + res.ToString(2));
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
            Console.WriteLine("\t\tМАТРИЦА ПРОИЗВОДНЫХ НЕВЯЗОК:\n" + res.ToString(2));
            return res;
        }
        private SLAU createSLAU(Vector N0, double precision)
        {
            var Nmatrix = createNMatrix();
            var Nslau = createNmatrixForSLau(Nmatrix, N0);
            var resSlau = new SLAU(Nslau, (-1) * N0, precision);
            Console.WriteLine("\t\tСЛАУ ПО НЕВЯЗКАМ:\n" + resSlau.ToString(2));
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
            Console.WriteLine("\n\t\tПОЛУЧЕННАЯ НЕВЯЗКА: {0} С НОРМОЙ: {1}", Nres.ToString(), Nres.norm());
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
