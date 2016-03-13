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

        public MainProblem(Quaternion lam_0, Quaternion lam_t, double precision, Quaternion psi_start, int stepsRk, double T_start)
        {
            Console.WriteLine("\n\t* MainProblem created!");
            lambda0 = lam_0;
            lambdaT = lam_t;
            eps = precision;
            newtonMethod = new NewtonMethod(this, psi_start, stepsRk, T_start);
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
            Console.WriteLine("\t\tt=0:");
            Console.Write("\t\t");
            lambda0.print();
            Console.WriteLine("\t\tt=T:");
            Console.Write("\t\t");
            lambdaT.print();

        }


    }
}
