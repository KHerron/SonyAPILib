using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonyAPILib;

// SonyAPILib.dll library written by Kirk Herron.
// This Example Console application shows how to use the SonyAPILib in your own applications.
// This API connects to Sony Smart Devices(TV, Blue Ray, Tuners) via LAN or Wifi connection.
// Provides a method to send IRCC Remote control commands throught the LAN connection and remotely control the device.
// Retrives the devices Remote Command list for your own use or needs.
// Uses UPnP Protocols for device discovery, sending commands and future advanced features.

namespace ConsoleExample
{
    class Program
    {   
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("SonyAPILib v5.3 by: Kirk Herron");
            Console.WriteLine("Starting Console Example Program");
            Console.WriteLine("================================");

            // 1st we create a new instance of the SonyAPILib
            APILibrary mySonyLib = new APILibrary();

            // Now create new instances of the UPnP/DLNA services
            //SonyAPI_Lib.IRCC1 ircc1 = new SonyAPI_Lib.IRCC1();  // Only required if you plan to use this service
            //SonyAPI_Lib.AVTransport1 avtransport1 = new SonyAPI_Lib.AVTransport1(); // Only required if you plan to use this service
            //SonyAPI_Lib.ConnectionManager1 connectionmanager1 = new SonyAPI_Lib.ConnectionManager1(); // Only required if you plan to use this service
            //SonyAPI_Lib.RenderingControl1 renderingcontrol1 = new SonyAPI_Lib.RenderingControl1(); // Only required if you plan to use this service
            //SonyAPI_Lib.Party1 party1 = new SonyAPI_Lib.Party1(); // Only required if you plan to use this service
             
            #region Set Logging
            // Next Set the API logging information.
            // Enable Logging: default is set to FALSE.
            mySonyLib.Log.Enable = true;

            // Set Logging Level. 
            // Set to "Basic" to only Log Minimum information
            // Set to "All" for all Logging information
            // Default is set to "Basic"
            mySonyLib.Log.Level = "All";

            // Set where the logging file will be saved.
            // Folder will be created if it does not exist!
            // Set to Null to use Default
            // Default is set to C:\ProgramData\Sony
            mySonyLib.Log.Path = null;

            // Set the name of the Logging file.
            // Default is set to SonyAPILib_LOG.txt
            mySonyLib.Log.Name = "SonyAPILib_LOG.txt";

            // Clears the existing log file and starts a new one
            // Send Null as the param to just clear the file and start a new one
            // Enter a new File name as the param, and log file will be copied to new name before it is cleared.
            // Example: mySonyLib.LOG.clearlog(datestamp + "_Old_Sony_Log_File.txt");
            mySonyLib.Log.ClearLog(null);
            #endregion

            #region Device Locator
            // Perform a Discovery to find all/any compatiable devices on the LAN.
            // Returns a list of all Devices found through the UPnP Broadcast
            // Return value is a list containg the Description.xml file for each Device object discovered.
            // Each returned object will contain the full URL to the devices Description.xml file

            Console.WriteLine("Searching for Devices...");
            List<string> foundDevices = mySonyLib.Locator.LocateDevices();
            #endregion

            // foundDevices.Count will return the number of devices found
            #region Console Output 
            Console.WriteLine("Device Count: " + foundDevices.Count);
            Console.WriteLine("---------------------------------");
            int i = 0;
            foreach (string fd in foundDevices)
            {
                Console.WriteLine(i + " - " + fd.ToString());
                i = i + 1;
            }
            Console.WriteLine("---------------------------------");
            #endregion

            // TODO: Here you can perform task or other code as to which device to select or use.
            // You could also do a For Next loop and go through each one.
            // Or save the Description.xml URL for later use!

