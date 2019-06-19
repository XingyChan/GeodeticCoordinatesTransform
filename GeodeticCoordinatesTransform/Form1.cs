using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;

namespace GeodeticCoordinatesTransform
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        public struct Ellipse
        {
            public string Name;  //椭球名称             
            public double _a;    //长轴半径             
            public double _b;    //短轴半径             
            public double _e2;   //第二偏心率 
        }
        private Ellipse SetEllipse(int _izWhat)         
        {             
            Ellipse _re = new Ellipse(); 
            switch (_izWhat)             
            {                 
                case 0:                     
                    {                         
                        _re.Name = "克拉索夫斯基";                         
                        _re._a = 6378245.0000;                         
                        _re._b = 6356863.01877;                         
                        _re._e2 = 0.00673852541468;                         
                        break;                     
                    }                 
                case 1:                     
                    {                         
                        _re.Name = "Bessel椭球";                         
                        _re._a = 6377397.155;                         
                        _re._b = 6356078.963;                         
                        _re._e2 = 0.0067192188;                         
                        break;                     
                    }                 
                case 2:                     
                    {                        
                        _re.Name = "西安80/国际1975年椭球";                         
                        _re._a = 6378140.0000;                         
                        _re._b = 6356755.288158;                        
                        _re._e2 = 0.00673950182;  //第二偏心率                         
                        break;                     
                    }                 
                case 3:                     
                    {                         
                        _re.Name = "WGS-84椭球";                         
                        _re._a = 6378137.0000;                         
                        _re._b = 6356752.3142;                         
                        _re._e2 = (Math.Pow(_re._a, 2) - Math.Pow(_re._b, 2)) /  Math.Pow(_re._b, 2);                           
                        break;                     
                    }             
            }             
            return _re;         
        } 

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int iPos = comboBox1.SelectedIndex; //当前选中的行。
            
            Ellipse _elsp = SetEllipse(comboBox1.SelectedIndex); //获得当前椭球索引             
            textBox7.Text = _elsp.Name;             
            textBox1.Text = _elsp._a.ToString("F4");             
            textBox2.Text = _elsp._b.ToString("F6");             
            textBox4.Text = _elsp._e2.ToString("F12");             
            //计算扁率f             
            double _dzT = (_elsp._a - _elsp._b) / _elsp._a;             
            textBox5.Text = _dzT.ToString("F9");             
            _dzT = 1.0 / _dzT;             
            string szf = "  ( 1/" + _dzT.ToString("F2") + " )";             
            textBox5.Text += szf;             
            //计算第一偏心率             
            _dzT = (Math.Pow(_elsp._a, 2) - Math.Pow(_elsp._b, 2)) / Math.Pow(_elsp._a, 2);             
            textBox3.Text = _dzT.ToString("F12");             
            //计算极曲率半径            
            _dzT = Math.Pow(_elsp._a, 2) / _elsp._b;             
            textBox6.Text = _dzT.ToString("F4"); 
        }


        private void listView1_SelectedIndexChanged(object sender, EventArgs e)
        {
            int izCount = listView1.Items.Count;      //取得表格中的行数             
            for (int i = 0; i < izCount; i++)         //循环遍历表格中的数据             
            {
                string szName = listView1.Items[i].SubItems[1].Text;
                string szL = listView1.Items[i].SubItems[2].Text;
                double _dzL = double.Parse(szL);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void listView2_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void label13_Click(object sender, EventArgs e)
        {

        }

        public struct data
        {
            public static string[][] s;
        }

        public struct select
        {
            public static string[] s1;
        }

        private void 添加数据ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "文本文件（text)|*.tex|所有文件(*.*)|*.*";
            string sepatator = ",";
            char[] cgap = sepatator.ToCharArray();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                FileStream fs = new FileStream(ofd.FileName, FileMode.Open);
                StreamReader sr1 = new StreamReader(fs, Encoding.ASCII);
                string[][] s = new string[999][];
                for (int i = 0; i < 9999; i++)
                {
                    string str1 = sr1.ReadLine();
                    if (str1 == null)
                        break;
                    string[] str2 = str1.Split(cgap,StringSplitOptions.None);
                    ListViewItem b = new ListViewItem(new string[] { str2[0], str2[1], str2[2], str2[3], str2[4], str2[5], str2[6] });
                    listView1.Items.Add(b);
                    s[i] = str2;
                }
                Form1.data.s = s;
            } 
        }

        private void button5_Click(object sender, EventArgs e)
        {
            const double a = 6378140, e1 = 0.00669438499959, e2 = 0.00673950181947;
            double B, L;
            double x = double.Parse(textBox10.Text);
            double y = double.Parse(textBox11.Text);
            double L0 = double.Parse(textBox13.Text);
            double b = x / 6367452.133; double bf = b + (50228976 + (293697 + (2383 + 22 * Math.Pow(Math.Cos(b), 2)) * Math.Pow(Math.Cos(b), 2)) * Math.Pow(Math.Cos(b), 2)) * 1e-10 * Math.Sin(b) * Math.Cos(b);
            double tf = Math.Tan(bf);
            double Mf = a * (1 - e1) / Math.Pow(Math.Sqrt(1 - e1 * Math.Sin(bf) * Math.Sin(bf)), 3);
            double Nf = a / Math.Sqrt(1 - e1 * Math.Sin(bf) * Math.Sin(bf));
            double nf = e2 * Math.Cos(bf) * Math.Cos(bf);
            B = bf - tf / (2 * Mf * Nf) * y * y + Math.Pow(tf, 3) / (24 * Mf * Math.Pow(Nf, 3) * (5 + 3 * tf * tf) + nf - 9 * nf * tf * tf) * Math.Pow(y, 4); B = B * 180 / Math.PI;//高斯投影反算公式
            L = 1 / (Nf * Math.Cos(bf)) * y - 1 / 6 / Math.Pow(Nf, 3) / Math.Cos(bf) * (1 + 2 * tf * tf + nf) * Math.Pow(y, 3) + 1 / 120 / Math.Pow(Nf, 5) / Math.Cos(bf) * (5 + 28 * tf * tf + 24 * Math.Pow(tf, 4)) * Math.Pow(y, 5);
            L = L * 180 / Math.PI;
            L = L0  + L;

            textBox9.Text=B.ToString();
            textBox8.Text=L.ToString();
        }

        private void button1_Click_1(object sender, EventArgs e)
        {
            double X, Y, Z, N,
                e2 = double.Parse(textBox3.Text), 
                a = double.Parse(textBox1.Text),
                L = double.Parse(textBox15.Text),
                B = double.Parse(textBox12.Text),
                H = double.Parse(textBox14.Text);
            N = a / (Math.Sqrt(1 - e2 * Math.Sin(B * Math.PI / 180) * Math.Sin(B * Math.PI / 180)));
            X = (N + H) * Math.Cos(L * Math.PI / 180) * Math.Cos(B * Math.PI / 180);
            textBox18.Text = X.ToString();
            Y = (N + H) * Math.Sin(L * Math.PI / 180) * Math.Cos(B * Math.PI / 180);
            textBox16.Text = Y.ToString();
            Z = (N * (1 - e2) + H) * Math.Sin(B * Math.PI / 180);
            textBox17.Text = Z.ToString();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            double X, Y, Z, L, H, B0, B, B1, N, E, E2, a = 6378137.000, b = 6356752.314, c, t;
            X = double.Parse(textBox18.Text);
            Y = double.Parse(textBox16.Text);
            Z = double.Parse(textBox17.Text);
            E = double.Parse(textBox3.Text);
            E2 = double.Parse(textBox4.Text);
            c = a * Math.Sqrt(1 - E2);
            B0 = Math.Atan(Z / Math.Sqrt((X * X) + (Y * Y)));
            B1 = Math.Atan((Z + (c * E2 * Math.Tan(B0) / Math.Sqrt(1 + E + (Math.Tan(B0) * Math.Tan(B0))))) / Math.Sqrt((X * X) + (Y * Y)));
            t = Math.Abs(B1 - B0);
            while (t > 0.001 / 3600 / 180 * Math.PI)
            {
                B0 = B1;
                B1 = Math.Atan(((Z + (c * E2 * Math.Tan(B1) / Math.Sqrt(1 + E + (Math.Tan(B1) * Math.Tan(B1))))) / Math.Sqrt((X * X) + (Y * Y))));
                t = Math.Abs(B1 - B0);
            }
            N = a / Math.Sqrt(1 - (E * Math.Sin(B1) * Math.Sin(B1)));
            L = Math.Atan(Y / X) / Math.PI * 180;
            H = Math.Sqrt((X * X) + (Y * Y)) / Math.Cos(B1) - N;
            B = B1 / Math.PI * 180;
            L = L + 180;
            textBox12.Text = B.ToString();
            textBox15.Text = L.ToString();
            textBox14.Text = H.ToString();

        }

        private void button4_Click(object sender, EventArgs e)
        {
            double x, y;
            double a = double.Parse(textBox1.Text),
                   b = double.Parse(textBox2.Text);
            double e1 = Math.Sqrt((a * a - b * b) / (a * a));
            double e2 = Math.Sqrt((a * a - b * b) / (b * b));
            double L = double.Parse(textBox8.Text);
            double B = double.Parse(textBox9.Text);
            double L0 = Math.Floor(L / 6) * 6 - 3;
            double l = (L - L0)*3600/206265;
            double N = 6399596.652 - (21565.045 - (108.996 - 0.603 * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2);
            double a0 = 32144.5189 - (135.3646 - (0.7034 - 0.0041 * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2);
            double a3 = (0.3333333 + 0.001123 * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2) - 0.1666667;
            double a4 = (0.25 + 0.00253 * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2) - 0.04167;
            double a5 = 0.00878 - (0.1702 - 0.20382 * Math.Pow(Math.Cos(B * Math.PI / 180), 2)) * Math.Pow(Math.Cos(B * Math.PI / 180), 2);
            double a6 = (0.167 * Math.Pow(Math.Cos(B * Math.PI / 180), 2) - 0.083) * Math.Pow(Math.Cos(B * Math.PI / 180), 2);
            x = 6367452.1328 * B * 3600 / 206265 - (a0 - (0.5 + (a4 + a6 * l * l) * l * l) * l * l * N) * Math.Cos(B * Math.PI / 180) * Math.Sin(B * Math.PI / 180);
            y = (1 + (a3 + a5 * l * l) * l * l) * l * N * Math.Cos(B * Math.PI / 180);
            textBox10.Text = x.ToString();
            textBox11.Text = y.ToString();
        }

        private void groupBox3_Enter(object sender, EventArgs e)
        {

        }

        private void contextMenuStrip2_Opening(object sender, CancelEventArgs e)
        {

        }
        }
    }

