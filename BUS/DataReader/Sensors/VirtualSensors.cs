using System;

namespace DataReader.Sensors
{
    class VirtualSensors : IGenericSensors, ISensor
    {
        private static Random rnd = new Random();
        static double initialnum = 0;
        static double initialac = 0;
        double[] lastPos = new double[2];
        int maxPeople = 0;
        string id = "";

        public VirtualSensors(string _ID, double[] pos, int max)
        {
            id = _ID;
            //lastPos = pos;
            maxPeople = max;
        }

        public string Apertura()
        {
            double ran = rnd.Next(0, 4);
            if(initialac==ran && ran == 0 && ran > 1)
            {
                return null;

            }
            else
            {
                initialac = ran;
                return ran.ToString();
            }
        }

        public string ContaPersone()
        {
            if (initialac == 1)
            {
                //Conta Persone
                double ran1 = rnd.Next(-6, 6);
                //if (ran1 == 0)
                //{
                //    ran1 = 1;
                //}
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

        public double[] Posizione()
        {

            double lat = 45.963274;
            double lon = 12.660361;
            double[] list = new double[2];
            
            double ran = Convert.ToDouble(rnd.Next(-300, 300)) / 10000000;
            list[0] = lat + ran;

            ran = Convert.ToDouble(rnd.Next(-300, 300)) / 10000000;
            list[1] = lon + ran;        

            lastPos = list;

            return list;
        }

        public string Id()
        {
           return id;            
        }


        public string ToJson(bool fermo, string ora)
        {
            //Dati inviati nel caso il bus sia fermo
            if(fermo==true)
            { 
            string[] list = new string[5];
            list[0] = "1"; //Apertura è aperto
            string oraapertura = ora; //Ora passata da program del momento in cui il bus è arrivato alla fermata
            list[1] = ContaPersone();   //Il nuero di persone varia 
            list[2] = lastPos[0].ToString(); //La posizione è l'ultima registrata
            list[3] = lastPos[1].ToString(); //La posizione è l'ultima registrata
            string oraposizione = (System.DateTime.UtcNow.Ticks-System.DateTime.Parse("01/01/1970 00:00:00").Ticks).ToString(); //La data di invio dati cambia anche da bus fermo
            list[4] = Id(); //L'id è quello fornito
            return "{" +
                        "\"Apertura\":\"" + list[0] + "\"," +
                        "\"Conta_Persone\":\"" + list[1] + "\"," +
                        "\"Latitudine\":\"" + list[2] + "\"," +
                        "\"Longitudine\":\"" + list[3] + "\"," +
                        "\"IdVeicolo\":\"" + list[4] + "\"," +
                        "\"DataOraBus\":\"" + oraposizione + "\"," +
                        "\"DataOraPorte\":\"" + oraapertura +"\""+
                   "}";
            }

            //Dati inviati nel caso il bus sia in movimento
             else
            {
                string[] list = new string[5];
                list[0] = "1"; //Apertura è chiuso
                string oraapertura = ora; //Ora passata da program del momento in cui il bus è partito dalla fermata
                list[1] = initialnum.ToString(); //Il numero di persone è fisso
                list[2] = Posizione()[0].ToString(); //La posizione varia
                list[3] = Posizione()[1].ToString(); //La posizione varia
                string oraposizione = (System.DateTime.UtcNow.Ticks - System.DateTime.Parse("01/01/1970 00:00:00").Ticks).ToString(); //La data di invio dati cambia anche da bus fermo
                list[4] = Id(); //L'id è quello fornito
                return "{" +
                            "\"Apertura\":\"" + list[0] + "\"," +
                            "\"Conta_Persone\":\"" + list[1] + "\"," +
                            "\"Latitudine\":\"" + list[2] + "\"," +
                            "\"Longitudine\":\"" + list[3] + "\"," +
                            "\"IdVeicolo\":\"" + list[4] + "\"," +
                            "\"DataOraBus\":\"" + oraposizione + "\"," +
                            "\"DataOraPorte\":\"" + oraapertura + "\"" +
                       "}";
            }
        }
    }
}
