using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class SLAU
    {
        Vector b; // Вектор свободных членов
        Matrix A; // Коэффициенты СЛАУ 
        Vector x; // решение СЛАУ Ax = b
        Stack<int> logChanges;


        private int dim;
        private double eps;

        public SLAU(double[,] massA, double[] massb, double epsilon)
        {
            eps = epsilon;
            dim = massb.Length;
            A = new Matrix(massA);
            b = new Vector(massb);
            x = new Vector(dim);
            logChanges = new Stack<int>();
            if ((dim * dim != massA.Length))
            {
                throw new Exception("try to solve different dimension!");
            }
        }
        public SLAU(Matrix A, Vector b, double epsilon)
        {
            eps = epsilon;
            dim = A.length;
            this.A = new Matrix(A);
            this.b = new Vector(b);
            x = new Vector(dim);
            logChanges = new Stack<int>();
            if ((dim != A.length))
            {
                throw new Exception("try to solve different dimension!");
            }
        }

        public Vector getResult() 
        {
            solve();
            if(checkResult())
            {
                
                return x;
            }
            else
            {
                Console.WriteLine("");
                Console.ReadKey();
                Environment.Exit(1);
                return null;
            }
        }
        private bool checkResult()
        {
            bool correct = true;
            for (int i = 0; i < dim; i++)
            {
                if(x[i] == null )
                {
                    correct = false;
                }
            }
            return correct;
        }
        private void solve()
        {
           
            for (int k = 0; k < dim; k++)
			{
                findMax(k, k); // нашли главный элемент и заменили столбцы и строки друг на друга, запомнили в стек замену строк
                
                for (int i = k + 1; i < dim; i++)
                {
                    double q_i = (double) A[i, k] / A[k, k];
                    for (int j = k; j < dim; j++)
                    {
                        A[i, j] = A[i, j] - q_i * A[k, j];
                    }
                    b[i] = b[i] - q_i * b[k];        
                }
			}
            // далее обратный ход
            for (int i = dim - 1; i > -1; i--)
            {
                if (i == dim - 1)
                {
                    x[i] = b[i] / A[i, i];
                    continue;
                }
                double sum = 0;
                for (int j = dim - 1; j > i; j--)
                {
                    sum += x[j] * A[i, j];
                }
                x[i] = (b[i] - sum ) / A[i, i];
            }

            //необходимо вернуть порядок столбцов в векторе х
            var currentIndex = dim - 1;
            foreach (var item in logChanges)
            {
                if (item != -1)
                {
                    var tmp = x[currentIndex];
                    x[currentIndex] = x[item];
                    x[item] = tmp;
                }
                currentIndex--;
            }
        }
        
        private void findMax(int cur_i, int cur_j) 
        {
            int max_i = cur_i;
            int max_j = cur_j;

            for (int i = cur_i; i < dim; i++)
            {
                for (int j = cur_j; j < dim; j++)
                {
                    if (Math.Abs(A[i, j]) > Math.Abs(A[max_i, max_j]))
                    {
                        max_i = i;
                        max_j = j;
                    }
                    
                }
            }
            if (cur_i == max_i)
            {
                if (cur_j == max_j)
                {
                    logChanges.Push(-1);
                }
                else
                {
                    changeTab(cur_j, max_j);
                    logChanges.Push(max_j);
                }
            }
            else
            {
                changeRow(cur_i, max_i);
                changeTab(cur_j, max_j);
                logChanges.Push(max_j);
            }
        }
        private void changeRow(int up_i, int down_i) 
        {
            double tmpb = b[up_i];
            b[up_i] = b[down_i];
            b[down_i] = tmpb; 

            for (int j = 0; j < dim; j++)
            {
                double temp = A[up_i, j];
                A[up_i, j] = A[down_i, j];
                A[down_i, j] = temp;
            }
        }
        private void changeTab(int left_j, int  right_j)
        {
            for (int i = 0; i < dim; i++)
            {
                double temp = A[i,left_j];
                A[i, left_j] = A[i, right_j];
                A[i, right_j] = temp;
            }
        }

        public string ToString(int tabAmount)
        {
            try
            {
                String tab = "";
                for (int i = 0; i < tabAmount + 1; i++)
                {
                    tab += '\t';
                }

                String result = "";
                for (int i = 0; i < dim; i++)
                {
                    result += tab;
                    for (int j = 0; j < dim; j++)
                    {
                        result += String.Format("{0,20:0.00000000}   ", A[i, j]);
                        if (j == dim - 1)
                        {
                            result += String.Format("|{0,15:0.00000000}", b[i]);
                        }
                    }
                    result += '\n';
                }
                return result;
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Empty some massive!");
                return null;
            }
        }

        public void print() 
        {
            try
            {
            }
            catch (NullReferenceException)
            {
                Console.WriteLine("Empty some massive!");
            }
        }
    }
}