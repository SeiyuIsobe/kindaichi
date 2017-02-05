using AForge.Video.DirectShow;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace ComJanWpf.ViewModels
{
    public static class BitmapHelper
    {
        public static Bitmap ToBitmap(this BitmapSource bitmapSource, System.Drawing.Imaging.PixelFormat pixelFormat)
        {
            int width = bitmapSource.PixelWidth;
            int height = bitmapSource.PixelHeight;
            int stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);  // 行の長さは色深度によらず8の倍数のため
            IntPtr intPtr = IntPtr.Zero;
            try
            {
                intPtr = Marshal.AllocCoTaskMem(height * stride);
                bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), intPtr, height * stride, stride);
                using (var bitmap = new Bitmap(width, height, stride, pixelFormat, intPtr))
                {
                    // IntPtrからBitmapを生成した場合、Bitmapが存在する間、AllocCoTaskMemで確保したメモリがロックされたままとなる
                    // （FreeCoTaskMemするとエラーとなる）
                    // そしてBitmapを単純に開放しても解放されない
                    // このため、明示的にFreeCoTaskMemを呼んでおくために一度作成したBitmapから新しくBitmapを
                    // 再作成し直しておくとメモリリークを抑えやすい
                    return new Bitmap(bitmap);
                }
            }
            finally
            {
                if (intPtr != IntPtr.Zero)
                    Marshal.FreeCoTaskMem(intPtr);
            }
        }

        [DllImport("gdi32.dll", EntryPoint = "DeleteObject")]
        [return: MarshalAs(UnmanagedType.Bool)]
        public static extern bool DeleteObject([In] IntPtr hObject);

        public static ImageSource ToImageSource(this Bitmap bmp)
        {
            var handle = bmp.GetHbitmap();
            try
            {
                return Imaging.CreateBitmapSourceFromHBitmap(handle, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
            }
            catch (Exception e)
            {
                System.Diagnostics.Debug.WriteLine($"-> {e.Message}");
                return null;
            }
            finally { DeleteObject(handle); }
        }

        public static VideoCapabilities Search(this VideoCaptureDevice device, int width, int height)
        {
            foreach (var cap in device.VideoCapabilities)
            {
                if (cap.FrameSize.Width == width && cap.FrameSize.Height == height)
                {
                    return cap;
                }
            }

            return null;
        }

        // ピクセル毎のARGB値の平均二乗誤差
        public static double Comp(this Bitmap bp, Bitmap targetBp)
        {
            double ret = 0;

            for (int x = 0; x < bp.Width; x++)
            {
                for (int y = 0; y < bp.Height; y++)
                {
                    System.Drawing.Color c_bp = bp.GetPixel(x, y);
                    System.Drawing.Color c_target = targetBp.GetPixel(x, y);

                    double sa_color = Math.Sqrt(Math.Pow(c_bp.ToArgb() - c_target.ToArgb(), 2));

                    // 累積
                    ret = ret + sa_color;
                }
            }

            return ret;
        }

        public static Bitmap CutFrom(ImageSource src, int x, int y, int w, int h)
        {
            return CutFrom(((BitmapSource)src).ToBitmap(System.Drawing.Imaging.PixelFormat.Format32bppArgb), x, y, w, h);
        }

        public static Bitmap CutFrom(Bitmap src, int x, int y, int w, int h)
        {
            Bitmap dest = new Bitmap(w, h);
            Graphics g = Graphics.FromImage(dest);
            Rectangle srcRect = new Rectangle(x, y, w, h);
            Rectangle desRect = new Rectangle(0, 0, w, h);
            g.DrawImage(src, desRect, srcRect, GraphicsUnit.Pixel);
            g.Dispose();
            return dest;
        }

    }
}
