using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Events;

namespace WraithJet
{
    public class WraithEngineController
    {
        public void engineKeyPressed()
        {
            if (VehicleFramework.Engines.WraithEngine.engineHigh == false)
            {
                VehicleFramework.Engines.WraithEngine.engineHigh = true;
            }
            else
            {
                VehicleFramework.Engines.WraithEngine.engineHigh = false;
            }
        }
    }
}
