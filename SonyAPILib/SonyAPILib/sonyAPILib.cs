using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;
using System.Text;
using System.Net.Sockets;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;

namespace SonyAPILib
{
    /// <summary>
    /// Sony API Library
    /// </summary>
    public class SonyAPI_Lib
    {
        #region Library Varables

        /// <summary>
        /// Gets or Sets the API Logging Object
        /// </summary>
        public APILogging LOG { get { return _Log; } set { _Log = value; } }

        /// <summary>
        /// Gets or Sets the Sony_API Object
        /// </summary>
        public Locate Locator { get { return _Locator; } set { _Locator = value; } }

        /// <summary>
        /// Represents the IRCC1 Service
        /// </summary>
        public IRCC1 ircc1 = new IRCC1();

        /// <summary>
        /// Represents the AVTransport1 Service
        /// </summary>
        public AVTransport1 avtransport1 = new AVTransport1();

        /// <summary>
        /// Represents the ConnectionManager Service
        /// </summary>
        public ConnectionManager1 connectionmanager1 = new ConnectionManager1();

        /// <summary>
        /// Represents the RenderingControl Service
        /// </summary>
        public RenderingControl1 renderingcontrol1 = new RenderingControl1();

        /// <summary>
        /// Represents the Party Service
        /// </summary>
        public Party1 party1 = new Party1();

        /// <summary>
        /// API Logging Object
        /// </summary>
        static APILogging _Log = new APILogging();

        /// <summary>
        /// API Object
        /// </summary>
        static Locate _Locator = new Locate();

        #endregion

        #region Device Locator
        // Sony Device Locator Class
        /// <summary>
        /// Class used to Discover Sony devices with the IRCC service via a LAN connection
        /// </summary>
        public partial class Locate
        {
            // Sony Locate Variables
            #region Public Variables

            /// <summary>
            /// Contains the a Dataset of the Device's Command List
            /// </summary>
            SonyCommandList dataSet = new SonyCommandList();
            
            /// <summary>
            /// Variable for storing the devices Action List URL used in API Class and Discovery.
            /// </summary>
            public string actionListUrl { get; set; }

            

            #endregion

            #region Locate Devices by Service
            /// <summary>
            /// Execute to scan and locate all compatiable devices on network
            /// </summary>
            /// <returns>A list containing the full URL to each device's Description.xml file</returns>
            [STAThread]
            public List<string> locateDevices()
            {
                SonyDevice fdev = new SonyDevice();
                //if (service == null) { service = "IRCC:1"; }
                _Log.writetolog("UPnP is Discovering devices....", true);
                List<string> foundDevices = new List<string>();
                SonyAPILib.SSDP.Start();
                Thread.Sleep(15000);
                SonyAPILib.SSDP.Stop();
                foreach (string u in SSDP.Servers)
                {
                    foundDevices.Add(u);
                }
                _Log.writetolog("Devices Discovered: " + foundDevices.Count.ToString(), true);
                return foundDevices;
            }
            #endregion

            #region Load Device
            /// <summary>
            /// Loads a device from a file
            /// </summary>
            /// <param name="path">The FULL path to the Device XML file</param>
            public SonyAPI_Lib.SonyDevice DeviceLoad(string path)
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(SonyDevice));
                SonyAPI_Lib.SonyDevice sDev = new SonyAPI_Lib.SonyDevice();
                TextReader reader = new StreamReader(path);

                //deserialize
                sDev = (SonyDevice)deserializer.Deserialize(reader);
                reader.Close();
                sDev.checkReg();
                return sDev;
            }

            #endregion

