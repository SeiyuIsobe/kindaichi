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
using System.Windows.Threading;

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

        private MainWindowViewModel _mainVM = null;
        private string _loadfile = @"C:\Users\s-isobe\Pictures\sample.jpg";

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            _mainVM = this.DataContext as MainWindowViewModel;
            _mainVM.Messenger.Raised += (ss, ee) =>
            {
                switch(ee.Message.MessageKey)
                {
                    case "OpenCommandMessage":
                        _loadfile = _mainVM.OpenFilePath;
                        LoadBitmapFile(_capturedImageBox);
                        break;
                }
            };

            if(null == _selectCamera)
            {
                LoadBitmapFile(_picture);
            }
            else
            {
                _device = new VideoCaptureDevice(_selectCamera.MonikerString);

                // 初期値は低解像度をハイビジョン1920x1080に設定する
                _device.VideoResolution = _device.Search(1920, 1080);

                if(null == _device.VideoResolution)
                {
                    _device.VideoResolution = _device.Search(1280, 960);
                }

                if (null != _device)
                {
                    _device.NewFrame += (ss, ee) =>
                    {
                        AForge.Video.NewFrameEventArgs eventArgs = ee as AForge.Video.NewFrameEventArgs;
                        _picture.Dispatcher.Invoke(() =>
                        {
                            _picture.Source = BitmapToBitmapFrame.Convert(eventArgs.Frame);
                        }, DispatcherPriority.Normal, new System.Threading.CancellationToken());
                    };

                    // ピクセル計算用
                    _video_width = _device.VideoResolution.FrameSize.Width;
                    _video_hight = _device.VideoResolution.FrameSize.Height;

                    // 開始
                    _device.Start();
                }
            }
        }

        private void LoadBitmapFile(System.Windows.Controls.Image img)
        {
            Bitmap bp = new Bitmap(_loadfile);
            img.Dispatcher.Invoke(() =>
            {
                img.Source = bp.ToImageSource();
            });

            // ピクセル計算用
            _video_width = bp.Width;
            _video_hight = bp.Height;
        }

        private int _video_width = 0;
        private int _video_hight = 0;
        private void _capturedImageBox_MouseDown(object sender, MouseButtonEventArgs e)
        {
            _capturedImageBox.Source = _picture.Source;

            // 保存
            BitmapSource bs = _capturedImageBox.Source as BitmapSource;
            _mainVM.SaveSource(bs.ToBitmap(System.Drawing.Imaging.PixelFormat.Format32bppArgb));
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
        private List<Bitmap> _paiList = null;

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

                //_pctPai.Margin = new Thickness(_srcPoint[0].X, _srcPoint[0].Y, 0.0, 0.0);

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

                // 200x200
                list = ChangeSizeTo200x200(list);

                for(int i = 0; i < 14; i++)
                {
                    _pctPaiList[i].Source = list[i].ToImageSource();
                }

                _paiList = list;
            }
            
        }

        private List<Bitmap> ChangeSizeTo200x200(List<Bitmap> list)
        {
            List<Bitmap> newlist = new List<Bitmap>();

            foreach (var bmp in list)
            {
                System.Drawing.Point[] src = new System.Drawing.Point[]
                {
                    new System.Drawing.Point(0, 0),
                    new System.Drawing.Point(bmp.Width - 1, 0),
                    new System.Drawing.Point(bmp.Width - 1, bmp.Height - 1),
                    new System.Drawing.Point(0, bmp.Height - 1)
                };

                System.Drawing.Point[] dst = new System.Drawing.Point[]
                {
                    new System.Drawing.Point(0, 0),
                    new System.Drawing.Point(299, 0),
                    new System.Drawing.Point(299, 299),
                    new System.Drawing.Point(0, 299)
                };

                Affine aff = new Affine();
                var newbmp = aff.Resize(bmp, src, dst);
                newlist.Add(newbmp);
            }

            return newlist;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            LoadBitmap();
        }

        private bool _loadBitmap_mode = false;
        private void LoadBitmap()
        {
            Bitmap bp = new Bitmap(@"C: \Users\ISeiy\Documents\ComJanData\Outdata\Source\m-707073601.bmp");
            _capturedImageBox.Dispatcher.Invoke(() =>
            {
                _capturedImageBox.Source = bp.ToImageSource();
            });

            // ピクセル計算用
            _video_width = bp.Width;
            _video_hight = bp.Height;

            _loadBitmap_mode = true;
        }

        private void Button_Click_1(object sender, RoutedEventArgs e)
        {
            _mainVM.SavePai(_paiList);
        }

        private void _capturedImageBox_MouseMove(object sender, MouseEventArgs e)
        {
            IInputElement iip = sender as IInputElement;
            var p = e.GetPosition(iip);
            //Debug.WriteLine($"-> ({p.X}, {p.Y}");

            // 画像上の位置
            System.Windows.Point preal = new System.Windows.Point(CalcRealX(p.X), CalcRealY(p.Y));

            var bmp = BitmapHelper.CutFrom(_capturedImageBox.Source, (int)(preal.X - 10), (int)(preal.Y - 10), 20, 20);

            _pointer.Source = bmp.ToImageSource();
        }

        private async void Button_Click_2(object sender, RoutedEventArgs e)
        {
            string s = await WatsonHelper.CogniteAsync(@"C:\Users\ISeiy\Documents\ComJanData\Outdata\MANZU\4\m-149922835.jpg");
            //string s = WatsonHelper.Cognite(@"C:\Users\ISeiy\Documents\ComJanData\Outdata\MANZU\4\m-149922835.jpg");

            System.Diagnostics.Debug.WriteLine($"-> {s}");
            
        }
    }

    
    
}
