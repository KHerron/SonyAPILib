using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonyAPILib;
using Newtonsoft;


namespace DLNALib
{
    class Program
    {
        static void Main(string[] args)
        {
            // 1st we create a new instance of the SonyAPILib
            APILibrary mySonyLib = new APILibrary();
            

            #region No parameters
            if (args.Length < 1)
            {
                Console.WriteLine("ERROR: Missing parameters....");
                showhelp();
            }
            #endregion

            #region Logging
            if (args[args.Length - 1] == "/log")
            {
                mySonyLib.Log.Enable = true;
                mySonyLib.Log.Level = "All";
            }
            #endregion

            #region /h and /? Help and Individual Help
            if (args[0] == "/h" | args[0] == "/?")
            {
                showhelp();
            }
            if (args.Length > 1)
            {
                if (args[1] == "/?")
                {
                    showcommandhelp(args[0]);
                }
            }
            #endregion

            #region WOL - Wake-On-Lan
            if (args[0] == "/WOL")
            {
                APILibrary.SonyDevice device = mySonyLib.Locator.DeviceLoad(args[1]);
                device.WOL();
            }
            #endregion

            #region /b - Build From Document
            else if (args[0] == "/b")
            {
                try
                {
                    string du = args[1];
                    string pa = args[2] + @"\";
                    APILibrary.SonyDevice nDev = new APILibrary.SonyDevice();
                    nDev.BuildFromDocument(new Uri(du));
                    pa = pa + nDev.Name + ".xml";
                    Console.WriteLine("Saving Device: " + nDev.Name + ".xml to " + pa);
                    mySonyLib.Locator.DeviceSave(pa, nDev);
                }
                catch
                {
                    Console.WriteLine("ERROR: Incorrect parameters....");
                    showhelp();
                }
            }
            #endregion

            #region /l and /ls - Locate and Locate Save
            else if (args[0] == "/l" | args[0] == "/ls")
            {
                try
                {
                    Console.WriteLine("Locating Devices. Please wait.........");
                    List<string> fdev = mySonyLib.Locator.LocateDevices();
                    Console.WriteLine("Devices Found: " + fdev.Count);
                    if (fdev.Count > 0)
                    {
                        int i = 1;
                        foreach (string d in fdev)
                        {
                            Console.WriteLine(i + ") " + d);
                            i = i + 1;
                        }
                    }
                    else
                    {
                        Console.WriteLine("NO Devices Found");
                        System.Environment.Exit(0);
                    }
                    if (args[0] == "/ls")
                    {
                        foreach (string dv in fdev)
                        {
                            string pa = args[2] + @"\";
                            APILibrary.SonyDevice nDev = new APILibrary.SonyDevice();
                            nDev.BuildFromDocument(new Uri(dv));
                            pa = pa + nDev.Name + ".xml";
                            Console.WriteLine("Saving Device: " + nDev.Name + ".xml to " + pa);
                            mySonyLib.Locator.DeviceSave(pa, nDev);
                        }
                    }

                }
                catch
                {
                    Console.WriteLine("ERROR: Incorrect parameters....");
                    showhelp();
                }
            }
            #endregion

            #region /r - Register
            else if (args[0] == "/r")
            {
                if (args.Length > 1)
                {
                    string devFile = "";
                    devFile = args[1];
                    APILibrary.SonyDevice device = mySonyLib.Locator.DeviceLoad(devFile);
                    bool mySonyReg = false;
                    if (device.Registered == false)
                    {
                        Console.WriteLine(device.Name + ": Performing Registration....");
                        Console.WriteLine("Before continuing, you may need to set the device to Registration Mode,");
                        Console.WriteLine("Confirm Registration or enter the Registration PIN code.");
                        Console.WriteLine("Go to the device and perfrom any step, or be ready to before hitting enter below!");
                        Console.WriteLine("=====================================");
                        Console.WriteLine("Hit enter to Continue....");
                        string c = Console.ReadLine();
                        mySonyReg = device.Register();
                        if (device.Registered == false)
                        {
                            if (device.Actionlist.RegisterMode == 3)
                            {
                                string ckii;
                                Console.WriteLine("Enter PIN Code.");
                                ckii = Console.ReadLine();
                                Console.WriteLine("Sending Authitication PIN Code.");
                                mySonyReg = device.SendAuth(ckii);
                            }
                        }
                    }
                    else
                    {
                        mySonyReg = true;
                    }
                    mySonyLib.Locator.DeviceSave(devFile, device);
                }
                else
                {
                    Console.WriteLine("ERROR: missing parameters....");
                    showhelp();
                }
                
            }
            #endregion

            #region /a - Action
            else if (args[0] == "/a")
            {
                if (args.Length > 4)
                {
                    string devFile = "";
                    string devService = "";
                    string devAction = "";
                    string devP = "";
                    string devP2 = "";
                    devFile = args[1];
                    devService = args[2];
                    devAction = args[3];
                    devP = args[4];
                    APILibrary.SonyDevice device = mySonyLib.Locator.DeviceLoad(devFile);
                    if (devService == "IRCC")
                    {
                        if (devAction == "XSendIRCC")
                        {
                            device.Ircc.SendIRCC(device, devP);
                        }
                    }
                    else if (devService == "AVTransport")
                    {
                        if (devAction == "SetAVTransportURI")
                        {
                            if (args.Length > 5)
                            {
                                if (args[5] != "/log")
                                {
                                    devP2 = args[5];
                                }
                                else
                                {
                                    devP2 = null;
                                }
                            }
                            else
                            {
                                devP = null;
                            }
                            device.AVTransport.SetAVTransportURI(device, devP,devP2);
                        }
                        if (devAction == "SetNextAVTransportURI")
                        {
                            if (args.Length > 5)
                            {
                                if (args[5] != "/log")
                                {
                                    devP2 = args[5];
                                }
                                else
                                {
                                    devP2 = null;
                                }
                            }
                            else
                            {
                                devP2 = null;
                            }
                            device.AVTransport.SetNextAVTransportURI(device, devP, devP2);
                        }
                        if (devAction == "Play")
                        {
                            device.AVTransport.Play(device, Convert.ToInt32(devP));
                        }
                        if (devAction == "Stop")
                        {
                            device.AVTransport.Stop(device);
                        }
                        if (devAction == "Pause")
                        {
                            device.AVTransport.Pause(device);
                        }
                        if (devAction == "Next")
                        {
                            device.AVTransport.Next(device);
                        }
                        if (devAction == "Previous")
                        {
                            device.AVTransport.Previous(device);
                        }
                    }
                    else if (devService == "ConnectionManager")
                    {

                    }
                    else if (devService == "RenderingControl")
                    {
                        if (devAction == "SetMute")
                        {
                            Boolean p1 = false;
                            if (devP == "true" | devP == "True")
                            {
                                p1 = true;
                            }
                            device.RenderingControl.SetMute(device, p1);
                        }
                        if (devAction == "SetVolume")
                        {
                            int p1 = 0;
                            p1 = Convert.ToInt32(devP);
                            device.RenderingControl.SetVolume(device, p1);
                        }
                    }
                    else if (devService == "Party")
                    {

                    }
                }
                else
                {
                    Console.WriteLine("ERROR: missing parameters....");
                    showhelp();
                }
            }
            #endregion
        }

