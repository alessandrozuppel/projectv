using System;
using System.Collections.Generic;
using DataReader.Sensors;
using CSRedis;
using System.Net.Sockets;
using System.Threading;
using System.Data.SqlClient;

namespace DataReader
{
    class Program
    {
        static void Main(string[] args)
        {
            Thread bus1 = new Thread(bus);
            Thread bus2 = new Thread(bus);
            bus1.Name = "bus1";
            bus2.Name = "bus2";
            bus1.Start();
            bus2.Start();

        }
        static void bus()
        {
            Random randomBus = new Random();
            dbBus db = new dbBus();
            List<int> elencoID = db.RecuperaId();
            int id = randomBus.Next(1,elencoID.Count);
            Console.WriteLine(id);
            bus b = db.RecuperaDati(elencoID[id]);
            double[] pos = new double[2];
            pos[0] = Convert.ToDouble(b.lat);
            pos[1] = Convert.ToDouble(b.lon);
            string stringID = Convert.ToString(b.id);

            // init sensors
            List<ISensor> sensors = new List<ISensor>
            {
                new VirtualSensors(stringID, pos, b.posti)
            };




            // configure Redis
            var redis = new RedisClient("127.0.0.1");

            while (true)
            {
                Random ran = new Random();
                int c = ran.Next(1, 5);
                int cicli = 0;
                bool fermo = false;
                string oraapertura = (System.DateTime.UtcNow.Ticks - System.DateTime.Parse("01/01/1970 00:00:00").Ticks).ToString();

                //Viaggio
                switch (c)
                {
                    case (1): { cicli = 2; break; }
                    case (2): { cicli = 4; break; }
                    case (3): { cicli = 5; break; }
                    case (4): { cicli = 6; break; }
                    case (5): { cicli = 7; break; }
                }

                for (int x = 0; x < cicli; x++)
                {

                    foreach (ISensor sensor in sensors)
                    {
                    
                        var data = sensor.ToJson(fermo, oraapertura);
                        Console.WriteLine(data);


                        // push to redis queue
                        redis.LPush("sensors_data", data);
                        Console.WriteLine(Thread.CurrentThread.Name);
                        // wait 10 second
                        System.Threading.Thread.Sleep(10000);
                    }
                }

                //Fermata
                c = ran.Next(1, 5);
                cicli = 0;
                fermo = false;
                oraapertura = (System.DateTime.UtcNow.Ticks - System.DateTime.Parse("01/01/1970 00:00:00").Ticks).ToString();

                switch (c)
                {
                    case (1): { cicli = 2; break; }
                    case (2): { cicli = 3; break; }
                    case (3): { cicli = 4; break; }
                    case (4): { cicli = 5; break; }
                    case (5): { cicli = 6; break; }
                }

                for (int x = 0; x < cicli; x++)
                {

                    foreach (ISensor sensor in sensors)
                    {

                        var data = sensor.ToJson(fermo, oraapertura);
                        Console.WriteLine(data);


                        // push to redis queue
                        redis.LPush("sensors_data", data);
                        Console.WriteLine(Thread.CurrentThread.Name);
                        // wait 10 second
                        System.Threading.Thread.Sleep(10000);
                    }
                }

            }

        }
    }
}
