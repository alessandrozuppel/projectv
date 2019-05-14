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
                string json = redis.BRPop(30, "sensors_data");
                //Console.WriteLine(redis.BLPop(30, "sensors_data"));

                // send value to remote API
                try
                {
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://192.168.101.23:3000/api/busdati");
                    //var httpWebRequest = (HttpWebRequest)WebRequest.Create("http://127.0.0.2:5000/api/busdati/");
                    httpWebRequest.ContentLength = json.Length;
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.Proxy = null;
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        try
                        {
                            streamWriter.Write(json);
                            streamWriter.Flush();
                            streamWriter.Close();
                        }
                        catch(Exception e)
                        {
                            Console.WriteLine(e.Message);
                        }
                    }

                    //Read Response
                    //HttpWebResponse response = (HttpWebResponse)httpWebRequest.GetResponse();
                    //WebHeaderCollection header = response.Headers;

                    //using (var reader = new System.IO.StreamReader(response.GetResponseStream()))
                    //{
                    //    string responseText = reader.ReadToEnd();
                    //    Console.WriteLine(responseText);
                    //}

                    Console.Write("si ");
                    for(int i=97; i<113; i++)
                        {
                            Console.Write(json[i]);
                        }
                    Console.WriteLine();
                }
                catch (Exception err)
                {
                    Console.Write("no ");
                    for(int i=97; i<113; i++)
                        {
                            Console.Write(json[i]);
                        }
                    Console.WriteLine();
                }
            }
        }
    }
}
