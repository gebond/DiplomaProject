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

            Console.Write("\n\t\t\t * RK ");
            double Time = psitime.T; // взято время Т
            h = Time / callMain.N; // посчитан шаг
            Console.Write("N = {0}, h = {1}, T = {2} \n", callMain.N, h, Time);
            Console.Write("\t\t\tPSI :"); psitime.psi.print();

            Quaternion lam_k = callMain.Lambda0; // получено начальное lam0
            Quaternion psi_k = new Quaternion(psitime.psi); // получен psi0
            Quaternion omega_k = new Quaternion();

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
                 * */


                Quaternion psi_k_next = RungeKutta.сalcNext(psi_k, omega_k, k);
                Quaternion lam_k_next = RungeKutta.сalcNext(lam_k, omega_k, k);

                psi_k = psi_k_next;
                lam_k = lam_k_next;


                //Console.Write("lamd_next = "); lam_k.print();
                //Console.Write("psi_next= "); psi_k.print();
                //Console.Write("omega_next = "); omega_k.print();
            }
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            Console.WriteLine("norm after: {0}", lam_k.getMagnitude());

            double resHamilton = Hamiltonian(psi_k, omega_k, lam_k);
            Console.WriteLine("Hamilton = " + resHamilton);
            Console.Write("ResLambda = "); lam_k.print();
            Console.Write("Difference = "); (lam_k - callMain.LambdaT).print();
            Console.WriteLine("~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~~");
            return new Tuple<double, Quaternion>(resHamilton, lam_k);
        }

        /**
         * функция зависимости PSI(t), Lambda(t)
         * 
         * */
        private static Quaternion func(Quaternion x, Quaternion omeg)
        {
            Quaternion res = (1.0 / 2.0) * (x % omeg);
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