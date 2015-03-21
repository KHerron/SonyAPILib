using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonyAPILib;

namespace SonyOff
{
    class Program
    {
        static void Main(string[] args)
        {
            SonyAPI_Lib mySonyLib = new SonyAPI_Lib();
            mySonyLib.LOG.enableLogging = false;
            SonyAPILib.SonyAPI_Lib.SonyDevice mySonyDev = new SonyAPI_Lib.SonyDevice();
            mySonyDev.Name = "STR-DN840 9C7529";
            mySonyDev.Device_IP_Address = "192.168.0.16";
            mySonyDev.actionList_URL = @"http://192.168.0.16:50001/cers/ActionList.xml";  // If not Gen 3, this must point to your device's ActionList.xml file. If Gen 3, set to empty string. ""
            mySonyDev.control_URL = @"http://192.168.0.16:8080/upnp/control/IRCC"; // This must match the controlURL for the IRCC:1 service in all generations device description file.
            mySonyDev.Generation = 1; // If manually setting the 2 above properties, you must set this property.
            mySonyDev.initialize();
            string results = mySonyDev.send_ircc(mySonyDev.getIRCCcommandString("VolumeUp"));
        }
    }
}
