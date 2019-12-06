using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace DataReader.Sensors
{
    internal static class Utils
    {
        //PER GPS
        internal static List<JToken> ListaTokens = VirtualSensors.ReadTrackFile();
        internal static int LTcounter = 0;
    }
}
