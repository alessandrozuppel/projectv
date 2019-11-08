using System;
using System.IO;
using System.Collections.Generic;

namespace External
{
    class Program
    {
        static void Main(string[] args)
        {
            List<string> commands = new List<string>()
            {
                "wait start",
                "wait stop",
                "speed start",
                "speed stop",
                "clear server",
                "clear vehicle",
                "clear redis",
                "time",
                "history",
                "serverip",
                "serverport",
                "apipath",
                "commands"
        };

            List<string> history = new List<string>();

            int time = -1;

            string path = @"EXTERNALAPP.txt";
            while (true)
            {
                string el = Console.ReadLine();
                history.Add(el);

                if (el.Contains("time"))
                {
                    string[] el1 = el.Split(' ');
                    try
                    {
                        el = el1[0];
                        time = Convert.ToInt32(el1[1]);
                    }
                    catch
                    {
                        el = "no";
                    }

                }

                if (el=="history")
                {
                    Console.WriteLine();
                    Console.WriteLine("######HISTORY######");
                    foreach (string cmd in history)
                    {
                        Console.WriteLine(cmd);
                    }
                    Console.WriteLine("###################");
                    Console.WriteLine();
                }

                if(el=="commands")
                {
                    Console.WriteLine();
                    Console.WriteLine("######COMMANDS######");
                    foreach (string cmd in commands)
                        Console.WriteLine(cmd);

                    Console.WriteLine("####################");
                    Console.WriteLine();
                }

                if(commands.Contains(el))
                {
                    if(time>0)
                    {
                        el = el + " " + time;
                        time = -1;
                    }

                    File.WriteAllText(path, el);
                    //Console.Clear();
                }
                else
                {
                    File.WriteAllText(path, el);
                }
            }
        }
    }
}