        #region Main Help
        static void showhelp()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("DLNALib.EXE Help");
            Console.WriteLine("=======================================================");
            Console.WriteLine("Must have a single space between all switches and parameters");
            Console.WriteLine("");
            Console.WriteLine("DLNALib.exe /a \"[Filename]\" [Service] [Action] \"[Parameter]\" /log");
            Console.WriteLine("DLNALib.exe /WOL \"[Filename]\" /log");
            Console.WriteLine("DLNALib.exe /l /log");
            Console.WriteLine("DLNALib.exe /ls \"[FolderPath]\" /log");
            Console.WriteLine("DLNALib.exe /b [DocumentURL] \"[Folderpath]\" /log");
            Console.WriteLine("DLNALib.exe /r \"[Filename]\" /log");
            Console.WriteLine("DLNALib.exe /h");
            Console.WriteLine("DLNALib.exe [Switch] /?");
            Console.WriteLine("");
            Console.WriteLine("=======================================================");
            Console.WriteLine("/a - Executes a Service Action on the device");
            Console.WriteLine("/WOL - Executes the Wake-On-Lan command");
            Console.WriteLine("/l - Runs the Device Locator (Discovery)");
            Console.WriteLine("/ls - Creates an XML file for all devices located.");
            Console.WriteLine("/b - Builds a device from the Description URL.");
            Console.WriteLine("/r - Executes a Device Registration");
            Console.WriteLine("/h - Shows this help file");
            Console.WriteLine("/log - Enables the API Logging function");
            Console.WriteLine("[Switch] /? - Shows help file for specified Switch");
            Console.WriteLine("[Filename] - Enter FULL path to device XML file. (C:\\mydevice.xml)");
            Console.WriteLine("[Service] - Enter the Service Identifier. (IRCC)");
            Console.WriteLine("[Action] - Enter the Action/Method to execute. (XSendIRCC)");
            Console.WriteLine("[FolderPath]- Sets the Path to save the device files.");
            Console.WriteLine("[DocumentURL]- Sets the URL to the devices Decription Document xml file on the device.");
            Console.WriteLine("[Parmeter] - Optional: Only required if needed (AAAAAQAAAAEAAAAvAw==)");
            Console.WriteLine("");
            Console.WriteLine("Enter [Switch] /? for more help.");
            Console.WriteLine("For Example:");
            Console.WriteLine("   DLNALib.exe /a /?");
            Console.WriteLine("   DLNALib.exe Service /?");
            Console.WriteLine("   DLNALib.exe FolderPath /?");
            Console.WriteLine("");
            System.Environment.Exit(0);
        }
        #endregion

