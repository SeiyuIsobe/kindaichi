using AForge.Video.DirectShow;
using ComJan;
using ComJanWpf.ViewModels;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
//using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ComJanWpf.Views
{
    /* 
	 * ViewModelからの変更通知などの各種イベントを受け取る場合は、PropertyChangedWeakEventListenerや
     * CollectionChangedWeakEventListenerを使うと便利です。独自イベントの場合はLivetWeakEventListenerが使用できます。
     * クローズ時などに、LivetCompositeDisposableに格納した各種イベントリスナをDisposeする事でイベントハンドラの開放が容易に行えます。
     *
     * WeakEventListenerなので明示的に開放せずともメモリリークは起こしませんが、できる限り明示的に開放するようにしましょう。
     */

    /// <summary>
    /// MainWindow.xaml の相互作用ロジック
    /// </summary>
    public partial class MainWindow : Window
    {
        private VideoCaptureDevice _device = null;
        private DeviceFilters _selectCamera = null;
        private List<System.Windows.Controls.Image> _pctPaiList = new List<System.Windows.Controls.Image>();

        public MainWindow()
        {
            InitializeComponent();

            ViewModels.DeviceFilters ds = new ViewModels.DeviceFilters();
            var qs = ds.Get();
            foreach(var q in qs)
            {
                _selectCamera = q as DeviceFilters;
                break;
            }

            _pctPaiList.Add(_pai01);
            _pctPaiList.Add(_pai02);
            _pctPaiList.Add(_pai03);
            _pctPaiList.Add(_pai04);
            _pctPaiList.Add(_pai05);
            _pctPaiList.Add(_pai06);
            _pctPaiList.Add(_pai07);
            _pctPaiList.Add(_pai08);
            _pctPaiList.Add(_pai09);
            _pctPaiList.Add(_pai10);
            _pctPaiList.Add(_pai11);
            _pctPaiList.Add(_pai12);
            _pctPaiList.Add(_pai13);
            _pctPaiList.Add(_pai14);
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if(null == _selectCamera)
            {
                Bitmap bp = new Bitmap(@"C:\Users\s-isobe\Pictures\sample.jpg");
                _picture.Dispatcher.Invoke(() =>
                {
                    _picture.Source = bp.ToImageSource();
                });

                // ピクセル計算用
                _video_width = bp.Width;
                _video_hight = bp.Height;
            }
            else
            {
                _device = new VideoCaptureDevice(_selectCamera.MonikerString);

                // 初期値は低解像度をハイビジョン1920x1080に設定する
                _device.VideoResolution = _device.Search(1920, 1080);

                if(null != _device)
                {
                    _device.NewFrame += (ss, ee) =>
                    {
                        AForge.Video.NewFrameEventArgs eventArgs = ee as AForge.Video.NewFrameEventArgs;
                        _picture.Dispatcher.Invoke(() =>
                        {
                            _picture.Source = BitmapToBitmapFrame.Convert(eventArgs.Frame);
                        });
                    };

                    // ピクセル計算用
                    _video_width = _device.VideoResolution.FrameSize.Width;
                    _video_hight = _device.VideoResolution.FrameSize.Height;

                    // 開始
                    _device.Start();
                }
            }
        }

        private int _video_width = 0;
        private int _video_hight = 0;
        private void _capturedImageBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _capturedImageBox.Source = _picture.Source;
        }

        private double CalcRealX(double x)
        {
            return x * _video_width / _capturedImageBox.ActualWidth;
        }

        private double CalcRealY(double y)
        {
            return y * _video_hight / _capturedImageBox.ActualHeight;
        }

        private double CalcScreenX(double x)
        {
            return x * _capturedImageBox.ActualWidth / _video_width;
        }

        private double CalcScreenY(double y)
        {
            return y * _capturedImageBox.ActualHeight / _video_hight;
        }

        private System.Drawing.Point[] _srcPoint = new[] { new System.Drawing.Point(), new System.Drawing.Point(), new System.Drawing.Point(), new System.Drawing.Point() };
        private System.Drawing.Point[] _dstPoint = new[] { new System.Drawing.Point(), new System.Drawing.Point(), new System.Drawing.Point(), new System.Drawing.Point() };
        private int _numPointClick = 0;

        private void _capturedImageBox_MouseDown_1(object sender, MouseButtonEventArgs e)
        {
            Debug.WriteLine($"クリック位置：{e.GetPosition((UIElement)sender).X}, {e.GetPosition((UIElement)sender).Y}");
            Debug.WriteLine($"画像のピクセル位置：{CalcRealX((int)e.GetPosition((UIElement)sender).X)}, {CalcRealY((int)e.GetPosition((UIElement)sender).Y)}");

            System.Windows.Point capPoint = e.GetPosition((UIElement)sender);
            _srcPoint[_numPointClick].X = (int)CalcRealX(capPoint.X);
            _srcPoint[_numPointClick].Y = (int)CalcRealY(capPoint.Y);
            _numPointClick++;

            if (_numPointClick == 4) // 4つ目のクリックで処理する
            {
                // 平面射影のための座標を計算する
                CalcPaiRect();

                _pctPai.Margin = new Thickness(_srcPoint[0].X, _srcPoint[0].Y, 0.0, 0.0);

                // 平面射影を得る
                GetPerspectiveTransform();

                // クリア
                _numPointClick = 0;

                // 切り出し
                Bitmap dest = new Bitmap(_dstPoint[1].X - _dstPoint[0].X, _dstPoint[2].Y - _dstPoint[1].Y, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(dest);
                System.Drawing.Rectangle srcRect = new System.Drawing.Rectangle(_dstPoint[0].X, _dstPoint[0].Y, _dstPoint[1].X - _dstPoint[0].X, _dstPoint[2].Y - _dstPoint[1].Y);
                System.Drawing.Rectangle desRect = new System.Drawing.Rectangle(0, 0, _dstPoint[1].X - _dstPoint[0].X, _dstPoint[2].Y - _dstPoint[1].Y);
                g.DrawImage(((BitmapSource)_pctStill.Source).ToBitmap(System.Drawing.Imaging.PixelFormat.Format32bppArgb), desRect, srcRect, GraphicsUnit.Pixel);
                g.Dispose();
                _pctStill.Source = dest.ToImageSource();

                ComJanEngine cje = new ComJanEngine();
                var list = cje.InputTehai(dest);

                for(int i = 0; i < 14; i++)
                {
                    _pctPaiList[i].Source = list[i].ToImageSource();
                }
            }
            
        }

        private void GetPerspectiveTransform()
        {
            BitmapSource bs = _capturedImageBox.Source as BitmapSource;

            Affine aff = new Affine();
            _pctStill.Source = aff.CalcAffine(bs.ToBitmap(System.Drawing.Imaging.PixelFormat.Format32bppArgb), _srcPoint, _dstPoint).ToImageSource();
        }

        private void CalcPaiRect()
        {
            double x01 = _srcPoint[1].X - _srcPoint[0].X;
            double y01 = _srcPoint[1].Y - _srcPoint[0].Y;
            double lw = Math.Sqrt(x01 * x01 + y01 * y01);

            double x30 = _srcPoint[3].X - _srcPoint[0].X;
            double y30 = _srcPoint[3].Y - _srcPoint[0].Y;
            double lh = Math.Sqrt(x30 * x30 + y30 * y30);

            _dstPoint[0].X = _srcPoint[0].X;
            _dstPoint[0].Y = _srcPoint[0].Y;
            _dstPoint[1].X = _dstPoint[0].X + (int)Math.Round(lw);
            _dstPoint[1].Y = _dstPoint[0].Y;
            _dstPoint[2].X = _dstPoint[1].X;
            _dstPoint[2].Y = _dstPoint[0].Y + (int)Math.Round(lh);
            _dstPoint[3].X = _dstPoint[0].X;
            _dstPoint[3].Y = _dstPoint[2].Y;
        }
    }

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
            finally { DeleteObject(handle); }
        }

        public static VideoCapabilities Search(this VideoCaptureDevice device, int width, int height)
        {
            foreach(var cap in device.VideoCapabilities)
            {
                if(cap.FrameSize.Width == width && cap.FrameSize.Height == height)
                {
                    return cap;
                }
            }

            return null;
        }
    }
    
}
