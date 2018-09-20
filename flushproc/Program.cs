using System;
using System.Runtime.InteropServices;
using System.Diagnostics;

namespace MyNamespace
{
    class MyProgram
    {
        [DllImport("psapi.dll")]
        public static extern bool EmptyWorkingSet(IntPtr hProcess);


        static void PrintHeader()
        {
            Console.WriteLine();
            Console.WriteLine("flushproc 1.0.0.0");
            Console.WriteLine("Sarang Baheti c 2018");
            Console.WriteLine();
        }

        static void PrintUsage()
        {
            Console.WriteLine("a handy utility to flush page of a process from working set");
            Console.WriteLine("this a low level utility for performance measurements");
            Console.WriteLine("might make applications/system behave odd, try at your own risk");
            Console.WriteLine();
            Console.WriteLine("flushproc <pid>");
            Console.WriteLine("flushproc <procname>");
            Console.WriteLine();
        }

        static void Main(string[] args)
        {
            PrintHeader();

            if (args.Length != 1)
            {
                PrintUsage();
                return;
            }

            string instr = args[0];
            int pid = 0;

            Process[] processes = null;
            if (Int32.TryParse(instr, out pid))
                processes = new Process[] { Process.GetProcessById(pid) };
            else
                processes = Process.GetProcessesByName(instr);

            var timer = new System.Diagnostics.Stopwatch();
            timer.Start();
            try
            {
                Console.WriteLine("Found {0} processes\n", processes.Length);
                foreach (var process in processes)
                {
                    if (EmptyWorkingSet(process.Handle))
                    {
                        Console.WriteLine("\tsuccessfully flushed pages for {0}, PID: {1}", process.ProcessName, process.Id);
                    }
                    else
                    {
                        Console.WriteLine("\tfailed to flush pages for {0}, PID: {1}", process.ProcessName, process.Id);
                    }
                }
            }
            finally
            {
                Console.WriteLine("\ntook {0} seconds", timer.ElapsedMilliseconds / 1000.0);
            }
        }
    }
}

