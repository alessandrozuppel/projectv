using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;


namespace DataReader.Sensors
{
    interface IGenericSensors
    {
        string Apertura();
        string ContaPersone();
        double[] Posizione();
        string Id();
    }
}
