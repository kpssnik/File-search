using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Threading;

namespace StepProject
{
    class Program
    {
        static List<string> foundFiles = new List<string>();
        static void Main(string[] args)
        {

            if (args.Length <= 0)
            {
                PrintMessage("Insert filename to search", ConsoleColor.Red);
            }
            else
            {
                Console.WriteLine("Search started...");
                foreach (var drive in DriveInfo.GetDrives())
                {
                    new Thread(() =>
                    {
                        KpssSearch($"{drive}\\", args[0]);
                    }).Start();
                }

                //foreach (var a in foundFiles) PrintMessage(a, ConsoleColor.Green);            
            }
            Console.ReadLine();
        }


        static void KpssSearch(string dir, string pattern)
        {
            for (int i = 0; i < Directory.GetDirectories(dir).Length; i++)
            {
                try
                {
                    foreach (string f in Directory.GetFiles(Directory.GetDirectories(dir)[i]))
                    {
                        if (f.EndsWith($"\\{pattern}"))
                        {
                            foundFiles.Add(f);
                            PrintMessage(f, ConsoleColor.Green);
                        }
                    }
                    KpssSearch(Directory.GetDirectories(dir)[i], pattern);
                }
                catch (Exception ex)
                {
                    //PrintMessage(ex.Message, ConsoleColor.Red);
                    continue;
                }
            }
        }

        static void PrintMessage(string msg, ConsoleColor color)
        {
            Console.ForegroundColor = color;
            Console.WriteLine(msg);
            Console.ResetColor();
        }
    }
}
