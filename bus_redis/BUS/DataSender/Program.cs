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
        static bool i = true;
        
        //Set Url
        static string port = Properties.Settings.Default.LocalPort;
        static string ip = Properties.Settings.Default.LocalHost;
        static string api_path = Properties.Settings.Default.ApiPath;
        static string urlToken = "http://" + ip + ":" + port + "/token";
        static string url = "http://" + ip + ":" + port + api_path;
        static void Main(string[] args)
        {
            //Esegue autenticazione
            string token = "";
            using (WebClient webClient = new WebClient())
            {
                string credentials = "{\"id\": \"pippo\", \"password\": \"passwordsicura\"}";
                webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                //webClient.Credentials = new NetworkCredential("pippo","passwordsicura");
                token = webClient.UploadString(urlToken, credentials);
            }


            // configure Redis
            var redis = new RedisClient("127.0.0.1");

            while (true)
            {
                i = true;
                // read from Redis queue
                string json = redis.BRPop(30, "sensors_data");

                try
                    {
                        using (WebClient webClient = new WebClient())
                        {
                            webClient.Headers[HttpRequestHeader.ContentType] = "application/json";
                            webClient.Headers[HttpRequestHeader.Authorization] = "Bearer " + token;
                            webClient.UseDefaultCredentials = true;
                            webClient.Credentials = new NetworkCredential("pippo", "poppo");
                            string response = webClient.UploadString(url, json);

                            if (response != "201")
                                i = false;

                            Console.WriteLine(response);
                        }
                    }
                    catch (Exception err)
                    {
                        Console.Write(err.Message);
                        i = false;
                    }

                    Console.WriteLine();
                    
                    if(i=false)
                    redis.LPush("sensors_data", json);
            }
        }
    }
}
