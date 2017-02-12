using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

using Livet;
using Livet.Commands;
using Livet.Messaging;
using Livet.Messaging.IO;
using Livet.EventListeners;
using Livet.Messaging.Windows;

using ComJanWpf.Models;
using System.Drawing;
using ComJan;
using System.Diagnostics;

namespace ComJanWpf.ViewModels
{
    public class MainWindowViewModel : ViewModel
    {
        private string _outdatapath = string.Empty;
        private System.Diagnostics.Stopwatch _stopwatch = new System.Diagnostics.Stopwatch();

        public MainWindowViewModel()
        {
            string myDocumentPath = System.IO.Path.Combine(Environment.ExpandEnvironmentVariables("%userprofile%"), "Documents");

            Action createFolderAction = new Action(() =>
            {
                try
                {
                    System.IO.Directory.CreateDirectory(myDocumentPath + "\\ComJanData\\Outdata");
                    _outdatapath = myDocumentPath + "\\ComJanData\\Outdata";
                }
                catch{}
            });

            if(true == System.IO.Directory.Exists(myDocumentPath + "\\ComJanData"))
            {
                if(true == System.IO.Directory.Exists(myDocumentPath + "\\ComJanData\\Outdata"))
                {
                    _outdatapath = myDocumentPath + "\\ComJanData\\Outdata";
                }
                else
                {
                    createFolderAction();
                }
            }
            else
            {
                createFolderAction();
            }

            // ストップウォッチでアプリ稼働時間を計測
            _stopwatch.Start();
        }

        public void Initialize()
        {
        }

        private string _manpinsou = string.Empty;
        private string _kazesangen = string.Empty;
        
        private bool _isManzu = false;

        public bool IsManzu
        {
            get { return _isManzu; }
            set
            {
                _isManzu = value;
                if (true == _isManzu) _manpinsou = "MANZU";
            }
        }
        
        private bool _isPinzu = false;

        public bool IsPinzu
        {
            get { return _isPinzu; }
            set
            {
                _isPinzu = value;
                if (true == _isPinzu) _manpinsou = "PINZU";
            }
        }
        private bool _isSouzu = false;

        public bool IsSouzu
        {
            get { return _isSouzu; }
            set
            {
                _isSouzu = value;
                if (true == _isSouzu) _manpinsou = "SOUZU";
            }
        }
        private bool _isKaze = false;

        public bool IsKaze
        {
            get { return _isKaze; }
            set
            {
                _isKaze = value;
                if (true == _isKaze) _kazesangen = "KAZE";
            }
        }
        private bool _isSangen = false;
        private bool _isCreatedStudyData = false;

        public bool IsSangen
        {
            get { return _isSangen; }
            set
            {
                _isSangen = value;
                if (true == _isSangen) _kazesangen = "SANGEN";
            }
        }

