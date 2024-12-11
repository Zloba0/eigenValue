using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace eigenValue
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox5.Text = "";
            int n = 0;
            try
            {
                n = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Input dimension", "Worning");
                return;
            }
            double eps = 0;
            try
            {
                eps = Convert.ToDouble(textBox4.Text);
            }
            catch
            {
                MessageBox.Show("Input infelicity", "Worning");
                return;
            }
            double[,] matr = new double[n, n];
            try
            {
                string[] line;
                line = textBox1.Lines;
                for (int i = 0; i < n; i++)
                {
                    int j = 0;
                    string subLine = "";
                    for (int g = 0; g < line[i].Length; g++)
                    {
                        if (line[i][g] != ' ')
                        {
                            subLine += line[i][g];
                        }
                        else
                        {
                            matr[i, j] = Convert.ToDouble(subLine);
                            j++;
                            subLine = "";
                        }
                    }
                    matr[i, j] = Convert.ToDouble(subLine);
                    j++;
                    if (j != n)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Wrong input of matrix", "Worning");
                return;
            }
            if (!IsSimetric(matr, n))
            {
                MessageBox.Show("Matrix is not simetric", "Worning");
                return;
            }
            if (!IsPositiveDefinite(matr, n))
            {
                MessageBox.Show("Matrix is not positive definite", "Worning");
                return;
            }
            double[] x0 = new double[n];
            double[] xNext = new double[n];
            double lambda = 0;
            for (int i = 0; i < n; i++)
            {
                x0[i] = 1;
                xNext[i] = 1;
            }
            lambda = SkolarCmposition(Cmposition(matr, x0, n), x0, n);
            double lambdaNext = lambda;
            int iteration = 0;
            do
            {
                lambda = lambdaNext;
                x0 = xNext;
                xNext = DivisionByConstant(Cmposition(matr, x0, n), Norma(Cmposition(matr, x0, n), n), n);
                lambdaNext = SkolarCmposition(Cmposition(matr, xNext, n), xNext, n);
                iteration++;
            } while (Math.Abs(lambdaNext - lambda) >= eps);
            textBox3.Text = $"{lambda}";
            textBox5.Text = $"{iteration}";
        }
        public bool IsSimetric(double[,] matr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    if (matr[i, j] != matr[j, i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool IsPositiveDefinite(double[,] matr, int n)
        {
            for (int n1 = 1; n1 <= n; n1++)
            {
                double[,] matr1 = new double[n1, n1];
                for (int i = 0; i < n1; i++)
                {
                    for (int j = 0; j < n1; j++)
                    {
                        matr1[i, j] = matr[i, j];
                    }
                }
                if (Det(matr1, n1) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        public double Det(double[,] matr, int n)
        {
            if (n == 1)
            {
                return matr[0, 0];
            }
            if (n == 2)
            {
                return matr[0, 0]*matr[1, 1] - matr[0, 1] * matr[1, 0];
            }
            double res = 0;
            bool pozitiv = true;
            for (int i = 0; i < n; i++)
            {
                double[,] matr1 = new double[n-1, n-1];
                int i1 = 0;
                for (int j = 0; j < n; j++)
                {
                    if (j==i)
                    {
                        continue;
                    }
                    int j1 = 0;
                    for (int k = 1; k < n; k++)
                    {
                        matr1[i1, j1] = matr[j, k];
                        j1++;
                    }
                    i1++;
                }
                res += matr[i, 0] * Det(matr1, n-1) *(pozitiv ? 1 : -1);
                pozitiv = !pozitiv;
            }
            return res;
        }
        public double[] Cmposition(double[,] matr, double[] x, int n)
        {
            double[] newX = new double[n];
            for (int i = 0; i < n; i++)
            {
                double sum = 0;
                for (int j = 0; j < n; j++)
                {
                    sum += matr[i, j]*x[j];
                }
                newX[i] = sum;
            }
            return newX;
        }
        public double Norma(double[] x, int n)
        {
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                result += x[i]*x[i];
            }
            return Math.Sqrt(result);
        }
        public double[] DivisionByConstant(double[] x, double k, int n)
        {
            for (int i = 0; i < n; i++)
            {
                x[i] /= k;
            }
            return x;
        }
        public double SkolarCmposition(double[] x, double[] y, int n)
        {
            double result = 0;
            for (int i = 0; i < n; i++)
            {
                result += x[i]*y[i];
            }
            return result;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = $"2 2 -2{Environment.NewLine}2 5 -4{Environment.NewLine}-2 -4 5";
            textBox2.Text = "3";
            textBox4.Text = "0,0001";
        }

        private void button2_Click(object sender, EventArgs e)
        {
            textBox3.Text = "";
            textBox5.Text = "";
            textBox6.Text = "";
            int n = 0;
            try
            {
                n = Convert.ToInt32(textBox2.Text);
            }
            catch
            {
                MessageBox.Show("Input dimension", "Worning");
                return;
            }
            double[,] matr = new double[n, n];
            try
            {
                string[] line;
                line = textBox1.Lines;
                for (int i = 0; i < n; i++)
                {
                    int j = 0;
                    string subLine = "";
                    for (int g = 0; g < line[i].Length; g++)
                    {
                        if (line[i][g] != ' ')
                        {
                            subLine += line[i][g];
                        }
                        else
                        {
                            matr[i, j] = Convert.ToDouble(subLine);
                            j++;
                            subLine = "";
                        }
                    }
                    matr[i, j] = Convert.ToDouble(subLine);
                    j++;
                    if (j != n)
                    {
                        throw new Exception();
                    }
                }
            }
            catch
            {
                MessageBox.Show("Wrong input of matrix", "Worning");
                return;
            }
            if (!IsSimetric(matr, n))
            {
                MessageBox.Show("Matrix is not simetric", "Worning");
                return;
            }
            if (!IsPositiveDefinite(matr, n))
            {
                MessageBox.Show("Matrix is not positive definite", "Worning");
                return;
            }
            double[,] newA = matr;
            for (int i = 1; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    double Cos = CosFi(i,j,matr);
                    double Sin = SinFi(i,j, matr);
                    newA = A(B(matr, n, Cos, Sin, i, j), n, Cos, Sin, i, j);
                    
                }
            }
            double[,] ext = ToExtend(newA, n);
            double[] lyamda = RunMethod(ext, n);

            textBox6.Text += $"D = {D(lyamda[0], newA, n)}";
            textBox6.AppendText(Environment.NewLine);
            Output(lyamda, n);
        }
        public double[,] ToExtend(double[,] matr, int n)
        {
            double[,] ext = new double[n,n+1];
            for(int i = 0; i < n; i++)
            {
                for(int j = 0; j < n; j++)
                {
                    if(j == n)
                    {
                        ext[i,j] = 0;
                    }
                    else
                    {
                        ext[i,j] = matr[i,j];
                    }
                }
            }
            return ext;
        }
        //public double[,] Multiplex(double[,] a, double[,] b, int n)
        //{
        //    double[,] res = new double[n, n];
        //    for (int i = 0; i < n; i++)
        //    {
        //        for (int j = 0; j < n; j++)
        //        {
        //            double sum = 0;
        //            for (int i1 = 0; i1 < n; i1++)
        //            {
        //                sum += a[i, i1] * b[i1, j];
        //            }
        //            res[i, j] = sum;
        //        }
        //    }
        //    return res;
        //}
        public void Output(double[,] matr, int n)
        {
            textBox3.Text = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textBox6.Text += $"{matr[i, j]}\t";
                }
                textBox6.AppendText(Environment.NewLine);
            }
        }
        public void Output(double[] matr, int n)
        {
            textBox3.Text = "";
            for (int i = 0; i < n; i++)
            {

                textBox6.Text += $"Lyamda{i+1} = {matr[i]}";
                textBox6.AppendText(Environment.NewLine);
            }
        }
        public double CosFi(int l, int m, double[,] a)
        {
            return a[l-1, l]/(Math.Sqrt(a[l-1, l]*a[l-1, l] + a[l-1, m]*a[l-1, m]));
        }
        public double SinFi(int l, int m, double[,] a)
        {
            return a[l-1, m]/(Math.Sqrt(a[l-1, l]*a[l-1, l] + a[l-1, m]*a[l-1, m]));
        }
        //public (double[] l1, double[] m1) B(double[,] a, int n, double Cos, double Sin, int l, int m)
        //{
        //    (double[] l1, double[] m1) b = (new double[n], new double[n]);
        //    for (int j = 1; j < n; j++)
        //    {
        //        b.l1[j] = a[l, j]*Cos + a[m, j]*Sin;
        //        b.m1[j] = -a[l, j]*Sin + a[m, j]*Cos;
        //    }
        //    return b;
        //}
        public double[,] B(double[,] a, int n, double Cos, double Sin, int l, int m)
        {
            double[,] b = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for(int j = 0;j < n; j++)
                {
                    if (i!= l && i!= m)
                    {
                        b[i, j] = a[i, j];
                    }
                }
            }
            for (int j = 0; j < n; j++)
            {
                b[l, j] = a[l, j]*Cos + a[m, j]*Sin;
                b[m, j] = -a[l, j]*Sin + a[m, j]*Cos;
            }
            return b;
        }
        public double[,] A(double[,] b, int n, double Cos, double Sin, int l, int m)
        {
            double[,] aNew = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    if (j!= l && j!= m)
                    {
                        aNew[i, j] = b[i, j];
                    }
                }
            }
            for (int i = 0; i < n; i++)
            {
                aNew[i, l] = b[i, l]*Cos + b[i, m]*Sin;
                aNew[i, m] = -b[i, l]*Sin + b[i, m]*Cos;
            }
            return aNew;
        }
        public double D(double x, double[,] a, int n)
        {
            if (n == 0)
            {
                return 1;
            }
            else if (n == 1)
            {
                return a[0,0] - x;
            }
            else
            {
                return (a[n-1,n-1] - x) * D(x, a, n-1) - a[n-1,n-2] * a[n-2,n-1]*D(x,a,n-2);
            }
        }
        public double[] RunMethod(double[,] matr, int n)
        {
            double an1 = -matr[n-1, n-2] / matr[n-1, n-1];
            double bn1 = matr[n-1, n] / matr[n-1, n-1];
            double[] a = new double[n-1];
            double[] b = new double[n-1];
            a[n-2] = an1;
            b[n-2] = bn1;
            double[] xi = new double[n];
            for (int i = n-3; i >= 0; i--)
            {
                a[i] = -matr[i+1, i] / (a[i+1]*matr[i+1, i+2] + matr[i+1, i+1]);
                b[i] = (matr[i+1, n] - b[i+1]*matr[i+1, i+2]) / (a[i+1]*matr[i+1, i+2] + matr[i+1, i+1]);
            }
            xi[1] = (-b[0]- matr[0, n] * a[0] / matr[0, 0]) / (-1 - matr[0, 1] * a[0] / matr[0, 0]);
            xi[0] = (matr[0, n] - matr[0, 1] * xi[1]) / matr[0, 0];
            for (int i = 2; i < n; i++)
            {
                xi[i] = a[i-1] * xi[i-1] + b[i-1];
            }
            return xi;
        }
    }
}