        #region Individual Help
        static void showcommandhelp(string cmd)
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("DLNALib.EXE Help");
            Console.WriteLine("=======================================================");
            if (cmd == "Filename")
            {
                Console.WriteLine("");
                Console.WriteLine("[Filename]");
                Console.WriteLine("   Device file path to a previously saved .xml file. Must be FULL path.");
                Console.WriteLine("");
                Console.WriteLine("   If Filename has any spaces in it, please surround it with quotes!");
                Console.WriteLine("");
                Console.WriteLine("   For example:");
                Console.WriteLine("   DLNALib.exe /a C:\\myDevice.xml");
                Console.WriteLine("   DLNALib.exe /a \"C:\\Bravia KDL-55EX720.xml\"");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "Service")
            {
                Console.WriteLine("");
                Console.WriteLine("[Service]");
                Console.WriteLine("   The Friendly Service name for the action to execute.");
                Console.WriteLine("   Must be one of the following:");
                Console.WriteLine("      IRCC");
                Console.WriteLine("      RenderingControl");
                Console.WriteLine("      AVTransport");
                Console.WriteLine("      Party");
                Console.WriteLine("");
                Console.WriteLine("   For example:");
                Console.WriteLine("   DLNALib.exe /a C:\\myDevice.xml IRCC");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "/r")
            {
                Console.WriteLine("/r - Register Switch");
                Console.WriteLine("");
                Console.WriteLine("Executes a device Registration Process");
                Console.WriteLine("");
                Console.WriteLine("   DLNALib.exe /r \"[Filename]\" /log");
                Console.WriteLine("");
                Console.WriteLine("   Paramters must be entered in the above order.");
                Console.WriteLine("");
                Console.WriteLine("   Be Prepared to react at the Device or enter a PIN code");
                Console.WriteLine("   If device is Gen3, console will pronpt for PIN.");
                Console.WriteLine("   Cookie data will be saved to the C:\\PtogramData\\Sony folder.");
                Console.WriteLine("");
                Console.WriteLine("   An Example would be:");
                Console.WriteLine("   DLNALib.exe /r C:\\myDevice.xml /log");
            }
            else if (cmd == "/a")
            {
                Console.WriteLine("/a - Action Switch");
                Console.WriteLine("");
                Console.WriteLine("Executes a Service Action on the device");
                Console.WriteLine("");
                Console.WriteLine("DLNALib.exe /a \"[Filename]\" [Service] [Action] \"[Parameter]\"");
                Console.WriteLine("");
                Console.WriteLine("Paramters must be entered in the above order.");
                Console.WriteLine("Please place a single space between each.");
                Console.WriteLine("");
                Console.WriteLine("   [Filename]");
                Console.WriteLine("      Device file path to a previously saved .xml file. Must be FULL path.");
                Console.WriteLine("");
                Console.WriteLine("      If Filename has any spaces in it, please surround it with quotes!");
                Console.WriteLine("");
                Console.WriteLine("      For example:");
                Console.WriteLine("      DLNALib.exe /a C:\\myDevice.xml");
                Console.WriteLine("      DLNALib.exe /a \"C:\\Bravia KDL-55EX720.xml\"");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("   [Service]");
                Console.WriteLine("      The Friendly Service name for the action to execute.");
                Console.WriteLine("      Must be one of the following:");
                Console.WriteLine("         IRCC");
                Console.WriteLine("         RenderingControl");
                Console.WriteLine("         AVTransport");
                Console.WriteLine("         Party");
                Console.WriteLine("");
                Console.WriteLine("      For example:");
                Console.WriteLine("      DLNALib.exe /a C:\\myDevice.xml IRCC");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("   [Action]");
                Console.WriteLine("      Depending on the [Service] above:");
                Console.WriteLine("      This MUST be one of the following:");
                Console.WriteLine("         IRCC:");
                Console.WriteLine("           XSendIRCC - Sends an IRCC command value to the device.");
                Console.WriteLine("");
                Console.WriteLine("         RenderingControl:");
                Console.WriteLine("           SetMute - Sets the Mute of the device. Set parameter to True or False.");
                Console.WriteLine("           SetVolume - Sets the Volume Level on the device. Set parameter to 0-100.");
                Console.WriteLine("");
                Console.WriteLine("         AVTransport:");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("         Party:");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("      For example:");
                Console.WriteLine("      DLNALib.exe /a C:\\myDevice.xml IRCC XSendIRCC");
                Console.WriteLine("");
                Console.WriteLine("");
                Console.WriteLine("   [Parameter]");
                Console.WriteLine("      Sets a Parameter value to be used with the Action");
                Console.WriteLine("");
                Console.WriteLine("      If Parameter has any spaces in it, please surround with quotes!");
                Console.WriteLine("");
                Console.WriteLine("      Not all Actions require a Parameter.");
                Console.WriteLine("");
                Console.WriteLine("      An Example would be:");
                Console.WriteLine("      DLNALib.exe /a C:\\myDevice.xml IRCC XSendIRCC AAAAAQAAAAEAAAAuAw==");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "Parameter")
            {
                Console.WriteLine("");
                Console.WriteLine("[Parameter]");
                Console.WriteLine("   Sets a Parameter value to be used with the Action");
                Console.WriteLine("");
                Console.WriteLine("   If Parameter has any spaces in it, please surround with quotes!");
                Console.WriteLine("");
                Console.WriteLine("   Not all Actions require a Parameter.");
                Console.WriteLine("");
                Console.WriteLine("   An Example would be:");
                Console.WriteLine("   DLNALib.exe /a C:\\myDevice.xml IRCC XSendIRCC AAAAAQAAAAEAAAAuAw==");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "FolderPath")
            {
                Console.WriteLine("");
                Console.WriteLine("[FolderPath]");
                Console.WriteLine("   Sets a Folder Path used to save device XML files");
                Console.WriteLine("");
                Console.WriteLine("   If Parameter has any spaces in it, please surround with quotes!");
                Console.WriteLine("");
                Console.WriteLine("   An Example would be:");
                Console.WriteLine("   DLNALib.exe /l /ls \"[FolderPath]\"");
                Console.WriteLine("   DLNALib.exe /b http://192.168.0.100:52323/DMR.xml C:\\myDevices");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "/WOL")
            {
                Console.WriteLine("/WOL - Wake-On-Lan Switch");
                Console.WriteLine("");
                Console.WriteLine("DLNALib.exe /WOL \"[Filename]\"");
                Console.WriteLine("");
                Console.WriteLine("   Executes the Wake-On-Lan command for the device.");
                Console.WriteLine("");
                Console.WriteLine("   For example:");
                Console.WriteLine("   DLNALib.exe /f \"C:myDevice.xml\" /WOL");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "/log")
            {
                Console.WriteLine("[log]");
                Console.WriteLine("");
                Console.WriteLine("DLNALib.exe /WOL \"[Filename]\" /log");
                Console.WriteLine("");
                Console.WriteLine("   Enables the API Default Logging System");
                Console.WriteLine("   Log File location: DEFAULT - C:\\ProgramData\\Sony");
                Console.WriteLine("");
                Console.WriteLine("   For example:");
                Console.WriteLine("   DLNALib.exe /a C:\\myDevice.xml IRCC XSendIRCC AAAAAQAAAAEAAAAuAw== /log");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "/l")
            {
                Console.WriteLine("/l - Locate Switch");
                Console.WriteLine("");
                Console.WriteLine("DLNALib.exe /l");
                Console.WriteLine("");
                Console.WriteLine("   Executes the Device Locator function.");
                Console.WriteLine("");
                Console.WriteLine("   For example:");
                Console.WriteLine("   DLNALib.exe /l");
                Console.WriteLine("");
                Console.WriteLine("   Returns the count and Description URL for each device");
                Console.WriteLine("   ---- EXAMPLE Return ----");
                Console.WriteLine("   Found Devices - 2");
                Console.WriteLine("   1) http://192.168.1.100:8080/description.xml");
                Console.WriteLine("   2) http://192.168.1.110:52323/dmr.xml");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "/ls")
            {
                Console.WriteLine("/ls - Locate Save Switch");
                Console.WriteLine("");
                Console.WriteLine("DLNALib.exe /ls \"[FolderPath]\"");
                Console.WriteLine("");
                Console.WriteLine("Sets the Device Locator Save Function");
                Console.WriteLine("");
                Console.WriteLine("For Example:  DLNALib.exe /l /ls C:\\myDevices");
                Console.WriteLine("The above line will save ALL devices found to the C:\\myDevices\\ folder");
                Console.WriteLine("");
                Console.WriteLine("The file name will be the Devices Friendly name with the .xml extention");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "/b")
            {
                Console.WriteLine("/b - Build from Document Switch");
                Console.WriteLine("");
                Console.WriteLine("DLNALib.exe /b [DocumentURL] \"[Folderpath]\"");
                Console.WriteLine("");
                Console.WriteLine("   Executes the Build Device from Document Url function.");
                Console.WriteLine("");
                Console.WriteLine("   [DocumnetURL] - REQUIRED. This can be optained from the /l option");
                Console.WriteLine("   [FolderPath] - REQUIRED. This sets the location to save the device file");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "FolderPath")
            {
                Console.WriteLine("[FolderPath]");
                Console.WriteLine("");
                Console.WriteLine("   Sets the Device Locator Save Folder Path");
                Console.WriteLine("   This folder must exist!");
                Console.WriteLine("   Do NOT include a \"\\\" at the end!");
                Console.WriteLine("");
                Console.WriteLine("   For Example:  DLNALib.exe /d /ds 0 /dp \"C:\\myDevices\"");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else if (cmd == "DocumentURL")
            {
                Console.WriteLine("[DocumentURL]");
                Console.WriteLine("");
                Console.WriteLine("   Sets the URL to the devices Document XML file on the device.");
                Console.WriteLine("");
                Console.WriteLine("   For example:");
                Console.WriteLine("   DLNALib.exe /b http://192.168.0.100:52323/DMR.xml C:\\myDevices");
                Console.WriteLine("");
                Console.WriteLine("");
            }
            else
            {
                showhelp();
            }
            System.Environment.Exit(0);
        }
        #endregion
    }
}
