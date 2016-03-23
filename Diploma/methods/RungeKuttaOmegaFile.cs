using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Diploma.entity;

namespace Diploma.methods
{
    class RungeKuttaOmegaFile
    {
        private static double h;
        private static double htime; // шаг для интервала Т для вывода в файл
        private static double Umax;
 
        public static void Run(PsiTime psitime, MainProblem callMain)
        {
            /**
             * Vector vectPsiTime = [Quaternion Psi, Time]
             * vect[0,1,2,3] == Psi[0,1,2,3]
             * vect[4] == Time
             * */

            System.Threading.Thread.CurrentThread.CurrentCulture = new System.Globalization.CultureInfo("en-US");
            Console.Write("\n\t\t\t  ~~~ МЕТОД РУНГЕ КУТТЫ ДЛЯ ФАЙЛА ПРИ ");
            double Time = psitime.T; // взято время Т
            h = Time / callMain.N; // посчитан шаг
            htime = (double) Time / 100.0; // шаг для вывода
            Umax = callMain.Umax;
            Console.WriteLine("" + psitime.ToString());
            Console.Write("\t\t\t  ~~~ n={0} h={1} ", callMain.N, h);
            Console.Write("\n\t\t\t  ~~~ htime={0} ", htime);

            Vector3 omega_k = callMain.Omega0; // получено начальное omega0
            Vector3 psi_0 = new Vector3(psitime.psi); // получен psi0

            int ktime = 0;
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



                Vector3 omega_k_next = сalcNext(psi_0, omega_k);
                if (ktime * htime == k * h) // когда совпал шаг по 
                {
                    Console.Write("\nK={0} ktime={1} omega={2}", k, ktime, omega_k.ToString());
                    var clear = (ktime == 0) ? true : false;
                    printOutput(omega_k, clear);
                    printTime(ktime * htime, clear);
                    ktime++;
                }
                omega_k = new Vector3(omega_k_next);
                

                //Console.Write("lamd_next = "); lam_k.print();
                //Console.Write("psi_next= "); psi_k.print();
                //Console.Write("omega_next = "); omega_k.print();
            }
            Console.WriteLine("\n\t\t\t  ~~~ РЕЗУЛЬТАТЫ:");
            Console.WriteLine("\t\t\t  ~~~ ПОЛУЧЕНО Omega(T) = {0}\n\t\t\t  ~~~ ЗАДАЧА Omega(T) = {1}", omega_k.ToString(),  callMain.OmegaT.ToString());

            double resHamilton = Hamiltonian(psi_0, func(psi_0));
            //return new Tuple<double, Vector3>(resHamilton, omega_k);
        }

        /**
         * функция зависимости PSI(t), Lambda(t)
         * 
         * */
        private static void printOutput(Vector3 omegaToPrint, bool clear)
        {
            var dirPath1 = @"D:\Develop\Diploma\Diploma\output\omega\omega1.txt";
            using (StreamWriter sw = new StreamWriter(dirPath1, !clear, System.Text.Encoding.Default))
            {
                sw.Write(omegaToPrint.X + "; ");
            } 
            var dirPath2 = @"D:\Develop\Diploma\Diploma\output\omega\omega2.txt";
            using (StreamWriter sw = new StreamWriter(dirPath2, !clear, System.Text.Encoding.Default))
            {
                sw.Write(omegaToPrint.Y + "; ");
            }
            var dirPath3 = @"D:\Develop\Diploma\Diploma\output\omega\omega3.txt";
            using (StreamWriter sw = new StreamWriter(dirPath3, !clear, System.Text.Encoding.Default))
            {
                sw.Write(omegaToPrint.Z + "; ");
            }
        }
        private static void printTime(double Time, bool clear)
        {
            var dirPath1 = @"D:\Develop\Diploma\Diploma\output\time.txt";
            using (StreamWriter sw = new StreamWriter(dirPath1, !clear, System.Text.Encoding.Default))
            {
                sw.Write(Time + "; ");
            }
        }
        private static Vector3 func(Vector3 psi)
        {
            var res = Umax * psi.getNormalize();
            return res;
        }
        private static double Hamiltonian(Vector3 psi, Vector3 omegaopt)
        {
            return - 1.0 + psi * omegaopt;
        }
        private static Vector3 сalcNext(Vector3 psi, Vector3 omeg)
        {
            var k1 = h * func(psi);
            var k2 = h * func(psi + (1.0 / 2.0) * k1);
            var k3 = h * func(psi + (1.0 / 2.0) * k2);
            var k4 = h * func(psi + k3);
            return omeg + (1.0 / 6.0) * (k1 + 2.0 * k2 + 2.0 * k3 + k4);
        }
    }
}
