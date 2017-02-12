using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ComJanShell
{
    class Program
    {
        static void Main(string[] args)
        {
            string mydocumentspath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);

            //string hyoka_file = @"C:\Users\ISeiy\Documents\ComJanData\Outdata\MANZU\3\m-1548352973.jpg";
            Console.Write(">");
            string hyoka_file = Console.ReadLine();

            string myparam_file = mydocumentspath +  @"\ComJanData\myparams.json";

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
            process.Start();

            // Synchronously read the standard output of the spawned process. 
            StreamReader reader = process.StandardOutput;
            string output = reader.ReadToEnd();

            // Write the redirected output to this application's window.
            Console.WriteLine(output);

            process.WaitForExit();
            process.Close();

            Console.WriteLine("\n\nPress any key to exit.");
            Console.ReadLine();
        }
    }
}
