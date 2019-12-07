using System;
using System.IO;
using System.Collections.Generic;
using DataReader.Sensors;
using CSRedis;
using System.Net.Sockets;
using System.Linq;
using System.Diagnostics;
using Newtonsoft.Json.Linq;
using DataSender;

namespace DataReader
{
    class Program
    {

        public static int time;
        internal static JToken Track;

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    //Get Track
                    bool ok = false;
                    while (ok == false)
                    {
                        if (DataSender.Program.Track.Count() > 0)
                        {
                            Track = DataSender.Program.Track;
                            ok = true;
                        }
                    }

                    //Set Banner
                    StartBanner(Track);

                    // init sensors
                    List<ISensor> sensors = new List<ISensor>
                    {
                        new VirtualSensors()
                    };

                    // configure Redis
                    var redis = new RedisClient("127.0.0.1");
                    StartServer(redis);


                    time = 5000;
                    while (true)
                    {
                        SetTime();

                        foreach (ISensor sensor in sensors)
                        {
                            // get current sensor value
                            var data = sensor.ToJson(Properties.Settings.Default.IdMezzo);
                            Console.WriteLine(data);

                            // push to redis queue
                            redis.LPush("sensors_data", data);

                            // wait 1 second
                            System.Threading.Thread.Sleep(5000);
                        }

                    }
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void StartServer(RedisClient redis)
        {
            bool ok = false;
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false;
            process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = Directory.GetFiles("../../..", "redis-server.exe", SearchOption.AllDirectories)[0];
            process.Start();

            while(ok==false)
            {
                try
                {
                    redis.Ping();
                    ok = true;
                }
                catch(Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }


        
        public static void StartBanner(JToken track)
        {
            string filename = Directory.GetFiles("../../..", "Banner.exe", SearchOption.AllDirectories)[0];
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = true;
            process.StartInfo.FileName = filename;
            process.StartInfo.Arguments = track.Value<string>("track") + " \"" + track.Value<string>("stops") + "\"" + " 30";
            process.Start();
            Console.ReadKey();
        }
        

        public static void SetTime()
        {
            string path = Directory.GetFiles("../../..", "EXTERNALTIME.txt", SearchOption.AllDirectories)[0];
            string[] lines = File.ReadAllLines(path);

            if(lines.Length>0)
                time = Convert.ToInt32(lines[0]);
        }
    }
}