            #region Save Device
            /// <summary>
            /// Saves a device to a file
            /// </summary>
            /// <param name="path">Full path including File Name: c://temp/device.xml</param>
            /// <param name="dev">The Device Object to save.</param>
            public void DeviceSave(string path, SonyDevice dev)
            {
                XmlSerializer serializer = new XmlSerializer(typeof(SonyDevice));
                FileStream fs = new FileStream(path, FileMode.Create);
                TextWriter writer = new StreamWriter(fs, new UTF8Encoding());
                serializer.Serialize(writer, dev);
                writer.Close();
                string newPath1 = path.Substring(0, path.Length - 4);
                string newPath2 = newPath1 + "_IRCC.xml";
            }
            #endregion
        }
        #endregion

        #region Sony Device Class
        /// <summary>
        /// Sony Device Object used to Register and Control via Lan connection
        /// </summary>
        [Serializable]
        public class SonyDevice
        {
            #region Variables

            #region Public Variables

            /// <summary>
            /// Gets or Sets the Sony Device Object Name
            /// </summary>
            public string Name { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device Manufacture
            /// </summary>
            public string Manufacture { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device Model Name
            /// </summary>
            public string ModelName { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device Model Description
            /// </summary>
            public string ModelDescription { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device Model Number
            /// </summary>
            public string ModelNumber { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device Unique Identifier Number
            /// </summary>
            public string UDN { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device type Identifier
            /// </summary>
            public string DeviceType { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's IP Address
            /// </summary>
            public string Device_IP_Address { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's MAC Address
            /// </summary>
            public string Device_Macaddress { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's Port number
            /// </summary>
            public int Device_Port { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's List of Commands
            /// </summary>
            public List<SonyCommands> Commands { get; set; }
            
            /// <summary>
            /// Gets or Sets the Mac Address of he controlling device
            /// </summary>
            public string Server_Macaddress { get; set; }
            
            /// <summary>
            /// Gets or Sets the Name of he controlling device
            /// </summary>
            public string Server_Name { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Gen 3 Object's Authintication Cookie
            /// </summary>
            public string Cookie { get; set; }
            
            /// <summary>
            /// Gets or Sets the status of registration
            /// </summary>
            public bool Registered { get; set; }

            /// <summary>
            /// This will contain the retrieved X_CERS_ActionList_URL used in registration and other commands.
            /// </summary>
            public string Actionlist_Url { get; set; }

            /// <summary>
            /// This will contain the retrieved ActionList objects.
            /// </summary>
            public ActionList Actionlist = new ActionList();
                                   
            /// <summary>
            /// List of Device Commands
            /// </summary>
            SonyCommandList dataSet = new SonyCommandList();
            
            // Socket _socket = null;
            
            /// <summary>
            /// Default PIN code used with Gen3 devices
            /// </summary>
            public string pincode = "0000";
            
            /// <summary>
            /// Cookie container for Gen3 Devices
            /// </summary>
            CookieContainer allcookies = new CookieContainer();
            
            /// <summary>
            /// Gets or Sets the RenderingControl Service if it exist
            /// </summary>
            public renderingcontrol RenderingControl =new renderingcontrol();

            /// <summary>
            /// Gets or Sets the ConnectionManager Service if it exist
            /// </summary>
            public connectionmanager ConnectionManager = new connectionmanager();

            /// <summary>
            /// Gets or Sets the AVTransport Service if it exist
            /// </summary>
            public avtransport AVTransport = new avtransport();

            /// <summary>
            /// Gets or Sets the Party Service if it exist
            /// </summary>
            public party Party = new party();

            /// <summary>
            /// Gets or Sets the IRCC Service if it exist
            /// </summary>
            public ircc IRCC = new ircc();

            /// <summary>
            /// Gets or Sets the Devices Document URL
            /// </summary>
            public string DocumentURL { get; set; }

            #endregion

            #endregion

            #region Events

            #endregion

            #region register
            /// <summary>
            /// Sends the Registration command to the selected device
            /// </summary>
            /// <returns>Returns a bool True or False</returns>
            public bool register()
            {
                _Log.writetolog("Controlling Mac address: " + this.Server_Macaddress, false);
                string reg = "false";
                string args1 = "name=" + this.Server_Name + "&registrationType=initial&deviceId=TVSideView%3A" + this.Server_Macaddress + " ";
                string args2 = "name=" + this.Server_Name + "&registrationType=new&deviceId=TVSideView%3A" + this.Server_Macaddress + " ";
                if (this.Actionlist.RegisterMode == 1)
                {
                    reg = HttpGet(this.Actionlist.RegisterUrl + "?" + args1);
                    _Log.writetolog("Register Mode: 2 Sony Device", false);
                }
                else if (this.Actionlist.RegisterMode == 2)
                {
                    reg = HttpGet(this.Actionlist.RegisterUrl + "?" + args2);
                    _Log.writetolog("Register Mode: 1 Sony Device", false);
                }
                else if (this.Actionlist.RegisterMode == 3)
                {
                    _Log.writetolog("Register Mode 3 Sony Sevice", false);
                    string hostname = this.Server_Name;
                    string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                    try
                    {
                        _Log.writetolog("Creating JSON Web Request", false);
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + this.Device_IP_Address + "/sony/accessControl");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.AllowAutoRedirect = true;
                        httpWebRequest.Timeout = 500;
                        _Log.writetolog("Sending Generation 3 JSON Registration", false);
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWriter.Write(jsontosend);
                        }
                        try
                        {
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var responseText = streamReader.ReadToEnd();
                                _Log.writetolog("Registration response: " + responseText, false);
                                this.Registered = true;
                            }
                            string answerCookie = JsonConvert.SerializeObject(httpWebRequest.CookieContainer.GetCookies(new Uri("http://" + this.Device_IP_Address + "/sony/appControl")));
                            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                            file.WriteLine(answerCookie);
                            file.Close();
                            this.Cookie = answerCookie;
                            reg = "";
                        }
                        catch
                        {
                            _Log.writetolog("Must run Method: SendAuth(pincode)", true);
                            reg = "Gen3 Pin Code Required";
                        }
                    }
                    catch
                    {
                        _Log.writetolog("device not reachable", false);
                    }
                }
                if (reg == "")
                {
                    _Log.writetolog("Registration was Successful for device at: " + this.Device_IP_Address, true);
                    this.Registered = true;
                    return true;
                }
                else if (reg == "Gen3 Pin Code Required")
                {
                    _Log.writetolog("Registration not complete for Gen3 device at: " + this.Device_IP_Address, true);
                    this.Registered = false;
                    return true;
                }
                else
                {
                    _Log.writetolog("Registration was NOT successful for device at: " + this.Device_IP_Address, true);
                    this.Registered = false;
                    return false;
                }
            }
            #endregion

            #region Send Authentication Pin Code
            /// <summary>
            /// Sends the Authorization PIN code to the Gen3 Device
            /// </summary>
            /// <param name="pincode"></param>
            /// <returns>True or False</returns>
            public bool sendAuth(string pincode)
            {
                bool reg = false;
                string hostname = this.Server_Name;
                string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                try
                {
                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + this.Device_IP_Address + @"/sony/accessControl");
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "POST";
                    httpWebRequest2.AllowAutoRedirect = true;
                    httpWebRequest2.CookieContainer = allcookies;
                    httpWebRequest2.Timeout = 500;
                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        streamWriter.Write(jsontosend);
                    }
                    string authInfo = "" + ":" + pincode;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                    var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        _Log.writetolog("Registration response: " + responseText, false);
                        this.Registered = true;
                        this.pincode = pincode;
                        reg = true;
                    }
                    string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + this.Device_IP_Address + "/sony/appControl")));
                    System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                    file.WriteLine(answerCookie);
                    file.Close();
                    this.Cookie = answerCookie;
                }
                catch
                {
                    _Log.writetolog("Registration process Timed Out", false);
                    this.Registered = false;
                }
                return reg;
            }
            #endregion

            #region get remote command list
            /// <summary>
            /// This method will retrieve Gen1 and Gen2 XML IRCC Command List or Gen3 JSON Command List.
            /// </summary>
            /// <returns>Returns a string containing the contents of the returned XML Command List for your Use</returns>
            /// <remarks>This method will also populate the SonyDevice.Commands object list with the retrieved command list</remarks>
            public string get_remote_command_list()
            {
                string cmdList = "";
                if (this.Actionlist.RegisterMode <= 2)
                {
                    _Log.writetolog(this.Name + " is Retrieving Generation:" + this.Actionlist.RegisterMode + " Remote Command List", false);
                    cmdList = HttpGet(this.Actionlist.getRemoteCommandList);
                    if (cmdList != "")
                    {
                        _Log.writetolog("Retrieve Command List was Successful", true);
                        DataSet CommandList = new DataSet();
                        System.IO.StringReader xmlSR = new System.IO.StringReader(cmdList);
                        CommandList.ReadXml(xmlSR, XmlReadMode.Auto);
                        DataTable IRCCcmd = new DataTable();
                        var items = CommandList.Tables[0].AsEnumerable().Select(r => new SonyCommands { name = r.Field<string>("name"), value = r.Field<string>("value") });
                        var itemlist = items.ToList();
                        this.Commands = itemlist;
                        _Log.writetolog(this.Name + " Commands have been Populated", true);
                    }
                    else
                    {
                        _Log.writetolog("Retrieve Command List was NOT successful", true);
                    }
                }
                else
                {
                    _Log.writetolog(this.Name + " is Retrieving Generation:" + this.Actionlist.RegisterMode + " Remote Command List", false);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.Device_IP_Address + @"/sony/system");
                    httpWebRequest.ContentType = "text/json";
                    httpWebRequest.Method = "POST";
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        string json = "{\"id\":20,\"method\":\"getRemoteControllerInfo\",\"version\":\"1.0\",\"params\":[]}";
                        streamWriter.Write(json);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        cmdList = responseText;
                        if (cmdList != "")
                        {
                            _Log.writetolog("Response Returned: " + cmdList, true);
                            _Log.writetolog("Retrieve Command List was Successful", true);
                        }
                        else
                        {
                            _Log.writetolog("Retrieve Command List was NOT successful", true);
                        }
                        dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                    }
                    string first = dataSet.result[1].ToString();
                    List<SonyCommands> bal = JsonConvert.DeserializeObject<List<SonyCommands>>(first);
                    this.Commands = bal;
                    _Log.writetolog(this.Name + " Commands have been Populated: " + this.Commands.Count().ToString(), true);
                }
                return cmdList;
            }
            #endregion

            #region HTTP GET command
            private string HttpGet(string Url)
            {
                _Log.writetolog("Creating HttpWebRequest to URL: " + Url, true);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.KeepAlive = true;
                // Set our default header Info
                _Log.writetolog("Setting Header Information: " + req.Host.ToString(), false);
                req.Host = this.Device_IP_Address + ":" + this.Device_Port;
                req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                req.Headers.Add("X-CERS-DEVICE-INFO", "Android4.03/TVSideViewForAndroid2.7.1/EVO");
                req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + this.Server_Macaddress);
                req.Headers.Add("Accept-Encoding", "gzip");
                try
                {
                    _Log.writetolog("Creating Web Request Response", false);
                    System.Net.WebResponse resp = req.GetResponse();
                    _Log.writetolog("Executing StreamReader", false);
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string results = sr.ReadToEnd().Trim();
                    _Log.writetolog("Response returned: " + results, false);
                    sr.Close();
                    resp.Close();
                    return results;
                }
                catch (Exception e)
                {
                    _Log.writetolog("There was an error during the Web Request or Response! " + e.ToString(), true);
                    return "false : " + e;
                }
            }
            #endregion

            #region HTTP POST command

            /// <summary>
            /// Executes the HTTP Post command
            /// </summary>
            /// <param name="Url">URL of Device Command to send</param>
            /// <param name="Parameters">Additional parameters</param>
            /// <returns></returns>
            public string HttpPost(string Url, String Parameters)
            {
                _Log.writetolog("Creating HttpWebRequest to URL: " + Url, true);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                _Log.writetolog("Sending the following parameter: " + Parameters.ToString(), true);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
                req.KeepAlive = true;
                req.Method = "POST";
                req.ContentType = "text/xml; charset=utf-8";
                req.ContentLength = bytes.Length;
                _Log.writetolog("Setting Header Information: " + req.Host.ToString(), false);
                if (this.Device_Port != 80)
                {
                    req.Host = this.Device_IP_Address + ":" + this.Device_Port;
                }
                else
                {
                    req.Host = this.Device_IP_Address;
                }
                _Log.writetolog("Header Host: " + req.Host.ToString(), false);
                req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                _Log.writetolog("Setting Header User Agent: " + req.UserAgent, false);
                if (this.Actionlist.RegisterMode == 3)
                {
                    _Log.writetolog("Processing Auth Cookie", false);
                    req.CookieContainer = new CookieContainer();
                    List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(this.Cookie);
                    req.CookieContainer.Add(new Uri(@"http://" + this.Device_IP_Address + bal[0].Path.ToString()), new Cookie(bal[0].Name, bal[0].Value));
                    _Log.writetolog("Cookie Container Count: " + req.CookieContainer.Count.ToString(), false);
                    _Log.writetolog("Setting Header Cookie: auth=" + bal[0].Value, false);
                }
                else
                {
                    _Log.writetolog("Setting Header X-CERS-DEVICE-ID: TVSideView-" + this.Server_Macaddress, false);
                    req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + this.Server_Macaddress);
                }
                req.Headers.Add("SOAPAction", "\"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                if (this.Actionlist.RegisterMode != 3)
                {
                    req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                    req.Headers.Add("Accept-Encoding", "gzip, deflate");
                }
                else
                {
                    req.Headers.Add("Accept-Encoding", "gzip");
                }
                _Log.writetolog("Sending WebRequest", false);
                System.IO.Stream os = req.GetRequestStream();
                // Post data and close connection
                os.Write(bytes, 0, bytes.Length);
                _Log.writetolog("Sending WebRequest Complete", false);
                // build response object if any
                _Log.writetolog("Creating Web Request Response", false);
                System.Net.HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                Stream respData = resp.GetResponseStream();
                StreamReader sr = new StreamReader(respData);
                string response = sr.ReadToEnd();
                _Log.writetolog("Response returned: " + response, false);
                os.Close();
                sr.Close();
                respData.Close();
                return response;
            }
            #endregion

            #region Verify Registration

            /// <summary>
            /// This method Checks the current Status of the device Registration
            /// </summary>
            /// <returns>Returns a Bool True or False</returns>
            public bool checkReg()
            {
                bool results = false;
                
                // Gen 1 or 2 Devices
                if (this.Actionlist.RegisterMode <= 2)
                {
                    _Log.writetolog("Verifing Registration for: " + this.Name, false);
                    // Will NOT return a Status if not Registered
                    if (this.checkStatus() != "") 
                    { 
                        this.Registered = true;
                        results = true;
                    }
                }
                else
                {
                    // Generation 3 devices uses a Cookie
                    try
                    {
                        // Check and Load cookie. If Found then Registration = True
                        _Log.writetolog("Verifing Registration for: " + this.Name, false);
                        _Log.writetolog("Checking for Generation 3 Cookie", false);
                        System.IO.StreamReader myFile = new System.IO.StreamReader(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                        string myString = myFile.ReadToEnd();
                        myFile.Close();
                        List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(myString);
                        _Log.writetolog(this.Name + ": Cookie Loaded: " + bal[0].Value, false);

                        // Check if cookie has expired
                        DateTime CT = DateTime.Now;
                        _Log.writetolog(this.Name + ": Checking if Cookie has Expired: " + bal[0].Expires, false);
                        _Log.writetolog(this.Name + ": Cookie Expiration Date: " + bal[0].Expires, false);
                        _Log.writetolog(this.Name + ": Current Date and Time : " + CT, false);
                        if (CT > Convert.ToDateTime(bal[0].Expires))
                        {
                            _Log.writetolog(this.Name + ": Cookie is Expired!", true);
                            _Log.writetolog(this.Name + ": Retriving NEW Cookie!", true);
                            string hostname = this.Server_Name;
                            string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                            try
                            {
                                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + this.Device_IP_Address + @"/sony/accessControl");
                                httpWebRequest2.ContentType = "application/json";
                                httpWebRequest2.Method = "POST";
                                httpWebRequest2.AllowAutoRedirect = true;
                                httpWebRequest2.CookieContainer = allcookies;
                                httpWebRequest2.Timeout = 500;
                                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                                {
                                    streamWriter.Write(jsontosend);
                                }
                                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var responseText = streamReader.ReadToEnd();
                                    _Log.writetolog("Registration response: " + responseText, false);
                                }
                                string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + this.Device_IP_Address + "/sony/appControl")));
                                System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                                file.WriteLine(answerCookie);
                                file.Close();
                                this.Cookie = answerCookie;
                                bal = JsonConvert.DeserializeObject<List<SonyCookie>>(answerCookie);
                                allcookies.Add(new Uri(@"http://" + this.Device_IP_Address + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                                this.Registered = true;
                                results = true;
                                _Log.writetolog(this.Name + ": New Cookie auth=" + this.Cookie, false);
                            }
                            catch
                            {
                                _Log.writetolog(this.Name + ": Failed to retrieve new Cookie", false);
                                this.Registered = false;
                                results = false;
                            }
                        }
                        else
                        {
                            _Log.writetolog(this.Name + ": Cookie is not Expired.", false);
                            _Log.writetolog(bal[0].Name + ": Adding Cookie to Device: " + bal[0].Value, false);
                            allcookies.Add(new Uri(@"http://" + this.Device_IP_Address + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                            this.Cookie = myString;
                            this.Registered = true;
                            results = true;
                            _Log.writetolog(this.Name + ": Cookie Found: auth=" + this.Cookie, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _Log.writetolog("No Cookie was found", false);
                        _Log.writetolog(ex.ToString(), true);
                        results = false;
                        this.Registered = false;
                    }
                }
                _Log.writetolog(this.Name + ": Registration Check returned: " + results.ToString(), false);
                return results;
            }
            #endregion

            #region Check Status

            /// <summary>
            /// This method Gets the current Status of the device
            /// </summary>
            /// <returns>Returns the device response as a string</returns>
            public string checkStatus()
            {
                string retstatus = "";
                if (this.Actionlist.RegisterMode != 3)
                {
                    try
                    {
                        _Log.writetolog("Checking Status of Device " + this.Name, false);
                        string cstatus;
                        int x;
                        cstatus = HttpGet(this.Actionlist.getStatus);
                        cstatus = cstatus.Replace("/n", "");
                        x = cstatus.IndexOf("name=");
                        cstatus = cstatus.Substring(x + 6);
                        x = cstatus.IndexOf("\"");
                        string sname = cstatus.Substring(0, x);
                        cstatus = cstatus.Substring(x);
                        x = cstatus.IndexOf("value=");
                        cstatus = cstatus.Substring(x + 7);
                        x = cstatus.IndexOf("\"");
                        string sval = cstatus.Substring(0, x);
                        retstatus = sname + ":" + sval;
                        _Log.writetolog("Device returned a Status of: " + retstatus, true);
                    }
                    catch (Exception ex)
                    {
                        _Log.writetolog("Checking Device Status for " + this.Name + " Failed!", true);
                        _Log.writetolog(ex.ToString(), true);
                        retstatus = "";
                    }
                }
                else
                {
                    try
                    {
                        _Log.writetolog("Checking Status of Device " + this.Name, false);
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.Device_IP_Address + @"/sony/system");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            string json = "{\"id\":19,\"method\":\"getPowerStatus\",\"version\":\"1.0\",\"params\":[]}\"";
                            streamWriter.Write(json);
                        }
                        var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                        using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                        {
                            var responseText = streamReader.ReadToEnd();
                            dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                        }
                        string first = dataSet.result[0].ToString();
                        first = first.Replace("{", "");
                        first = first.Replace("\"", "");
                        retstatus = first;
                        _Log.writetolog("Device returned a Status of: " + retstatus, true);
                    }
                    catch (Exception ex)
                    {
                        _Log.writetolog("Check Status Failed: " + ex, true);
                    }
                }
                return retstatus;
            }
            #endregion

            #region getRegistrationMode

            /// <summary>
            /// Gets the Registration Mode from the ActionList.
            /// Or uses Gen 3 if no action list is found.
            /// </summary>
            /// <returns>Returns a string wih the Mode (1, 2 or 3)</returns>
            private string getRegistrationMode()
            {
                string lAction = "register";
                string foundVal = "3";
                _Log.writetolog("Retriving Device Registration Mode", false);
                DataSet acList = new DataSet();
                if (Actionlist_Url != "")
                {
                    acList.ReadXml(Actionlist_Url);
                    DataTable act = new DataTable();
                    act = acList.Tables[0];
                    var results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == lAction select myRow;
                    foundVal = results.ElementAt(0).Field<string>("mode");
                }
                else
                {
                    _Log.writetolog("No Action List found.", true);
                }
                _Log.writetolog("Using Registration mode: " + foundVal, true);
                return foundVal;
            }
            #endregion

            #region getIRCCCommandString

            /// <summary>
            /// This method is used to find the Device Command Value based on the Command Name from the SonyDevice.Commands object list.
            /// </summary>
            /// <param name="cmdname">A valid command name found in the SonyDevice.Commands object list. (ie: "ChannelUp")</param>
            /// <returns>Returns the command value for the matched command name. ie: "AAAAAQAAAAEAAAAQAw==". or returns an empty string if no match is found</returns>
            /// <remarks>This can be used with send_ircc("AAAAAQAAAAEAAAAQAw==")
            ///  like this: send_ircc(getIRCCcommandString("ChannelUp")
            /// </remarks>
            public string getIRCCcommandString(string cmdname)
            {
                // Convert XML String to Dataset
                _Log.writetolog("Retrieving Command String for Command: " + cmdname, true);
                string foundCmd="";
                var results = this.Commands.FirstOrDefault(o => o.name.ToLower() == cmdname.ToLower());
                if (results != null)
                {
                    try
                    {
                        foundCmd = results.value;
                        _Log.writetolog("Found Command String for: " + cmdname + " - " + foundCmd, false);
                    }
                    catch (Exception e)
                    {
                        foundCmd = "";
                        _Log.writetolog("Command String for: " + cmdname + " NOT Found! ", true);
                        _Log.writetolog(e.ToString(), true);
                    }
                }
                return foundCmd;
            }
            #endregion

            #region Send Text command

            /// <summary>
            /// This method send Inputed Text via an HTTP GET command
            /// </summary>
            /// <param name="sendtext">A string containing the text to send to the device</param>
            /// <returns>Returns the device response as a string</returns>
            public string send_text(string sendtext = "")
            {
                string response = "";
                if (this.Actionlist.RegisterMode < 3)
                {
                    _Log.writetolog("Sending TEXT to device", false);
                    response = HttpGet(this.Actionlist.sendText + "?text=" + sendtext);
                    if (response != "")
                    {
                        _Log.writetolog("Send Text WAS sent Successfully", true);
                    }
                    else
                    {
                        _Log.writetolog("Send Text was NOT sent successfully", true);
                    }
                }
                else
                {
                    string jsontosend = "{\"id\":78,\"method\":\"setTextForm\",\"version\":\"1.0\",\"params\":[\"" + sendtext + "\"]}";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.Device_IP_Address + "/sony/appControl");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(this.Cookie);
                    httpWebRequest.CookieContainer.Add(new Uri(@"http://" + this.Device_IP_Address + bal[0].Path.ToString()), new Cookie(bal[0].Name, bal[0].Value));
                    using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWriter.Write(jsontosend);
                    }
                    var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        response += responseText + "\r\n";
                    }
                }
            return response;
            }
            #endregion

            #region Get Text command

            /// <summary>
            /// This method Gets Text
            /// </summary>
            /// <returns>Returns the device response as a string</returns>
            public string get_text()
            {
                string response = HttpGet(this.Actionlist.getText);
                return response;
            }
            #endregion

            #region Convert mdf commands

            private string send_ircc_mdf(Int32 manu, Int32 device, Int32 function)
            {
                // Join each as a char and remove any /n
                Int32 zero = 0;
                Int32 three = 3;
                string[] payload = new string[12];
                payload[0] = zero.ToString();
                payload[1] = zero.ToString();
                payload[2] = zero.ToString();
                payload[3] = manu.ToString();
                payload[4] = zero.ToString();
                payload[5] = zero.ToString();
                payload[6] = zero.ToString();
                payload[7] = device.ToString();
                payload[8] = zero.ToString();
                payload[9] = zero.ToString();
                payload[10] = zero.ToString();
                payload[11] = function.ToString();
                payload[12] = three.ToString();
                byte[] pL = payload.Select(s => Convert.ToByte(s, 64)).ToArray();
                string pLEncoded = pL.ToString();
                //string[] sendarg = new string[10];
                //pLEncoded = "irrcCode:" + pLEncoded;
                return pLEncoded;
                //this.execute("IRCC:1","X_SendIRCC",pLEncoded);
            }
            #endregion

            #region mdf commands    // Not sure what this does. Have not tested yet
            /// <summary>
            /// Sends a mdf command for input HDMI 1
            /// </summary>
            public void hdmi_1()
            {
                send_ircc_mdf(2, 26, 90);
            }

            /// <summary>
            /// Sends a mdf command for input HDMI 2
            /// </summary>
            public void hdmi_2()
            {
                send_ircc_mdf(2, 26, 91);
            }

            /// <summary>
            /// Sends a mdf command for input HDMI 3
            /// </summary>
            public void hdmi_3()
            {
                send_ircc_mdf(2, 26, 92);
            }

            /// <summary>
            /// Sends a mdf command for input HDMI 4
            /// </summary>
            public void hdmi_4()
            {
                send_ircc_mdf(2, 26, 93);
            }
            #endregion

            #region Volume Up
            /// <summary>
            /// Sends IRCC value for Volume Up
            /// </summary>
            public void volume_up()
            {
                //this.send_ircc(getIRCCcommandString("VolumeUp"));
            }
            #endregion

            #region Volume Down
            /// <summary>
            /// Sends IRCC value for Volume Down
            /// </summary>
            public void volume_down()
            {
                //this.send_ircc(getIRCCcommandString("VolumeDown"));
            }
            #endregion

            #region Channel Up
            /// <summary>
            /// Sends IRCC value for Channel Up
            /// </summary>
            public void channel_up()
            {
                //this.send_ircc(getIRCCcommandString("ChannelUp"));
            }
            #endregion

            #region Channel Down
            /// <summary>
            /// Sends IRCC value for Channel Down
            /// </summary>
            public void channel_down()
            {
                //this.send_ircc(getIRCCcommandString("ChannelDown"));
            }
            #endregion

            #region Channel Set
            /// <summary>
            /// This public method can be used to tune to a complete channel.
            /// </summary>
            /// <param name="channel">A string containing a valid complete channel. (ie. 47.1)</param>
            public void channel_set(string channel)
            {
                int chlen = channel.Length;
                string ircc_cmd = "";
                for (int i = 0; i <= chlen - 1; i++)
                {
                    string chchar = channel.Substring(i, 1);
                    switch (chchar)
                    {
                        case "1":
                            ircc_cmd = getIRCCcommandString("Num1");
                            break;
                        case "2":
                            ircc_cmd = getIRCCcommandString("Num2");
                            break;
                        case "3":
                            ircc_cmd = getIRCCcommandString("Num3");
                            break;
                        case "4":
                            ircc_cmd = getIRCCcommandString("Num4");
                            break;
                        case "5":
                            ircc_cmd = getIRCCcommandString("Num5");
                            break;
                        case "6":
                            ircc_cmd = getIRCCcommandString("Num6");
                            break;
                        case "7":
                            ircc_cmd = getIRCCcommandString("Num7");
                            break;
                        case "8":
                            ircc_cmd = getIRCCcommandString("Num8");
                            break;
                        case "9":
                            ircc_cmd = getIRCCcommandString("Num9");
                            break;
                        case "0":
                            ircc_cmd = getIRCCcommandString("Num0");
                            break;
                        case ".":
                            ircc_cmd = getIRCCcommandString("DOT");
                            break;
                    }
                    //this.execute("IRCC","X_SendIRCC", ircc_cmd);
                    System.Threading.Thread.Sleep(250);
                }
            }
            #endregion

            #region Check for full path
            /// <summary>
            /// If Param is NOT full path, returns the Full path. 
            /// Example: /cers/IRCC is Not a full path, will return: HTTP://192.168.0.xxx:8080/cers/IRCC
            /// </summary>
            /// <param name="cpath"> Path to check</param>
            /// <returns></returns>
            public string checkFullPath(string cpath)
            {
                if (cpath.StartsWith("http://"))
                {
                    //Full path already exist
                }
                else
                {
                    //Complete the Path
                    cpath = "http://" + this.Device_IP_Address + ":" + this.Device_Port + cpath ;
                }
                return cpath;
            }
            #endregion

            #region getServerMac

            /// <summary>
            /// getControlMac retrieves the MAC address from the static method GetMacAddress().
            /// </summary>
            /// <returns>A string containing a processed MAC address. 
            /// For example: Actual Mac 01:02:03:04:05:06 returns 01-02-03-04-05-06</returns>
            public string getServerMac()
            {
                _Log.writetolog("Retrieving Controlling devices Mac Address. (this computer)", false);
                string mac = GetMacAddress();
                _Log.writetolog("Mac Address retrieved: " + mac, false);
                _Log.writetolog("Re-Parsing Mac Address. (Replace : with -)", false);
                string new_mac = "";
                int i = mac.Length;
                int y = 1;
                int z;
                for (z = 1; z <= i; z++)
                {
                    new_mac += mac.Substring(z - 1, 1);
                    if (y == 2)
                    {
                        if (z < i)
                        {
                            new_mac += "-";
                            y = 0;
                        }
                    }
                    y = y + 1;
                }
                mac = new_mac;
                _Log.writetolog("Mac Address has been re-Parsed: " + mac, true);
                return mac;
            }
            #endregion

            #region GetMacAddress

            /// <summary>
            /// Static method used to obtain your NIC MAC address.
            /// </summary>
            /// <returns>A string containing the MAC address of the fastest NIC found in your system.</returns>
            /// <remarks>Should not be used publically. Use the getControlMac() method instead.</remarks>
            static string GetMacAddress()
            {
                const int MIN_MAC_ADDR_LENGTH = 12;
                string macAddress = string.Empty;
                long maxSpeed = -1;

                foreach (NetworkInterface nic in NetworkInterface.GetAllNetworkInterfaces())
                {
                    string tempMac = nic.GetPhysicalAddress().ToString();
                    if (nic.Speed > maxSpeed &&
                        !string.IsNullOrEmpty(tempMac) &&
                        tempMac.Length >= MIN_MAC_ADDR_LENGTH)
                    {
                        maxSpeed = nic.Speed;
                        macAddress = tempMac;
                    }
                }
                return macAddress;
            }
            #endregion

            #region Get Device Mac
            /// <summary>
            /// Method used to retrieve Gen3 Devices Mac Address
            /// </summary>
            /// <returns></returns>
            public string getDeviceMac(SonyDevice mDev)
            {
                String macaddress = "";
                _Log.writetolog("Retrieving the Mac Address from: " + mDev.Name + " at IP: " + mDev.Device_IP_Address, true);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + mDev.Device_IP_Address + @"/sony/system");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
                SonyCommandList dataSet = new SonyCommandList();
                using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    string json = "{\"id\":19,\"method\":\"getSystemSupportedFunction\",\"version\":\"1.0\",\"params\":[]}\"";
                    streamWriter.Write(json);
                }
                var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                {
                    var responseText = streamReader.ReadToEnd();
                    dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                }
                string first = dataSet.result[0].ToString();
                List<SonyOption> bal = JsonConvert.DeserializeObject<List<SonyOption>>(first);
                macaddress = bal.Find(x => x.option.ToLower() == "WOL".ToLower()).value.ToString();
                _Log.writetolog("Devices Mac Address: " + macaddress, true);
                return macaddress;
            }

            #endregion

            #region Build From Document

            /// <summary>
            /// Builds a SonyDevice object based on the devices Description document URL.
            /// </summary>
            /// <param name="dPath">A URI containg the URL to the device's Description XML file.</param>
            /// <returns>A Fully built and populated Sony Device</returns>
            public void buildFromDocument(Uri dPath)
            {
                this.DocumentURL = dPath.ToString();
                _Log.writetolog("Retrieving Device Description Document from URI:", false);
                _Log.writetolog(dPath.ToString(), false);
                XDocument dDoc = XDocument.Load(dPath.ToString());
                this.buildFromDocument(dDoc.Root.Document.ToString(), dPath.ToString());
            }

            /// <summary>
            /// Builds a SonyDevice object based on the devices Description document text.
            /// </summary>
            /// <param name="dDoc">A string containg the Description XML.</param>
            /// <param name="fPath">A string containg the Full Path to the device's Description XML file.</param>
            /// <returns>A Fully built and populated Sony Device</returns>
            public void buildFromDocument(string dDoc, string fPath)
            {
                this.DocumentURL = fPath;
                Uri d = new Uri(fPath);
                this.Device_IP_Address = d.Host;
                this.Device_Port = d.Port;
                this.Server_Macaddress = this.getServerMac();
                this.Server_Name = System.Windows.Forms.SystemInformation.ComputerName + "(SonyAPILib)";
                XmlDocument xDoc = new XmlDocument();
                xDoc.LoadXml(dDoc);
                XmlNode xNode = xDoc.DocumentElement.ChildNodes[1];
                _Log.writetolog("Document Found", false);
                foreach (XmlNode node in xNode.ChildNodes)
                {
                    if (node.Name == "av:X_UNR_DeviceInfo")
                    {
                        foreach (XmlNode dserv in node.ChildNodes)
                        {
                            if (dserv.Name == "av:X_CERS_ActionList_URL")
                            {
                                _Log.writetolog("Action List Found.", false);
                                string alPath = dserv.FirstChild.InnerText;
                                this.Actionlist_Url = alPath;
                                DataSet acList = new DataSet();
                                acList.ReadXml(alPath);
                                DataTable act = new DataTable();
                                act = acList.Tables[0];
                                var results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "register" select myRow;
                                this.Actionlist.RegisterMode = Convert.ToInt16(results.ElementAt(0).ItemArray[1].ToString());
                                _Log.writetolog("Device has a registration Mode of: " + this.Actionlist.RegisterMode.ToString(), false);
                                this.Actionlist.RegisterUrl = this.checkFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getSystemInformation" select myRow;
                                this.Actionlist.getSystemInformation = this.checkFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getRemoteCommandList" select myRow;
                                this.Actionlist.getRemoteCommandList = this.checkFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                this.get_remote_command_list();
                                results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getStatus" select myRow;
                                this.Actionlist.getStatus = this.checkFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getText" select myRow;
                                this.Actionlist.getText = this.checkFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "sendText" select myRow;
                                this.Actionlist.sendText = this.checkFullPath(results.ElementAt(0).ItemArray[2].ToString());
                            }
                        }
                    }
                    if (node.Name == "av:X_TrackID_DeviceInfo")
                    {
                        this.Actionlist.RegisterMode = 3;

                        _Log.writetolog("Device has a registration Mode of: " + this.Actionlist.RegisterMode.ToString(), false);
                        this.Device_Macaddress = this.getDeviceMac(this);
                        this.get_remote_command_list();
                    }
                    if (node.Name == "friendlyName") { this.Name = node.FirstChild.InnerText; }
                    if (node.Name == "manufacturer") { this.Manufacture = node.FirstChild.InnerText; }
                    if (node.Name == "modelDescription") { this.ModelDescription = node.FirstChild.InnerText; }
                    if (node.Name == "modelName") { this.ModelName = node.FirstChild.InnerText; }
                    if (node.Name == "modelNumber") { this.ModelNumber = node.FirstChild.InnerText; }
                    if (node.Name == "UDN") { this.UDN = node.FirstChild.InnerText; }
                    if (node.Name == "deviceType") { this.DeviceType = node.FirstChild.InnerText; }
                    if (node.Name == "serviceList")
                    {
                        foreach (XmlNode cnode in node.ChildNodes)
                        {
                            deviceService dServ = new deviceService();
                            foreach (XmlNode dserv in cnode.ChildNodes)
                            {
                                if (dserv.Name == "serviceType")
                                {
                                    
                                    dServ.serviceType = dserv.InnerText;
                                    dServ.friendlyServiceIdentifier = dServ.serviceType.ChopOffBefore("service:");
                                }
                                if (dserv.Name == "serviceId") { dServ.serviceID = dserv.InnerText; }
                                if (dserv.Name == "SCPDURL") { dServ.SCPDURL = this.checkFullPath(dserv.InnerText); }
                                if (dserv.Name == "controlURL") { dServ.controlURL = this.checkFullPath(dserv.InnerText); }
                                if (dserv.Name == "eventSubURL")
                                {
                                    if (dserv.InnerText != "")
                                    {
                                        dServ.eventSubURL = this.checkFullPath(dserv.InnerText);
                                    }
                                }
                            }
                            if (dServ.friendlyServiceIdentifier == "IRCC:1")
                            {
                                _Log.writetolog("IRCC:1 Service discovered on this device", false);
                                this.IRCC.controlURL = dServ.controlURL;
                                this.IRCC.SCPDURL = dServ.SCPDURL;
                                this.IRCC.eventSubURL = dServ.eventSubURL;
                                this.IRCC.serviceID = dServ.serviceID;
                                this.IRCC.friendlyServiceIdentifier = dServ.friendlyServiceIdentifier;
                                this.IRCC.serviceType = dServ.serviceType;
                            }
                            if (dServ.friendlyServiceIdentifier == "AVTransport:1")
                            {
                                _Log.writetolog("AVTransport:1 Service discovered on this device", false);
                                this.AVTransport.controlURL = dServ.controlURL;
                                this.AVTransport.SCPDURL = dServ.SCPDURL;
                                this.AVTransport.eventSubURL = dServ.eventSubURL;
                                this.AVTransport.serviceID = dServ.serviceID;
                                this.AVTransport.friendlyServiceIdentifier = dServ.friendlyServiceIdentifier;
                                this.AVTransport.serviceType = dServ.serviceType;
                            }
                            if (dServ.friendlyServiceIdentifier == "RenderingControl:1")
                            {
                                _Log.writetolog("RenderingControl:1 Service discovered on this device", false);
                                this.RenderingControl.controlURL = dServ.controlURL;
                                this.RenderingControl.SCPDURL = dServ.SCPDURL;
                                this.RenderingControl.eventSubURL = dServ.eventSubURL;
                                this.RenderingControl.serviceID = dServ.serviceID;
                                this.RenderingControl.friendlyServiceIdentifier = dServ.friendlyServiceIdentifier;
                                this.RenderingControl.serviceType = dServ.serviceType;
                            }
                            if (dServ.friendlyServiceIdentifier == "ConnectionManager:1")
                            {
                                _Log.writetolog("ConnectionManager:1 Service discovered on this device", false);
                                this.ConnectionManager.controlURL = dServ.controlURL;
                                this.ConnectionManager.SCPDURL = dServ.SCPDURL;
                                this.ConnectionManager.eventSubURL = dServ.eventSubURL;
                                this.ConnectionManager.serviceID = dServ.serviceID;
                                this.ConnectionManager.friendlyServiceIdentifier = dServ.friendlyServiceIdentifier;
                                this.ConnectionManager.serviceType = dServ.serviceType;
                            }
                            if (dServ.friendlyServiceIdentifier == "Party:1")
                            {
                                _Log.writetolog("Party:1 Service discovered on this device", false);
                                this.Party.controlURL = dServ.controlURL;
                                this.Party.SCPDURL = dServ.SCPDURL;
                                this.Party.eventSubURL = dServ.eventSubURL;
                                this.Party.serviceID = dServ.serviceID;
                                this.Party.friendlyServiceIdentifier = dServ.friendlyServiceIdentifier;
                                this.Party.serviceType = dServ.serviceType;
                            }
                        }
                    }
                }
                this.checkReg();
            }

            #endregion

            #region Wake On Lan

            /// <summary>
            /// Sends the device a "Wake-On-Lan" command
            /// </summary>
            public void WOL()
            {
                if (this.Actionlist.RegisterMode == 3)
                {
                    _Log.writetolog("Sending Wake On Lan command to device", false);
                    Byte[] datagram = new byte[102];
                    for (int i = 0; i <= 5; i++)
                    {
                        datagram[i] = 0xff;
                    }
                    string[] macDigits = null;
                    if (this.Device_Macaddress.Contains("-"))
                    {
                        macDigits = this.Device_Macaddress.Split('-');
                    }
                    else
                    {
                        macDigits = this.Device_Macaddress.Split(':');
                    }
                    if (macDigits.Length != 6)
                    {
                        throw new ArgumentException("Incorrect MAC address supplied!");
                    }
                    int start = 6;
                    for (int i = 0; i < 16; i++)
                    {
                        for (int x = 0; x < 6; x++)
                        {
                            datagram[start + i * 6 + x] = (byte)Convert.ToInt32(macDigits[x], 16);
                        }
                    }
                    UdpClient client = new UdpClient();
                    client.Send(datagram, datagram.Length, "255.255.255.255", 3);
                    _Log.writetolog("Send WOL command to " + this.Name, true);
                }
                else
                {
                    _Log.writetolog("Device does not support WOL", true);
                }
            }
            #endregion

        }
        #endregion

        #region Logging
        /// <summary>
        /// Sony Device Logging Class
        /// Very Basic Logging System to txt file.
        /// </summary>
        public partial class APILogging
        {
            #region Public Variables
            /// <summary>
            /// Gets or Sets Enabling cerDevice API Logging
            /// True - Turns Loggin On
            /// False - Turns Loggin Off
            /// </summary>
            public bool enableLogging 
            {
                get
                {
                    return _logging;
                }
                set
                {
                    _logging = value;
                }
            }

            /// <summary>
            /// Gets or Sets Enabling cerDevice API Logging Level
            /// Basic - for only basic entries
            /// All - for all entries
            /// </summary>
            public string enableLogginglev
            {
                get
                {
                    return _loglev;
                }
                set
                {
                    _loglev = value;
                }
            }

            /// <summary>
            /// Gets or Sets the cerDevice API logging path
            /// Destination Folder MUST exist.
            /// Must be Full Path. ex: c:\programdata\sony\
            /// </summary>
            public string loggingPath
            {
                get
                {
                    return _logpath;
                }
                set
                {
                    _logpath = value;
                }
            }

            /// <summary>
            /// Gets or Sets the cerDevice API logging file name
            /// Must be a .txt file
            /// default is cerDevice_LOG.txt
            /// </summary>
            public string loggingName
            {
                get
                {
                    return _logname;
                }
                set
                {
                    _logname = value;
                }
            }
            #endregion

            #region Private Variables

            // Private Variables
            private bool _logging = false;
            private string _loglev = "Basic";
            private string _logname = @"SonyAPILib_LOG.txt";
            private string _logpath = @"c:\ProgramData\Sony\";
            #endregion

            #region Write to Log File
            /// <summary>
            /// This method writes the log entries to the specified file location.
            /// </summary>
            /// <param name="message">This is the Text message to be added to the log file</param>
            /// <param name="oride">Set to true to ALWAYS log this message. Otherwise set to false</param>
            public void writetolog(string message, bool oride)
            {
                if (this.loggingPath == null | this.loggingPath == "") { this.loggingPath = @"c:\ProgramData\Sony\"; }
                Directory.CreateDirectory(this.loggingPath);
                if (this.loggingName == null | this.loggingName == "") { this.loggingName = @"SonyAPILib_LOG.txt"; }
                if (this.enableLogginglev == null | this.enableLogginglev == "") { this.enableLogginglev = "Basic"; }
                string logPath = this.loggingPath + this.loggingName;
                if (enableLogging == true)
                {
                    if (this.enableLogginglev == "Basic")
                    {
                        if (oride == true)
                        {
                            message = DateTime.Now.ToString() + " - " + message + Environment.NewLine;
                            File.AppendAllText(logPath, message, Encoding.UTF8);
                        }
                    }
                    else
                    {
                        message = DateTime.Now.ToString() + " - " + message + Environment.NewLine;
                        File.AppendAllText(logPath, message, Encoding.UTF8);
                    }
                }
            }
            #endregion

            #region Clear Log File
            /// <summary>
            /// This method is used to Clear the current log and start a new.
            /// </summary>
            /// <param name="newName">Default is Null. If NOT Null, Log file is copied to newName before it is Cleared!</param>
            public void clearLog(string newName)
            {
                if (newName != null)
                {
                    File.Copy(@loggingPath + loggingName, @loggingPath + newName);
                    File.Delete(@loggingPath + loggingName);
                    writetolog("Saving Log file as: " + @loggingPath + newName, true);
                }
                else
                {
                    File.Delete(@loggingPath + loggingName);
                    writetolog("Clearing Log file: " + @loggingPath + loggingName, true);
                }

            }
            #endregion
        }
        #endregion

        #region Sony Command List
        /// <summary>
        /// Gets or Sets the Sony Command List Object
        /// </summary>
        [Serializable]
        private class SonyCommandList
        {
            /// <summary>
            /// Gets or Sets the Devices Command List ID
            /// </summary>
            public int id { get; set; }
            /// <summary>
            /// Gets or Sets the Devices Command List Results
            /// </summary>
            public List<object> result { get; set; }
        }
        #endregion

        #region Sony Command
        /// <summary>
        /// Gets or Sets the Sony Device Command
        /// </summary>
        [Serializable]
        public class SonyCommands
        {
            /// <summary>
            /// Gets or Sets the Devices Command Name
            /// </summary>
            public string name { get; set; }
            /// <summary>
            /// Gets or Sets the Devices Command Value
            /// </summary>
            public string value { get; set; }
        }
        #endregion

        #region Sony Option
        /// <summary>
        /// Gets or Sets the Sony Option object
        /// </summary>
        private class SonyOption
        {
            /// <summary>
            /// Gets or Sets the Devices Option Name
            /// </summary>
            public string option { get; set; }
            /// <summary>
            /// Gets or Sets the Devices Option Value
            /// </summary>
            public string value { get; set; }
        }
        #endregion

        #region Sony Cookie
        /// <summary>
        /// Gets or Sets the Sony Device Cookie Container Object
        /// </summary>
        [Serializable]
        private class SonyCookie
        {
            /// <summary>
            /// Gets or Sets the Cookie Comment
            /// </summary>
            public string Comment { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Comment URI
            /// </summary>
            public object CommentUri { get; set; }
            /// <summary>
            /// Gets or Sets Cookie for HTTP Only
            /// </summary>
            public bool HttpOnly { get; set; }
            /// <summary>
            /// gets or Sets the Cookie Discard
            /// </summary>
            public bool Discard { get; set; }
            /// <summary>
            /// gets or Sets the Cookie Domain
            /// </summary>
            public string Domain { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Expired
            /// </summary>
            public bool Expired { get; set; }
            /// <summary>
            /// Gets or Sets the Cookies Expiration
            /// </summary>
            public string Expires { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Path
            /// </summary>
            public string Path { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Port
            /// </summary>
            public string Port { get; set; }
            /// <summary>
            /// Gets or Sets the Is Cookie Secure
            /// </summary>
            public bool Secure { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Time Stamp
            /// </summary>
            public string TimeStamp { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Value
            /// </summary>
            public string Value { get; set; }
            /// <summary>
            /// Gets or Sets the Cookie Version
            /// </summary>
            public int Version { get; set; }
        }
        #endregion

        #region HelperDLNA
        /// <summary>
        /// DLNA Helper Communication class
        /// </summary>

            public static class HelperDLNA
            {
                /// <summary>
                /// Makes a new request to the device for sending SOAP.
                /// </summary>
                /// <param name="Methord"></param>
                /// <param name="Url"></param>
                /// <param name="ContentLength"></param>
                /// <param name="SOAPAction"></param>
                /// <param name="IP"></param>
                /// <param name="Port"></param>
                /// <returns></returns>
                public static string MakeRequest(string Methord, string Url, int ContentLength, string SOAPAction, string IP, int Port)
                {//Make a request that is sent out to the DLNA server on the LAN using TCP
                    string R = Methord.ToUpper() + " /" + Url + " HTTP/1.1" + Environment.NewLine;
                    R += "Cache-Control: no-cache" + Environment.NewLine;
                    R += "Connection: Close" + Environment.NewLine;
                    R += "Pragma: no-cache" + Environment.NewLine;
                    R += "Host: " + IP + ":" + Port + Environment.NewLine;
                    R += "User-Agent: Microsoft-Windows/6.3 UPnP/1.0 Microsoft-DLNA DLNADOC/1.50" + Environment.NewLine;
                    //R += "FriendlyName.DLNA.ORG: " + Environment.MachineName + Environment.NewLine;
                    if (ContentLength > 0)
                    {
                        R += "Content-Length: " + ContentLength + Environment.NewLine;
                        R += "Content-Type: text/xml; charset=\"utf-8\"" + Environment.NewLine;
                    }
                    if (SOAPAction.Length > 0)
                        R += "SOAPAction: \"" + SOAPAction + "\"" + Environment.NewLine;
                    return R + Environment.NewLine;
                }


                /// <summary>
                /// Creates a new Socket for communications
                /// </summary>
                /// <param name="ip"></param>
                /// <param name="port"></param>
                /// <returns></returns>
                public static Socket MakeSocket(string ip, int port)
                {//Just returns a TCP socket ready to use
                    IPEndPoint IPWeb = new IPEndPoint(IPAddress.Parse(ip), port);
                    Socket SocWeb = new Socket(IPWeb.Address.AddressFamily, SocketType.Stream, ProtocolType.Tcp);
                    SocWeb.ReceiveTimeout = 6000;
                    SocWeb.Connect(IPWeb);
                    return SocWeb;
                }

                /// <summary>
                /// reads the Sockets response information
                /// </summary>
                /// <param name="Soc"></param>
                /// <param name="CloseOnExit"></param>
                /// <param name="ReturnCode"></param>
                /// <returns></returns>
                public static string ReadSocket(Socket Soc, bool CloseOnExit, ref int ReturnCode)
                {//We have some data to read on the socket 
                    ReturnCode = 0;
                    int ContentLength = 0;
                    int HeadLength = 0;
                    Thread.Sleep(300);
                    MemoryStream MS = new MemoryStream();
                    byte[] buffer = new byte[8000];
                    int Count = 0;
                    while (Count < 8)
                    {
                        Count++;
                        if (Soc.Available > 0)
                        {
                            int Size = Soc.Receive(buffer);
                            string Head = UTF8Encoding.UTF32.GetString(buffer).ToLower();
                            if (ContentLength == 0 && Head.IndexOf(Environment.NewLine + Environment.NewLine) > -1 && Head.IndexOf("content-length:") > -1)
                            {//We have a contant length so we can test if we have all the page data.
                                HeadLength = Head.LastIndexOf(Environment.NewLine + Environment.NewLine);
                                string StrCL = Head.ChopOffAfter("content-length:").ChopOffAfter(Environment.NewLine);
                                int.TryParse(StrCL, out ContentLength);
                            }
                            MS.Write(buffer, 0, Size);
                            if (ContentLength > 0 && MS.Length >= HeadLength + ContentLength)
                            {
                                if (CloseOnExit) Soc.Close();
                                return UTF8Encoding.UTF8.GetString(MS.ToArray());
                            }
                        }
                        Thread.Sleep(200);
                    }
                    if (CloseOnExit) Soc.Close();
                    string HTML = UTF8Encoding.UTF8.GetString(MS.ToArray());
                    string Code = HTML.ChopOffBefore("HTTP/1.1").Trim().ChopOffAfter(" ");
                    int.TryParse(Code, out ReturnCode);
                    return HTML;
                }
            }
            #endregion

        #region Services

            #region Rendering Control

            /// <summary>
            /// Service class for the RenderingControl1 (urn:schemas-upnp-org:service:RenderingControl:1) service.
            /// </summary>
            [Serializable]
            public partial class RenderingControl1
            {
           
                #region Public Constants

                /// <summary>
                /// Gets the service type identifier for the RenderingControl1 service.
                /// </summary>
                public const string ServiceType = "urn:schemas-upnp-org:service:RenderingControl:1";

                /// <summary>
                /// Gets the service instanceID for the RenderingControl1 service.
                /// </summary>
                public const int InstanceID = 0;

                /// <summary>
                /// Gets the service type identifier for the RenderingControl1 service.
                /// </summary>
                public const string Channel = "Master";
                #endregion

                #region Initialisation

                

                #endregion

                #region Event Handlers


                #endregion

                #region Event Callers

            

                #endregion

                #region Public Methods

                #region List Presets
                /// <summary>
                /// Executes the ListPresets action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Information from.</param>
                /// <returns>Out value for the CurrentPresetNameList action parameter.</returns>
                public String ListPresets(SonyDevice parent)
                {
                    if (parent.RenderingControl.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:ListPresets xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "</m:ListPresets>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.RenderingControl.controlURL.IndexOf(":") + 3;
                        string req = parent.RenderingControl.controlURL.Substring(reqi);
                        reqi = parent.RenderingControl.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#ListPresets", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        string ret = "";
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            ret += node.InnerText;
                        }
                        parent.RenderingControl.sv_LastChange = "ListPresets: " + ret;
                        parent.RenderingControl.sv_PresetNameList = ret;
                        return ret;
                    }
                    return null;
                }
                #endregion

                #region Select Preset
                /// <summary>
                /// Executes the SelectPreset action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="presetName">In value for the PresetName action parameter.</param>
                public void SelectPreset(SonyAPI_Lib.SonyDevice parent, string presetName)
                {
                //    object[] loIn = new object[2];

    //                loIn[0] = instanceID;
    //                loIn[1] = ToStringAARGTYPEPresetName(presetName);
    //                InvokeAction(csAction_SelectPreset, loIn);

                }
                #endregion

                #region Get Mute
                /// <summary>
                /// Executes the GetMute action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                /// <returns>Boolean value for the CurrentMute action parameter.</returns>
                public Boolean GetMute(SonyDevice parent)
                {
                    if (parent.RenderingControl.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetMute xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "</m:GetMute>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.RenderingControl.controlURL.IndexOf(":") + 3;
                        string req = parent.RenderingControl.controlURL.Substring(reqi);
                        reqi = parent.RenderingControl.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#GetMute", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;

                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        Boolean ret = false;
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            if (node.InnerText == "1")
                            {
                                ret = true;
                            }
                        }
                        parent.RenderingControl.sv_LastChange = "GetMute: " + ret.ToString();
                        parent.RenderingControl.sv_Mute = ret;
                        return ret;
                    }
                    return false;
                }
                #endregion

                #region Set Mute
                /// <summary>
                /// Executes the SetMute action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                /// <param name="desiredMute">In value for the DesiredMute action parameter.</param>
                public void SetMute(SonyDevice parent, Boolean desiredMute)
                {
                    if (parent.RenderingControl.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        int sMute = 0;
                        if (desiredMute == true)
                        {
                            sMute = 1;
                        }
                        XML += "<m:SetMute xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "<DesiredMute xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"boolean\">" + sMute.ToString() + "</DesiredMute>" + Environment.NewLine;
                        XML += "</m:SetMute>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.RenderingControl.controlURL.IndexOf(":") + 3;
                        string req = parent.RenderingControl.controlURL.Substring(reqi);
                        reqi = parent.RenderingControl.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#SetMute", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        parent.RenderingControl.sv_LastChange = "SetMute: " + desiredMute;
                        parent.RenderingControl.sv_Mute = desiredMute;
                        parent.RenderingControl.sv_Channel = "Master";
                    }
                }
                #endregion

                #region Get Volume
                /// <summary>
                /// Executes the GetVolume action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                /// <returns>Out value for the CurrentVolume action parameter. With range of 0 to 100. Increment of 1.</returns>
                public int GetVolume(SonyDevice parent)
                {
                    if (parent.RenderingControl.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetVolume xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "</m:GetVolume>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.RenderingControl.controlURL.IndexOf(":") + 3;
                        string req = parent.RenderingControl.controlURL.Substring(reqi);
                        reqi = parent.RenderingControl.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#GetVolume", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        int ret = 0;
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            if (node.InnerText != "0")
                            {
                                ret = Convert.ToInt32(node.InnerText);
                            }
                        }
                        parent.RenderingControl.sv_LastChange = "GetVolume: " + ret.ToString();
                        parent.RenderingControl.sv_Volume = ret;
                        parent.RenderingControl.sv_Channel = "Master";
                        return ret;
                    }
                    return 0;
                }
                #endregion

                #region Set Volume
                /// <summary>
                /// Executes the SetVolume action.
                /// </summary>
                /// <param name="parent">Parent Device object to Set Volume on.</param>
                /// <param name="desiredVolume">In value for the DesiredVolume action parameter. With range of 0 to 100. Increment of 1.</param>
                public void SetVolume(SonyDevice parent, int desiredVolume)
                {
                    if (parent.RenderingControl.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:SetVolume xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "<DesiredVolume xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui2\">" + desiredVolume.ToString() + "</DesiredVolume>" + Environment.NewLine;
                        XML += "</m:SetVolume>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.RenderingControl.controlURL.IndexOf(":") + 3;
                        string req = parent.RenderingControl.controlURL.Substring(reqi);
                        reqi = parent.RenderingControl.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#SetVolume", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        parent.RenderingControl.sv_LastChange = "SetVolume: " + desiredVolume;
                        parent.RenderingControl.sv_Volume = desiredVolume;
                        parent.RenderingControl.sv_Channel = "Master";
                    }
                }
                #endregion

                #endregion

                #region Public Properties
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;

                #endregion
            }

            #endregion

            #region Connection Manager

            /// <summary>
            /// Service class for the ConnectionManager1 (urn:schemas-upnp-org:service:ConnectionManager:1) service.
            /// </summary>
            [Serializable]
            public partial class ConnectionManager1
            {
                #region Public Constants

                /// <summary>
                /// Gets the service type identifier for the ConnectionManager1 service.
                /// </summary>
                public const string ServiceType = "urn:schemas-upnp-org:service:ConnectionManager:1";

                #endregion

                #region Initialisation

                

                #endregion

                #region Event Handlers

           
                #endregion

                #region Event Callers

            

                #endregion

                #region Public Methods

                #region Get Protocol Information
                /// <summary>
                /// Executes the GetProtocolInfo action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                /// <remarks>Populates the following State Variables: Sink and Source</remarks>
               public void GetProtocolInfo(SonyDevice parent)
               {
                   if (parent.ConnectionManager.controlURL != null)
                   {
                       string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                       string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                       string XML = XMLHead;
                       XML += "<m:GetProtocolInfo xmlns:m=\"urn:schemas-upnp-org:service:ConnectionManager:1\">" + Environment.NewLine;
                       XML += "</m:GetProtocolInfo>" + XMLFoot + Environment.NewLine;
                       Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                       int reqi = parent.ConnectionManager.controlURL.IndexOf(":") + 3;
                       string req = parent.ConnectionManager.controlURL.Substring(reqi);
                       reqi = parent.ConnectionManager.controlURL.IndexOf(":") + 1;
                       req = req.Substring(reqi);
                       reqi = req.IndexOf("/") + 1;
                       req = req.Substring(reqi);
                       string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:ConnectionManager:1#GetProtocolInfo", parent.Device_IP_Address, parent.Device_Port) + XML;
                       SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                       string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                       GG = Extentions.ChopOffBefore(GG, "<?xml");
                       GG = "<?xml" + GG;
                       XmlDocument xDoc = new XmlDocument();
                       xDoc.LoadXml(GG);
                       XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                       foreach (XmlNode node in xNode.ChildNodes)
                       {
                           foreach (XmlNode snode in node.ChildNodes)
                           {
                               if (snode.Name == "Source") { parent.ConnectionManager.sv_ProticolSource = snode.InnerText; }
                               if (snode.Name == "Sink") { parent.ConnectionManager.sv_ProticolSink = snode.InnerText; }
                           }
                       }
                       parent.ConnectionManager.sv_LastChange = "GetProtocolInfo";
                   }
                }
                #endregion

                #region Get Current Connection IDs
                /// <summary>
                /// Executes the GetCurrentConnectionIDs action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                /// <returns>Out value for the ConnectionIDs action parameter.</returns>
                public int GetCurrentConnectionIDs(SonyDevice parent)
                {
                    if (parent.ConnectionManager.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetCurrentConnectionIDs xmlns:m=\"urn:schemas-upnp-org:service:ConnectionManager:1\">" + Environment.NewLine;
                        XML += "</m:GetCurrentConnectionIDs>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.RenderingControl.controlURL.IndexOf(":") + 3;
                        string req = parent.RenderingControl.controlURL.Substring(reqi);
                        reqi = parent.RenderingControl.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:ConnectionManager:1#GetCurrentConnectionIDs", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        int ret = 0;
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            if (node.InnerText != "")
                            {
                                ret = Convert.ToInt32(node.InnerText);
                            }
                        }
                        parent.ConnectionManager.sv_LastChange = "GetCurrentConnectionIDs: " + ret.ToString();
                        parent.ConnectionManager.sv_ConnectionID = ret;
                        return ret;
                    }
                    return 0;
                }
                #endregion

                #region Get Current ConnectionInformation
                /// <summary>
                /// Executes the GetCurrentConnectionInfo action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                public void GetCurrentConnectionInfo(SonyDevice parent)
                {
                    if (parent.ConnectionManager.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetCurrentConnectionInfo xmlns:m=\"urn:schemas-upnp-org:service:ConnectionManager:1\">" + Environment.NewLine;
                        XML += "<ConnectionID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + parent.ConnectionManager.sv_ConnectionID.ToString() + "</ConnectionID>" + Environment.NewLine;
                        XML += "</m:GetCurrentConnectionInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.ConnectionManager.controlURL.IndexOf(":") + 3;
                        string req = parent.ConnectionManager.controlURL.Substring(reqi);
                        reqi = parent.ConnectionManager.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:ConnectionManager:1#GetCurrentConnectionInfo", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            foreach (XmlNode snode in node.ChildNodes)
                            {
                                if (snode.Name == "RcsID") { parent.ConnectionManager.sv_RcsID = snode.InnerText; }
                                if (snode.Name == "AVTransportID") { parent.ConnectionManager.sv_AVTransportID = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "PeerConnectionManager") { parent.ConnectionManager.sv_ConnectionManager = snode.InnerText; }
                                if (snode.Name == "Status") { parent.ConnectionManager.sv_ConnectionStatus = snode.InnerText; }
                                if (snode.Name == "Direction") { parent.ConnectionManager.sv_Direction = snode.InnerText; }
                                if (snode.Name == "PeerConnectionID") { parent.ConnectionManager.sv_PeerConnectionID = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "ProtocolInfo") { parent.ConnectionManager.sv_ProtocolInfo = snode.InnerText; }
                                if (snode.Name == "RcsID") { parent.ConnectionManager.sv_Direction = snode.InnerText; }
                            }
                        }
                        parent.ConnectionManager.sv_LastChange = "GetCurrentConnectionInfo";
                    }
                }
                #endregion

                #endregion

                #region Public Properties
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;

                #endregion
            }

            #endregion

            #region AVTransport
       
            /// <summary>
            /// Service class for the AVTransport1 (urn:schemas-upnp-org:service:AVTransport:1) service.
            /// </summary>
            [Serializable]
            public partial class AVTransport1
            {
            
                #region Public Constants

                /// <summary>
                /// Gets the service type identifier for the AVTransport1 service.
                /// </summary>
                public const string ServiceType = "urn:schemas-upnp-org:service:AVTransport:1";

                /// <summary>
                /// Gets the service instanceID for the RenderingControl1 service.
                /// </summary>
                public const int InstanceID = 0;

                /// <summary>
                /// Gets the service type identifier for the RenderingControl1 service.
                /// </summary>
                public const string Channel = "Master";

                #endregion

                #region Public Enumerations

                /// <summary>
                /// Gets the enumeration type to hold a value for the TransportState state variable.
                /// </summary>
                public enum TransportStateEnum
                {
                    /// <summary>
                    /// Gets the TransportState state var 'STOPPED' value.
                    /// </summary>
                    STOPPED,

                    /// <summary>
                    /// Gets the TransportState state var 'PLAYING' value.
                    /// </summary>
                    PLAYING,

                    /// <summary>
                    /// Gets the TransportState state var 'PAUSEDPLAYBACK' value.
                    /// </summary>
                    PAUSEDPLAYBACK,

                    /// <summary>
                    /// Gets the TransportState state var 'TRANSITIONING' value.
                    /// </summary>
                    TRANSITIONING,

                    /// <summary>
                    /// Gets the TransportState state var 'NOMEDIAPRESENT' value.
                    /// </summary>
                    NOMEDIAPRESENT,

                    /// <summary>
                    /// Value describing an invalid or unknown TransportState value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the TransportStatus state variable.
                /// </summary>
                public enum TransportStatusEnum
                {
                    /// <summary>
                    /// Gets the TransportStatus state var 'OK' value.
                    /// </summary>
                    OK,

                    /// <summary>
                    /// Gets the TransportStatus state var 'ERROROCCURRED' value.
                    /// </summary>
                    ERROROCCURRED,

                    /// <summary>
                    /// Value describing an invalid or unknown TransportStatus value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the PlaybackStorageMedium state variable.
                /// </summary>
                public enum PlaybackStorageMediumEnum
                {
                    /// <summary>
                    /// Gets the PlaybackStorageMedium state var 'NETWORK' value.
                    /// </summary>
                    NETWORK,

                    /// <summary>
                    /// Value describing an invalid or unknown PlaybackStorageMedium value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the RecordStorageMedium state variable.
                /// </summary>
                public enum RecordStorageMediumEnum
                {
                    /// <summary>
                    /// Gets the RecordStorageMedium state var 'NOTIMPLEMENTED' value.
                    /// </summary>
                    NOTIMPLEMENTED,

                    /// <summary>
                    /// Value describing an invalid or unknown RecordStorageMedium value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the CurrentPlayMode state variable.
                /// </summary>
                public enum CurrentPlayModeEnum
                {
                    /// <summary>
                    /// Gets the CurrentPlayMode state var 'NORMAL' value.
                    /// </summary>
                    NORMAL,

                    /// <summary>
                    /// Gets the CurrentPlayMode state var 'RANDOM' value.
                    /// </summary>
                    RANDOM,

                    /// <summary>
                    /// Gets the CurrentPlayMode state var 'REPEATONE' value.
                    /// </summary>
                    REPEATONE,

                    /// <summary>
                    /// Gets the CurrentPlayMode state var 'REPEATALL' value.
                    /// </summary>
                    REPEATALL,

                    /// <summary>
                    /// Value describing an invalid or unknown CurrentPlayMode value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the TransportPlaySpeed state variable.
                /// </summary>
                public enum TransportPlaySpeedEnum
                {
                    /// <summary>
                    /// Gets the TransportPlaySpeed state var '_1' value.
                    /// </summary>
                    _1,

                    /// <summary>
                    /// Value describing an invalid or unknown TransportPlaySpeed value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the RecordMediumWriteStatus state variable.
                /// </summary>
                public enum RecordMediumWriteStatusEnum
                {
                    /// <summary>
                    /// Gets the RecordMediumWriteStatus state var 'NOTIMPLEMENTED' value.
                    /// </summary>
                    NOTIMPLEMENTED,

                    /// <summary>
                    /// Value describing an invalid or unknown RecordMediumWriteStatus value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the CurrentRecordQualityMode state variable.
                /// </summary>
                public enum CurrentRecordQualityModeEnum
                {
                    /// <summary>
                    /// Gets the CurrentRecordQualityMode state var 'NOTIMPLEMENTED' value.
                    /// </summary>
                    NOTIMPLEMENTED,

                    /// <summary>
                    /// Value describing an invalid or unknown CurrentRecordQualityMode value.
                    /// </summary>
                    _Unknown
                }

                /// <summary>
                /// Gets the enumeration type to hold a value for the AARGTYPESeekMode state variable.
                /// </summary>
                public enum AARGTYPESeekModeEnum
                {
                    /// <summary>
                    /// Gets the AARGTYPESeekMode state var 'TRACKNR' value.
                    /// </summary>
                    TRACKNR,

                    /// <summary>
                    /// Gets the AARGTYPESeekMode state var 'RELTIME' value.
                    /// </summary>
                    RELTIME,

                    /// <summary>
                    /// Value describing an invalid or unknown AARGTYPESeekMode value.
                    /// </summary>
                    _Unknown
                }

                #endregion

                #region Initialisation


                #endregion

                #region Event Handlers

              


                #endregion

                #region Event Callers

                

                #endregion

                #region Public Methods

                #region Set AVTransport URI
                /// <summary>
                /// Executes the SetAVTransportURI action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <param name="currentURI">In value for the CurrentURI action parameter.</param>
                /// <param name="currentURIMetaData">In value for the CurrentURIMetaData action parameter.</param>
                public string SetAVTransportURI(SonyDevice parent, String currentURI, String currentURIMetaData)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:SetAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">" + Environment.NewLine;
                        XML += "<InstanceID>" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<CurrentURI>" + currentURI.Replace(" ", "%20") + "</CurrentURI>" + Environment.NewLine;
                        XML += "<CurrentURIMetaData>" + currentURIMetaData + "</CurrentURIMetaData>" + Environment.NewLine;
                        XML += "</u:SetAVTransportURI>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#SetAVTransportURI", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.sv_LastChange = "SetAVTransportURI";
                        parent.AVTransport.sv_AVTransportURI = currentURI;
                        parent.AVTransport.sv_A_ARG_TYPE_InstanceID = (int)InstanceID;
                        return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    return null;
                }
                #endregion

                #region Set Next AVTransport URI
                /// <summary>
                /// Executes the SetNextAVTransportURI action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <param name="instanceID">In value for the InstanceID action parameter.</param>
                /// <param name="nextURI">In value for the NextURI action parameter.</param>
                /// <param name="nextURIMetaData">In value for the NextURIMetaData action parameter.</param>
                public string SetNextAVTransportURI(SonyDevice parent, String nextURI, String nextURIMetaData, UInt32 instanceID = 0)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:SetNextAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">" + Environment.NewLine;
                        XML += "<InstanceID>" + instanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<NextURI>" + nextURI.Replace(" ", "%20") + "</NextURI>" + Environment.NewLine;
                        XML += "<NextURIMetaData>" + nextURIMetaData + "</NextURIMetaData>" + Environment.NewLine;
                        XML += "</u:SetNextAVTransportURI>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#SetNextAVTransportURI", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.sv_LastChange = "SetNextAVTransportURI";
                        parent.AVTransport.sv_NextAVTransportURI = nextURI;
                        parent.AVTransport.sv_A_ARG_TYPE_InstanceID = (int)instanceID;
                        return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    return null;
                }
                #endregion

                #region Get Media Information
                /// <summary>
                /// Executes the GetMediaInfo action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on.</param>
                /// <remarks>Populates the following State Variable: Number of Tracks, Track Duration, Track Meta Data, Track URI, Next URI, Next URI Meta Data, Record Medium and Current Record Write Status.</remarks>
                public void GetMediaInfo(SonyAPI_Lib.SonyDevice parent)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetMediaInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetMediaInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetMediaInfo", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            foreach (XmlNode snode in node.ChildNodes)
                            {
                                if (snode.Name == "NrTracks") { parent.AVTransport.sv_NumberOfTracks = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "MediaDuration") { parent.AVTransport.sv_CurrentTrackDuration = snode.InnerText; }
                                if (snode.Name == "CurrentURIMetaData") { parent.AVTransport.sv_CurrentTrackMetaData = snode.InnerText; }
                                if (snode.Name == "CurrentURI") { parent.AVTransport.sv_CurrentTrackURI = snode.InnerText; }
                                if (snode.Name == "NextURI") { parent.AVTransport.sv_NextAVTransportURI = snode.InnerText; }
                                if (snode.Name == "NextURIMetaData") { parent.AVTransport.sv_NextAVTransportURIMetaData = snode.InnerText; }
                                if (snode.Name == "PlayMedium") { parent.AVTransport.sv_PlayBackStorageMedium = snode.InnerText; }
                                if (snode.Name == "RecordMedium") { parent.AVTransport.sv_RecordStorageMedium = snode.InnerText; }
                                if (snode.Name == "WriteStatus") { parent.AVTransport.sv_RecordMediumWriteStatus = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.sv_LastChange = "GetMediaInfo";
                    }
                }
                #endregion

                #region Get Transport Information
                /// <summary>
                /// Executes the GetTransportInfo action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <remarks>Populates the following State Variables: Current Transport State, Current Transport Status, Current Play Speed</remarks>
                public void GetTransportInfo(SonyAPI_Lib.SonyDevice parent)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetTransportInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetTransportInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            foreach (XmlNode snode in node.ChildNodes)
                            {
                                if (snode.Name == "CurrentTransportState") { parent.AVTransport.sv_TransportState = snode.InnerText; }
                                if (snode.Name == "CurrentTransportStatus") { parent.AVTransport.sv_TransportStatus = snode.InnerText; }
                                if (snode.Name == "CurrentSpeed") { parent.AVTransport.sv_TransportPlaySpeed = Convert.ToInt32(snode.InnerText); }
                            }
                        }
                        parent.AVTransport.sv_LastChange = "GetTransportInfo";
                    }
                }
                #endregion

                #region Get Position
                /// <summary>
                /// Executes the GetPositionInfo action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <remarks>Populates the following State Variables: Curent Track, Track Duration, Track Meta Data, Track URI, Relative Time, Absolute Time, Relative Counter and Absolute Counter.</remarks>
                public string GetPosition(SonyDevice parent)
                {//Returns the current position for the track that is playing on the DLNA server
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetPositionInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetPositionInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            foreach (XmlNode snode in node.ChildNodes)
                            {
                                if (snode.Name == "Track") { parent.AVTransport.sv_CurrentTrack = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "TrackDuration") { parent.AVTransport.sv_CurrentTrackDuration = snode.InnerText; }
                                if (snode.Name == "TrackMetaData") { parent.AVTransport.sv_CurrentTrackMetaData = snode.InnerText; }
                                if (snode.Name == "TrackURI") { parent.AVTransport.sv_CurrentTrackURI = snode.InnerText; }
                                if (snode.Name == "RelTime") { parent.AVTransport.sv_RelativeTimePosition = snode.InnerText; }
                                if (snode.Name == "AbsTimne") { parent.AVTransport.sv_AbsoluteTimePosition = snode.InnerText; }
                                if (snode.Name == "RelCount") { parent.AVTransport.sv_RelativeCounterPosition = snode.InnerText; }
                                if (snode.Name == "AbsCount") { parent.AVTransport.sv_AbsoluteCounterPosition = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.sv_LastChange = "GetPosition";

                    }
                    return null;
                }
                #endregion

                #region Get Device Capabilities
                /// <summary>
                /// Executes the GetDeviceCapabilities action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <remarks>Populates the following State Variables: PlayBack Medium, Record Medium and Available Record Quality Modes</remarks>
                public void GetDeviceCapabilities(SonyAPI_Lib.SonyDevice parent)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetTransportInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetTransportInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            foreach (XmlNode snode in node.ChildNodes)
                            {
                                if (snode.Name == "PlayMedia") { parent.AVTransport.sv_PlayBackStorageMedium = snode.InnerText; }
                                if (snode.Name == "RecMedia") { parent.AVTransport.sv_RecordStorageMedium = snode.InnerText; }
                                if (snode.Name == "RecQualityModes") { parent.AVTransport.sv_PossibleRecordQualityModes = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.sv_LastChange = "GetTransportInfo";
                    }
                }
                #endregion

                #region Get Transport Settings
                /// <summary>
                /// Executes the GetTransportSettings action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <remarks>Populates the following State Variables: Current Play Mode, Record Quality Mode</remarks>
                public void GetTransportSettings(SonyAPI_Lib.SonyDevice parent)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetTransportSettings xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetTransportSettings>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportSettings", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        GG = Extentions.ChopOffBefore(GG, "<?xml");
                        GG = "<?xml" + GG;
                        XmlDocument xDoc = new XmlDocument();
                        xDoc.LoadXml(GG);
                        XmlNode xNode = xDoc.DocumentElement.ChildNodes[0];
                        foreach (XmlNode node in xNode.ChildNodes)
                        {
                            foreach (XmlNode snode in node.ChildNodes)
                            {
                                if (snode.Name == "PlayMode") { parent.AVTransport.sv_CurrentPlayMode = snode.InnerText; }
                                if (snode.Name == "RecQualityModes") { parent.AVTransport.sv_PossibleRecordQualityModes = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.sv_LastChange = "GetTransportSettings";
                    }
                }
                #endregion

                #region Stop
                /// <summary>
                /// Executes the Stop action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                public string Stop(SonyDevice parent)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Stop xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID><Speed>1</Speed></u:Stop>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#PStop", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.sv_LastChange = "Stop";
                        parent.AVTransport.sv_A_ARG_TYPE_InstanceID = (int)InstanceID;
                        parent.AVTransport.sv_TransportPlaySpeed = 0;
                        return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    return null;
                }
                #endregion

                #region Play
                /// <summary>
                /// Executes the Play action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="speed">In value for the Speed action parameter.</param>
                public string Play(SonyDevice parent, int speed)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID><Speed>" + speed + "</Speed></u:Play>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Play", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.sv_LastChange = "Play";
                        parent.AVTransport.sv_TransportPlaySpeed = speed;
                        parent.AVTransport.sv_A_ARG_TYPE_InstanceID = (int)InstanceID;
                        return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    return null;
                }
                #endregion

                #region Pause
                /// <summary>
                /// Executes the Pause action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                public string Pause(SonyDevice parent)
                {
                    if (parent.AVTransport.controlURL != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Pause xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID></u:Pause>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.Device_IP_Address, parent.Device_Port);
                        int reqi = parent.AVTransport.controlURL.IndexOf(":") + 3;
                        string req = parent.AVTransport.controlURL.Substring(reqi);
                        reqi = parent.AVTransport.controlURL.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Pause", parent.Device_IP_Address, parent.Device_Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.sv_LastChange = "Pause";
                        parent.AVTransport.sv_TransportPlaySpeed = 0;
                        parent.AVTransport.sv_A_ARG_TYPE_InstanceID = (int)InstanceID;
                        return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    return null;
                }
                #endregion


                /// <summary>
                /// Executes the Seek action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="unit">In value for the Unit action parameter.</param>
                /// <param name="target">In value for the Target action parameter.</param>
                public void Seek(SonyAPI_Lib.SonyDevice parent, int unit, String target)
                {
                //    object[] loIn = new object[3];

                //    loIn[0] = instanceID;
                //    loIn[1] = ToStringAARGTYPESeekMode(unit);
                //    loIn[2] = target;
                //    InvokeAction(csAction_Seek, loIn);

                }

                /// <summary>
                /// Executes the Next action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                public void Next(SonyAPI_Lib.SonyDevice parent)
                {
                //    object[] loIn = new object[1];

                //    loIn[0] = instanceID;
                //    InvokeAction(csAction_Next, loIn);

                }

                /// <summary>
                /// Executes the Previous action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                public void Previous(SonyAPI_Lib.SonyDevice parent)
                {
                //    object[] loIn = new object[1];

                //    loIn[0] = instanceID;
                //    InvokeAction(csAction_Previous, loIn);

                }

                /// <summary>
                /// Executes the SetPlayMode action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="newPlayMode">In value for the NewPlayMode action parameter. Default value of NORMAL.</param>
                public void SetPlayMode(SonyAPI_Lib.SonyDevice parent, CurrentPlayModeEnum newPlayMode)
                {
                //    object[] loIn = new object[2];

                //    loIn[0] = instanceID;
                //    loIn[1] = ToStringCurrentPlayMode(newPlayMode);
                //    InvokeAction(csAction_SetPlayMode, loIn);

                }

                /// <summary>
                /// Executes the GetCurrentTransportActions action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <returns>Out value for the Actions action parameter.</returns>
                public String GetCurrentTransportActions(SonyAPI_Lib.SonyDevice parent)
                {
                //    object[] loIn = new object[1];

                //    loIn[0] = instanceID;
                //    object[] loOut = InvokeAction(csAction_GetCurrentTransportActions, loIn);

                    return"";
                }

                /// <summary>
                /// Executes the XGetOperationList action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <returns>Out value for the OperationList action parameter.</returns>
                public String XGetOperationList(SonyAPI_Lib.SonyDevice parent)
                {
                //    object[] loIn = new object[1];

                 //   loIn[0] = aVTInstanceID;
                //    object[] loOut = InvokeAction(csAction_XGetOperationList, loIn);

                    return "";
                }

                /// <summary>
                /// Executes the XExecuteOperation action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="actionDirective">In value for the ActionDirective action parameter.</param>
                /// <returns>Out value for the Result action parameter.</returns>
                public String XExecuteOperation(SonyAPI_Lib.SonyDevice parent, String actionDirective)
                {
                //   object[] loIn = new object[2];

                //   loIn[0] = aVTInstanceID;
                //    loIn[1] = actionDirective;
                //    object[] loOut = InvokeAction(csAction_XExecuteOperation, loIn);

                    return "";
                }

                #endregion

                #region Public Properties
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;


                #endregion
            }

            #endregion

            #region Party

            /// <summary>
            /// Service class for the Party1 (urn:schemas-sony-com:service:Party:1) service.
            /// </summary>
            [Serializable]
            public partial class Party1
            {

                #region Public Constants

                /// <summary>
                /// Gets the service type identifier for the Party1 service.
                /// </summary>
                public const string ServiceType = "urn:schemas-sony-com:service:Party:1";

                #endregion

                #region Initialisation

                

                #endregion

                #region Event Handlers

            


                #endregion

                #region Event Callers

            

                #endregion

                #region Public Methods

                /// <summary>
                /// Executes the XGetDeviceInfo action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <remarks>Populates the following State Variables: Singer Capability and Transport Port</remarks>
                public void XGetDeviceInfo(SonyAPI_Lib.SonyDevice parent)
                {
    //                object[] loIn = new object[0];
    //                object[] loOut = InvokeAction(csAction_XGetDeviceInfo, loIn);
    //                singerCapability = (Byte)loOut[0];
    //                transportPort = (UInt16)loOut[1];
                }

                /// <summary>
                /// Executes the XGetState action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <remarks>Populates the Following State Variables: Party State, Party Mode, Party Song, Session ID, Number of Listeners, Listener List, Singer UUID and Singer Session ID</remarks>
                public void XGetState(SonyAPI_Lib.SonyDevice parent)
                {
    //                object[] loIn = new object[0];
    //                object[] loOut = InvokeAction(csAction_XGetState, loIn);
    //                partyState = (String)loOut[0];
    //                partyMode = (String)loOut[1];
    //                partySong = (String)loOut[2];
    //                sessionID = (UInt32)loOut[3];
    //                numberOfListeners = (Byte)loOut[4];
    //                listenerList = (String)loOut[5];
    //                singerUUID = (String)loOut[6];
    //                singerSessionID = (UInt32)loOut[7];
                }

                /// <summary>
                /// Executes the XStart action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="partyMode">In value for the PartyMode action parameter.</param>
                /// <param name="listenerList">In value for the ListenerList action parameter.</param>
                /// <returns>Out value for the SingerSessionID action parameter.</returns>
                public UInt32 XStart(SonyAPI_Lib.SonyDevice parent, String partyMode, String listenerList)
                {
    //                object[] loIn = new object[2];
    //                loIn[0] = partyMode;
    //                loIn[1] = listenerList;
    //                object[] loOut = InvokeAction(csAction_XStart, loIn);
                    return 0;
                }

                /// <summary>
                /// Executes the XEntry action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="singerSessionID">In value for the SingerSessionID action parameter.</param>
                /// <param name="listenerList">In value for the ListenerList action parameter.</param>
                public void XEntry(SonyAPI_Lib.SonyDevice parent, UInt32 singerSessionID, String listenerList)
                {
    //                object[] loIn = new object[2];
    //                loIn[0] = singerSessionID;
    //                loIn[1] = listenerList;
    //                InvokeAction(csAction_XEntry, loIn)
                }

                /// <summary>
                /// Executes the XLeave action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="singerSessionID">In value for the SingerSessionID action parameter.</param>
                /// <param name="listenerList">In value for the ListenerList action parameter.</param>
                public void XLeave(SonyAPI_Lib.SonyDevice parent, UInt32 singerSessionID, String listenerList)
                {
    //                object[] loIn = new object[2];
    //                loIn[0] = singerSessionID;
    //                loIn[1] = listenerList;
    //                InvokeAction(csAction_XLeave, loIn);
                }

                /// <summary>
                /// Executes the XAbort action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="singerSessionID">In value for the SingerSessionID action parameter.</param>
                public void XAbort(SonyAPI_Lib.SonyDevice parent, UInt32 singerSessionID)
                {
    //                object[] loIn = new object[1];
    //                loIn[0] = singerSessionID;
    //                InvokeAction(csAction_XAbort, loIn);
                }

                /// <summary>
                /// Executes the XInvite action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="partyMode">In value for the PartyMode action parameter.</param>
                /// <param name="singerUUID">In value for the SingerUUID action parameter.</param>
                /// <param name="singerSessionID">In value for the SingerSessionID action parameter.</param>
                /// <returns>Out value for the ListenerSessionID action parameter.</returns>
                public UInt32 XInvite(SonyAPI_Lib.SonyDevice parent, String partyMode, String singerUUID, UInt32 singerSessionID)
                {
    //                object[] loIn = new object[3];
    //                loIn[0] = partyMode;
    //                loIn[1] = singerUUID;
    //                loIn[2] = singerSessionID;
    //                object[] loOut = InvokeAction(csAction_XInvite, loIn);
                    return 0;
                }

                /// <summary>
                /// Executes the XExit action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="listenerSessionID">In value for the ListenerSessionID action parameter.</param>
                public void XExit(SonyAPI_Lib.SonyDevice parent, UInt32 listenerSessionID)
                {
    //                object[] loIn = new object[1];
    //                loIn[0] = listenerSessionID;
    //                InvokeAction(csAction_XExit, loIn);
                }

                #endregion

                #region Public Properties
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;

                #endregion
            }

            #endregion

            #region IRCC

            /// <summary>
            /// Service class for the IRCC1 (urn:schemas-sony-com:service:IRCC:1) service.
            /// </summary>
            [Serializable]
            public partial class IRCC1
            {
            
                #region Public Constants

                /// <summary>
                /// Gets the service type identifier for the IRCC1 service.
                /// </summary>
                public const string ServiceType = "urn:schemas-sony-com:service:IRCC:1";

                #endregion

                #region Public Methods

                #region XSendIRCC
                /// <summary>
                /// Executes the XSendIRCC action.
                /// </summary>
                /// <param name="parent">The Parent Device to use.</param>
                /// <param name="irccCode">In value for the IRCCCode action parameter.</param>
                public string XSendIRCC(SonyDevice parent, String irccCode)
                {
                    if (parent.IRCC.controlURL != null & parent.Registered == true)
                    {
                        _Log.writetolog("IRCC Recieved XSendIRCC Command: " + irccCode, false);
                        string response = "";
                        StringBuilder body = new StringBuilder("<?xml version=\"1.0\"?>");
                        body.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">");
                        body.Append("<s:Body>");
                        body.Append("<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">");
                        body.Append("<IRCCCode>" + irccCode + "</IRCCCode>");
                        body.Append("</u:X_SendIRCC>");
                        body.Append("</s:Body>");
                        body.Append("</s:Envelope>");
                        _Log.writetolog("Sending IRCC Command: " + irccCode, true);
                        string Url = parent.IRCC.controlURL;
                        string Parameters = body.ToString();
                        _Log.writetolog("Creating HttpWebRequest to URL: " + Url, true);
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                        _Log.writetolog("Sending the following parameter: " + Parameters.ToString(), true);
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
                        req.KeepAlive = true;
                        req.Method = "POST";
                        req.ContentType = "text/xml; charset=utf-8";
                        req.ContentLength = bytes.Length;
                        _Log.writetolog("Setting Header Information: " + req.Host.ToString(), false);
                        if (parent.Device_Port != 80)
                        {
                            req.Host = parent.Device_IP_Address + ":" + parent.Device_Port;
                        }
                        else
                        {
                            req.Host = parent.Device_IP_Address;
                        }
                        _Log.writetolog("Header Host: " + req.Host.ToString(), false);
                        req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                        _Log.writetolog("Setting Header User Agent: " + req.UserAgent, false);
                        if (parent.Actionlist.RegisterMode == 3)
                        {
                            _Log.writetolog("Processing Auth Cookie", false);
                            req.CookieContainer = new CookieContainer();
                            List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(parent.Cookie);
                            req.CookieContainer.Add(new Uri(@"http://" + parent.Device_IP_Address + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                            _Log.writetolog("Cookie Container Count: " + req.CookieContainer.Count.ToString(), false);
                            _Log.writetolog("Setting Header Cookie: auth=" + bal[0].Value, false);
                        }
                        else
                        {
                            _Log.writetolog("Setting Header X-CERS-DEVICE-ID: TVSideView-" + parent.Server_Macaddress, false);
                            req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + parent.Server_Macaddress);
                        }
                        req.Headers.Add("SOAPAction", "\"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                        if (parent.Actionlist.RegisterMode != 3)
                        {
                            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                            req.Headers.Add("Accept-Encoding", "gzip, deflate");
                        }
                        else
                        {
                            req.Headers.Add("Accept-Encoding", "gzip");
                        }
                        _Log.writetolog("Sending WebRequest", false);
                        try
                        {
                            System.IO.Stream os = req.GetRequestStream();
                            // Post data and close connection
                            os.Write(bytes, 0, bytes.Length);
                            _Log.writetolog("Sending WebRequest Complete", false);
                            // build response object if any
                            _Log.writetolog("Creating Web Request Response", false);
                            System.Net.HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                            Stream respData = resp.GetResponseStream();
                            StreamReader sr = new StreamReader(respData);
                            response = sr.ReadToEnd();
                            _Log.writetolog("Response returned: " + response, false);
                            os.Close();
                            sr.Close();
                            respData.Close();
                            if (response != "")
                            {
                                _Log.writetolog("Command WAS sent Successfully", true);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                _Log.writetolog("Command was NOT sent successfully", true);
                            }
                            parent.IRCC.sv_LastChange = response;
                        }
                        catch
                        {
                            _Log.writetolog("Error communicating with device", true);
                        }
                        return response;
                    }
                    if (parent.IRCC.controlURL == null)
                    {
                        _Log.writetolog("ERROR: controlURL for IRCC Service is NULL", true);
                    }
                    else
                    {
                        _Log.writetolog("ERROR: Device Registration is FALSE", true);
                    }
                    _Log.writetolog("Or, this device is not compatiable with this Service!", false);
                    return "Error";
                }

                #endregion

                #region XGetStatus
                /// <summary>
                /// Executes the XGetStatus action.
                /// </summary>
                /// <param name="parent">Parent Device object to get the Status from.</param>
                public string XGetStatus(SonyDevice parent)
                {
                    string retstatus = "";
                    if (parent.Actionlist.RegisterMode != 3)
                    {
                        if (parent.Actionlist.getStatus != null)
                        {
                            try
                            {
                                _Log.writetolog("Checking Status of Device " + parent.Name, false);
                                string cstatus;
                                int x;
                                //cstatus = HttpGet(parent.Actionlist.getStatus);
                                String Url = parent.Actionlist.getStatus;
                                _Log.writetolog("Creating HttpWebRequest to URL: " + Url, true);
                                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                                req.KeepAlive = true;
                                // Set our default header Info
                                _Log.writetolog("Setting Header Information: " + req.Host.ToString(), false);
                                req.Host = parent.Device_IP_Address + ":" + parent.Device_Port;
                                req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                                req.Headers.Add("X-CERS-DEVICE-INFO", "Android4.03/TVSideViewForAndroid2.7.1/EVO");
                                req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + parent.Server_Macaddress);
                                req.Headers.Add("Accept-Encoding", "gzip");
                                try
                                {
                                    _Log.writetolog("Creating Web Request Response", false);
                                    System.Net.WebResponse resp = req.GetResponse();
                                    _Log.writetolog("Executing StreamReader", false);
                                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                                    cstatus = sr.ReadToEnd().Trim();
                                    _Log.writetolog("Response returned: " + cstatus, false);
                                    sr.Close();
                                    resp.Close();
                                    cstatus = cstatus.Replace("/n", "");
                                    x = cstatus.IndexOf("name=");
                                    cstatus = cstatus.Substring(x + 6);
                                    x = cstatus.IndexOf("\"");
                                    string sname = cstatus.Substring(0, x);
                                    cstatus = cstatus.Substring(x);
                                    x = cstatus.IndexOf("value=");
                                    cstatus = cstatus.Substring(x + 7);
                                    x = cstatus.IndexOf("\"");
                                    string sval = cstatus.Substring(0, x);
                                    retstatus = sname + ":" + sval;
                                    _Log.writetolog("Device returned a Status of: " + retstatus, true);
                                }
                                catch (Exception e)
                                {
                                    _Log.writetolog("There was an error during the Web Request or Response! " + e.ToString(), true);
                                }
                            }
                            catch (Exception ex)
                            {
                                _Log.writetolog("Checking Device Status for " + parent.Name + " Failed!", true);
                                _Log.writetolog(ex.ToString(), true);
                                retstatus = "";
                            }
                        }
                        else
                        {
                            _Log.writetolog("ERROR: getStatusUrl is NULL", true);
                            _Log.writetolog("This device is not compatiable with this Service!", false);
                            return "Error";
                        }
                    }
                    else
                    {
                        try
                        {
                            _Log.writetolog("Checking Status of Device " + parent.Name, false);
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + parent.Device_IP_Address + @"/sony/system");
                            httpWebRequest.ContentType = "application/json";
                            httpWebRequest.Method = "POST";
                            SonyCommandList dataSet = new SonyCommandList();
                            using (var streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
                            {
                                string json = "{\"id\":19,\"method\":\"getPowerStatus\",\"version\":\"1.0\",\"params\":[]}\"";
                                streamWriter.Write(json);
                            }
                            var httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
                            using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                            {
                                var responseText = streamReader.ReadToEnd();
                                dataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                            }
                            string first = dataSet.result[0].ToString();
                            first = first.Replace("{", "");
                            first = first.Replace("\"", "");
                            retstatus = first;
                            _Log.writetolog("Device returned a Status of: " + retstatus, true);
                        }
                        catch (Exception ex)
                        {
                            _Log.writetolog("Check Status Failed: " + ex, true);
                        }
                    }
                    parent.IRCC.sv_LastChange = retstatus;
                    parent.IRCC.sv_CurrentStatus = retstatus;
                    return retstatus;
                }

                #endregion

                #endregion
            }

            #endregion

            #region Content Directory

            #endregion

        #endregion
    }
}
