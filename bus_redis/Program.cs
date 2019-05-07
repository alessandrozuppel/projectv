using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Timers;

namespace Sensors {
    class Sensori {
        private static Random rnd = new Random ();
        static double initialnum = 0;
        static double initialac = 0;
        static void Main (string[] args) {
            int i = 1;
            while (i == 1) {
                OnTimedEvent ();
                System.Threading.Thread.Sleep (2000);
            }
        }

        private static void OnTimedEvent () {
            
            double num1 = initialnum;
            double AC = initialac;

            //Apertura Chiusura
            double ran = rnd.Next (0, 2);
            AC=ran;
            Console.WriteLine (AC);
            if(AC==1)
            {
            //Conta Persone
            double ran1 = rnd.Next (-6, 6);
            double num = num1 + ran1;
            if (num < 0) {
                num = 0;
            }
            Console.WriteLine (num);
            initialnum = num;
            AC=0;
            }

            //Posizione
            ran = Convert.ToDouble(rnd.Next (-99, 100)) / 10000000;
            double lan = 45.9536714 + ran;

            ran = Convert.ToDouble(rnd.Next (-99, 100)) / 10000000;
            double len = 12.6855359 + ran;

            Console.WriteLine (lan);
            Console.WriteLine (len);

            initialac = AC;

        }
    }
}