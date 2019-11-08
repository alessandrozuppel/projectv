using CSRedis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataSender
{
    public static class External
    {
        //write application values
        public static void WriteValues()
        {
            string valuesToWrite = String.Empty;
            foreach (string[] el in SetValList())
            {
                valuesToWrite += el[0] + " " + el[1] + "\r\n";
            }
            /*
            valuesToWrite = "ip " + Program.ip + "\r\n" +
                            "port " + Program.port + "\r\n" +
                            "api_path " + Program.api_path + "\r\n" +
                            "url " + Program.url + "\r\n" +
                            "urlToken " + Program.urlToken + "\r\n";*/
            File.WriteAllText(Properties.Settings.Default.ExternalValuesPath, valuesToWrite);
            
        }

        //read application input
        public static string ReadValues()
        {
            string[] lines = File.ReadAllLines(Properties.Settings.Default.ExternalAppPath);
            try
            {
                if (lines.Length > 0)
                {
                    File.WriteAllText(Properties.Settings.Default.ExternalAppPath, null);
                    return lines[0];
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            return String.Empty;
        }



        public static void Execute(string el)
        {
            int time = 5000;

            /*if (el.Contains("time"))
            {
                string[] el1 = el.Split(' ');

                el = el1[0];
                time = Convert.ToInt32(el1[1]);
            }*/

            string[] el1 = el.Split(' ');

            el = el1[0];

            switch (el)
            {
                case ("wait stop"): { Program.wait = false; break; }
                case ("wait start"): { Program.wait = true; break; }
                case ("speed start"): { Program.speed = true; break; }
                case ("speed stop"): { Program.speed = false; break; }
                case ("serverip"): { Program.ip = el1[1]; SetValues(); break; }
                case ("serverport"): { Program.port = el1[1]; SetValues(); break; }
                case ("apipath"): { Program.api_path = el1[1]; break; }
                case ("autentication"): { Program.autentication = Convert.ToBoolean(el1[1]); break; }
                case ("clear server"):
                    {
                        try
                        {
                            Console.WriteLine("Command not available");
                        }
                        catch
                        { }

                        break;
                    }
                case ("clear vehicle"):
                    {
                        try
                        {
                            Console.WriteLine("Command not available");
                        }
                        catch
                        { }

                        break;
                    }
                case ("clear redis"):
                    {
                        var redis = new RedisClient("127.0.0.1");
                        redis.Del("sensors_data");
                        redis.Dispose();
                        break;
                    }
                case ("time"):
                    {
                        time = Convert.ToInt32(el1[1]);
                        File.WriteAllText(Properties.Settings.Default.ExternalTimePath, time.ToString());
                        break;
                    }
            }
        }


        internal static void SetValues()
        {
            Program.url = "http://" + Program.ip + ":" + Program.port + Program.api_path;
            Program.urlToken = "http://" + Program.ip + ":" + Program.port + "/token";
        }

        internal static List<string[]> SetValList()
        {
            List<string[]> vallist = new List<string[]>
            {
                new string[2] {"ip", Program.ip},
                new string[2] {"port", Program.port },
                new string[2] {"api_path", Program.api_path },
                new string[2] {"url", Program.url },
                new string[2] {"urlToken", Program.urlToken },
            };

            return vallist;
        }
    }
}
