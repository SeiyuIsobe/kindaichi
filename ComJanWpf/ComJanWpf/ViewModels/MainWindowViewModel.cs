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
        
        private bool _isManzu = true;

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
        private bool _isKaze = true;

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

        public bool IsSangen
        {
            get { return _isSangen; }
            set
            {
                _isSangen = value;
                if (true == _isKaze) _kazesangen = "SANGEN";
            }
        }

        internal void SavePai(List<System.Drawing.Bitmap> list)
        {
            string[] kaze = new string[] { "ton", "nan", "sha", "pee" };
            string[] sangen = new string[] { "haku", "hatu", "chun" };

            for (int i = 1; i <= 9; i++)
            {
                var outdatapath = _outdatapath + "\\" + _manpinsou;

                // 出力先フォルダ（1～9）
                var bitmappath = GetBitmapPath(outdatapath, i.ToString());

                // 保存
                Save(list[i-1], bitmappath);
            }

            if (true == _isKaze)
            {
                var outdatapath = _outdatapath + "\\" + _kazesangen;

                for (int i = 1; i <= kaze.Length; i++)
                {
                    // 出力先フォルダ（ton, nan, sha, pee）
                    var bitmappath = GetBitmapPath(outdatapath, kaze[i - 1]);

                    // 保存
                    Save(list[i + 8], bitmappath);
                }
            }
            else if (true == _isSangen)
            {
                var outdatapath = _outdatapath + "\\" + _kazesangen;

                for (int i = 1; i <= sangen.Length; i++)
                {
                    // 出力先フォルダ（ton, nan, sha, pee）
                    var bitmappath = GetBitmapPath(outdatapath, sangen[i - 1]);

                    // 保存
                    Save(list[i + 8], bitmappath);
                }
            }
        }

        private static void Save(System.Drawing.Bitmap bitmap, string bitmappath)
        {

            if (true == System.IO.File.Exists(bitmappath))
            {
                // どうするか。。。
            }
            else
            {
                bitmap.Save(bitmappath);
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
    }
}
