using System;
using System.IO;
using System.Text;
using CSRedis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;

namespace DataSender
{
    public class Program
    {
        public static bool wait = false;
        public static bool erserver = false;
        public static bool erbus = false;
        public static bool speed = false;

        //Set Url
        internal static string ip = Properties.Settings.Default.ServerIp;
        internal static string port = Properties.Settings.Default.ServerPort;
        internal static string api_path = Properties.Settings.Default.ApiPath;
        internal static string url = "http://" + ip + ":" + port + api_path;
        internal static string urlToken = "http://" + ip + ":" + port + "/token";
        internal static bool autentication = Properties.Settings.Default.Autentication;
        internal static string token = String.Empty;

        static void Main(string[] args)
        {
            while (true)
            {
                try
                {
                    //Configura Redis
                    var redis = new RedisClient("127.0.0.1");

                    //Sending Loop
                    while (true)
                    {
                        //Read, execute and write external data
                        string command = External.ReadValues();
                        //string command = "serverip 192.168.1.26";
                        External.Execute(command);
                        External.WriteValues();

                        //Read from Redis queue
                        List<string> redislist = redis.LRange("sensors_data", 0, redis.LLen("sensors_data") + 1).ToList();

                        if (wait == true)
                        {
                            Thread.Sleep(5000);
                        }
                        else
                        {
                            string json = redis.BRPop(30, "sensors_data");

                            // send value to remote API
                            try
                            {
                                using (WebClient webClient = new WebClient())
                                {
                                    //Ottiene token autenticazione
                                    if (autentication == true)
                                    {
                                        using (WebClient wc = new WebClient())
                                        {
                                            string credentials = "{\"id\": \"" + Properties.Settings.Default.TokenId + "\", \"password\": \"" + Properties.Settings.Default.TokenPass + "\"}";
                                            wc.Headers[HttpRequestHeader.ContentType] = "application/json";
                                            token = webClient.UploadString(urlToken, credentials);
                                        }

                                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                                        webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                                        webClient.UseDefaultCredentials = true;
                                        webClient.Credentials = new NetworkCredential(Properties.Settings.Default.NetUser, Properties.Settings.Default.NetPass);
                                    }

                                    string response = webClient.UploadString(url, json);
                                }

                                Console.Write("si ");
                                Console.WriteLine();
                            }
                            catch (Exception err)
                            {
                                Console.Write("no ");
                                Console.WriteLine(" : " + err.Message);
                                redis.RPush("sensors_data", json);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}