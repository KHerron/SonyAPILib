using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SonyAPILib;

// SonyAPILib.dll library written by Kirk Herron.
// This Example explains how to use the SonyAPILib in your own applications.
// This API connects to Sony Smart Devices(TV, Blue Ray, Tuners) via LAN or Wifi connection.
// Provides a method to send IRCC Remote control commands throught the LAN connection and remotely control the device.
// Retrives the devices Remote Command list for your own use or needs.
// Uses UPnP Protocols for device discovery and future advanced features.

namespace SmartRemote_CS
{
    class Program
    {   
        [STAThread]
        static void Main(string[] args)
        {
            Console.WriteLine("SonyAPILib v5.0 by: Kirk Herron");
            Console.WriteLine("Starting Console Example Program");
            Console.WriteLine("================================");
            
            // 1st create new instance of the SonyAPILib
            // This Class will be used to Discover Sony Devices on the LAN.
            SonyAPI_Lib mySonyLib = new SonyAPI_Lib();
           
            // Next Set the API logging information.
            // Enable Logging: default is set to FALSE.
            // This Library will report to a logging file even when EnableLogging is set to False.
            mySonyLib.LOG.enableLogging = true;

            // Set Logging Level. 
            // Set to "Basic" to Log Minimum information
            // Set to "All" for all Logging information
            // Default is set to "Basic"
            mySonyLib.LOG.enableLogginglev = "All";

            // Set where the logging file will be saved.
            // Folder will be created if it does not exist!
            // Default is set to C:\ProgramData\Sony
            mySonyLib.LOG.loggingPath = null;

            // Set the name of the Logging file.
            // Default is set to SonyAPILib_LOG.txt
            mySonyLib.LOG.loggingName = "SonyAPILib_LOG.txt";

            // Clears the existing log file and starts a new one
            // enter a new name as the Param, and log file will be copied to new name before it is cleared.
            // Example: mySonyLib.LOG.clearlog("Old_Sony_Log_File.txt");
            mySonyLib.LOG.clearLog(null);
            
            // Perform a Discovery to find all/any compatiable devices on the LAN.
            // Returns a list of all Sony Devices that matches service criteria.
            // Send null as default to locate Sony devices that support the IRCC service.
            // If a different service is used, the SonyAPILib may NOT function properly. (To be Used in Future projects)
            // Return value is a list with each item containg a SonyDevice object.
            Console.WriteLine("Searching for Devices...");
            List<SonyAPI_Lib.SonyDevice> fDev = mySonyLib.API.sonyDiscover(null);

            // fDev.Count will return the number of devices found
            #region Console Output 
            Console.WriteLine("Device Count: " + fDev.Count);
            Console.WriteLine("---------------------------------");
            int i = 0;
            foreach (SonyAPI_Lib.SonyDevice fd in fDev)
            {
                Console.WriteLine(i + " - " + fd.Name);
                i = i + 1;
            }
            Console.WriteLine("---------------------------------");
            #endregion
            // TODO: Here you can perform task or other code as to which device to select or use.
                // You could also do a For Next loop and go through each one
            
            // This example checks if there are any devices in the list
            if (fDev.Count > 0)
            {
                // Allow User to select the device to test
                Console.WriteLine("Enter the Device # to Test");
                string cki;
                cki = Console.ReadLine();
                SonyAPI_Lib.SonyDevice selDev = new SonyAPI_Lib.SonyDevice();

                // Here you can save the device information to a database or text file.
                // This will allow you to Initialize a device WITHOUT having to run the sonyDiscover() method.
                // Once you have discovered the devices, saving their information will speed up your application the next time you run it!
                // You can Initialize a device with as minimimal of information as: 
                //     1) The Name - MUST Match what the device returns as it's name with the property:  selDev.Name
                //     2) The IP address of the device:  selDev.Device_IP_Address
                //     3) The Port the device uses to communicate (Not always required) :  selDev.Device_Macaddress
                // 
                //

                // Now we initialize a new sonyDevice object with the selected device above.
                #region initialize(SonyDevice device)
                // selDev or each item in the list fDev[] is a SonyDevice object.

                // To initialize with the default information retrieved from sonyDiscovery, use the following method:
                // mySonyDevice.initialize(selDev) or mySonyDevice.initialize(fDev[1]) by setting the index of the list.

                // You can manually initialize the sonyDevice by setting the device information yourself.
                // To manually initialize without using sonyDiscovery, use the following method:
                // mySonyDevice.initialize("Name","IP Address","Port","Server Mac")

                // To use the information found by discovery, but need to override one of the elements, use the following method:
                // mySonyDevice.initialize(selDev, Name, IP, Port, Server Mac);
                // Set param to null to use the default information.
                Console.WriteLine("");
                selDev.initialize(fDev[Convert.ToInt32(cki)]);

                #endregion

                // Next we will check the status of the device.
                #region Check Status
                
                //The following will return the current status of the device
                //The value returned is: status_name:value
                //An example would be: viewing:TV
                //An emplty string "" will be returned if there is no response from the device. This could also mean the device is off.
                //Also, this method requires the device to be registered, so if this returns any values, then Registration = True.
                string status = selDev.checkStatus(); 
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
                #region register()

                //If Status above returned any value, then skip registration
                bool mySonyReg;
                if (status == "" | status == null)
                {
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

                    mySonyReg = selDev.register();

                    // Check if register returned false
                    if (mySonyReg == false)
                    {
                        //Check if Generaton 3. If yes, prompt for pin code
                        if (selDev.Generation == 3)
                        {
                            Console.WriteLine("Enter the Device PIN Code");
                            string ckii;
                            ckii = Console.ReadLine();
                            // Send PIN code to TV to create Autorization cookie
                            mySonyReg = selDev.sendAuth(ckii);
                        }
                    }
                }
                else
                {
                    mySonyReg = true;
                    selDev.Registered = true;
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
                    Console.WriteLine("Mame: " + selDev.Name);
                    Console.WriteLine("Mac Address: " + selDev.Device_Macaddress);
                    Console.WriteLine("IP Address: " + selDev.Device_IP_Address);
                    Console.WriteLine("Port: " + selDev.Device_Port);
                    Console.WriteLine("Generation: " + selDev.Generation);
                    Console.WriteLine("Registration: " + selDev.Registered.ToString());
                    Console.WriteLine("Server Name: " + selDev.Server_Name);
                    Console.WriteLine("Server Mac: " + selDev.Server_Macaddress);
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
                #region get_remote_command_list()
                // The next command is used to retrieve the IRCC command list from the device.
                // ### You must register before this method will return any data ###
                // This method will populate the Commands list in the SonyDevice object when executed.
                // This Methed also returnes a string that contains the contents of the Devices Command List XML file for your own use.
                string CmdList = selDev.get_remote_command_list();

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
                #region getIRCCcommandString(command name)
                // The next method is used to search for an IRCC_Command that matches the param.

                // param is a string containing the command name to search for.
                // Returns a string containing the command's value (If Successful)
                // Returna a Null if the search command is not found in the devices IRCC command list

                // This example will search for the command "ChannelUp"
                string irccCmd = selDev.getIRCCcommandString("VolumeUp");

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
                    Console.WriteLine("Search Command: VolumeUp");
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
                // We will use the irccCmd we retrieved above in the getIRCCCommandString method.
                string results = selDev.send_ircc(irccCmd);
                System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command

                #region Console Output
                // Show the IRCC_Command value found information
                Console.WriteLine("Sent Command: VolumeUp");
                Console.WriteLine("Hit any key to continue");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
                #endregion
                #endregion

                #region Example 2
                // The next example will use the getIRCCcommandString("CommandName") method to get the command value for "VolumeDown".
                // Then send it to the device
                String mycommand = selDev.getIRCCcommandString("VolumeDown");
                selDev.send_ircc(mycommand);
                System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command

                #region Console Output
                // Show the IRCC_Command value found information
                Console.WriteLine("Sent Command: VolumeDown");
                Console.WriteLine("Command Value: " + mycommand);
                Console.WriteLine("Hit any key to continue");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
                #endregion
                #endregion

                #region Example 3
                // The next example will use a combination of both examples above for the command "VolumeUp".
                selDev.send_ircc(selDev.getIRCCcommandString("VolumeUp"));
                System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command
                #region Console Output
                // Show the IRCC_Command value found information
                Console.WriteLine("Sent Command: VolumeUp");
                Console.WriteLine("Command Value: " + mycommand);
                Console.WriteLine("Hit any key to continue");
                Console.WriteLine("---------------------------------");
                Console.ReadKey();
                #endregion
                #endregion

                #region Example 4
                // The next example will use the "channel_set" method to send a complete channel number.
                // This example will use channel 47.1 since it is a valid station in my area. You can change this to what ever you need to.
                // This example should only be used on TV's, as Home theater systems and DVD players will not respond to this!
                string checkChannel = selDev.getIRCCcommandString("ChannelUp");
                if (checkChannel != "")
                {
                    selDev.channel_set("47.1");
                    System.Threading.Thread.Sleep(500);  // give the device time to react before sending another command
                }

                #region Console Output
                // Show the Set Channel Information
                if (checkChannel != "")
                {
                    Console.WriteLine("Sent Command: Channel_Set");
                    Console.WriteLine("Command Value: " + selDev.current_Channel);
                }
                else
                {
                    Console.WriteLine("Set Channel Not sent, Device is NOT a TV!");
                }
                Console.WriteLine("");
                Console.WriteLine("That's It. Hit any key to quit.");
                Console.WriteLine("---------------------------------");
                
                Console.ReadKey();
                #endregion

                #endregion
            }
            else
            {
                Console.WriteLine("Error: No Devices were found!");
                Console.ReadKey();
            }
        }
    }
}
