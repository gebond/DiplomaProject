using System;
using System.Linq;
using System.Text;
using System.Collections.Generic;
using Diploma.entity;
using Diploma.methods;

namespace Diploma
{
    class Program
    {

        static void Main(string[] args)
        {
            Console.WriteLine("\t\t\tStart of application");

            Quaternion Quat0 = new Quaternion(1.0, 1.0, 1.0, 1.0);
            Quaternion QuatT = new Quaternion(1.0, 1.0, 2.0, 3.0);
            Quaternion psiStart = new Quaternion(1.0, 1.0, 1.0, 1.0);

            double epsilon = 0.01;
            int n = 100;
            double T_start = 10.0;
            IVector Omega = new Vector(new double[] {-1.0, 1.0});
            

            MainProblem main = new MainProblem(Quat0, QuatT, epsilon, psiStart, T_start, n, Omega);
            //main.start();
            //main.printCurrentParameters();

            //double[,] A = new double[,] {{0, 2, -1, 1}, {-1, 2, 4, 19}, {15, 1, 2, 1}, {4, 1, -5, 0}};
            //double[] b = new double[] {-1, 3, 15, 0};
            //double[,] A = new double[,] { { 1, 2, 4 , 3}, { 1, 3, -5 ,2}, { 7, 2, 0,9 } , {3, -2, 1, 2} };
            //double[] b = new double[] { 3.5, 5, -2.5 ,4};

            double[,] A = new double[,] { { 0.0001, 0 , -1}, { 0.3, 0.07 , 1.0015}, {-0.0056, 1.8, 0 }};
            double[] b = new double[] { 0.46, 0.18, 1.5};

            //double[,] A = new double[,] {{1, 2}, {1, 3}};
            //double[] b = new double[] {3, 2};

            //double[,] A = new double[1,1] {{2}};
            //double[] b = new double[] {3};
            
            SLAU newSlau = new SLAU(A, b, 0.001);
            newSlau.print();
            Vector res = newSlau.getResult();
            Console.WriteLine("\n\n\n%%%%%%%%%");
            res.print();

            Console.Write("\n\n\n\n\n\t\t\tEnter an key to exit... ");
            Console.ReadKey();
        }
    }
}
