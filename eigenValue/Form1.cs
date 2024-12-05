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
                    if(j != n)
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
            if(!IsSimetric(matr, n))
            {
                MessageBox.Show("Matrix is not simetric", "Worning");
                return;
            }
            if(!IsPositiveDefinite(matr, n))
            {
                MessageBox.Show("Matrix is not positive definite", "Worning");
                return;
            }
            double[] x0 = new double[n];
            double[] xNext = new double[n];
            double lambda = 0;
            for(int i = 0; i < n; i++)
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
            }while(Math.Abs(lambdaNext - lambda) >= eps);
            textBox3.Text = $"{lambda}";
            textBox5.Text = $"{iteration}";
        }
        public bool IsSimetric(double[,] matr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                for (int j = i+1; j < n; j++)
                {
                    if (matr[i,j] != matr[j, i])
                    {
                        return false;
                    }
                }
            }
            return true;
        }
        public bool IsPositiveDefinite(double[,] matr, int n)
        {
            for(int n1 = 1; n1 <= n; n1++)
            {
                double[,] matr1 = new double[n1,n1];
                for(int i  = 0; i < n1; i++)
                {
                    for(int j = 0; j < n1; j++)
                    {
                        matr1[i,j] = matr[i,j];
                    }
                }
                if(Det(matr1, n1) < 0)
                {
                    return false;
                }
            }
            return true;
        }
        public double Det(double[,] matr, int n)
        {
            if(n == 1)
            {
                return matr[0,0];
            }
            if(n == 2)
            {
                return matr[0, 0]*matr[1,1] - matr[0,1] * matr[1,0];
            }
            double res = 0;
            bool pozitiv = true;
            for (int i = 0; i < n; i++)
            {
                double[,] matr1 = new double[n-1,n-1];
                int i1 = 0;
                for(int j = 0; j < n; j++)
                {
                    if (j==i)
                    {
                        continue;
                    }
                    int j1 = 0;
                    for (int k = 1; k < n; k++)
                    {
                        matr1[i1,j1] = matr[j,k];
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
            for(int i = 0; i < n; i++)
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
            textBox1.Text = $"5 2{Environment.NewLine}2 5";
            textBox2.Text = "2";
            textBox4.Text = "0,0001";
        }
    }
}
