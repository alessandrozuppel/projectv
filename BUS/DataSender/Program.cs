using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;

using CSRedis;

namespace DataSender
{
    public class Program
    {
        public static bool wait = false;
        public static bool erserver = false;
        public static bool erbus = false;
        public static bool speed = false;

        public static int bcryptrounds=10;

        //Set Url
        internal static string lbip = Properties.Settings.Default.ServerIp;
        internal static string lbport = Properties.Settings.Default.ServerPort;
        internal static string ip;
        internal static string port;
        internal static string api_path = Properties.Settings.Default.ApiPath;
        internal static bool autentication = Properties.Settings.Default.Autentication;
        internal static string token = String.Empty;
        
        internal static bool gotserver=false;
        internal static bool gottoken=false;

        static void Main(string[] args)
        {
            while (true)
            {
                string url = "http://" + ip + ":" + port + api_path;
                string urlToken = "http://" + ip + ":" + port + "/token";
                string urllb = "http://" + lbip + ":" + lbport + "/getip/";

                try
                {
                    //Configura Redis
                    var redis = new RedisClient("127.0.0.1");

                    //ottiene ip da balancer
                    using (WebClient wb = new WebClient())
                    {
                        string response = wb.DownloadString(urlToken);

                        if(!(response=="400" | response=="500"))
                        {
                            string[] temp = response.Split(':');
                            ip = temp[0];
                            port = temp[1];
                            gotserver = true;
                        }
                    }

                    //Sending Loop
                    while (gotserver==true)
                    {
                        //Read, execute and write external data
                        string command = External.ReadValues();
                        //string command = "serverip 192.168.1.26";
                        External.Execute(command);
                        External.WriteValues();


                        //Ottiene token autenticazione
                        if (autentication == true)
                        {
                            using (WebClient wb = new WebClient())
                            {
                                string credentials =
                                    "{\"Username\": \"" + 
                                    Properties.Settings.Default.TokenId + 
                                    "\", \"Password\": \"" +
                                    BCrypt.Net.BCrypt.HashPassword(Properties.Settings.Default.TokenPass + "\"}", 10)
                                    ;

                                wb.Headers[HttpRequestHeader.ContentType] = "application/json";
                                string response = wb.UploadString(urlToken, credentials);

                                if (!(response == "400" | response == "500"))
                                {
                                    token = response;
                                    autentication = false;
                                    gottoken = true;
                                }
                            }
                        }
                            

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
                                if(gottoken==true)
                                {
                                    using (WebClient webClient = new WebClient())
                                    {
                                        webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                                        webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                                        webClient.UseDefaultCredentials = true;
                                        webClient.Credentials = new NetworkCredential(Properties.Settings.Default.NetUser, Properties.Settings.Default.NetPass);
                                        string response = webClient.UploadString(url, json);
                                    }
                                }
                                else
                                {
                                    autentication = true;
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