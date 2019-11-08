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
        public static int time = 5;
        static void Main(string[] args)
        {
            // init sensors
            List<ISensor> sensors = new List<ISensor>
            {
                new VirtualSensors()
            };

            while (true)
            {
                try
                {
                    //Start redis server
                    StartRedisServer();

                    // configure Redis
                    var redis = new RedisClient("127.0.0.1");

                    while (true)
                    {
                        //SetTime();

                        foreach (ISensor sensor in sensors)
                        {
                            // get current sensor value
                            var data = sensor.ToJson();
                            Console.WriteLine(data);

                            // push to redis queue
                            redis.LPush("sensors_data", data);

                            // wait tot second
                            System.Threading.Thread.Sleep(time*1000);
                        }

                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }

        public static void StartRedisServer()
        {
            bool ok = false;
            Process process = new Process();
            process.StartInfo.UseShellExecute = true;
            process.StartInfo.CreateNoWindow = false;
            //process.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            process.StartInfo.FileName = Properties.Settings.Default.RedisServerPath;
            process.Start();

            while (ok == false)
            {
                using (var redis = new RedisClient("127.0.0.1"))
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
        }


        public static void SetTime()
        {
            string[] lines = File.ReadAllLines(Properties.Settings.Default.ExternalTimePath);

            if (lines.Length > 0)
                time = Convert.ToInt32(lines[0]);
        }


        public static string[] GetBusInfo()
        {
            string connectionstring = new SQL.Connection.ConnectionStringBuilder()
                                                                                .IP(Properties.Settings.Default.SQLIP)
                                                                                .Port(Properties.Settings.Default.SQLPort)
                                                                                .NetLib(Properties.Settings.Default.SQLNetworkLibrary)
                                                                                .InitCat(Properties.Settings.Default.SQLInitialCatalog)
                                                                                .User(Properties.Settings.Default.SQLUser)
                                                                               .Pass(Properties.Settings.Default.SQLPassword)
                                                                               .Build();

            SQL.Connection connection = new SQL.Connection();
            return connection.Connect(connectionstring);
        }
    }
}