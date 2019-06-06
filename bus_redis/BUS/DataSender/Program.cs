using System;

using System.Net;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using CSRedis;

namespace DataSender
{
    class Program
    {
        //Variabile per decidere se re-inserire la stringa json in coda 
        static int i = 0;
        
        //Set Url
        static string port = Properties.Settings.Default.ServerPort;
        static string ip = Properties.Settings.Default.ServerIP;
        static string api_path = Properties.Settings.Default.ApiPath;
        static string url = "http://" + ip + ":" + port + api_path;
        static void Main(string[] args)
        {
            // configure Redis
            var redis = new RedisClient("127.0.0.1");

            while (true)
            {
                i = 0;
                // read from Redis queue
                string json = redis.BRPop(30, "sensors_data");

                try
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                            string response = webClient.UploadString(url, json);

                            if (response != "201")
                                i = 1;

                            Console.WriteLine(response);
                        }
                    }
                    catch (Exception err)
                    {
                        Console.Write(err.Message);
                        i = 1;
                    }

                    Console.WriteLine();
                    
                    if(i==1)
                    redis.LPush("sensors_data", json);
            }
        }
    }
}
