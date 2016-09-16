using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SonyAPILib;

namespace EventingExample
{
    class Program
    {
        private static APILibrary mySonyLib = new APILibrary();
        private static APILibrary.SonyDevice mySonyDevice = new APILibrary.SonyDevice();
        private static APILibrary.EventServer myEventServer = new APILibrary.EventServer();

        static void Main(string[] args)
        {
            mySonyLib.Log.Enable = true;
            mySonyDevice = mySonyLib.Locator.DeviceLoad(@"C:\myDevices\STR-DN840.xml");
            myEventServer.IpAddress = "192.168.0.2";
            myEventServer.Port = 8080;
            myEventServer.CallBackUrl = @"http://192.168.0.2:" + myEventServer.Port;
            myEventServer.PropertyChanged += EventServerOnChange;
            myEventServer.Start();

            while (true)
            {
                Console.WriteLine("Enter the command to send and hit Enter.");
                Console.WriteLine("Or type ‘End’ and hit Enter to Quit.");
                string cki = Console.ReadLine();
           
                if (cki == "End")
                {
                    ShutDown(myEventServer);
                }
                else if (cki == "Sub")
                {
                    myEventServer.SubscribeToEvents(mySonyDevice,mySonyDevice.RenderingControl.ServiceIdentifier,1800);
                }
                else if (cki == "Resub")
                {
                    myEventServer.ReSubscribeToEvents(mySonyDevice.Name,mySonyDevice.RenderingControl.ServiceIdentifier);
                }
                else if (cki == "Unsub")
                {
                    myEventServer.UnSubscribeToEvents(mySonyDevice.Name,mySonyDevice.RenderingControl.ServiceIdentifier);
                }
                else
                {
                    string results = mySonyDevice.Ircc.SendIRCC(mySonyDevice, mySonyDevice.GetCommandString(cki));
                }
            }
        }

        private static void ShutDown(APILibrary.EventServer x)
        {
            x.Stop();
            Environment.Exit(1);
        }

        static void EventServerOnChange(object sender, EventArgs args)
        {
            Console.WriteLine(myEventServer.Output);
        }
    }
}

