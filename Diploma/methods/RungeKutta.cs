using System;
using System.Collections.Generic;
using Diploma.entity;

namespace Diploma.methods
{
    class RungeKutta
    {
        private static double h;
 
        public static Tuple<double, Quaternion> Run(PsiTime psitime, MainProblem callMain)
        {
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] == Psi[0,1,2,3]
             * vect[4] == Time
             * */

            Console.Write("\n\t\t\t  ~~~ МЕТОД РУНГЕ КУТТЫ ПРИ ");
            double Time = psitime.T; // взято время Т
            h = Time / callMain.N; // посчитан шаг
            Console.WriteLine("" + psitime.ToString());
            Console.Write("\t\t\t  ~~~ n={0} h={1} ", callMain.N, h);

            Quaternion lam_k = callMain.Lambda0; // получено начальное lam0
            var psi_k = new Vector3(psitime.psi); // получен psi0

            /*Quaternion omega_k = new Quaternion();

            for (int k = 0; k < callMain.N; k++)
            {
                // необходимо посчитать вектор3 omega
                // расчитываем все частные производные ф-ии H
                //Console.WriteLine("k={0}", k);
                omega_k[0] = 0;

                double dHdm1 = (1.0 / 2.0) * (0 - psi_k[0] * lam_k[1] + psi_k[1] * lam_k[0] + psi_k[2] * lam_k[3] - 
                                psi_k[3] * lam_k[2]);

                omega_k[1] = (dHdm1 >= 0 )? callMain.OmegaMax: callMain.OmegaMin;

                double dHdm2 = (1.0 / 2.0) * (0 - psi_k[0] * lam_k[2] + psi_k[2] * lam_k[0] + psi_k[3] * lam_k[1] - 
                                psi_k[1] * lam_k[3]);

                omega_k[2] = (dHdm2 >= 0) ? callMain.OmegaMax : callMain.OmegaMin;

                double dHdm3 = (1.0 / 2.0) * (0 - psi_k[0] * lam_k[3] + psi_k[1] * lam_k[2] + psi_k[3] * lam_k[0] - 
                                psi_k[2] * lam_k[1]);

                omega_k[3] = (dHdm3 >= 0) ? callMain.OmegaMax : callMain.OmegaMin;

                /*
                 * составлен кватернион omega(opt) для текущего шага k
                 * 
                 * теперь необходимо посчитать коэффициенты к1...к4 для Р-К и получть следующее приближение
                 * c помощью метода calcNext
                 * 


                Quaternion psi_k_next = RungeKutta.сalcNext(psi_k, omega_k, k);
                Quaternion lam_k_next = RungeKutta.сalcNext(lam_k, omega_k, k);*/

                psi_k = psi_k_next;
                lam_k = lam_k_next;


                //Console.Write("lamd_next = "); lam_k.print();
                //Console.Write("psi_next= "); psi_k.print();
                //Console.Write("omega_next = "); omega_k.print();
            }
            Console.WriteLine("\n\t\t\t  ~~~ РЕЗУЛЬТАТЫ:\n\t\t\t  ~~~ НОРМА ПОСЛЕ: {0} НОРМА ДО: {1}", lam_k.getMagnitude(), callMain.Lambda0.getMagnitude());
            Console.WriteLine("\t\t\t  ~~~ ПОЛУЧЕНО lambda(T) = {0}\n\t\t\t  ~~~ ЗАДАЧА lambda(T) = {1}", lam_k.ToString(), callMain.LambdaT.ToString());

            double resHamilton = Hamiltonian(psi_k, omega_k, lam_k);
            return new Tuple<double, Quaternion>(resHamilton, lam_k);
        }

        /**
         * функция зависимости PSI(t), Lambda(t)
         * 
         * */
        private static Vector3 func(Vector3 x, Vector3 omeg)
        {
            var res = (1.0 / 2.0) * (x % omeg);
            return res;
        }
        private static double Hamiltonian(Quaternion psi, Quaternion omegaopt, Quaternion lambda)
        {
            return - 1.0 + psi / (0.5 * (lambda % omegaopt));
        }
        private static Quaternion сalcNext(Quaternion x, Quaternion omeg, int k)
        {
            Quaternion k1 = h * func(x, omeg);
            Quaternion k2 = h * func(x + (1.0 / 2.0) * k1, omeg);
            Quaternion k3 = h * func(x + (1.0 / 2.0) * k2, omeg);
            Quaternion k4 = h * func(x + k3, omeg);
            return x + (1.0 / 6.0) * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
        }
    }
}