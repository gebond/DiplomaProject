using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Diploma.entity
{
    public class Matrix
    {
        private short dim;
        private double[,] matr;

        public Matrix(short dimension)
        {
            dim = dimension;
            matr = new double[dim, dim];
        }
        public Matrix(double[,] mass)
        {   
            dim = (short)Math.Sqrt(mass.Length);
            matr = new double[dim, dim];
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
            matr = new double[m.length, m.length];
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
        public double this[int i, int j]
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
        public static Matrix operator *(double a, Matrix obj)
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
            IVector result ;

            if(vec.length == 3)
            {
                 result = new Vector3();
            }
            else
            {
                result = new Vector(vec.length);
            }

            if (vec.length != mat.length)
            {
                Console.WriteLine("допускается перемножение только квадратных матриц на вектор одной размерности");
                System.Environment.Exit(0);
            }
            for (int i = 0; i < vec.length; i++)
            {
                double summ = 0;
                for (int j = 0; j < vec.length; j++)
                {
                    summ += mat[i, j] * vec[j];
                }
                result[i] = summ;
            }
            return result;
        }

        public double getNorm()
        {
            double norm = 0;
            for (int i = 0; i < length; i++)
            {
                double summ = 0;
                for (int j = 0; j < length; j++)
                {
                    summ += Math.Abs(matr[i, j]);
                }
                if (summ > norm)
                {
                    norm = summ;
                }
            }
            return norm;
        }
        public bool notNoolDiag()
        {
            bool mark = true;
            for (int i = 0; i < dim; i++)
            {
                if (matr[i, i] == 0)
                {
                    mark = false;
                }
            }
            return mark;
        }
        public double getMaxAboveDiag()
        {
            double max = 0;
            for (int i = 0; i < length; i++)
            {
                for (int j = i + 1; j < length; j++)
                {
                    if (Math.Abs(matr[i, j]) > max)
                    {
                        max = Math.Abs(matr[i, j]);
                    }
                }
            }
            return max;
        }
        public double getDeterminant()
        {
            Matrix chg = new Matrix(length);
            for (int i = 0; i < length; i++)
            {
                for (int j = 0; j < length; j++)
                {
                    chg[i, j] = matr[i, j];
                }
            }

            double det = 1;
            for (int k = 0; k < length; k++)
            {
                for (int i = k + 1; i < length; i++)
                {
                    for (int j = k + 1; j < length; j++)
                    {
                        chg[i, j] = chg[i, j] + chg[k, j] * (-chg[i, k] / chg[k, k]);
                    }
                    chg[i, k] = 0;
                }
            }
            for (int i = 0; i < length; i++)
            {
                det *= chg[i, i];
            }
            return det;
        }
        public IVector getRowAsVector(int line)
        {
            IVector x;
            if(dim == 3)
            {
                x = new Vector3();
            }
            else
            {
                x = new Vector(dim);
            }
            for (int j = 0; j < length; j++)
            {
                x[j] = matr[line, j];
            }
            return x;
        }
        
    }
}