        internal void SavePai(List<System.Drawing.Bitmap> list)
        {
            string[] kaze = new string[] { "ton", "nan", "sha", "pee" };
            string[] sangen = new string[] { "haku", "hatu", "chun" };
            string[] manzucomps = new string[]
            {
@"C:\Users\seiyu\Documents\ComJanData\MANZU1.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU2.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU3.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU4.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU5.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU6.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU7.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU8.zip",
@"C:\Users\seiyu\Documents\ComJanData\MANZU9.zip"
            };
            string[] pinzucomps = new string[]
            {
@"C:\Users\seiyu\Documents\ComJanData\PINZU1.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU2.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU3.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU4.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU5.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU6.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU7.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU8.zip",
@"C:\Users\seiyu\Documents\ComJanData\PINZU9.zip"
            };
            string[] souzucomps = new string[]
            {
@"C:\Users\seiyu\Documents\ComJanData\SOUZU1.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU2.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU3.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU4.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU5.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU6.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU7.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU8.zip",
@"C:\Users\seiyu\Documents\ComJanData\SOUZU9.zip"
            };
            string[] kazecomps = new string[]
            {
@"C:\Users\seiyu\Documents\ComJanData\ton.zip",
@"C:\Users\seiyu\Documents\ComJanData\nan.zip",
@"C:\Users\seiyu\Documents\ComJanData\sha.zip",
@"C:\Users\seiyu\Documents\ComJanData\pee.zip"
            };
            string[] sangencomps = new string[]
            {
@"C:\Users\seiyu\Documents\ComJanData\haku.zip",
@"C:\Users\seiyu\Documents\ComJanData\hatu.zip",
@"C:\Users\seiyu\Documents\ComJanData\chun.zip"
            };
            int num = 300;

            if(string.Empty != _manpinsou)
            {
                for (int i = 1; i <= 9; i++)
                {
                    // 圧縮用
                    List<string> complist = new List<string>();

                    var outdatapath = _outdatapath + "\\" + _manpinsou;

                    // 出力先フォルダ（1～9）
                    var bitmappath = GetBitmapPath(outdatapath, i.ToString());

                    // 保存
                    var ss = Save(list[i - 1], bitmappath);

                    // 圧縮
                    complist.Add(ss);

                    // 学習データ
                    if (true == _isCreatedStudyData)
                    {
                        for (int k = 0; k < num; k++)
                        {
                            var bmp = CreateStudyData(list[i - 1]);
                            var bmp_path = GetBitmapPath(outdatapath, i.ToString());
                            var s3 = Save(bmp, bmp_path);

                            // 圧縮
                            complist.Add(s3);
                        }

                        // 圧縮実行
                        string compspath = string.Empty;
                        if (true == _isManzu) compspath = manzucomps[i - 1];
                        if (true == _isPinzu) compspath = pinzucomps[i - 1];
                        if (true == _isSouzu) compspath = souzucomps[i - 1];

                        ZipHelper.Compress(complist, compspath);
                    }
                }

                System.Diagnostics.Debug.WriteLine("マンズ、ピンズ、ソウズを保存しました");
            }
            else
            {
                System.Diagnostics.Debug.WriteLine("マンズ、ピンズ、ソウズが選ばれてないので保存しません");
            }

            if (true == _isKaze)
            {
                var outdatapath = _outdatapath + "\\" + _kazesangen;

                for (int i = 1; i <= kaze.Length; i++)
                {
                    // 圧縮用
                    List<string> complist = new List<string>();

                    // 出力先フォルダ（ton, nan, sha, pee）
                    var bitmappath = GetBitmapPath(outdatapath, kaze[i - 1]);

                    // 保存
                    var ss = Save(list[i + 8], bitmappath);

                    // 圧縮
                    complist.Add(ss);

                    // 学習データ
                    if (true == _isCreatedStudyData)
                    {
                        for (int k = 0; k < num; k++)
                        {
                            var bmp = CreateStudyData(list[i + 8]);
                            var bmp_path = GetBitmapPath(outdatapath, kaze[i - 1]);
                            var s3 = Save(bmp, bmp_path);

                            // 圧縮
                            complist.Add(s3);
                        }

                        // 圧縮実行
                        string compspath = string.Empty;
                        compspath = kazecomps[i - 1];
                        ZipHelper.Compress(complist, compspath);
                    }
                }

                Debug.WriteLine("風牌を保存しました");
            }
            else if (true == _isSangen)
            {
                var outdatapath = _outdatapath + "\\" + _kazesangen;

                for (int i = 1; i <= sangen.Length; i++)
                {
                    // 圧縮用
                    List<string> complist = new List<string>();

                    // 出力先フォルダ（ton, nan, sha, pee）
                    var bitmappath = GetBitmapPath(outdatapath, sangen[i - 1]);

                    // 保存
                    var ss = Save(list[i + 8], bitmappath);

                    // 圧縮
                    complist.Add(ss);

                    // 学習データ
                    if (true == _isCreatedStudyData)
                    {
                        for (int k = 0; k < num; k++)
                        {
                            var bmp = CreateStudyData(list[i + 8]);
                            var bmp_path = GetBitmapPath(outdatapath, sangen[i - 1]);
                            var s3 = Save(bmp, bmp_path);

                            // 圧縮
                            complist.Add(s3);
                        }

                        // 圧縮実行
                        string compspath = string.Empty;
                        compspath = sangencomps[i - 1];
                        ZipHelper.Compress(complist, compspath);
                    }
                }

                System.Diagnostics.Debug.WriteLine("三元牌を保存しました");
            }
            else
            {
                Debug.WriteLine("風牌か三元牌を選択していないので保存しません");
            }
        }

        internal void SaveSource(Bitmap bitmap)
        {
            var outdatapath = GetBitmapPath(_outdatapath, "Source");
            bitmap.Save(outdatapath);
        }

