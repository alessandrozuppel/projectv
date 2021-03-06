﻿using System;
using System.IO;
using System.Text;
using CSRedis;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Net;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataSender
{
    public class Program
    {
        public static bool wait = false;

        //For sensors
        public static JToken Track=null;

        //Set Url
        internal static string ip = Properties.Settings.Default.ServerIp;
        internal static string port = Properties.Settings.Default.ServerPort;
        internal static string api_path = Properties.Settings.Default.ApiPath;
        internal static string url = "http://" + ip + ":" + port + api_path;
        internal static string urlToken = "http://" + ip + ":" + port + "/token";
        internal static bool autentication = true;
        internal static string token = String.Empty;
        internal static bool gottrack=false;

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
                        External.Execute(command);
                        External.WriteValues();




                        //Ottiene token autenticazione
                        if (autentication == true)
                        {
                            string credentials = "{\"Id\": \"" + Properties.Settings.Default.IdMezzo + "\", \"Password\": \"" + Properties.Settings.Default.PassMezzo + "\"}";
                            token = Send("http://" + ip + ":" + port + "/token", credentials);
                            autentication = false;
                        }


                        //Ottiene percorso giornaliero del mezzo/////////////
                        if(gottrack==false && token.Length>0)
                        {
                            string url = "http://" + ip + ":" + port + "/api/trackBus";
                            Track= JConstructor.Parse(Send(url, Properties.Settings.Default.IdMezzo));
                            gottrack = true;
                        }
                        ///////////////////////////////

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
                                string response = Send(url, json);

                                Console.Write(response);
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


        internal static string Send(string url, string data)
        {
            using(WebClient webClient = new WebClient())
            {
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";

                if (token.Length>0)
                {
                    webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                    webClient.UseDefaultCredentials = true;
                }
                return webClient.UploadString(url, data);
            }
        }
    }
}