using System;
using System.Collections.Generic;
using System.Threading;
using System.IO;

namespace FileSearch
{
    class Program
    {
        static List<string> foundFiles = new List<string>();

        static void Main(string[] args)
        {
            if (args.Length <= 0)
            {
                PrintMessage("Launch this application only by cmd. Insert application path and filename to search.", ConsoleColor.Red);
                return;
            }
            
            if (args.Length > 1 && !string.IsNullOrWhiteSpace(args[1]))
            {
                Console.WriteLine($"Search started at {args[1]}:\\");
                CheckRootDirectory($"{args[1]}:\\", args[0]);
                KpssSearch($"{args[1]}:\\", args[0]);
                Console.WriteLine("Search finished...");

            }
            else
            {          
                Console.WriteLine("Full search started...");
                foreach (var drive in DriveInfo.GetDrives())
                {
                    CheckRootDirectory(drive.ToString(), args[0]);
                    new Thread(() =>
                    {
                        KpssSearch($"{drive}", args[0]);
                    }).Start();
                }
            }

            Console.ReadLine();
        }

        static void KpssSearch(string dir, string pattern)
        {            
            for (int i = 0; i < Directory.GetDirectories(dir).Length; i++)
            {
                try
                {
                    //Console.WriteLine(Directory.GetDirectories(dir)[i]);
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
                catch (Exception)
                {
                    continue;
                }
            }
        }

        static void CheckRootDirectory(string dir, string pattern)
        {
            foreach (var file in Directory.GetFiles(dir))
            {
                if (file.EndsWith($"\\{pattern}"))
                {
                    foundFiles.Add(file);
                    PrintMessage(file, ConsoleColor.Green);
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

