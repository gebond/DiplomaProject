using System;
using System.Collections.Generic;
using Diploma.entity;
using Diploma.methods;

namespace Diploma.methods
{
    class MainProblem
    {
        private Quaternion lambda_0; // начальный кватернион
        private Quaternion lambda_t; // конечный кватернион

        private Quaternion curLambda; // текущий кватернион
        private Quaternion curPsi; // текущее пси


        public MainProblem(Quaternion lam_0, Quaternion lam_t)
        {
            lambda_0 = lam_0;
            lambda_t = lam_t;
        }

        public void start()
        {

        }

        public void printCurrentParameters()
        {
            Console.WriteLine("\tCurrent Parameters:");
            Console.WriteLine("t=0:");
            lambda_0.print();
            Console.WriteLine("t=T:");
            lambda_t.print();
        }


    }
}
