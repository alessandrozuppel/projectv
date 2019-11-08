using System;
using System.Threading;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TwinCAT.Ads;

namespace DataSender
{
    public static class Twincat
    {
        internal static string amsnetid = Properties.Settings.Default.AmsNetID;
        internal static string amsnetport = Properties.Settings.Default.AmsNetPort;
        public static void TellTwincat(string data)
        {
            try
            {
                List<string> Datas = data.Split(',', ':').ToList();
                for (int i = 0; i < Datas.Count; i++)
                {
                    string stringa = "";
                    object valore = null;
                    bool write = false;

                    string riga = Datas[i];

                    if (riga.Contains("Apertura"))
                    {
                        stringa = "GVL.PorteChiuse";

                        if (Datas[i + 1].Contains("0"))
                            valore = false;

                        if (Datas[i + 1].Contains("1"))
                            valore = true;

                        if (!(valore is null))
                        {
                            write = true;
                        }

                    }

                    if (riga.Contains("Latitudine"))
                    {
                        stringa = "GVL.GPS";
                        valore = true;


                        if (!(valore is null))
                            write = true;
                    }

                    if (riga.Contains("Conta_Persone"))
                    {
                        stringa = "GVL.ContaPersone";
                        valore = true;

                        if (!(valore is null))
                            write = true;
                    }

                    if (write)
                    {
                        WriteTwincat(stringa, valore);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("TellTwincatc: " + ex.Message);
            }
        }


        public static void WriteBusQueue(List<string> list)
        {
            try
            {
                WriteTwincat("GVL.EraseTable", true);

                TcAdsClient client = new TcAdsClient();
                client.Connect(amsnetid, Convert.ToInt32(amsnetport));
                int handle = client.CreateVariableHandle("GVL.DataFromBus");

                foreach (string el in list)
                {
                    AdsStream stream = new AdsStream(500);
                    AdsBinaryWriter writer = new AdsBinaryWriter(stream);
                    writer.WritePlcString(el, 500, Encoding.Unicode);
                    client.Write(handle, stream);
                    stream.Dispose();
                    writer.Dispose();
                    Thread.Sleep(10);
                }

                client.DeleteVariableHandle(handle);
                client.Dispose();
            }
            catch (Exception ex)
            {
                Console.WriteLine("BusWrite: " + ex.Message);
            }
        }

        public static void WriteTwincat(string comando, object valore)
        {
            if (!(valore is null))
            {
                using (TcAdsClient client = new TcAdsClient())
                {
                    try
                    {
                        client.Connect(amsnetid, Convert.ToInt32(amsnetport));
                        int handle = client.CreateVariableHandle(comando);

                        if (valore.GetType().FullName.Contains("String"))
                        {
                            string el = valore.ToString();
                            AdsStream stream = new AdsStream(500);
                            AdsBinaryWriter writer = new AdsBinaryWriter(stream);
                            writer.WritePlcString(el, 500, Encoding.Unicode);
                            client.Write(handle, stream);

                            stream.Dispose();
                            writer.Dispose();
                        }
                        else
                        {
                            client.WriteAny(handle, valore);
                        }

                        //client.Dispose();
                        client.DeleteVariableHandle(handle);
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine(ex.Message);
                    }
                }
            }
        }
    }
}