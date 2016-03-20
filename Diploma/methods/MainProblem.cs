using System;
using System.Collections.Generic;
using Diploma.entity;
using Diploma.methods;

namespace Diploma.methods
{
    class MainProblem
    {
        private Quaternion lambda0; //  кватернион при t=0
        public Quaternion Lambda0
        {
            get{return lambda0;}
        }
        private Quaternion lambdaT; // кватернион при t=T
        public Quaternion LambdaT
        {
            get { return lambdaT; }
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

        private double omega_max;
        public double OmegaMax
        {
            get { return omega_max; }
        }
        private double omega_min;
        public double OmegaMin
        {
            get { return omega_min; }
        }
    
        // САМОЕ ВАЖОЕ - ТЕКУЩИЙ ВЕКТОР PSITIME
        private PsiTime psiTime;

        private NewtonMethod newtonMethod;

        public MainProblem(Quaternion lam_0, Quaternion lam_t, Quaternion psi, double T, int n, IVector omega, double precision)
        {
            Console.WriteLine("\n\t* MainProblem created!");
            lambda0 = lam_0;
            lambdaT = lam_t;
            eps = precision;
            N = n;
            omega_min = omega[0];
            omega_max = omega[1];
            psiTime = new PsiTime(psi, T);
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
            Console.Write("\t\t"); lambda0.print();
            Console.Write("\t\t"); lambda0.printMagnitude();
            Console.WriteLine("\t\tt=T:");
            Console.Write("\t\t"); lambdaT.print();
            Console.Write("\t\t T = {0}, PSI = ", psiTime.T); psiTime.psi.print();  
        }


    }
}
