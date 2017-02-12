using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO.Compression;
using System.IO;

namespace ComJanWpf.ViewModels
{
    class ZipHelper
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="srcpath">圧縮したいファイルがあるフォルダのパス</param>
        /// <param name="zippath"></param>
        static public void Compress(List<string> srcpathlist, string zippath)
        {
            using (var z = ZipFile.Open(zippath, ZipArchiveMode.Create))
            {
                foreach (var src in srcpathlist)
                {
                    z.CreateEntryFromFile(src, Path.GetFileName(src));
                }
            }
        }
    }
}
