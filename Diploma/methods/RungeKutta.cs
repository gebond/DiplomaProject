using System;
using System.Collections.Generic;
using Diploma.entity;

namespace Diploma.methods
{
    class RungeKutta
    {
        private static double h;
        private static double Umax;
 
        public static Tuple<double, Vector3> Run(PsiTime psitime, MainProblem callMain)
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

            var omega_k = callMain.Omega0; // получено начальное omega0
            var psi_0 = new Vector3(psitime.psi); // получен psi0


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


                var omega_k_next = RungeKutta.сalcNext(psi_0, omega_k, k);
                omega_k = omega_k_next;


                //Console.Write("lamd_next = "); lam_k.print();
                //Console.Write("psi_next= "); psi_k.print();
                //Console.Write("omega_next = "); omega_k.print();
            }
            Console.WriteLine("\n\t\t\t  ~~~ РЕЗУЛЬТАТЫ:\n\t\t\t  ~~~ ");
            Console.WriteLine("\t\t\t  ~~~ ПОЛУЧЕНО Omega(T) = {0}\n\t\t\t  ~~~ ЗАДАЧА Omega(T) = {1}", omega_k.ToString(),  callMain.OmegaT.ToString());

            double resHamilton = Hamiltonian(psi_0, omega_k);
            return new Tuple<double, Vector3>(resHamilton, omega_k);
        }

        /**
         * функция зависимости PSI(t), Lambda(t)
         * 
         * */
        private static Vector3 func(Vector3 psi)
        {
            var res = Umax * psi.getNormalize();
            return res;
        }
        private static double Hamiltonian(Vector3 psi, Vector3 omegaopt)
        {
            return - 1.0 + psi * omegaopt;
        }
        private static Vector3 сalcNext(Vector3 psi, Vector3 omeg, int k)
        {
            var k1 = h * func(psi);
            var k2 = h * func(psi + (1.0 / 2.0) * k1);
            var k3 = h * func(psi + (1.0 / 2.0) * k2);
            var k4 = h * func(psi + k3);
            return omeg + (1.0 / 6.0) * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
        }
    }
}