            // This example makes sure that there is at least one object returned in the list, if not then it will Exit.
            if (foundDevices.Count > 0)
            {
                // Allow User to select from the devices found which one to test
                Console.WriteLine("Enter the Device # to Test");
                string cki;
                cki = Console.ReadLine();

                // 1st create a new Device Object
               APILibrary.SonyDevice mySonyDevice = new APILibrary.SonyDevice();

                // Here you can save the device information to a database or text file.
                // This will allow you to Initialize a device WITHOUT having to run the sonyDiscover() method every time.
                // Once you have discovered the devices, saving their information will speed up your application the next time you run it!

                // You can Initialize a device with only the following minimimal information: 
                //     1) The Device Name - MUST Match what the device returns as it's name. (selDev.Name)
                //     2) The IP address of the device. (selDev.Device_IP_Address)

                // Now we build the new sonyDevice object with the selected device above.
                #region Build Device from Description.xml file
                // The buildFromDocument() method will retrieve the devices Description file, and build/populate the device object.

                // From above, selDev or each item in the list foundDevices[] is a SonyDevice object.

                // To build the device with the default information retrieved from locateDevices, use the following method:
                // mySonyDevice.DocumentURL = foundDevices[0].ToString(); or set the array to the device you want.
                // mySonyDevice.buildFromDocument(new Uri(mySonyDevice.DocumentURL));

                // This example will use the first method to initialize the device chosen by the user.
                Console.WriteLine("");
                Console.WriteLine(foundDevices[Convert.ToInt16(cki)].ToString() + ": Building Device....");
                
                // Set the devices DocumentUrl property first
                mySonyDevice.DocumentUrl = foundDevices[Convert.ToInt16(cki)].ToString();

                // Then build the device from the document
                mySonyDevice.BuildFromDocument(new Uri(mySonyDevice.DocumentUrl));

                #endregion

                // Next we will check the status of the device.
                #region Check Status
                
                //The following will return the current status of the device
                //The value returned is: status_name:value
                //An example would be: viewing:TV
                //An emplty string "" will be returned if there is no response from the device. This could also mean the device is off.
                //Also, this method requires the device to be registered on Generation 1 and 2 devices.
                Console.WriteLine(mySonyDevice.Name + ": Checking Device Status....");
                // Without a response: mySonyLib.ircc1.XGetStatus(mySonyDevice);
                string status = mySonyDevice.Ircc.GetStatus(mySonyDevice);
                // Use the Device State Variable: if (mySonyDevice.Ircc.CurrentStatus == "" | mySonyDevice.Ircc.CurrentStatus == null)
                if (status == "" | status == null)
                {
                    // NO Response!!
                }
                else
                {
                    #region Console Output
                    Console.WriteLine("");
                    Console.WriteLine("Get Status returned: " + status);
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                    #endregion
                }
               
                #endregion

                // Now we must register with the device
                #region register

                bool mySonyReg = false;

                // first check to see if the Build process determined the registration value.
                if (mySonyDevice.Registered == false)
                {
                    Console.WriteLine(mySonyDevice.Name + ": Performing Registration....");
                    Console.WriteLine("Before continuing, you may need to set the device to Registration Mode,");
                    Console.WriteLine("Confirm Registration or enter the Registration PIN code.");
                    Console.WriteLine("Go to the device and perfrom any step, and be ready before hitting enter below!");
                    Console.WriteLine("=====================================");
                    Console.WriteLine("Hit any key to continue");
                    Console.ReadKey();
                    // The next method is very IMPORTANT.

                    // YOU MUST RUN THE FOLLOWING METHOD AND RECEIVE A SUCCESSFUL RETURN at least ONCE!
                    // Before you can send any IRCC commands or receive and data back from the device!

                    // The very first time this is executed, you will need to be at your device (TV, Blue Ray)
                    // to confim the registration. Also, some devices (Blue Ray, Home Theater Tuners) require you to put
                    // the device in to "Registration" mode, before you try to register this application as
                    // a controlling device. (Registration uses the MAC address of the computer/device trying
                    //to gain control). If installed on more than 1 computer, each one will require registration.

                    // Register as Controller 
                    // Returns true if successful
                    // Returns false if not successful 

                    mySonyReg = mySonyDevice.Register();

                    // Check if register returned false
                    if (mySonyDevice.Registered == false)
                    {
                        //Check if Generaton 4. If yes, prompt for pin code
                        if (mySonyDevice.Actionlist.RegisterMode > 2)
                        {
                            string ckii;
                            Console.WriteLine("Enter PIN Code.");
                            ckii = Console.ReadLine();
                            // Send PIN code to TV to create Autorization cookie
                            Console.WriteLine("Sending Authitication PIN Code.");
                            mySonyReg = mySonyDevice.SendAuth(ckii);
                        }
                    }
                }
                else
                {
                    mySonyReg = true;
                }
                #endregion

                //TODO: Add more code in case of false, or true
                #region Console Output
                Console.WriteLine("Registration returned: " + mySonyReg.ToString());
                Console.WriteLine("---------------------------------");
                Console.WriteLine("");
                #endregion

                // This example will: If true, display device information
                #region Console Output
                if (mySonyReg)
                {

                    Console.WriteLine("Device Information");
                    Console.WriteLine("Mame: " + mySonyDevice.Name);
                    Console.WriteLine("Mac Address: " + mySonyDevice.MacAddress);
                    Console.WriteLine("IP Address: " + mySonyDevice.IPAddress);
                    Console.WriteLine("Port: " + mySonyDevice.Port);
                    Console.WriteLine("Registration Mode: " + mySonyDevice.Actionlist.RegisterMode);
                    Console.WriteLine("Registration: " + mySonyDevice.Registered.ToString());
                    Console.WriteLine("Server Name: " + mySonyDevice.ServerName);
                    Console.WriteLine("Server Mac: " + mySonyDevice.ServerMacAddress);
                    Console.WriteLine("Action List URL: " + mySonyDevice.ActionListUrl);
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                }
                else
                {
                    // Display this if NOT true
                    Console.WriteLine("There was an error");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                }
                #endregion

                // Get the IRCC command list from the device so we know it's capabilities.
                #region get_remote_command_list
                // The next command is used to retrieve the IRCC command list from the device.
                // ### You must register before this method will return any data ###
                // This method will populate the Commands list in the SonyDevice object when executed.
                // This Methed also returnes a string that contains the contents of the Devices Command List XML file for your own use.
                Console.WriteLine(mySonyDevice.Name + ": Retrieving Remote Command List");
                string CmdList = mySonyDevice.GetRemoteCommandList();

                // TODO: Parse this information as your application requires.
                // convert to an XMLDocument or dataset for your own use
                #endregion

                // Checks if the list contains any data
                #region Console Output
                if (CmdList != "")
                {
                    Console.WriteLine("Retrieved Command List Successful");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                }
                else
                {
                    Console.WriteLine("ERROR Retrieving Command List");
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                }
                #endregion

                // Get the IRCC command value by searching the command name
                #region GetCommandString
                // The next method is used to search for an IRCC Command String that matches the param.

                // param is a string containing the command name to search for.
                // Returns a string containing the command's value (If Successful)
                // Returns a Null if the search command is not found in the devices IRCC command list

                // This example will search for the command "VolumeUp"
                Console.WriteLine(mySonyDevice.Name + ": Retrieving Command Value for: VolumeUp");
                string irccCmd = mySonyDevice.GetCommandString("VolumeUp");

                //Check if command was found
                if (irccCmd == "")
                {
                    Console.WriteLine("Command Not Found: VolumeUp");
                }
                else
                {
                #endregion

                // Displays the IRCC command value retrieved
                #region Console Output
                    // Show the IRCC_Command value found information
                    Console.WriteLine("Found Command: VolumeUp");
                    Console.WriteLine("Command Value: " + irccCmd);
                    Console.WriteLine("---------------------------------");
                    Console.WriteLine("");
                    Console.WriteLine("Now we are ready to try to send a few IRCC commands to the device");
                    Console.WriteLine("Hit any key to continue");
                    Console.ReadKey();
                }
                #endregion

                // Next are 4 examples of how you can send the IRCC commands to the device
                #region Example 1
                // This first example will send a "VolumeUp" command value to the device
                // it asumes we already know the value to send to the device.
                // We will use the Command String we retrieved above in the GetCommandString method.
                Console.WriteLine(mySonyDevice.Name + ": Sending Command Value " + irccCmd + " to device");
                string results = mySonyDevice.Ircc.SendIRCC(mySonyDevice, irccCmd);
                System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command

                #region Console Output
                // Show the IRCC_Command value found information
                Console.WriteLine("Sent Command: VolumeUp:" + irccCmd);
                Console.WriteLine("Hit any key to continue");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
                #endregion
                #endregion

                #region Example 2
                // The next example will use the getIRCCcommandString("CommandName") method to get the command value for "VolumeDown".
                // Then send it to the device
                Console.WriteLine(mySonyDevice.Name + ": Sending Command VolumeDown to device");
                String mycommand = mySonyDevice.GetCommandString("VolumeDown");
                results = mySonyDevice.Ircc.SendIRCC(mySonyDevice, mycommand);
                System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command

                #region Console Output
                // Show the IRCC_Command value found information
                Console.WriteLine("Sent Command: VolumeDown:" + mycommand);
                Console.WriteLine("Hit any key to continue");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
                #endregion
                #endregion

                #region Example 3
                // The next example will use a combination of both examples above for the command "VolumeUp".
                Console.WriteLine(mySonyDevice.Name + ": Sending Command VolumeUp to device again");
                mySonyDevice.Ircc.SendIRCC(mySonyDevice, mySonyDevice.GetCommandString("VolumeUp"));
                System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command

                #region Console Output
                // Show the IRCC_Command value found information
                Console.WriteLine("Sent Command: VolumeUp:" + mycommand);
                Console.WriteLine("Hit any key to continue");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
                #endregion
                #endregion

                #region Example 5
                Console.WriteLine("");
                Console.WriteLine("Now You Try.");
                Console.WriteLine("Here are the Commands: Hit any key to Continue.");
                Console.WriteLine("---------------------------------");                
                Console.ReadKey();
                foreach (APILibrary.SonyCommands cmd in mySonyDevice.Commands)
                {
                    Console.WriteLine(cmd.name);
                }
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Enter a command from the list above.");
                cki = Console.ReadLine();
                results = mySonyDevice.Ircc.SendIRCC(mySonyDevice, mySonyDevice.GetCommandString(cki));
                #endregion

                #region Example 6
                Console.WriteLine("---------------------------------");
                Console.WriteLine("Now, using your TV remote control, navigate to a search screen.");
                Console.WriteLine("This can be Pandora, Youtube or any search where you enter TEXT.");
                Console.WriteLine("Now, enter the text here to send.");
                cki = Console.ReadLine();
                results = mySonyDevice.SendText(cki);
                Console.WriteLine("---------------------------------");
                #endregion
                
                //Added by jodriguez142514
                //Before added code, console application would close and not save any discovered configuration or registration
                //Now it will save it to a .xml file
                //I know you are given an option starting on line 102, however this makes it already built in so you can load
                //the xml file into the forms application
                System.Xml.Serialization.XmlSerializer x = new System.Xml.Serialization.XmlSerializer(mySonyDevice.GetType());
                TextWriter writer = new StreamWriter(mySonyDevice.Name + ".xml");
                x.Serialize(writer, mySonyDevice);
                //Added by jodriguez142514

                Console.WriteLine("That's about it for now. Hit enter to Quit.");
                Console.ReadKey();
            }
            else
            {
                Console.WriteLine("Error: No Devices were found!");
                Console.ReadKey();
            }
        }
    }
}
