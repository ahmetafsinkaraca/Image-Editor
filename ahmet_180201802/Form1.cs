using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace ahmet_180201802
{
    public partial class Form1 : Form
    {
        Bitmap SRC_IMG, DEST_IMG, TEMP_IMG;

        void Bubble_Sort (ref int []V, int N)
        {
            int i, j, tmp;

            for(i=1;i<N;i++)
                for(j=0;j<N-i;j++)
                    if(V[j] < V[j+1])
                {
                        tmp = V[j];
                        V[j] = V[j + 1];
                        V[j + 1] = tmp;
                }
        }

        public Form1()
        {
            InitializeComponent();
        }

        
        private void Convolution(Bitmap src_img, ref Bitmap dest_img, double[,] k, int w_size)
        {
            int fram = w_size / 2;

            for (int x = fram; x < src_img.Width - fram; x++)
                for (int y = fram; y < src_img.Height - fram; y++)
                {
                    double Res_R = 0;
                    double Res_G = 0;
                    double Res_B = 0;

                    for (int i = 0; i<w_size; i++)
                        for (int j = 0; j<w_size; j++)
                        {
                            Res_R += k[i, j] * src_img.GetPixel(x + i - fram, y + j - fram).R;
                            Res_G += k[i, j] * src_img.GetPixel(x + i - fram, y + j - fram).G;
                            Res_B += k[i, j] * src_img.GetPixel(x + i - fram, y + j - fram).B;
                        }


                  dest_img.SetPixel(x, y, Color.FromArgb((int) Res_R, (int) Res_G, (int) Res_B));
                }
                 pictureBox2.Image = DEST_IMG;
        }

        private void Convolution2(Bitmap src_img, ref Bitmap dest_img, int[,] k, int w_size)
        {
            int fram = w_size / 2;

            for (int x = fram; x < src_img.Width - fram; x++)
                for (int y = fram; y < src_img.Height - fram; y++)
                {
                    double Res_R = 0;
                    double Res_G = 0;
                    double Res_B = 0;

                    for (int i = 0; i < w_size; i++)
                        for (int j = 0; j < w_size; j++)
                        {
                            Res_R += k[i, j] * src_img.GetPixel(x + i - fram, y + j - fram).R;
                            Res_G += k[i, j] * src_img.GetPixel(x + i - fram, y + j - fram).G;
                            Res_B += k[i, j] * src_img.GetPixel(x + i - fram, y + j - fram).B;
                        }

                    if (Res_R > 255) Res_R = 255;else if (Res_R < 0) Res_R = 0;
                    if (Res_G > 255) Res_G = 255; else if (Res_G < 0) Res_G = 0;
                    if (Res_B > 255) Res_B = 255; else if (Res_B < 0) Res_B = 0;

                    dest_img.SetPixel(x, y, Color.FromArgb((int)Res_R, (int)Res_G, (int)Res_B));
                }
            pictureBox2.Image = DEST_IMG;
        }


        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog();
            string File_Name = openFileDialog1.FileName.Trim();
            try
            {
                if (File_Name != "") pictureBox1.Load(File_Name);
                SRC_IMG = new Bitmap(File_Name);
                DEST_IMG = new Bitmap(SRC_IMG.Width, SRC_IMG.Height);
                TEMP_IMG = new Bitmap(SRC_IMG.Width, SRC_IMG.Height);
            }
            catch
            {
                MessageBox.Show("Error in File Type");
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            SRC_IMG = DEST_IMG;
            pictureBox1.Image = SRC_IMG;
        }

        private void button3_Click(object sender, EventArgs e)
        {
            int i, j;

            for (i = 0; i < SRC_IMG.Width; i++)
                for (j = 0; j < SRC_IMG.Height; j++)
                {
                    int RR = SRC_IMG.GetPixel(i, j).R;
                    int GG = SRC_IMG.GetPixel(i, j).G;
                    int BB = SRC_IMG.GetPixel(i, j).B;

                    int GRAY = (RR + GG + BB) / 3;

                    DEST_IMG.SetPixel(i, j, Color.FromArgb(GRAY, GRAY, GRAY));
                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button4_Click(object sender, EventArgs e)
        {
            int W_Size = vScrollBar1.Value;

            double[,] K = new double[W_Size, W_Size];
            for (int i = 0; i < W_Size; i++)
                for (int j = 0; j < W_Size; j++)
                    K[i, j] = 1.0 / (W_Size * W_Size);

            Convolution(SRC_IMG, ref DEST_IMG, K, W_Size);

            pictureBox2.Image = DEST_IMG;
        }


        private void button5_Click(object sender, EventArgs e)
        {
            int W_Size = vScrollBar1.Value;

            double[,] K = new double[W_Size, W_Size];
            for(int i = 0; i < W_Size; i++)
                for(int j = 0; j < W_Size; j++)
                {
                    int x = W_Size / 2 - i;
                    int y = W_Size / 2 - j;

                    K[i, j] = 1.0 / (2 * Math.PI) * Math.Exp(-(x * x + y * y) / 2.0);
                }
            Convolution(SRC_IMG, ref DEST_IMG, K, W_Size);

            pictureBox2.Image = DEST_IMG;
        }


        private void button6_Click(object sender, EventArgs e)
        {
            int W_Size = vScrollBar1.Value;
            int[] Median_Vector_R = new int[W_Size * W_Size];
            int[] Median_Vector_G = new int[W_Size * W_Size];
            int[] Median_Vector_B = new int[W_Size * W_Size];

            int v = 0;

            int fram = W_Size / 2;

            for (int x = fram; x < SRC_IMG.Width - fram; x++)
                for (int y = fram; y < SRC_IMG.Height - fram; y++)
                {
                    v = 0;

                    for (int i = 0; i < W_Size; i++)
                        for (int j = 0; j < W_Size; j++)
                        {
                            Median_Vector_R[v] = SRC_IMG.GetPixel(x + i - fram, y + j - fram).R;
                            Median_Vector_G[v] = SRC_IMG.GetPixel(x + i - fram, y + j - fram).G;
                            Median_Vector_B[v] = SRC_IMG.GetPixel(x + i - fram, y + j - fram).B;
                            v++;
                        }


                    Bubble_Sort(ref Median_Vector_R, W_Size * W_Size);
                    Bubble_Sort(ref Median_Vector_G, W_Size * W_Size);
                    Bubble_Sort(ref Median_Vector_B, W_Size * W_Size);

                    int RR = Median_Vector_R[W_Size * W_Size / 2 + 1];
                    int GG = Median_Vector_G[W_Size * W_Size / 2 + 1];
                    int BB = Median_Vector_B[W_Size * W_Size / 2 + 1];

                    DEST_IMG.SetPixel(x, y, Color.FromArgb((int)RR, (int)GG, (int)BB));
                }

            pictureBox2.Image = DEST_IMG;
        }


        private void button7_Click(object sender, EventArgs e)
        {
            int W_Size = 3;
            int[,] K = new int[W_Size, W_Size];

            K[0, 0] = 0; K[0, 1] = -1; K[0, 2] = 0;
            K[1, 0] = -1; K[1, 1] = 4; K[1, 2] = -1;
            K[2, 0] = 0; K[2, 1] = -1; K[2, 2] = 0;


            Convolution2(SRC_IMG, ref DEST_IMG, K, W_Size);

            pictureBox2.Image = DEST_IMG;
        }


        private void button8_Click(object sender, EventArgs e)
        {
            int W_Size = 3;
            int[,] K = new int[W_Size, W_Size];

            K[0, 0] = -1; K[0, 1] = -1; K[0, 2] = -1;
            K[1, 0] = -1; K[1, 1] = 8; K[1, 2] = -1;
            K[2, 0] = -1; K[2, 1] = -1; K[2, 2] = -1;


            Convolution2(SRC_IMG, ref DEST_IMG, K, W_Size);

            pictureBox2.Image = DEST_IMG;
        }

        private void button9_Click(object sender, EventArgs e)
        {
            int W_Size = 3;
            int[,] K = new int[W_Size, W_Size];

            K[0, 0] = 0; K[0, 1] = -1; K[0, 2] = 0;
            K[1, 0] = -1; K[1, 1] = 5; K[1, 2] = -1;
            K[2, 0] = 0; K[2, 1] = -1; K[2, 2] = 0;


            Convolution2(SRC_IMG, ref DEST_IMG, K, W_Size);

            pictureBox2.Image = DEST_IMG;
        }


        private void button10_Click(object sender, EventArgs e)
        {
            int W_Size = 3;
            int[,] K = new int[W_Size, W_Size];

            K[0, 0] = -1; K[0, 1] = -1; K[0, 2] = -1;
            K[1, 0] = -1; K[1, 1] = 9; K[1, 2] = -1;
            K[2, 0] = -1; K[2, 1] = -1; K[2, 2] = -1;


            Convolution2(SRC_IMG, ref DEST_IMG, K, W_Size);

            pictureBox2.Image = DEST_IMG;
        }

        private void button11_Click(object sender, EventArgs e)
        {
            int Bright_Val = vScrollBar2.Value;

            for (int x = 0; x < SRC_IMG.Width; x++)
                for (int y = 0; y < SRC_IMG.Height; y++)
                {
                    int Res_R = SRC_IMG.GetPixel(x, y).R + Bright_Val;
                    int Res_G = SRC_IMG.GetPixel(x, y).G + Bright_Val;
                    int Res_B = SRC_IMG.GetPixel(x, y).B + Bright_Val;

                    if (Res_R > 255) Res_R = 255; else if (Res_R < 0) Res_R = 0;
                    if (Res_G > 255) Res_G = 255; else if (Res_G < 0) Res_G = 0;
                    if (Res_B > 255) Res_B = 255; else if (Res_B < 0) Res_B = 0;

                    DEST_IMG.SetPixel(x, y, Color.FromArgb((int)Res_R, (int)Res_G, (int)Res_B));

                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button12_Click(object sender, EventArgs e)
        {
            int Contrast_Val = vScrollBar3.Value;

            double F = (259 * (Contrast_Val + 255)) / (255 * (259 - Contrast_Val));

            for (int x = 0; x < SRC_IMG.Width; x++)
                for (int y = 0; y < SRC_IMG.Height; y++)
                {
                    int Res_R = (int)Math.Round(F * (SRC_IMG.GetPixel(x, y).R - 128) + 128);
                    int Res_G = (int)Math.Round(F * (SRC_IMG.GetPixel(x, y).G - 128) + 128);
                    int Res_B = (int)Math.Round(F * (SRC_IMG.GetPixel(x, y).B - 128) + 128);


                    if (Res_R > 255) Res_R = 255; else if (Res_R < 0) Res_R = 0;
                    if (Res_G > 255) Res_G = 255; else if (Res_G < 0) Res_G = 0;
                    if (Res_B > 255) Res_B = 255; else if (Res_B < 0) Res_B = 0;

                    DEST_IMG.SetPixel(x, y, Color.FromArgb((int)Res_R, (int)Res_G, (int)Res_B));

                }
            pictureBox2.Image = DEST_IMG;
        }


        private void button13_Click(object sender, EventArgs e)
        {
            double Y = vScrollBar2.Value/10.0;

            for (int x = 0; x < SRC_IMG.Width; x++)
                for (int y = 0; y < SRC_IMG.Height; y++)
                {
                    int Res_R = (int)Math.Round(255 * Math.Pow(SRC_IMG.GetPixel(x, y).R / 255.0, 1 / Y ));
                    int Res_G = (int)Math.Round(255 * Math.Pow(SRC_IMG.GetPixel(x, y).G / 255.0, 1 / Y ));
                    int Res_B = (int)Math.Round(255 * Math.Pow(SRC_IMG.GetPixel(x, y).B / 255.0, 1 / Y ));


                    if (Res_R > 255) Res_R = 255; else if (Res_R < 0) Res_R = 0;
                    if (Res_G > 255) Res_G = 255; else if (Res_G < 0) Res_G = 0;
                    if (Res_B > 255) Res_B = 255; else if (Res_B < 0) Res_B = 0;

                    DEST_IMG.SetPixel(x, y, Color.FromArgb((int)Res_R, (int)Res_G, (int)Res_B));

                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button14_Click(object sender, EventArgs e)
        {
            DEST_IMG.SetResolution(pictureBox1.Image.Width / 2, pictureBox1.Image.Height / 2);
            for (int x = 1; x < pictureBox1.Image.Width - 2; x += 2)
                for (int y = 1; y < pictureBox1.Image.Height - 2; y += 2)
                {
                    int Pivot = (SRC_IMG.GetPixel(x, y).G +
                                 SRC_IMG.GetPixel(x + 1, y).G +
                                 SRC_IMG.GetPixel(x, y + 1).G +
                                 SRC_IMG.GetPixel(x + 1, y + 1).G) / 4;


                    DEST_IMG.SetPixel(x / 2, y / 2, Color.FromArgb(Pivot, Pivot, Pivot));
                }
            pictureBox2.Image = DEST_IMG;
        }


        private void button15_Click(object sender, EventArgs e)
        {
            for (int x = 1; x < (pictureBox1.Image.Width - 2) / 2; x++)
                for (int y = 1; y < (pictureBox1.Image.Height - 2) / 2; y++)
                {
                    int Pivot = SRC_IMG.GetPixel(x, y).G;


                    DEST_IMG.SetPixel(x * 2, y * 2, Color.FromArgb(Pivot, Pivot, Pivot));
                    DEST_IMG.SetPixel(x * 2 + 1, y * 2, Color.FromArgb(Pivot, Pivot, Pivot));
                    DEST_IMG.SetPixel(x * 2, y * 2 + 1, Color.FromArgb(Pivot, Pivot, Pivot));
                    DEST_IMG.SetPixel(x * 2 + 1, y * 2 + 1, Color.FromArgb(Pivot, Pivot, Pivot));

                }
            pictureBox2.Image = DEST_IMG;
        }


        private void button16_Click(object sender, EventArgs e)
        {
            for (int x = 1; x < (pictureBox1.Image.Width - 2) / 2; x++)
                for (int y = 1; y < (pictureBox1.Image.Height - 2) / 2; y++)
                {
                    int Pivot = SRC_IMG.GetPixel(x, y).G;


                    TEMP_IMG.SetPixel(x * 2, y * 2, Color.FromArgb(Pivot, Pivot, Pivot));
                    TEMP_IMG.SetPixel(x * 2 + 1, y * 2, Color.FromArgb(Pivot, Pivot, Pivot));
                    TEMP_IMG.SetPixel(x * 2, y * 2 + 1, Color.FromArgb(Pivot, Pivot, Pivot));
                    TEMP_IMG.SetPixel(x * 2 + 1, y * 2 + 1, Color.FromArgb(Pivot, Pivot, Pivot));

                }

            for (int x = 1; x < pictureBox1.Image.Width - 1; x++)
                for (int y = 1; y < pictureBox1.Image.Height - 1; y++)
                {
                    int CONV = (TEMP_IMG.GetPixel(x, y).G +
                                TEMP_IMG.GetPixel(x + 1, y).G +
                                TEMP_IMG.GetPixel(x, y + 1).G +
                                TEMP_IMG.GetPixel(x + 1, y + 1).G) / 4;

                    DEST_IMG.SetPixel(x, y, Color.FromArgb(CONV, CONV, CONV));
                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button17_Click(object sender, EventArgs e)
        {
            Bitmap new_bmp = new Bitmap(pictureBox1.Image.Height, pictureBox1.Image.Width);

            for (int x = 0; x < pictureBox1.Image.Width; x++)
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    int x_ = y;
                    int y_ = pictureBox1.Image.Width - 1 - x;
                    int Pivot = SRC_IMG.GetPixel(x, y).G;
                    new_bmp.SetPixel(x_, y_, Color.FromArgb((int)Pivot, (int)Pivot, (int)Pivot));
                }
            pictureBox2.Image = new_bmp;
        }

        private void button18_Click(object sender, EventArgs e)
        {
            Bitmap new_bmp = new Bitmap(pictureBox1.Image.Height, pictureBox1.Image.Width);

            for (int x = 0; x < pictureBox1.Image.Width; x++)
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    int x_ = pictureBox1.Image.Height - 1 - y;
                    int y_ = x;
                    int Pivot = SRC_IMG.GetPixel(x, y).G;
                    new_bmp.SetPixel(x_, y_, Color.FromArgb((int)Pivot, (int)Pivot, (int)Pivot));
                }
            pictureBox2.Image = new_bmp;
        }

        private void button19_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < pictureBox1.Image.Width; x++)
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    int x_ = pictureBox1.Image.Width - 1 - x;
                    int y_ = pictureBox1.Image.Height - 1 - y;
                    int Pivot = SRC_IMG.GetPixel(x, y).G;
                    DEST_IMG.SetPixel(x_, y_, Color.FromArgb((int)Pivot, (int)Pivot, (int)Pivot));
                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button20_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < pictureBox1.Image.Width; x++)
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    int x_ = x;
                    int y_ = pictureBox1.Image.Height - 1 - y;
                    int Pivot = SRC_IMG.GetPixel(x, y).G;
                    DEST_IMG.SetPixel(x_, y_, Color.FromArgb((int)Pivot, (int)Pivot, (int)Pivot));
                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button21_Click(object sender, EventArgs e)
        {
            for (int x = 0; x < pictureBox1.Image.Width; x++)
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    int x_ = pictureBox1.Image.Width - 1 - x;
                    int y_ = y;
                    int Pivot = SRC_IMG.GetPixel(x, y).G;
                    DEST_IMG.SetPixel(x_, y_, Color.FromArgb((int)Pivot, (int)Pivot, (int)Pivot));
                }
            pictureBox2.Image = DEST_IMG;
        }

        private void button22_Click(object sender, EventArgs e)
        {
            double TH;
            double.TryParse(textBox5.Text, out TH);
            TH = TH / 180.0 * Math.PI;
            int x0 = pictureBox1.Image.Width / 2;
            int y0 = pictureBox1.Image.Height / 2;

            for (int x = 0; x < pictureBox1.Image.Width; x++)
                for (int y = 0; y < pictureBox1.Image.Height; y++)
                {
                    int x_ = (int)Math.Round(Math.Cos(TH) * (x - x0) + Math.Sin(TH) * (y - y0));
                    int y_ = (int)Math.Round(-Math.Sin(TH) * (x - x0) + Math.Cos(TH) * (y - y0));

                    x_ = x0 + x_;
                    y_ = y0 + y_;
                    int Pivot = SRC_IMG.GetPixel(x, y).G;

                    if ((x_ >= 0) && (x_ < DEST_IMG.Width) && (y_ >= 0) && (y_ < DEST_IMG.Height))
                        DEST_IMG.SetPixel(x_, y_, Color.FromArgb((int)Pivot, (int)Pivot, (int)Pivot));
                }
            pictureBox2.Image = DEST_IMG;
        }


        private void vScrollBar1_Scroll(object sender, ScrollEventArgs e)
        {
            textBox1.Text = vScrollBar1.Value.ToString();
        }

        private void vScrollBar2_Scroll(object sender, ScrollEventArgs e)
        {
            textBox2.Text = vScrollBar2.Value.ToString();
        }

        private void vScrollBar3_Scroll(object sender, ScrollEventArgs e)
        {
            textBox3.Text = vScrollBar3.Value.ToString();
        }

        private void vScrollBar4_Scroll(object sender, ScrollEventArgs e)
        {
            textBox4.Text = (vScrollBar4.Value / 10.0).ToString();
        }
    }
}
