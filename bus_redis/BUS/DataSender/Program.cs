using System;
using System.Net;
using System.IO;
using CSRedis;

namespace DataSender
{
    class Program
    {
        static void Main(string[] args)
        {
            // configure Redis
            var redis = new RedisClient("127.0.0.1");

            while (true)
            {
                // read from Redis queue
                string json = redis.BLPop(30, "sensors_data");
                Console.WriteLine(redis.BLPop(30, "sensors_data"));

                // send value to remote API
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.101.81:3000/api/busdati");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(json);
                        streamWriter.Flush();
                        streamWriter.Close();
                    }
                }
                catch (Exception err)
                {
                    Console.WriteLine(err);
                }
            }
        }
    }
}
