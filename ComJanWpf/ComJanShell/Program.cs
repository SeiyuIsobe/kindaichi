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
            string cmd = "curl.exe";
            string arg = "\"https://gateway-a.watsonplatform.net/visual-recognition/api/v3/classifiers?api_key=309369312becca2f4209ca0ae4a3e2c05aa8aa93&version=2016-05-20\"";
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
