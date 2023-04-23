using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Zadanie3_IT
{
    public partial class Form1 : Form
    {
         double A1, A2, A3;
         double v1, v2, v3;
         double ph1, ph2, ph3;
         double fd,Time, fd_opt;
         int k;
         int  N,N2,N3, border, wX1, hX1,wX2, hX2, wX3, hX3;
         float maxY = 0;
         float maxY2 = 0;
         float maxY3 = 0;
         int padding = 10;
         int left_keys_padding = 20;
         double[] sign;
         double[] spectr;
         PointF[] func_points;
         PointF[] func_points_static;
         PointF[] spectr_points;
         double[] sign2;
         double[] spectr2;
         PointF[] func_points2;
         PointF[] func_points2_static;
         PointF[] spectr_points2;
         PointF[] oursill;//порог
         double error;
         Graphics graphics1 ;
         Graphics graphics2;
         Graphics graphics3;
        
        Pen GridPen = new Pen(Color.Gray, 1f);
        private void Form1_Load(object sender, EventArgs e)
        {
            for_A1.Text = "1000";
            for_A2.Text = "300";
            for_A3.Text = "3000";
            for_v1.Text = "150";
            for_v2.Text = "450";
            for_v3.Text = "300";
            for_ph1.Text = "30";
            for_ph2.Text = "40";
            for_ph3.Text = "50";
            for_fd.Text = "8192";
            for_N.Text = "9";
            for_k.Text = "5";
            for_border.Text = "10";
            for_fd_opt.Text = "0";
        }

        public Form1()
        {
            InitializeComponent();
        }

        public void button1_Click(object sender, EventArgs e)//моделирование сигнала
        {
            pictureBox1.Image = null;
            pictureBox1.Update();
            pictureBox2.Image = null;
            pictureBox2.Update();
            pictureBox3.Image = null;
            pictureBox3.Update();
            Param();
            Time = N / fd;
            SumSin(fd, ref sign,ref func_points,N, ref func_points_static);
            BDVPF(fd,sign, ref spectr, ref spectr_points,N);
            //отрисовка 
            wX1 = pictureBox1.Width;
            hX1 = pictureBox1.Height;
            wX2 = pictureBox2.Width;
            hX2 = pictureBox2.Height;
            wX3 = pictureBox3.Width;
            hX3 = pictureBox3.Height;
            graphics1 = pictureBox1.CreateGraphics();
            graphics2 = pictureBox2.CreateGraphics();
            graphics3 = pictureBox3.CreateGraphics();
            Pen pen1 = new Pen(Color.Red, 2f);
            //координатные оси:
            Pen GreenPen;
            GreenPen = new Pen(Color.Black, 2f);
            //Ось X
            Point KX1, KX2;
            KX1 = new Point(30, (hX1 / 2));
            KX2 = new Point(wX1 - 10, (hX1 / 2));
            graphics1.DrawLine(GreenPen, KX1, KX2);
            Point KX12, KX22;
            KX12 = new Point(30, hX1 - 10);
            KX22 = new Point(wX1 - 10, hX1 - 10);
            graphics2.DrawLine(GreenPen, KX12, KX22);
            graphics3.DrawLine(GreenPen, KX12, KX22);
            //Ось Y
            Point KY1, KY2;
            KY1 = new Point(30, 10);
            KY2 = new Point(30, hX1 - 10);
            graphics1.DrawLine(GreenPen, KY1, KY2);
            graphics2.DrawLine(GreenPen, KY1, KY2);
            graphics3.DrawLine(GreenPen, KY1, KY2);
            //сетка
            int actual_width1 = wX1 - 2 * padding - left_keys_padding;
            int actual_height1 = hX1 - 2 * padding;
            int actual_top = padding;
            int actual_bottom1 = actual_top + actual_height1;
            int actual_left = padding + left_keys_padding;
            int actual_right1 = actual_left + actual_width1;
            int grid_size = 11;
            maxY = 0;
            for (int i = 0; i < N; i++)
            {
                if (maxY < Math.Abs(func_points[i].Y)) maxY = Math.Abs(func_points[i].Y);//макс значение Y
            }
            maxY2 = 0;
            for (int i = 0; i < N; i++)
            {
                if (maxY2 < Math.Abs(spectr_points[i].Y)) maxY2 = Math.Abs(spectr_points[i].Y);//макс значение Y
            }
            PointF K1, K2, K3, K4;
            for (double i = 0.5; i < grid_size; i += 1.0)
            {
                //вертикальная
                K1 = new PointF((float)(actual_left + i * actual_width1 / grid_size), actual_top);
                K2 = new PointF((float)(actual_left + i * actual_width1 / grid_size), actual_bottom1);
                graphics1.DrawLine(GridPen, K1, K2);
                double v = 0 + i * ((double)(N / fd)) / grid_size;
                string s1 = v.ToString("0.00");
                graphics1.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);
                //горизонтальная
                K3 = new PointF(actual_left, (float)(actual_top + i * actual_height1 / grid_size));
                K4 = new PointF(actual_right1, (float)(actual_top + i * actual_height1 / grid_size));
                double g = 0 + i * (2*maxY / grid_size);
                double g_ = 0 - i * (2 * maxY / grid_size);
                string s2 = g.ToString("0.0"); 
                string s2_ = g_.ToString("0.0");
                graphics1.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding-10, actual_bottom1 - (float)i * actual_height1 / grid_size - hX1 / 2);
                graphics1.DrawString(s2_, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding - 10, actual_bottom1 + (float)i * actual_height1 / grid_size - hX1 / 2);
                graphics1.DrawLine(GridPen, K3, K4);
            }
            int actual_width2 = wX2 - 2 * padding - left_keys_padding;
            int actual_height2 = hX2 - 2 * padding;
            int actual_bottom2 = actual_top + actual_height2;
            int actual_right2 = actual_left + actual_width2;
            PointF K5, K6, K7, K8;
            grid_size = 12;
            for (double i = 0.5; i < grid_size; i += 1.0)
            {
                K5 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_top);
                K6 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_bottom2);
                double v = 0 + i * ((fd) - 0) / grid_size;
                string s1 = v.ToString("0.00");
                graphics2.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);
                graphics2.DrawLine(GridPen, K5, K6);
                

                K7 = new PointF(actual_left, (float)(actual_top + i * actual_height2 / grid_size));
                K8 = new PointF(actual_right2, (float)(actual_top + i * actual_height2 / grid_size));
                double g = 0 + i * (maxY2 / grid_size);
                string s2 = g.ToString("0.0");
                graphics2.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding - 10, actual_bottom1 - (float)i * actual_height1 / grid_size);
                graphics2.DrawLine(GridPen, K7, K8);
            }
            PointF actual_tb = new PointF(actual_top, actual_bottom1);//для y
            PointF actual_rl = new PointF(actual_right1, actual_left);//для x
            PointF from_toX = new PointF(0, (float)(N / fd));
            PointF from_toY = new PointF(-maxY * (float)1.2, maxY * (float)1.2);
            PointF actual_tb2 = new PointF(actual_top, actual_bottom2);//для y
            PointF actual_rl2 = new PointF(actual_right2, actual_left);//для x
            PointF from_toX2 = new PointF(0, (float)fd);
            PointF from_toY2 = new PointF(0, maxY2 * (float)1.1);
            convert_range_graph(ref func_points, actual_rl, actual_tb, from_toX, from_toY);
            graphics1.DrawLines(pen1, func_points);
            convert_range_graph(ref spectr_points, actual_rl2, actual_tb2, from_toX2, from_toY2);
            graphics2.DrawLines(pen1, spectr_points);


        }
        public void SumSin(double f_d, ref double[] arr, ref PointF[] arr_points,int n, ref PointF[] arr_points_static)
        {
            arr = new double[n];
            arr_points = new PointF[n];
            arr_points_static = new PointF[n];
            float dt = (float)(1 / f_d);
            for (int i = 0; i < n; i++)
            {
                arr[i] = (float)(A1 * Math.Sin(2 * Math.PI * v1 * (float)i / f_d + ph1) + A2 * Math.Sin(2 * Math.PI * v2 * (float)i / f_d + ph2) + A3 * Math.Sin(2 * Math.PI * v3 * (float)i / f_d + ph3));
                arr_points[i] = new PointF((float)i * dt, (float)(arr[i]));
                arr_points_static[i] = new PointF((float)i * dt, (float)(arr[i]));
            }

        }
        public void BDVPF(double f_d,  double[] si, ref double[] sp, ref PointF[] sp_points,int n)
        {
            Cmplx[] arr = new Cmplx[n];
            for (int i = 0; i <n; i++)
            {
                arr[i] = new Cmplx(si[i], 0);
            }
            Cmplx.Fourea(n, ref arr, -1);
            sp = new double[n];
            sp_points = new PointF[n];
            float df = (float)f_d / (n - 1);
            for (int i = 0; i < n; i++)
            {
                sp[i] = Math.Sqrt(arr[i].re * arr[i].re + arr[i].im * arr[i].im);
                sp_points[i] = new PointF((float)(i * df), (float)sp[i]);  
            }
            
        }
        private void button2_Click(object sender, EventArgs e)
        {
            
            double p = border * maxY2/100;//порог
            float df = (float)fd / (N - 1);
            oursill = new PointF[N];
            for (int i = 0; i < N; i++)
            {
                oursill[i] = new PointF((float)i * df, (float)p);//порог
            }
            for (int i = 0; i < (int)(N / 2); i++)
            {
                if (spectr[i] >= p)
                {
                    fd_opt = (k * i*df);
                }
            }
            N2 = (int)(Time*fd_opt);
            SumSin(fd_opt, ref sign2, ref func_points2,N2,ref func_points2_static);
            //отрисовка спектра?
            int mi=0;
            do
            {
                N3 = (int)Math.Pow(2, mi);
                mi++;
                
            } while (N2 > Math.Pow(2, mi-1));
            double[] sign3=new double[N3];
            for(int i=0; i<N3;i++)
            {
                if(i<N2)
                sign3[i] = sign2[i];
                else sign3[i] = 0;
            }
            BDVPF(fd_opt, sign3, ref spectr2, ref spectr_points2, N3);

            int actual_top = padding;
            int actual_left = padding + left_keys_padding;
            int actual_width1 = wX1 - 2 * padding - left_keys_padding;
            int actual_height1 = hX1 - 2 * padding;
            int actual_bottom1 = actual_top + actual_height1;
            int actual_right1 = actual_left + actual_width1;
            int actual_width2 = wX2 - 2 * padding - left_keys_padding;
            int actual_height2 = hX2 - 2 * padding;
            int actual_bottom2 = actual_top + actual_height2;
            int actual_right2 = actual_left + actual_width2;
            PointF actual_tb = new PointF(actual_top, actual_bottom1);//для y
            PointF actual_rl = new PointF(actual_right1, actual_left);//для x
            PointF from_toX = new PointF(0, (float)(N2 / fd_opt));
            PointF from_toY = new PointF(-maxY * (float)1.2, maxY * (float)1.2);
            PointF actual_tb2 = new PointF(actual_top, actual_bottom2);//для y
            PointF actual_rl2 = new PointF(actual_right2, actual_left);//для x
            PointF from_toX2 = new PointF(0, (float)fd_opt);
            PointF from_toY2 = new PointF(0, maxY2 * (float)1.1);
            Pen penp = new Pen(Color.Black, 2f);
            Pen pen2 = new Pen(Color.Blue, 2f);
            convert_range_graph(ref oursill, actual_rl2, actual_tb2, from_toX2, from_toY2);
            graphics2.DrawLines(penp, oursill);
            convert_range_graph(ref func_points2, actual_rl, actual_tb, from_toX, from_toY);
            graphics1.DrawLines(pen2, func_points2);
            PointF K5, K6, K7, K8;
            int grid_size = 12;
            maxY3 = 0;
            for (int i = 0; i < N3; i++)
            {
                if (maxY3 < Math.Abs(spectr_points2[i].Y)) maxY3 = Math.Abs(spectr_points2[i].Y);//макс значение Y
            }
            for (double i = 0.5; i < grid_size; i += 1.0)
            {
                K5 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_top);
                K6 = new PointF((float)(actual_left + i * actual_width2 / grid_size), actual_bottom2);
                double v = 0 + i * ((fd_opt) - 0) / grid_size;
                string s1 = v.ToString("0.00");
                graphics3.DrawString(s1, new Font("Arial", 7), Brushes.Green, actual_left + (float)i * actual_width1 / grid_size, actual_bottom1 + 0);
                graphics3.DrawLine(GridPen, K5, K6);


                K7 = new PointF(actual_left, (float)(actual_top + i * actual_height2 / grid_size));
                K8 = new PointF(actual_right2, (float)(actual_top + i * actual_height2 / grid_size));
                double g = 0 + i * (maxY3 / grid_size);
                string s2 = g.ToString("0.0");
                graphics3.DrawString(s2, new Font("Arial", 7), Brushes.Green, actual_left - left_keys_padding - 10, actual_bottom1 - (float)i * actual_height1 / grid_size);
                graphics3.DrawLine(GridPen, K7, K8);
            }
            PointF from_toX3 = new PointF(0, (float)fd_opt);
            PointF from_toY3 = new PointF(0, maxY3 * (float)1.1);
            convert_range_graph(ref spectr_points2, actual_rl, actual_tb, from_toX3, from_toY3);
            graphics3.DrawLines(pen2, spectr_points2);
            for_fd_opt.Text = fd_opt.ToString("0.00");
            Interpolation();
        }
        public void Straight(double x1,double x2,double y1,double y2)
        {
            double y_;
            double k = (y1 - y2) / (x1 - x2);
            double b = y2 - k * x2;
            for (int i = 0; i < N; i++)
            {
                if (func_points_static[i].X > x1 && func_points_static[i].X < x2)
                {
                    y_ = k * func_points_static[i].X + b;//значение  передисркетизованного сигнала
                    error += (y_ - func_points_static[i].Y) * (y_ - func_points_static[i].Y);
                }
            }
        }
        public void Interpolation()
        {
            for(int i=0;i< sign2.Length-1;i++)
            {
                Straight(func_points2_static[i].X, func_points2_static[i + 1].X, func_points2_static[i].Y, func_points2_static[i + 1].Y);
            }
            error = Math.Sqrt(error) / N;
            for_error.Text = error.ToString("0.00");
        }
        public void convert_range_graph(ref PointF[] data, PointF actual_rl, PointF actual_tb, PointF from_toX, PointF from_toY)
        {
            //actual-размер:X-top/right Y-right,left
            //from_to: X-мин, Y-макс
            float kx = (actual_rl.X - actual_rl.Y) / (from_toX.Y - from_toX.X);
            float ky = (actual_tb.X - actual_tb.Y) / (from_toY.Y - from_toY.X);
            for (int i = 0; i < data.Length; i++)
            {
                data[i].X = (data[i].X - from_toX.X) * kx + actual_rl.Y;
                data[i].Y = (data[i].Y - from_toY.X) * ky + actual_tb.Y;
            }

        }
        private void Param()
        {
            A1 = Convert.ToDouble(for_A1.Text);
            A2 = Convert.ToDouble(for_A2.Text);
            A3 = Convert.ToDouble(for_A3.Text);
            v1 = Convert.ToDouble(for_v1.Text);
            v2 = Convert.ToDouble(for_v2.Text);
            v3 = Convert.ToDouble(for_v3.Text);
            ph1 = Convert.ToDouble(for_ph1.Text);
            ph2 = Convert.ToDouble(for_ph2.Text);
            ph3 = Convert.ToDouble(for_ph3.Text);
            fd = Convert.ToDouble(for_fd.Text);
            N = Convert.ToInt32(for_N.Text);
            N = (int)Math.Pow(2, N);
            k = Convert.ToInt32(for_k.Text);
            border = Convert.ToInt32(for_border.Text);
        }
       
    }
}
