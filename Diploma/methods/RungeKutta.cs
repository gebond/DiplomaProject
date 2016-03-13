using System;
using System.Collections.Generic;
using Diploma.entity;

namespace Diploma.methods
{
    class RungeKutta
    {

        private static double h;
        
        public static Quaternion Run(Vector vectPsitime, MainProblem callMain)
        {
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] = Psi[0,1,2,3]
             * vect[4] == Time
             * */

            Console.WriteLine("\n\t\t* run RungeKutta...");
            double Time = vectPsitime[4];
            h = Time/callMain.N; // посчитали шаг


            Quaternion psi_k = callMain.Lambda0; 
            Quaternion lam_k = new Quaternion(vectPsitime[0], vectPsitime[1], vectPsitime[2], vectPsitime[3]);
            Quaternion omega_k = new Quaternion();

            for (int k = 0; k < callMain.N; k++)
            {
                // необходимо посчитать вектор3 omega
                // расчитываем все частные производные ф-ии H

                double dHdm1 = (1.0 / 2.0) * (- psi_k[0] * lam_k[1] + psi_k[1] * lam_k[0] + psi_k[2] * lam_k[3] - 
                                psi_k[3] * lam_k[2]);

                omega_k[1] = (dHdm1 >= 0 )? callMain.OmegaMax: callMain.OmegaMin;

                double dHdm2 = (1.0 / 2.0) * (psi_k[0] * lam_k[2] + psi_k[2] * lam_k[0] + psi_k[3] * lam_k[1] - 
                                psi_k[1] * lam_k[3]);

                omega_k[2] = (dHdm2 >= 0) ? callMain.OmegaMax : callMain.OmegaMin; 

                double dHdm3 = (1.0 / 2.0) * (psi_k[0] * lam_k[3] + psi_k[1] * lam_k[2] + psi_k[3] * lam_k[0] - 
                                psi_k[2] * lam_k[1]);

                omega_k[3] = (dHdm3 >= 0) ? callMain.OmegaMax : callMain.OmegaMin;
                /*
                 * составлен кватернион omega(opt) для текущего шага k
                 * 
                 * теперь необходимо посчитать коэффициенты к1...к4 для Р-К и получть следующее приближение
                 * c помощью метода calcNext
                 * */

                psi_k = RungeKutta.сalcNext(psi_k, omega_k, k);
                lam_k = RungeKutta.сalcNext(lam_k, omega_k, k);
            }
            
            return null;
        }

        /**
         * функция зависимости PSI(t), Lambda(t)
         * 
         * */
        private static Quaternion func(Quaternion x, Quaternion omeg, double t)
        {
            Quaternion res = (1.0/2.0) * t * (x % omeg);
            return res;
        }

        private static Quaternion сalcNext(Quaternion x, Quaternion omeg, int k)
        {
            Quaternion k1 = h * func(x, omeg, k*h);
            Quaternion k2 = h * func(x + (1.0 / 2.0) * k1, omeg, k * h + h / 2);
            Quaternion k3 = h * func(x + (1.0 / 2.0) * k2, omeg, k * h + h / 2);
            Quaternion k4 = h * func(x + k3, omeg, k * h + h);

            return x + (1.0 / 6.0) * (k1 + 2 * k2 + 2*k3 + k4);
        }


    }
}