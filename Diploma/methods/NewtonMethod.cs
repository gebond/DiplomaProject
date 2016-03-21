using System;
using Diploma.entity;
using System.Collections.Generic;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        double delT = 0.01; // дельта для T
        double delPsi = 0.001;
        PsiTime currentPsiTime;// кватернион + время текущие ИСКОМОЕ ВРЕМЯ T
        List<double> xi = new List<double> {1.0}; // коэффициенты хи


        public NewtonMethod(MainProblem CallFrom, Vector3 psiStart, double T_start)
        {
            Console.WriteLine("\n\t* NewtonMethod created!");
            callMain = CallFrom;
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2] = Psi[0,1,2]
             * vect[3] == Time
             * */
            currentPsiTime = new PsiTime(psiStart, T_start);
        }

        public void RunProcess()
        {
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% МЕТОД НЬЮТОНА %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            var k = 1;
            Vector N0 = null;
            Vector Nnext = null;
            do
            {
                Console.WriteLine("\n\t\tМЕТОДА НЬЮТОНА {0} ИТЕРАЦИЯ:", k);
                while ((double) currentPsiTime.T / callMain.N >= 0.01) // проверяем будет ли ШАГ <= 0.01
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }

                //Console.WriteLine("\n\t\tНАЧАЛЬНАЯ НЕВЯЗКА N0:");
                // cоставляем первую невязку
                N0 = countN(currentPsiTime);

                // составляем СЛАУ по невязкам и решаем поправки
                SLAU slau = createSLAU(N0, callMain.Epsilon);
                Vector corrects = slau.getResult();
                Console.WriteLine("\t\tПОЛУЧЕННЫЕ ПОПРАВКИ: " + corrects.ToString());

                // получаем следуюущую невязку и в случае успеха меняем текущие psiTime
                Nnext = getNextAndChangePsiT(corrects, N0);

                // если Nnext было найдено - значит можно переходить к следующей итерации метода Ньютона
                k++;
            } while(N0.norm() - Nnext.norm() > callMain.Epsilon);
            Console.WriteLine("\n\n%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }

        private void showResults(Vector Nnext, Vector N0)
        {
            Console.WriteLine("\t%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            Console.Write("\n\t\t OK SEE CURRENT T= {0}; Psi: ", currentPsiTime.T); currentPsiTime.psi.print();
            Console.WriteLine("\t\t ТЕПЕРЬ:" + Nnext.norm() + "  БЫЛО:" + N0.norm());
            Console.WriteLine("\n\t%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
        }
        private Vector getNextAndChangePsiT(Vector corrects, Vector N0)
        {
            Vector Nnext = null;
            PsiTime tryPsiTime = new PsiTime(null);
            Console.WriteLine("\n\t\tПРИМЕНЯНЕМ ПОПРАВКИ:");
            var countxi = 0;
            do
            {
                var xi = getXi(countxi);
                do
                {
                    tryPsiTime = upgradePsiTime(corrects, xi);
                    countxi++;
                    if (countxi > 30) // сколько раз разрешается уменьшить ХИ
                    {
                        Console.WriteLine("\t\tВРЕМЯ T < 0 ДЛЯ Xi!");
                        Console.ReadKey();
                    }
                } while(tryPsiTime.T <= 0);
                Console.Write("\t\t -> XI={0} ", xi);
                Nnext = countN(tryPsiTime);
                countxi++;
                if (countxi > 30) // сколько раз разрешается уменьшить ХИ
                {
                    Console.WriteLine("\t\tНЕЛЬЗЯ УМЕНЬШИТЬ НОРМУ НЕВЯЗКИ!");
                    Console.ReadKey();
                }

            } while (Nnext.norm() > N0.norm());
            currentPsiTime = new PsiTime(tryPsiTime);
            Console.WriteLine("\t\tOK! ТЕПЕРЬ: " + currentPsiTime.ToString() + "\n\t\tНОРМА НЕВЯЗКИ ПОСЛЕ ИТЕРАЦИИ: " + Nnext.norm());
            Console.WriteLine("\t\tНОРМА УМЕНЬШИЛАСЬ НА " + (N0.norm() - Nnext.norm()));
            return Nnext;
            //showResults(Nnext, N0);
        }
        private PsiTime upgradePsiTime(Vector corrects, double xi)
        {
            PsiTime res = new PsiTime(currentPsiTime);
            for (int i = 0; i < 4; i++)
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
            Vector[] Nmass = new Vector[4];
            for (int i = 0; i < 4; i++)
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
            Matrix res = new Matrix((short) N0.length);
            for (int i = 0; i < res.length; i++)
            {
                for (int j = 0; j < res.length; j++)
                {
                    if (j == 0)
                    {
                        res[i, j] = (N[j, i] - N0[i]) / delT;
                    }
                    else
                    {
                        res[i, j] = (N[j, i] - N0[i]) / delPsi;
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
            Vector Nres = new Vector(4);
            // обращение к РК для получения кватерниона на конце
            // results = <hamilton, lambda>
            Tuple<double, Vector3> results = RungeKutta.Run(psiTimeRequest, callMain);

            Nres[0] = results.Item1; //  это значение функции гамильтона в полученной системе РК
            Vector3 diff = results.Item2 - callMain.OmegaT;
            
            for (int i = 0; i < 3; i++)
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
        private double getXi(int countXi)
        {
            double res;
            try
            {
                res = xi[countXi];
            }
            catch (Exception)
            {
                xi.Add(xi[countXi - 1] / 2);
                res = xi[countXi];
            }
            Console.WriteLine("return xi" +  res);
            return res;
        }
    }
}