        private static string Save(System.Drawing.Bitmap bitmap_org, string bitmappath)
        {
            Bitmap bitmap = bitmap_org;

            // 二値化
            if(true)
            {
                bitmap = ComJanEngine.TwoColorscale(bitmap_org, 100);
            }

            if (true == System.IO.File.Exists(bitmappath))
            {
                // どうするか。。。
                return null;
            }
            else
            {
                //bitmap.Save(bitmappath);

                // jpeg
                bitmappath = bitmappath.Replace(".bmp", ".jpg");
                bitmap.Save(bitmappath, System.Drawing.Imaging.ImageFormat.Jpeg);

                return bitmappath;
            }
        }

        private string GetBitmapPath(string outpath, string folder)
        {
            var outputfolder = outpath + "\\" + folder;
            if (false == System.IO.Directory.Exists(outputfolder)) System.IO.Directory.CreateDirectory(outputfolder);

            // ビットマップファイル名
            // 重複しないように年月日時間稼働時間で文字列を作って
            // さらにハッシュで整数化する
            var bitmappath = outputfolder + "\\" + DateTime.Now.ToLongDateString() + DateTime.Now.ToLongTimeString() + "_" + _stopwatch.ElapsedMilliseconds.ToString() + ".bmp";
            bitmappath = outputfolder + "\\" + "m" + bitmappath.GetHashCode().ToString() + ".bmp";

            return bitmappath;
        }

        public void LoadBitmap()
        {

        }

        public Bitmap CreateStudyData(Bitmap bmp)
        {
            Point[] src = new Point[4];
            src[0] = new Point(0, 0);
            src[1] = new Point(bmp.Width, 0);
            src[2] = new Point(bmp.Width, bmp.Height);
            src[3] = new Point(0, bmp.Height);

            Point[] dst = new Point[4];
            dst[0] = GetRandomPoint(src[0], (double)bmp.Width / 5.0);
            dst[1] = GetRandomPoint(src[1], (double)bmp.Width / 5.0);
            dst[2] = GetRandomPoint(src[2], (double)bmp.Width / 5.0);
            dst[3] = GetRandomPoint(src[3], (double)bmp.Width / 5.0);

            Affine aff = new Affine();
            Bitmap bb = aff.CalcAffine(bmp, dst, src);
            return bb;
        }

        public Point GetRandomPoint(Point p0, double hankei)
        {
            Random r = new Random(); // seed値は小数部の4桁だけ使う

            int v = r.Next(0, 90); // 0から90度の範囲
            double s = (double)r.Next(0, 150) / 100.0; // 0から1.5倍の範囲

            Point ret = new Point(0, 0);

            double x = hankei * s * Math.Cos(Math.PI * (double)v / 180.0);
            double y = hankei * s * Math.Sin(Math.PI * (double)v / 180.0);

            // 左上 -> (0, 0)
            if (0 == p0.X && 0 == p0.Y)
            {
                ret = new Point((int)x, (int)y);
            }
            else if(0 < p0.X && 0 == p0.Y) // 右上
            {
                ret = new Point(p0.X - (int)y, (int)x);
            }
            else if(0 < p0.X && 0 < p0.Y) // 右下
            {
                ret = new Point(p0.X - (int)x, p0.Y - (int)y);
            }
            else if(0 == p0.X && 0 < p0.Y) // 左下
            {
                ret = new Point((int)y, p0.Y - (int)x);
            }

            return ret;
        }

        public bool IsCreatedStudyData
        {
            get
            {
                return _isCreatedStudyData;
            }
            set
            {
                _isCreatedStudyData = value;
            }
        }

        #region OpenCommand
        private ListenerCommand<OpeningFileSelectionMessage> _openCommand = null;
        public ListenerCommand<OpeningFileSelectionMessage> OpenCommand
        {
            get
            {
                if(null == _openCommand)
                {
                    _openCommand = new ListenerCommand<OpeningFileSelectionMessage>(
                        new Action<OpeningFileSelectionMessage>((s) =>
                        {
                            OpeningFileSelectionMessage m = s as OpeningFileSelectionMessage;
                            if (null != m)
                            {
                                foreach(var ss in m.Response)
                                {
                                    this.OpenFilePath = ss;
                                    Messenger.Raise(new WindowActionMessage("OpenCommandMessage"));
                                    break;
                                }
                            }
                        }));
                }
                return _openCommand;
            }
        }
        public string OpenFilePath { get; set; }
        #endregion
    }
}
