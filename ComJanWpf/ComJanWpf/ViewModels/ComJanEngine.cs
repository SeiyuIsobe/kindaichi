using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using ComJanWpf.ViewModels;

namespace ComJan
{
    public class ComJanEngine
    {
        const int PAI_NUM = 14;

        Bitmap m_tehai;
        //Bitmap[] m_tehaimoto = new Bitmap[PAI_NUM];
        //Bitmap[] m_tehairg = new Bitmap[PAI_NUM];
        List<Bitmap> _tehai_moto = new List<Bitmap>();
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
        public List<Bitmap> InputTehai(Bitmap b)
        {
            System.Diagnostics.Debug.WriteLine("b.Width = " + b.Width);
            b.Save(@"C:\Users\ISeiy\Documents\ComJanData\temp\bb_still.bmp");

            // クリア
            _tehai_moto.Clear();

            m_tehai = b;

            for (int i = 0; i < PAI_NUM; i++)
            {
                double x0 = Math.Round((double)b.Width / (double)PAI_NUM * i);
                double sx = Math.Round((double)b.Width / (double)PAI_NUM);

                // 14等分の切り出し
                Rectangle r0 = new Rectangle((int)(x0), 0, (int)(sx), b.Height);
                Rectangle r = new Rectangle(0, 0, (int)(sx), b.Height);

                Bitmap bb = new Bitmap((int)sx, b.Height);
                using (Graphics g = Graphics.FromImage(bb))
                {
                    g.DrawImage(b, r, r0, GraphicsUnit.Pixel);
                }
                //bb.Save(@"C:\Users\ISeiy\Documents\ComJanData\temp\bb_" + i.ToString() + ".bmp");

                // 14等分の幅に対して左右10px
                Bitmap base_bb = GetBaseBitmap(b, i);

                bb = GetBitmap(base_bb, bb);

                _tehai_moto.Add(bb);
            }

            return _tehai_moto;
        }

        /// <summary>
        /// 1px横にずらしながら探索する
        /// </summary>
        /// <param name="base_bb"></param>
        /// <param name="bb"></param>
        /// <returns></returns>
        private Bitmap GetBitmap(Bitmap base_bb, Bitmap bb)
        {
            Rectangle r = new Rectangle(0, 0, bb.Width, bb.Height);

            int justfit = 0;
            double sa = -1.0;
            Bitmap justbmp = new Bitmap(1, 1);

            // 1px横にずらす
            for (int i = 0; i < base_bb.Width - bb.Width; i++)
            {
                Rectangle r0 = new Rectangle(i, 0, bb.Width, bb.Height);

                Bitmap b0 = new Bitmap(bb.Width, bb.Height);
                using (Graphics g = Graphics.FromImage(b0))
                {
                    g.DrawImage(base_bb, r, r0, GraphicsUnit.Pixel);
                }

                //b0.Save(@"C:\Users\ISeiy\Documents\ComJanData\temp\b0_" + i.ToString() + ".bmp");

                // 比べる
                double sa_temp = b0.Comp(bb);
                if(sa < 0.0 || sa > sa_temp)
                {
                    sa = sa_temp;
                    justfit = i;
                    justbmp = b0;

                    //System.Diagnostics.Debug.WriteLine($"fit: i = {i}, sa = {sa}");
                }
            }

            System.Diagnostics.Debug.WriteLine($"{justfit}pxで最も一致 -> 類似度：{sa}");
            return justbmp;
        }

        private Bitmap GetBaseBitmap(Bitmap b, int i)
        {
            int haba = 10;

            double x0 = Math.Round((double)b.Width / (double)PAI_NUM * i) - (double)haba;
            double sx = Math.Round((double)b.Width / (double)PAI_NUM) + 2.0 * (double)haba;

            // 14等分の切り出し
            Rectangle r0 = new Rectangle((int)(x0), 0, (int)(sx), b.Height);
            Rectangle r = new Rectangle(0, 0, (int)(sx), b.Height);

            Bitmap bb = new Bitmap((int)sx, b.Height);
            using (Graphics g = Graphics.FromImage(bb))
            {
                g.DrawImage(b, r, r0, GraphicsUnit.Pixel);
            }

            bb.Save(@"C:\Users\ISeiy\Documents\ComJanData\temp\" + i.ToString() + ".bmp");
            return bb;
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
                return _tehai_moto;
            }
        }

        public int f(double ff)
        {
            return 0;
        }

        public Bitmap TwoColorscale(Bitmap bmp, int nichi)
        {
            // 二値化
            for (int w = 0; w < bmp.Width; w++)
            {
                for (int h = 0; h < bmp.Height; h++)
                {
                    if (bmp.GetPixel(w, h).R >= nichi)
                    {
                        bmp.SetPixel(w, h, Color.White);
                    }
                    else
                    {
                        //System.Diagnostics.Debug.WriteLine("{0}\t{1}\t{2}", w, h, bmp.GetPixel(w, h).R);
                        bmp.SetPixel(w, h, Color.Black);
                    }
                }
            }

            return bmp;
        }

        
    }
}
