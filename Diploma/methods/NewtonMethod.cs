using System;
using Diploma.entity;

namespace Diploma.methods
{
    class NewtonMethod
    {        
        
        private MainProblem callMain;
        private int n; // кол-во шагов в методе РК
        Vector delta = new Vector(5);
        Vector vectPsiTime = new Vector(5);

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
            Console.WriteLine("%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%% Newton %%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%%");
            int newtonItaration = 0;
            do
            {
                Console.WriteLine("\n\t\t{0} Newton:", newtonItaration);
                while ( vectPsiTime[4] / callMain.N >= 0.001) // проверяем будет ли ШАГ >= 0.5
                {
                    callMain.N *= 2; // дробим количество шагов  если условие выполнилось
                }

                Quaternion resLambda = RungeKutta.Run(vectPsiTime, callMain); // обращение к метду РК

                Console.WriteLine("%%%%%%%%%%%%%%\nafter RK:");
                resLambda.printMagnitude();
                Console.WriteLine("Lambda t=0:");
                callMain.Lambda0.printMagnitude();
                

                // обращение к методам подсчета невязки, и пересчета поправки
                Quaternion difference = countR(resLambda, callMain.LambdaT);

                Console.WriteLine("result difference: ");
                difference.print();

                delta = delta; // поправки которые мы нашли
                newtonItaration++;

            } while (delta.norm() > callMain.Epsilon);
        }

        private Quaternion countR(Quaternion lambdaArpox, Quaternion lambdaExact) // считает невязку
        {
            Quaternion res = Quaternion.Abs(lambdaArpox - lambdaExact) ;
            return res;
        }

    }
}
