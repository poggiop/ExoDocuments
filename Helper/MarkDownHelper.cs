using System;
using System.Collections.Generic;
using System.Configuration;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Helper
{
    public class MarkDownHelper
    {
        public static string Convert(string source, string sourceFormat, string destFormat)
        {
            string processName = ConfigurationManager.AppSettings["PandocPath"]; //@"C:\Program Files\Pandoc\bin\pandoc.exe";
            string args = String.Format(@"--from {0} --to {1} ", sourceFormat,destFormat);

            ProcessStartInfo psi = new ProcessStartInfo(processName, args);

            psi.RedirectStandardOutput = true;
            psi.RedirectStandardInput = true;

            Process p = new Process();
            p.StartInfo = psi;
            psi.UseShellExecute = false;
            p.Start();

            string outputString = "";
            //byte[] inputBuffer = Encoding.UTF8.GetBytes(source);
            //p.StandardInput.BaseStream.Write(inputBuffer, 0, inputBuffer.Length);
            //p.StandardInput.Close();
            using (var wr = new System.IO.StreamWriter(p.StandardInput.BaseStream)) {
                wr.Write(source);
                wr.Flush();
            }

            p.WaitForExit(2000);
            using (System.IO.StreamReader sr = new System.IO.StreamReader(
                                                   p.StandardOutput.BaseStream))
            {

                outputString = sr.ReadToEnd();
            }

            return outputString;
        }
    }
}
