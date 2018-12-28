using System;
using System.IO;
using System.Threading;
using LibGGPK;

namespace DefragmentGPPK
{
    class Program
    {
        static void Main(string[] args)
        {
            var ggpkPath = args.Length > 0 ? args[0] : "";
            while (!TestPath(ggpkPath))
            {
                Console.Write("Enter full GGPK name (path + filename) # ");
                ggpkPath = Console.ReadLine();
            }
            

            var workerThread = new Thread(() =>
            {
                var content = new GrindingGearsPackageContainer();
                try
                {
                    content.Read(ggpkPath, Output);
                    content.Save(ggpkPath + ".defragmented", Output);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return;
                }
                OutputLine("Defragmented GGPK Successfully");
            });
            workerThread.Start();
        }

        private static bool TestPath(string path)
        {
            // check extension
            if (!File.Exists(path))
            {
                Console.WriteLine("File not found");
                return false;
            }

            if (!".ggpk".Equals(Path.GetExtension(path)))
            {
                Console.WriteLine("File has wrong extension");
                return false;
            }
                
            return true;
        }

        private static void Output(string msg)
        {
            Console.WriteLine(msg);
        }

        private static void OutputLine(string msg)
        {
            Output(msg + Environment.NewLine);
        }
    }
}
