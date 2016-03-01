using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Matrix
    {
        private short dim;
        private decimal[,] matr;

        public Matrix(short dimension)
        {
            dim = dimension;
            matr = new decimal[dim, dim];
        }
        public Matrix(decimal[,] mass)
        {   
            dim = (short)Math.Sqrt(mass.Length);
            matr = new decimal[dim, dim];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    matr[i, j] = mass[i, j];
                }
            }
        }
        public Matrix(Matrix m)
        {
            matr = new decimal[m.length, m.length];
            for (int i = 0; i < dim; i++)
            {
                for (int j = 0; j < dim; j++)
                {
                    matr[i, j] = m[i, j];
                }
            }
        }

        public void print()
        {
            try
            {  
                for (int i = 0; i < dim; i++)
                {   
                    if(i == 0)
                    {
                        System.Console.WriteLine("Matrix:");
                    }

                    for (int j = 0; j < dim; j++)
                    {
                        if (j == 0)
                        {
                            System.Console.Write("\t");
                        }
                        Console.Write("\t {0:0.000}", matr[i, j]);
                    }
                    Console.WriteLine("\n");
                }
            }
            catch(NullReferenceException)
            {
                Console.WriteLine("Empty Vect massive!");
            }
        }
        public short length
        {
            get { return dim; }
        }
        public decimal this[int i, int j]
        {
            get{ return matr[i,j]; }
            set { matr[i,j] = value; }
        }

        public static Matrix operator +(Matrix obj1, Matrix obj2)
        {
            Matrix summ = new Matrix(obj1.length);
            for (int i = 0; i < obj1.length; i++)
            {
                for (int j = 0; j < obj1.length; j++)
                {
                    summ[i, j] = obj1[i, j] + obj2[i, j];
                }
            }
            return summ;
        }
        public static Matrix operator -(Matrix obj1, Matrix obj2)
        {
            Matrix summ = new Matrix(obj1.length);
            for (int i = 0; i < obj1.length; i++)
            {
                for (int j = 0; j < obj1.length; j++)
                {
                    summ[i, j] = obj1[i, j] - obj2[i, j];
                }
            }
            return summ;
        }
        public static Matrix operator *(decimal a, Matrix obj)
        {
            Matrix result = new Matrix(obj.length);
            for (int i = 0; i < obj.length; i++)
            {
                for (int j = 0; j < obj.length; j++)
                {
                    result[i, j] = a * obj[i, j];
                }
            }
            return result;
        }
        public static Matrix operator *(Matrix obj1, Matrix obj2)
        {
            Matrix result = new Matrix(obj1.length);
            if (obj1.length != obj2.length)
            {
                Console.WriteLine("допускается перемножение только квадратных матриц одной размерности");
                System.Environment.Exit(0);
            }
            for (int i = 0; i < obj1.length; i++)
            {
                for (int j = 0; j < obj1.length; j++)
                {
                    for (int k = 0; k < obj1.length; k++)
                    {
                        result[i, j] += obj1[i, k] * obj2[k, j];
                    }
                }
            }
            return result;
        }
        public static IVector operator *(Matrix mat, IVector vec)
        {
            IVector result = new Vector(vec.length);
            if (vec.length != mat.length)
            {
                Console.WriteLine("допускается перемножение только квадратных матриц на вектор одной размерности");
                System.Environment.Exit(0);
            }
            for (int i = 0; i < vec.length; i++)
            {
                decimal summ = 0;
                for (int j = 0; j < vec.length; j++)
                {
                    summ += mat[i, j] * vec[j];
                }
                result[i] = summ;
            }
            return result;
        }

    }
}
