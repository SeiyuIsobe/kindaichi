using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace ComJan
{
    internal class ComJanEngine
    {
        const int PAI_NUM = 14;

        Bitmap m_tehai;
        //Bitmap[] m_tehaimoto = new Bitmap[PAI_NUM];
        //Bitmap[] m_tehairg = new Bitmap[PAI_NUM];
        List<Bitmap> m_tehai_moto = new List<Bitmap>();
        List<Bitmap> m_tehai_rg;

        public ComJanEngine()
        {
        }

        ~ComJanEngine()
        {
        }

        /// <summary>
        /// ビットマップbの幅は14の整数倍であること
        /// </summary>
        /// <param name="b"></param>
        public void InputTehai(Bitmap b)
        {
            System.Diagnostics.Debug.WriteLine("b.Width = " + b.Width);

            // クリア
            m_tehai_moto.Clear();

            m_tehai = b;

            for (int i = 0; i < PAI_NUM; i++)
            {
                double x0 = Math.Round((double)b.Width / (double)PAI_NUM * i);
                double sx = Math.Round((double)b.Width / (double)PAI_NUM);

                // 一回目の切り出し
                Rectangle r0 = new Rectangle((int)(x0), 0, (int)(sx), b.Height);
                Rectangle r = new Rectangle(0, 0, (int)(sx), b.Height);
                System.Diagnostics.Debug.WriteLine(x0.ToString());
                //Bitmap bb = b.Clone(r, System.Drawing.Imaging.PixelFormat.Format24bppRgb);
                Bitmap bb = new Bitmap((int)sx, b.Height);
                using (Graphics g = Graphics.FromImage(bb))
                {
                    g.DrawImage(b, r, r0, GraphicsUnit.Pixel);
                }
                //bb.Save("a" + i.ToString() + ".bmp");

                // 補正
                Point wp = CalcWeightPoint(BpSolution.GrayScale.TwoColorscale(bb, 100/*, "b" + i.ToString() + ".bmp"*/));
                int sx_center = (int)(Math.Round((double)sx / 2.0));
                int hosei = 0;
                if (sx_center > wp.X) // 左にズレている
                {
                    hosei = sx_center - wp.X;
                }
                else if (sx_center < wp.X) // 右にズレている
                {
                    hosei = wp.X - sx_center;
                }
                else
                {
                }

                // ズレを考慮してもう一回切り出す
                //if (hosei != 0)
                {
                    bb.Dispose();
                    r0 = new Rectangle((int)(x0) - hosei, 0, (int)(sx), b.Height);
                    r = new Rectangle(0, 0, (int)(sx), b.Height);
                    Bitmap bb2 = new Bitmap((int)sx, b.Height);
                    using (Graphics g = Graphics.FromImage(bb2))
                    {
                        g.DrawImage(b, r, r0, GraphicsUnit.Pixel);
                    }
                    if ((int)(x0) - hosei < 0) // いちばん左端
                    {
                        for (int w = 0; w < hosei - (int)x0; w++)
                        {
                            for (int h = 0; h < bb2.Height; h++)
                            {
                                bb2.SetPixel(w, h, Color.White);
                            }
                        }
                    }
                    //System.Diagnostics.Debug.WriteLine(x0.ToString());
                    //bb2.Save("c" + i.ToString() + ".bmp");
                    //BpSolution.GrayScale.TwoColorscale(bb2, 100, "d" + i.ToString() + ".bmp");
                    bb = bb2;
                }
                //else
                //{
                //    // ok
                //}

                //IsMatch(b, bb, (int)sx, -10);

                m_tehai_moto.Add(bb);
            }
        }

        private Point CalcWeightPoint(Bitmap bb)
        {
            int wpx = 0;
            int wpy = 0;
            int kosu = 0;
            for (int w = 0; w < bb.Width; w++)
            {
                for (int h = 0; h < bb.Height; h++)
                {
                    if (0 == bb.GetPixel(w, h).R)
                    {
                        wpx += w;
                        wpy += h;
                        kosu++;
                    }
                }
            }

            return new Point((int)(wpx / kosu), (int)(wpy / kosu));
        }

        /// <summary>
        /// Bitmap bbをbに重ねてピタリ一致する場所を探す
        /// 最初はw0+offsetの位置から始める
        /// </summary>
        /// <param name="b"></param>
        /// <param name="bb"></param>
        /// <param name="w0"></param>
        /// <param name="offset"></param>
        private void IsMatch(Bitmap b, Bitmap bb, int w0, int offset)
        {
            for (int i = 0; i < offset * 2; i++)
            {
               

                for (int w = 0; w < bb.Width; w++)
                {
                    for (int h = 0; h < bb.Height; h++)
                    {
                        int r_b = b.GetPixel(w0 + offset + w, h).R;
                        int g_b = b.GetPixel(w0 + offset + w, h).G;
                        int b_b = b.GetPixel(w0 + offset + w, h).B;

                        int r_bb = bb.GetPixel(w, h).R;
                        int g_bb = bb.GetPixel(w, h).G;
                        int b_bb = bb.GetPixel(w, h).B;
                    }
                }

                
            }
        }

        public List<Bitmap> TehaiMoto
        {
            get
            {
                return m_tehai_moto;
            }
        }

        public int f(double ff)
        {
            return 0;
        }
    }
}
