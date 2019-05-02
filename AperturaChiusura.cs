using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace projectv {
    class AperturaChiusura {
        private static Random rnd = new Random ();

        static double initialac = 0;
        static void Main (string[] args) {
            int i = 1;
            while (i == 1) {
                OnTimedEvent ();
                System.Threading.Thread.Sleep (2000);
            }
        }

        private static void OnTimedEvent () {

            double AC = initialac;

            //Apertura-chiusura
            double ran = rnd.Next (0, 2);
            if (AC != ran) {
                AC = (AC + 1) % 2;
            }
            
            //Scrittura
            try {
                var httpWebRequest = (HttpWebRequest) WebRequest.Create ("http://localhost:3000/api");
                httpWebRequest.ContentType = "application/json";
                httpWebRequest.Method = "POST";
                using (var streamWriter = new StreamWriter (httpWebRequest.GetRequestStream ())) {
                    string json = "{\"AperturaChiusura\":\"" + AC.ToString() + "\"}";
                    streamWriter.Write (json);
                    streamWriter.Flush ();
                    streamWriter.Close ();
                    WebResponse response = httpWebRequest.GetResponse ();
                    Console.WriteLine (response.GetResponseStream ());
                }
            } catch(Exception err) {
                Console.WriteLine(err.ToString());
            }

            initialnum = num;
            initialac = AC;
        }
    }
}