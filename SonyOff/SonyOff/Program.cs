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
            SonyAPI_Lib mySonyLib = new SonyAPI_Lib();  // Create an Instance of the Library
            mySonyLib.LOG.Enable = false;  // Set to NO logging
            SonyAPI_Lib.SonyDevice mySonyDev = new SonyAPI_Lib.SonyDevice();  // Create a new instance of a Device Object
            
            // Now there are 2 ways you can complete the next part.
            // You can build the Device from the Device's Description Document
            
            // Like This:
            //mySonyDev.buildFromDocument(new Uri("http://192.168.0.100:52323/dmr.xml"));

            //Or, if you have created a Device File, then you can load and use it.
            //This is the prefered method, as it would be the fastest.
            
            //Like This:
            mySonyDev = mySonyLib.Locator.DeviceLoad(@"c:\MyDevices\Bravia55.xml");

            string cmd = mySonyDev.getIRCCcommandString("VolumeUp");  //Get the Command String value
            string results = mySonyLib.ircc1.XSendIRCC(mySonyDev, cmd);  //Send Command Value to device using the IRCC:1 Service
        }
    }
}
