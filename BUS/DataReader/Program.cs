using System;
using System.IO;
using System.Collections.Generic;
using DataReader.Sensors;
using CSRedis;
using System.Net.Sockets;
using System.Linq;
using System.Diagnostics;

namespace DataReader
{
    class Program
    {
        public static int time;
        static void Main(string[] args)
        {
            // init sensors
            List<ISensor> sensors = new List<ISensor>
            {
                new VirtualSensors()
            };

            // configure Redis
            var redis = new RedisClient("127.0.0.1");
            //StartServer(redis);


            time = 5000;
            while (true)
            {
                //SetTime();

                foreach (ISensor sensor in sensors)
                {
                    // get current sensor value
                    var data = sensor.ToJson();
                    Console.WriteLine(data);

                    // push to redis queue
                    //redis.LPush("sensors_data", data);

                    // wait 1 second
                    System.Threading.Thread.Sleep(5000);
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
            process.StartInfo.FileName = @"C:\Users\PC\Downloads\64bit\redis-server.exe";
            process.Start();

            while(ok==false)
            {
                try
                {
                    redis.Ping();
                    ok = true;
                }
                catch
                {
                }
            }
        }


        public static void SetTime()
        {
            string path = @"C:\Users\PC\Desktop\prvgraf\BUS\EXTERNALTIME.txt";
            string[] lines = File.ReadAllLines(path);

            if(lines.Length>0)
                time = Convert.ToInt32(lines[0]);
        }
    }
}