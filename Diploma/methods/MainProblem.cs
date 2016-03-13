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

        private Quaternion lambdaResult;
        public Quaternion LambdaResult
        {
            set 
            {
                lambdaResult = value; 
                Console.WriteLine("\n\t* MainProblem ->");
                Console.WriteLine("\n\tCurrent Result:");
                Console.Write("\t\t");
                lambdaResult.print();
            }
        }


        private NewtonMethod newtonMethod;

        public MainProblem(Quaternion lam_0, Quaternion lam_t, double precision, Quaternion psi_start, double T_start, int n)
        {
            Console.WriteLine("\n\t* MainProblem created!");
            lambda0 = lam_0;
            lambdaT = lam_t;
            eps = precision;
            N = n;
            newtonMethod = new NewtonMethod(this, psi_start, T_start);
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
            Console.Write("\t\t");
            lambda0.print();
            Console.WriteLine("\t\tt=T:");
            Console.Write("\t\t");
            lambdaT.print();

        }


    }
}
