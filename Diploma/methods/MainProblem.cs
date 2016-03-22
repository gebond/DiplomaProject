using System;
using System.Collections.Generic;
using Diploma.entity;
using Diploma.methods;

namespace Diploma.methods
{
    class MainProblem
    {

        private Quaternion lambda0;
        public Quaternion Lambda0
        {
            get { return lambda0; }
        }
        private Vector3 omega0; //  вектор3 при t=0
        public Vector3 Omega0
        {
            get{return omega0;}
        }
        private Vector3 omegaT; //  вектор3 конечный
        public Vector3 OmegaT
        {
            get { return omegaT; }
        }
        private double umax;
        public double Umax
        {
            get{return umax;}
        }
        private double eps; // epsilon - точность
        public double Epsilon
        {
            get { return eps; }
        }
        private int n;
        public int N
        {
            get { return n; }
            set { n = value; }
        }
    
        // САМОЕ ВАЖОЕ - ТЕКУЩИЙ ВЕКТОР PSITIME
        private PsiTime psiTime;

        private NewtonMethod newtonMethod;

        public MainProblem(Vector3 omega_0, Vector3 omega_t, Vector3 psi, Quaternion lam_0,  double T, double U_max, int n, double precision)
        {
            Console.WriteLine("\n\t* MainProblem created!");
            omega0 = new Vector3(omega_0);
            omegaT = new Vector3(omega_t);
            lambda0 = new Quaternion(lam_0);
            eps = precision;
            N = n;
            psiTime = new PsiTime(psi, T);
            umax = U_max;
        }

        public void start()
        {
            Console.WriteLine("\n\t* MainProblem started!");
            printCurrentParameters();
            newtonMethod.RunProcess();
        }

        public void printCurrentParameters()
        {
            Console.WriteLine("\n\t* MainProblem ->");
            Console.WriteLine("\n\tCurrent Parameters:");
            Console.WriteLine("\t\tepsilon = {0}", eps);
            Console.WriteLine("\t\tn = {0}", n);
            Console.WriteLine("\t\tt=0:");
            Console.Write("\t\t"); omega0.print();
            Console.Write("\t\t"); omega0.norm();
            Console.WriteLine("\t\tt=T:");
            Console.Write("\t\t"); omegaT.print();
            Console.Write("\t\t T = {0}, PSI = ", psiTime.T); psiTime.psi.print();  
        }


    }
}
