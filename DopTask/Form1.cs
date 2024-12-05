using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace DopTask
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
            if(Det(matr, n) == 0)
            {
                MessageBox.Show("There is no inverse matrix : det(A) = 0", "Worning");
                return;
            }
            double[,] extendedMatr = Extending(matr, n);
            GuosUp(extendedMatr, n);
            GousBackward(extendedMatr, n);
            double[,] result = DeExtending(extendedMatr, n);
            Output(result, n);
        }
        public void Output(double[,] matr, int n)
        {
            textBox3.Text = "";
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    textBox3.Text += $"{matr[i, j]}\t";
                }
                textBox3.AppendText(Environment.NewLine);
            }
        }
        public double[,] DeExtending(double[,] matr, int n)
        {
            double[,] unExtend = new double[n, n];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    unExtend[i,j] = matr[i, j + n];
                }
            }
            return unExtend;
        }
        public double[,] Extending(double[,] matr, int n)
        {
            double[,] extend = new double[n, n*2];
            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n * 2; j++)
                {
                    if (i+n==j)
                    {
                        extend[i, j] = 1;
                    }
                    else if(j >= n)
                    {
                        extend[i, j] = 0;
                    }
                    else
                    {
                        extend[i, j] = matr[i,j];
                    }
                }
            }
            return extend;
        }
        public void GuosUp(double[,] matr, int n)
        {
            for (int i = 0; i < n; i++)
            {
                bool onlyZero = false;
                if (matr[i, i] == 0)
                {
                    onlyZero = true;
                    for (int i2 = i + 1; i2 < n; i2++)
                    {
                        if (matr[i2, i] != 0)
                        {
                            for (int j = i; j < n * 2; j++)
                            {
                                double tmp = matr[i, j];
                                matr[i, j] = matr[i2, j];
                                matr[i2, j] = tmp;
                            }
                            onlyZero = false;
                            break;
                        }
                    }
                }
                if (onlyZero)
                {
                    continue;
                }
                for (int i2 = i + 1; i2 < n; i2++)
                {
                    if (matr[i2, i] != 0)
                    {
                        double k = matr[i2, i] / matr[i, i];
                        for (int j = i; j < n * 2; j++)
                        {
                            matr[i2, j] -= matr[i, j]*k;
                        }
                    }
                }

            }
        }
        public void GousBackward(double[,] matr, int n)
        {
            for (int i = 0; i < n - 1; i++)
            {
                for(int i2 = i+1; i2 < n; i2++)
                {
                    if(matr[i, i2] == 0)
                    {
                        continue;
                    }
                    double k = -matr[i, i2]/matr[i2, i2];
                    for (int j2 = i2; j2 < n*2; j2++)
                    {
                        matr[i, j2] += matr[i2, j2]*k;
                    }
                }
            }
            for(int i = 0; i < n; i++)
            {
                double k = 1/matr[i, i];
                for (int j2 = i; j2 < n*2; j2++)
                {
                    matr[i, j2] *= k;
                }
            }
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
        private void Form1_Load(object sender, EventArgs e)
        {
            textBox1.Text = $"1 2 4{Environment.NewLine}5 1 2{Environment.NewLine}3 -1 1";
            textBox2.Text = "3";
        }
    }
}
