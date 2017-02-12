using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ComJanWpf.ViewModels
{
    class WatsonHelper
    {
        static async public Task<string> CogniteAsync(string hyoka_file)
        {
            var ret = await Task.Run(async () =>
            {
                string mydocumentspath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

                string myparam_file = mydocumentspath + @"\ComJanData\myparams.json";

                string cmd = "curl.exe ";
                string post = "-X POST ";
                string images_file = "-F \"images_file=@" + hyoka_file + "\" ";
                string myparams = "-F \"parameters=@" + myparam_file + "\" ";
                string api = "\"https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classify?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20\" ";
                string arg = post + images_file + myparams + api;
                Process process = new Process();
                process.StartInfo.FileName = cmd;
                process.StartInfo.Arguments = arg;
                process.StartInfo.UseShellExecute = false;
                process.StartInfo.RedirectStandardOutput = true;
                process.StartInfo.CreateNoWindow = true;
                process.Start();


                await Task.Run(() => Thread.Sleep(100));

                // Synchronously read the standard output of the spawned process. 
                StreamReader reader = process.StandardOutput;
                string output = reader.ReadToEnd();

                return output;
            });

            return ret;
        }

        static public string Cognite(string hyoka_file)
        {
            string mydocumentspath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            string myparam_file = mydocumentspath + @"\ComJanData\myparams.json";

            string cmd = "curl.exe ";
            string post = "-X POST ";
            string images_file = "-F \"images_file=@" + hyoka_file + "\" ";
            string myparams = "-F \"parameters=@" + myparam_file + "\" ";
            string api = "\"https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classify?api_key=367b29a6797114577d8ac9f8c9335b0ca5a3baa2&version=2016-05-20\" ";
            string arg = post + images_file + myparams + api;
            Process process = new Process();
            process.StartInfo.FileName = cmd;
            process.StartInfo.Arguments = arg;
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();


            // Synchronously read the standard output of the spawned process. 
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            return output;
        }

        static public void test()
        {
            string command = @"c:\windows\system32\ipconfig.exe";

            Process p = new Process();

            p.StartInfo.FileName = command; // 実行するファイル
            p.StartInfo.CreateNoWindow = true; // コンソールを開かない
            p.StartInfo.UseShellExecute = false; // シェル機能を使用しない

            p.StartInfo.RedirectStandardOutput = true; // 標準出力をリダイレクト

            p.Start(); // アプリの実行開始
            string output = p.StandardOutput.ReadToEnd(); // 標準出力の読み取り

            output = output.Replace("\r\r\n", "\n"); // 改行コードの修正
            Debug.Write(output); // ［出力］ウィンドウに出力
        }

    }
}
