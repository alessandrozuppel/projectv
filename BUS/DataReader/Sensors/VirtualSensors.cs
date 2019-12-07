using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace DataReader.Sensors
{
    class VirtualSensors : IGenericSensors, ISensor
    {
        private static Random rnd = new Random();
        static double initialnum = 0;
        static double initialac = 0;

        //Doors opening
        public string Apertura()
        {
            double ran = rnd.Next(0, 2);
            if(initialac==ran && ran==0)
            {
                return null;

            }
            else
            {
                initialac = ran;
                return ran.ToString();
            }
        }

        //People Counter
        public string ContaPersone()
        {
            if (initialac == 1)
            {
                double ran1 = rnd.Next(-6, 6);
                double num = initialnum + ran1;
                if (num < 0)
                {
                    num = 0;
                }
                initialnum = num;
                return initialnum.ToString();
            }
            else
            {
                return null;
            }
        }


        //Position
        public string[] Posizione()
        {
            JToken trackToken = Utils.ListaTokens[Utils.LTcounter];
            string[] list = new string[2];

            list[0] = trackToken.Value<string>("lat");
            list[1] = trackToken.Value<string>("lon");

            Utils.LTcounter++;

            return list;
        }

        
        internal static List<JToken> ReadTrackFile()
        {
            List<JToken> temp = new List<JToken>();
            /*
            using (StreamReader file = File.OpenText("track.json"))
            {
                using (JsonTextReader reader = new JsonTextReader(file))
                {
                    JObject o2 = (JObject)JToken.ReadFrom(reader);
                    var listObj = o2.GetValue("trackData");

                    temp = listObj.Children<JToken>().ToList<JToken>();
                }
            }
            */

            JObject o2 = (JObject)Program.Track;
            var listObj = o2.GetValue("trackData");
            temp = listObj.Children<JToken>().ToList<JToken>();

            return temp;
        }
        

        public string ToJson(string id)
        {
            string[] list = new string[4];

            list[0] = Apertura();
            list[1] = ContaPersone();
            string oraapertura = (System.DateTime.UtcNow.Ticks-System.DateTime.Parse("01/01/1970 00:00:00").Ticks).ToString();

            string[] pos = Posizione();
            list[2] = pos[0];
            list[3] = pos[1];
            string oraposizione = (System.DateTime.UtcNow.Ticks-System.DateTime.Parse("01/01/1970 00:00:00").Ticks).ToString();

            return "{" +
                "\"Apertura\":\"" + list[0] + "\"," +
                "\"Conta_Persone\":\"" + list[1] + "\"," +
                "\"Latitudine\":\"" + list[2] + "\"," +
                "\"Longitudine\":\"" + list[3] + "\"," +
                "\"IdVeicolo\":\"" + id + "\"," +
                "\"DataOraBus\":\"" + oraposizione + "\"," +
                "\"DataOraPorte\":\"" + oraapertura +"\""+
                "}";
        }
    }
}
