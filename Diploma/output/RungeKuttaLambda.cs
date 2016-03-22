using System;
using System.Collections.Generic;
using Diploma.entity;

namespace Diploma.methods
{
    class RungeKuttaLambda
    {
        private static double h;
        private static double Umax;

        public static Quaternion Run(PsiTime psitime, MainProblem callMain)
        {
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] == Psi[0,1,2,3]
             * vect[4] == Time
             * */

            Console.Write("\n\t\t\t  ~~~ МЕТОД РУНГЕ КУТТЫ ПРИ ");
            double Time = psitime.T; // взято время Т
            h = Time / callMain.N; // посчитан шаг
            Umax = callMain.Umax;
            Console.WriteLine("" + psitime.ToString());
            Console.Write("\t\t\t  ~~~ n={0} h={1} ", callMain.N, h);

            Quaternion lam_k = callMain.Lambda0; // получено начальное lambda0
            Vector3 psi_0 = new Vector3(psitime.psi); // получен psi0


            for (int k = 0; k < callMain.N; k++)
            {
                // необходимо посчитать вектор3 omega
                // расчитываем все частные производные ф-ии H
                //Console.WriteLine("k={0}", k);

                /*
                 * составлен кватернион omega(opt) для текущего шага k
                 * 
                 * теперь необходимо посчитать коэффициенты к1...к4 для Р-К и получть следующее приближение
                 * c помощью метода calcNext
                 * */


                Quaternion lam_k_next = сalcNext(lam_k, psi_0, k);
                lam_k = new Quaternion(lam_k_next);


                //Console.Write("lamd_next = "); lam_k.print();
                //Console.Write("psi_next= "); psi_k.print();
                //Console.Write("omega_next = "); omega_k.print();
            }
            Console.WriteLine("\n\t\t\t  ~~~ РЕЗУЛЬТАТЫ:");
            Console.WriteLine("\t\t\t  ~~~ ПОЛУЧЕНО Lambda(T) = {0}\n\t\t\t  ~~~ БЫЛО Lambda(0) = {1}", lam_k.ToString(), callMain.Lambda0.ToString());

            return new Quaternion(lam_k);
        }

        /**
         * функция зависимости PSI(t), Lambda(t)
         * 
         * */
        private static Quaternion func(Quaternion lambda, Vector3 psi)
        {
            var omegOpt = new Quaternion(0, Umax * psi.getNormalize());
            Quaternion res = (1.0 / 2.0) * (lambda % omegOpt);
            return res;
        }
        private static Quaternion сalcNext(Quaternion x, Vector3 psi, int k)
        {
            Quaternion k1 = h * func(x, psi);
            Quaternion k2 = h * func(x + (1.0 / 2.0) * k1, psi);
            Quaternion k3 = h * func(x + (1.0 / 2.0) * k2, psi);
            Quaternion k4 = h * func(x + k3, psi);
            return x + (1.0 / 6.0) * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
        }
    }
}