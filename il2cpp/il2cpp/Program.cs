using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace il2cpp
{
    internal class Program
    {
        public static int Main(string[] args)
        {

            string cmd = "";
            string projectPath = "";
            foreach(string arg in args)
            {
                // 修正参数
                string argFix = "";
                if(arg.Contains("="))
                {
                    string[] argps = arg.Split('=');

                    if(argps[0] == "--generatedcppdir")
                    {
                        projectPath = argps[1].Replace('\\', '/').Split(new string[]{"Temp"}, StringSplitOptions.None)[0];
                    }

                    argFix = string.Format(@"{0}=""{1}""", argps[0], argps[1].Replace('\\', '/'));
                }
                else
                {
                    argFix = arg;
                }
                cmd += " ";
                cmd += argFix;
            }

            var logfile = new StreamWriter(projectPath + "/il2cpp.txt", true);
            logfile.WriteLine("---------------------------------------------------------");
            logfile.WriteLine("il2cpp work directory: " + Process.GetCurrentProcess().MainModule.);

            logfile.WriteLine(cmd);
            logfile.Flush();

            ProcessStartInfo startInfo = new ProcessStartInfo();
            startInfo.CreateNoWindow = false;
            startInfo.UseShellExecute = false;
            startInfo.FileName = "/Applications/Unity/Unity.app/Contents/Frameworks/il2cpp/build/il2cpp-target.exe";
            startInfo.WindowStyle = ProcessWindowStyle.Hidden;
            startInfo.Arguments = cmd;
            startInfo.WorkingDirectory = Directory.GetCurrentDirectory();

            var proc = Process.Start(startInfo);
            proc.WaitForExit();
            logfile.WriteLine("proc exit");

            string todir = projectPath + "/Temp2/il2cppOutput/il2cppOutput/";
            if (!Directory.Exists(todir))
                Directory.CreateDirectory(todir);
            foreach (var s in Directory.GetFiles(projectPath + "/Temp/il2cppOutput/il2cppOutput/"))
            {
                File.Move(s, Path.Combine(todir, Path.GetFileName(s)));
            }

            logfile.Close();
            return proc.ExitCode;
        }
    }
}
