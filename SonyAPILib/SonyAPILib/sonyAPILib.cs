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
using System.Web;
using System.Xml.Linq;
using System.Xml;
using System.Xml.Serialization;
using System.Threading;
using System.Globalization;
using System.Reflection;

namespace SonyAPILib
{
    /// <summary>
    /// Sony API Library
    /// </summary>
    public class APILibrary
    {
        #region Library Varables

        /// <summary>
        /// Gets or Sets the API Logging Object
        /// </summary>
        public APILogging Log { get { return _Log; } set { _Log = value; } }

        /// <summary>
        /// Gets or Sets the Sony_API Object
        /// </summary>
        public Locate Locator { get { return _Locator; } set { _Locator = value; } }

        /// <summary>
        /// Represents the IRCC1 Service
        /// </summary>
        //public IRCC1 Ircc = new IRCC1();

        /// <summary>
        /// Represents the AVTransport1 Service
        /// </summary>
        //public AVTransport1 AVTransport = new AVTransport1();

        /// <summary>
        /// Represents the ConnectionManager Service
        /// </summary>
        //public ConnectionManager1 ConnectionManager = new ConnectionManager1();

        /// <summary>
        /// Represents the RenderingControl Service
        /// </summary>
        //public RenderingControl1 RenderingControl = new RenderingControl1();

        /// <summary>
        /// Represents the Party Service
        /// </summary>
        //public Party1 Party = new Party1();

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
        public class Locate
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
            public string ActionListUrl { get; set; }

            #endregion

            #region Locate Devices
            /// <summary>
            /// Execute an SSDP Scan to locate any UPnP/DLNA devices on the network
            /// </summary>
            /// <returns>A list containing the full URL to each device's Description.xml file</returns>
            [STAThread]
            public List<string> LocateDevices()
            {
                SonyDevice fdev = new SonyDevice();
                //if (service == null) { service = "IRCC:1"; }
                _Log.AddMessage("UPnP is Discovering devices....", true);
                List<string> foundDevices = new List<string>();
                global::SonyAPILib.SSDP.Start();
                Thread.Sleep(15000);
                global::SonyAPILib.SSDP.Stop();
                foreach (string u in SSDP.Servers)
                {
                    foundDevices.Add(u);
                }
                _Log.AddMessage("Devices Discovered: " + foundDevices.Count.ToString(), true);
                return foundDevices;
            }
            #endregion

            #region Load Device
            /// <summary>
            /// Loads a device from a file
            /// </summary>
            /// <param name="path">The FULL path to the Device XML file</param>
            public APILibrary.SonyDevice DeviceLoad(string path)
            {
                XmlSerializer deserializer = new XmlSerializer(typeof(SonyDevice));
                APILibrary.SonyDevice sDev = new APILibrary.SonyDevice();
                TextReader reader = new StreamReader(path);
                _Log.AddMessage("Loading API Device file: " + path, true);
                //deserialize
                sDev = (SonyDevice)deserializer.Deserialize(reader);
                reader.Close();
                sDev.CheckReg();
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
                _Log.AddMessage("Saving API Device file for: " + dev.Name, true);
                XmlSerializer serializer = new XmlSerializer(typeof(SonyDevice));
                FileStream fs = new FileStream(path, FileMode.Create);
                TextWriter writer = new StreamWriter(fs, new UTF8Encoding());
                serializer.Serialize(writer, dev);
                writer.Close();
                //string newPath1 = path.Substring(0, path.Length - 4);
                //string newPath2 = newPath1 + "_IRCC.xml";
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
            public string Udn { get; set; }

            /// <summary>
            /// Gets or Sets the Sony Device type Identifier
            /// </summary>
            public string Type { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's IP Address
            /// </summary>
            public string IPAddress { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's MAC Address
            /// </summary>
            public string MacAddress { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's Port number
            /// </summary>
            public int Port { get; set; }
            
            /// <summary>
            /// Gets or Sets the Sony Device Object's List of Commands
            /// </summary>
            public List<SonyCommands> Commands { get; set; }
            
            /// <summary>
            /// Gets or Sets the Mac Address of The controlling device (Server)
            /// </summary>
            public string ServerMacAddress { get; set; }
            
            /// <summary>
            /// Gets or Sets the Name of the controlling device (Server)
            /// </summary>
            public string ServerName { get; set; }
            
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
            public string ActionListUrl { get; set; }

            /// <summary>
            /// This will contain the retrieved ActionList objects.
            /// </summary>
            public ActionList Actionlist = new ActionList();
                                   
            /// <summary>
            /// List of Device Commands
            /// </summary>
            SonyCommandList DataSet = new SonyCommandList();
            
            /// <summary>
            /// Default PIN code used with Gen3 devices
            /// </summary>
            public string PinCode = "0000";
            
            /// <summary>
            /// Cookie container for Gen3 Devices
            /// </summary>
            CookieContainer AllCookies = new CookieContainer();
            
            /// <summary>
            /// Gets or Sets the RenderingControl Service if it exist
            /// </summary>
            public RenderingControl1 RenderingControl =new RenderingControl1();

            /// <summary>
            /// Gets or Sets the ConnectionManager Service if it exist
            /// </summary>
            public ConnectionManager1 ConnectionManager = new ConnectionManager1();

            /// <summary>
            /// Gets or Sets the AVTransport Service if it exist
            /// </summary>
            public AVTransport1 AVTransport = new AVTransport1();

            /// <summary>
            /// Gets or Sets the Party Service if it exist
            /// </summary>
            public Party1 Party = new Party1();

            /// <summary>
            /// Gets or Sets the IRCC Service if it exist
            /// </summary>
            public IRCC1 Ircc = new IRCC1();

            /// <summary>
            /// Gets or Sets the Devices Document URL
            /// </summary>
            public string DocumentUrl { get; set; }

            #endregion

            #endregion

            #region Event Handler
            /// <summary>
            /// Peocesses Event Notification messages sent from the Event TCP Listener Server
            /// </summary>
            /// <param name="eObj">Event Object sent from server</param>
            /// <remarks>
            /// This method will fire every time the Event Server receives a message from this device
            /// </remarks>
            public void ProcessEventMessages(EventObject eObj)
            {
                string Service = eObj.ServiceID.ToString();
                Service = Service.Substring(0, Service.Length - 2);
                switch(Service)
                {
                    case "RenderingControl":
                        RenderingControl.ProcessEventNotifications(this, eObj);
                        break;

                    case "AVTransport":
                        AVTransport.ProcessEventNotifications(this, eObj);
                        break;

                    case "ConnectionManager":
                        ConnectionManager.ProcessEventNotifications(this, eObj);
                        break;

                    case "Party":
                        Party.ProcessEventNotifications(this, eObj);
                        break;
                }
            }
            #endregion

            #region Register
            /// <summary>
            /// Sends the Registration command to the selected device
            /// </summary>
            /// <returns>Returns a bool True or False</returns>
            public bool Register()
            {
                _Log.AddMessage("Controlling Mac address: " + this.ServerMacAddress, false);
                string reg = "false";
                string args1 = "name=" + this.ServerName + "&registrationType=initial&deviceId=TVSideView%3A" + this.ServerMacAddress + " ";
                string args2 = "name=" + this.ServerName + "&registrationType=new&deviceId=TVSideView%3A" + this.ServerMacAddress + " ";
                if (this.Actionlist.RegisterMode == 1)
                {
                    reg = HttpGet(this.Actionlist.RegisterUrl + "?" + args1);
                    _Log.AddMessage("Register Mode: 2 Sony Device", false);
                }
                else if (this.Actionlist.RegisterMode == 2)
                {
                    reg = HttpGet(this.Actionlist.RegisterUrl + "?" + args2);
                    _Log.AddMessage("Register Mode: 1 Sony Device", false);
                }
                else if (this.Actionlist.RegisterMode == 3)
                {
                    _Log.AddMessage("Register Mode 3 Sony Sevice", false);
                    string hostname = this.ServerName;
                    string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                    try
                    {
                        _Log.AddMessage("Creating JSON Web Request", false);
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(" http://" + this.IPAddress + "/sony/accessControl");
                        httpWebRequest.ContentType = "application/json";
                        httpWebRequest.Method = "POST";
                        httpWebRequest.AllowAutoRedirect = true;
                        httpWebRequest.Timeout = 500;
                        _Log.AddMessage("Sending Generation 3 JSON Registration", false);
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
                                _Log.AddMessage("Registration response: " + responseText, false);
                                this.Registered = true;
                            }
                            string answerCookie = JsonConvert.SerializeObject(httpWebRequest.CookieContainer.GetCookies(new Uri("http://" + this.IPAddress + "/sony/appControl")));
                            System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                            file.WriteLine(answerCookie);
                            file.Close();
                            this.Cookie = answerCookie;
                            reg = "";
                        }
                        catch
                        {
                            _Log.AddMessage("Must run Method: SendAuth(pincode)", true);
                            reg = "Gen3 Pin Code Required";
                        }
                    }
                    catch
                    {
                        _Log.AddMessage("device not reachable", false);
                    }
                }
                if (reg == "")
                {
                    _Log.AddMessage("Registration was Successful for device at: " + this.IPAddress, true);
                    this.Registered = true;
                    return true;
                }
                else if (reg == "Gen3 Pin Code Required")
                {
                    _Log.AddMessage("Registration not complete for Gen3 device at: " + this.IPAddress, true);
                    this.Registered = false;
                    return true;
                }
                else
                {
                    _Log.AddMessage("Registration was NOT successful for device at: " + this.IPAddress, true);
                    this.Registered = false;
                    return false;
                }
            }
            #endregion

            #region Send Authentication Pin Code
            /// <summary>
            /// Sends the Authorization PIN code to the Gen3 Device
            /// </summary>
            /// <param name="PinCode"></param>
            /// <returns>True or False</returns>
            public bool SendAuth(string PinCode)
            {
                bool reg = false;
                string hostname = this.ServerName;
                string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                try
                {
                    var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + this.IPAddress + @"/sony/accessControl");
                    httpWebRequest2.ContentType = "application/json";
                    httpWebRequest2.Method = "POST";
                    httpWebRequest2.AllowAutoRedirect = true;
                    httpWebRequest2.CookieContainer = AllCookies;
                    httpWebRequest2.Timeout = 500;
                    using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                    {
                        streamWriter.Write(jsontosend);
                    }
                    string authInfo = "" + ":" + PinCode;
                    authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                    httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                    var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                    using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                    {
                        var responseText = streamReader.ReadToEnd();
                        _Log.AddMessage("Registration response: " + responseText, false);
                        this.Registered = true;
                        this.PinCode = PinCode;
                        reg = true;
                    }
                    string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + this.IPAddress + "/sony/appControl")));
                    System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                    file.WriteLine(answerCookie);
                    file.Close();
                    this.Cookie = answerCookie;
                }
                catch
                {
                    _Log.AddMessage("Registration process Timed Out", false);
                    this.Registered = false;
                }
                return reg;
            }
            #endregion

            #region Get remote command list
            /// <summary>
            /// This method will retrieve Gen1 and Gen2 XML IRCC Command List or Gen3+ JSON Command List.
            /// </summary>
            /// <returns>Returns a string containing the contents of the returned XML Command List for your Use</returns>
            /// <remarks>This method will also populate the SonyDevice.Commands object list with the retrieved command list</remarks>
            public string GetRemoteCommandList()
            {
                if(this.Registered == false)
                {
                    _Log.AddMessage("Can Not Retrieve Command List, No Registration", true);
                    return "";
                }
                string cmdList = "";
                if (this.Actionlist.RegisterMode <= 2)
                {
                    _Log.AddMessage(this.Name + " is Retrieving Generation:" + this.Actionlist.RegisterMode + " Remote Command List", false);
                    cmdList = HttpGet(this.Actionlist.RemoteCommandListUrl);
                    if (cmdList != "")
                    {
                        _Log.AddMessage("Retrieve Command List was Successful", true);
                        DataSet CommandList = new DataSet();
                        System.IO.StringReader xmlSR = new System.IO.StringReader(cmdList);
                        CommandList.ReadXml(xmlSR, XmlReadMode.Auto);
                        DataTable IRCCcmd = new DataTable();
                        var items = CommandList.Tables[0].AsEnumerable().Select(r => new SonyCommands { name = r.Field<string>("name"), value = r.Field<string>("value") });
                        var itemlist = items.ToList();
                        this.Commands = itemlist;
                        _Log.AddMessage(this.Name + " Commands have been Populated", true);
                    }
                    else
                    {
                        _Log.AddMessage("Retrieve Command List was NOT successful", true);
                    }
                }
                else
                {
                    _Log.AddMessage(this.Name + " is Retrieving Generation:" + this.Actionlist.RegisterMode + " Remote Command List", false);
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.IPAddress + @"/sony/system");
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
                            _Log.AddMessage("Response Returned: " + cmdList, true);
                            _Log.AddMessage("Retrieve Command List was Successful", true);
                        }
                        else
                        {
                            _Log.AddMessage("Retrieve Command List was NOT successful", true);
                        }
                        DataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                    }
                    string first = DataSet.result[1].ToString();
                    List<SonyCommands> bal = JsonConvert.DeserializeObject<List<SonyCommands>>(first);
                    this.Commands = bal;
                    _Log.AddMessage(this.Name + " Commands have been Populated: " + this.Commands.Count().ToString(), true);
                }
                return cmdList;
            }
            #endregion

            #region HTTP GET command
            private string HttpGet(string Url)
            {
                int getPort = this.Port;
                Uri getUri = new Uri(Url);
                if(getUri.Port != getPort)
                {
                    getPort = getUri.Port;
                }
                _Log.AddMessage("Creating HttpWebRequest to URL: " + Url, false);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                req.KeepAlive = true;
                // Set our default header Info
                _Log.AddMessage("Setting Header Information: " + req.Host.ToString(), false);
                req.Host = this.IPAddress + ":" + getPort.ToString();
                req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                req.Headers.Add("X-CERS-DEVICE-INFO", "Android4.03/TVSideViewForAndroid2.7.1/EVO");
                req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + this.ServerMacAddress);
                req.Headers.Add("Accept-Encoding", "gzip");
                try
                {
                    _Log.AddMessage("Creating Web Request Response", false);
                    System.Net.WebResponse resp = req.GetResponse();
                    _Log.AddMessage("Executing StreamReader", false);
                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                    string results = sr.ReadToEnd().Trim();
                    _Log.AddMessage("Response returned: " + results, false);
                    sr.Close();
                    resp.Close();
                    return results;
                }
                catch (Exception e)
                {
                    _Log.AddMessage("There was an error during the Web Request or Response! " + e.ToString(), true);
                    return "false : " + e;
                }
            }
            #endregion

            #region HTTP POST command

            /// <summary>
            /// Executes the HTTP Post command
            /// </summary>
            /// <param name="Url">URL to send POST to</param>
            /// <param name="Parameters">Additional parameters</param>
            /// <returns></returns>
            public string HttpPost(string Url, String Parameters)
            {
                int getPort = this.Port;
                Uri getUri = new Uri(Url);
                if (getUri.Port != getPort)
                {
                    getPort = getUri.Port;
                }
                _Log.AddMessage("Creating HttpWebRequest to URL: " + Url, true);
                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                _Log.AddMessage("Sending the following parameter: " + Parameters.ToString(), true);
                byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
                req.KeepAlive = true;
                req.Method = "POST";
                req.ContentType = "text/xml; charset=utf-8";
                req.ContentLength = bytes.Length;
                _Log.AddMessage("Setting Header Information: " + req.Host.ToString(), false);
                if (this.Port != 80)
                {
                    req.Host = this.IPAddress + ":" + getUri.Port;
                }
                else
                {
                    req.Host = this.IPAddress;
                }
                _Log.AddMessage("Header Host: " + req.Host.ToString(), false);
                req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                _Log.AddMessage("Setting Header User Agent: " + req.UserAgent, false);
                if (this.Actionlist.RegisterMode == 3)
                {
                    _Log.AddMessage("Processing Auth Cookie", false);
                    req.CookieContainer = new CookieContainer();
                    List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(this.Cookie);
                    req.CookieContainer.Add(new Uri(@"http://" + this.IPAddress + bal[0].Path.ToString()), new Cookie(bal[0].Name, bal[0].Value));
                    _Log.AddMessage("Cookie Container Count: " + req.CookieContainer.Count.ToString(), false);
                    _Log.AddMessage("Setting Header Cookie: auth=" + bal[0].Value, false);
                }
                else
                {
                    _Log.AddMessage("Setting Header X-CERS-DEVICE-ID: TVSideView-" + this.ServerMacAddress, false);
                    req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + this.ServerMacAddress);
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
                _Log.AddMessage("Sending WebRequest", false);
                System.IO.Stream os = req.GetRequestStream();
                // Post data and close connection
                os.Write(bytes, 0, bytes.Length);
                _Log.AddMessage("Sending WebRequest Complete", false);
                // build response object if any
                _Log.AddMessage("Creating Web Request Response", false);
                System.Net.HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                Stream respData = resp.GetResponseStream();
                StreamReader sr = new StreamReader(respData);
                string response = sr.ReadToEnd();
                _Log.AddMessage("Response returned: " + response, false);
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
            public bool CheckReg()
            {
                bool results = false;
                
                // Gen 1 or 2 Devices
                if (this.Actionlist.RegisterMode <= 2)
                {
                    _Log.AddMessage("Verifing Registration for: " + this.Name, false);
                    // Will NOT return a Status if not Registered
                    if (this.CheckStatus() != "") 
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
                        _Log.AddMessage("Verifing Registration for: " + this.Name, false);
                        _Log.AddMessage("Checking for Generation 3 Cookie", false);
                        System.IO.StreamReader myFile = new System.IO.StreamReader(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                        string myString = myFile.ReadToEnd();
                        myFile.Close();
                        List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(myString);
                        _Log.AddMessage(this.Name + ": Cookie Loaded: " + bal[0].Value, false);

                        // Check if cookie has expired
                        _Log.AddMessage(this.Name + ": Getting Country Date/Time format", false);
                        string sysFormat = CultureInfo.CurrentCulture.DateTimeFormat.ShortDatePattern;
                        _Log.AddMessage(this.Name + ": System Culture Date/Time format is: " + sysFormat, false);
                        CultureInfo ci = new CultureInfo(sysFormat);
                        DateTime CT = DateTime.Parse(DateTime.UtcNow.ToString()).ToUniversalTime();
                        DateTime cCT = DateTime.Parse(bal[0].Expires).ToUniversalTime();
                        _Log.AddMessage(this.Name + ": Checking if Cookie has Expired: " + cCT.ToString(), false);
                        _Log.AddMessage(this.Name + ": Cookie Expiration Date: " + cCT.ToString(), false);
                        _Log.AddMessage(this.Name + ": Current Date and Time : " + CT, false);
                        if (CT > Convert.ToDateTime(bal[0].Expires))
                        {
                            _Log.AddMessage(this.Name + ": Cookie is Expired!", true);
                            _Log.AddMessage(this.Name + ": Retriving NEW Cookie!", true);
                            string hostname = this.ServerName;
                            string jsontosend = "{\"id\":13,\"method\":\"actRegister\",\"version\":\"1.0\",\"params\":[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"nickname\":\"" + hostname + "\"},[{\"clientid\":\"" + hostname + ":34c43339-af3d-40e7-b1b2-743331375368c\",\"value\":\"yes\",\"nickname\":\"" + hostname + "\",\"function\":\"WOL\"}]]}";
                            try
                            {
                                var httpWebRequest2 = (HttpWebRequest)WebRequest.Create(@"http://" + this.IPAddress + @"/sony/accessControl");
                                httpWebRequest2.ContentType = "application/json";
                                httpWebRequest2.Method = "POST";
                                httpWebRequest2.AllowAutoRedirect = true;
                                httpWebRequest2.CookieContainer = AllCookies;
                                httpWebRequest2.Timeout = 500;
                                using (var streamWriter = new StreamWriter(httpWebRequest2.GetRequestStream()))
                                {
                                    streamWriter.Write(jsontosend);
                                }
                                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var responseText = streamReader.ReadToEnd();
                                    _Log.AddMessage("Registration response: " + responseText, false);
                                }
                                string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + this.IPAddress + "/sony/appControl")));
                                System.IO.StreamWriter file = new System.IO.StreamWriter(@"C:\ProgramData\Sony\" + this.Name + "_cookie.json");
                                file.WriteLine(answerCookie);
                                file.Close();
                                this.Cookie = answerCookie;
                                bal = JsonConvert.DeserializeObject<List<SonyCookie>>(answerCookie);
                                AllCookies.Add(new Uri(@"http://" + this.IPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                                this.Registered = true;
                                results = true;
                                _Log.AddMessage(this.Name + ": New Cookie auth=" + this.Cookie, false);
                            }
                            catch
                            {
                                _Log.AddMessage(this.Name + ": Failed to retrieve new Cookie", false);
                                this.Registered = false;
                                results = false;
                            }
                        }
                        else
                        {
                            _Log.AddMessage(this.Name + ": Cookie is not Expired.", false);
                            _Log.AddMessage(bal[0].Name + ": Adding Cookie to Device: " + bal[0].Value, false);
                            AllCookies.Add(new Uri(@"http://" + this.IPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                            this.Cookie = myString;
                            this.Registered = true;
                            results = true;
                            _Log.AddMessage(this.Name + ": Cookie Found: auth=" + this.Cookie, false);
                        }
                    }
                    catch (Exception ex)
                    {
                        _Log.AddMessage("No Cookie was found", false);
                        _Log.AddMessage(ex.ToString(), true);
                        results = false;
                        this.Registered = false;
                    }
                }
                _Log.AddMessage(this.Name + ": Registration Check returned: " + results.ToString(), false);
                return results;
            }
            #endregion

            #region Check Status

            /// <summary>
            /// This method Gets the current Status of the device
            /// </summary>
            /// <returns>Returns the device response as a string</returns>
            public string CheckStatus()
            {
                string retstatus = "";
                if (this.Actionlist.RegisterMode != 3)
                {
                    try
                    {
                        _Log.AddMessage("Checking Status of Device " + this.Name, false);
                        string cstatus;
                        int x;
                        cstatus = HttpGet(this.Actionlist.StatusUrl);
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
                        _Log.AddMessage("Device returned a Status of: " + retstatus, true);
                    }
                    catch (Exception ex)
                    {
                        _Log.AddMessage("Checking Device Status for " + this.Name + " Failed!", true);
                        _Log.AddMessage(ex.ToString(), true);
                        retstatus = "";
                    }
                }
                else
                {
                    try
                    {
                        _Log.AddMessage("Checking Status of Device " + this.Name, false);
                        var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.IPAddress + @"/sony/system");
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
                            DataSet = JsonConvert.DeserializeObject<SonyCommandList>(responseText);
                        }
                        string first = DataSet.result[0].ToString();
                        first = first.Replace("{", "");
                        first = first.Replace("\"", "");
                        retstatus = first;
                        _Log.AddMessage("Device returned a Status of: " + retstatus, true);
                    }
                    catch (Exception ex)
                    {
                        _Log.AddMessage("Check Status Failed: " + ex, true);
                    }
                }
                return retstatus;
            }
            #endregion

            #region Get Registration Mode

            /// <summary>
            /// Gets the Registration Mode from the ActionList.
            /// Or uses Gen 3 if no action list is found.
            /// </summary>
            /// <returns>Returns a string wih the Mode (1, 2 or 3)</returns>
            private string GetRegistrationMode()
            {
                string lAction = "register";
                string foundVal = "3";
                _Log.AddMessage("Retriving Device Registration Mode", false);
                DataSet acList = new DataSet();
                if (ActionListUrl != "")
                {
                    acList.ReadXml(ActionListUrl);
                    DataTable act = new DataTable();
                    act = acList.Tables[0];
                    var results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == lAction select myRow;
                    foundVal = results.ElementAt(0).Field<string>("mode");
                }
                else
                {
                    _Log.AddMessage("No Action List found.", true);
                }
                _Log.AddMessage("Using Registration mode: " + foundVal, true);
                return foundVal;
            }
            #endregion

            #region Get IRCC Command String

            /// <summary>
            /// This method is used to find the Device Command Value based on the Command Name from the SonyDevice.Commands object list.
            /// </summary>
            /// <param name="CmdName">A valid command name found in the SonyDevice.Commands object list. (ie: "ChannelUp")</param>
            /// <returns>Returns the command value for the matched command name. ie: "AAAAAQAAAAEAAAAQAw==". or returns an empty string if no match is found</returns>
            /// <remarks>This can be used with send_ircc("AAAAAQAAAAEAAAAQAw==")
            ///  like this: send_ircc(getIRCCcommandString("ChannelUp")
            /// </remarks>
            public string GetCommandString(string CmdName)
            {
                // Convert XML String to Dataset
                _Log.AddMessage("Retrieving Command String for Command: " + CmdName, true);
                string foundCmd="";
                var results = this.Commands.FirstOrDefault(o => o.name.ToLower() == CmdName.ToLower());
                if (results != null)
                {
                    try
                    {
                        foundCmd = results.value;
                        _Log.AddMessage("Found Command String for: " + CmdName + " - " + foundCmd, false);
                    }
                    catch (Exception e)
                    {
                        foundCmd = "";
                        _Log.AddMessage("Command String for: " + CmdName + " NOT Found! ", true);
                        _Log.AddMessage(e.ToString(), true);
                    }
                }
                return foundCmd;
            }
            #endregion

            #region Send Text command

            /// <summary>
            /// This method send Inputed Text via an HTTP GET command
            /// </summary>
            /// <param name="SendText">A string containing the text to send to the device</param>
            /// <returns>Returns the device response as a string</returns>
            public string SendText(string SendText = "")
            {
                string response = "";
                if (this.Actionlist.RegisterMode < 3)
                {
                    _Log.AddMessage("Sending TEXT to device", false);
                    response = HttpGet(this.Actionlist.SendTextUrl + "?text=" + SendText);
                    if (response != "")
                    {
                        _Log.AddMessage("Send Text WAS sent Successfully", true);
                    }
                    else
                    {
                        _Log.AddMessage("Send Text was NOT sent successfully", true);
                    }
                }
                else
                {
                    string jsontosend = "{\"id\":78,\"method\":\"setTextForm\",\"version\":\"1.0\",\"params\":[\"" + SendText + "\"]}";
                    var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.IPAddress + "/sony/appControl");
                    httpWebRequest.ContentType = "application/json";
                    httpWebRequest.Method = "POST";
                    httpWebRequest.CookieContainer = new CookieContainer();
                    List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(this.Cookie);
                    httpWebRequest.CookieContainer.Add(new Uri(@"http://" + this.IPAddress + bal[0].Path.ToString()), new Cookie(bal[0].Name, bal[0].Value));
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
            public string GetText()
            {
                string response = HttpGet(this.Actionlist.GetTextUrl);
                return response;
            }
            #endregion

            #region Convert mdf commands

            private string SendIrccMdf(Int32 manu, Int32 device, Int32 function)
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
                SendIrccMdf(2, 26, 90);
            }

            /// <summary>
            /// Sends a mdf command for input HDMI 2
            /// </summary>
            public void hdmi_2()
            {
                SendIrccMdf(2, 26, 91);
            }

            /// <summary>
            /// Sends a mdf command for input HDMI 3
            /// </summary>
            public void hdmi_3()
            {
                SendIrccMdf(2, 26, 92);
            }

            /// <summary>
            /// Sends a mdf command for input HDMI 4
            /// </summary>
            public void hdmi_4()
            {
                SendIrccMdf(2, 26, 93);
            }
            #endregion

            #region Volume Up
            /// <summary>
            /// Sends IRCC value for Volume Up
            /// </summary>
            public void VolumeUp()
            {
                //this.send_ircc(getIRCCcommandString("VolumeUp"));
                //this.Ircc.XSendIRCC(this, getIRCCcommandString("VolumeUp"));
            }
            #endregion

            #region Volume Down
            /// <summary>
            /// Sends IRCC value for Volume Down
            /// </summary>
            public void VolumeDown()
            {
                //this.send_ircc(getIRCCcommandString("VolumeDown"));
            }
            #endregion

            #region Channel Up
            /// <summary>
            /// Sends IRCC value for Channel Up
            /// </summary>
            public void ChannelUp()
            {
                //this.send_ircc(getIRCCcommandString("ChannelUp"));
            }
            #endregion

            #region Channel Down
            /// <summary>
            /// Sends IRCC value for Channel Down
            /// </summary>
            public void ChannelDown()
            {
                //this.send_ircc(getIRCCcommandString("ChannelDown"));
            }
            #endregion

            #region Channel Set
            /// <summary>
            /// This public method can be used to tune to a complete channel.
            /// </summary>
            /// <param name="channel">A string containing a valid complete channel. (ie. 47.1)</param>
            public void ChannelSet(string channel)
            {
                int chlen = channel.Length;
                string ircc_cmd = "";
                for (int i = 0; i <= chlen - 1; i++)
                {
                    string chchar = channel.Substring(i, 1);
                    switch (chchar)
                    {
                        case "1":
                            ircc_cmd = GetCommandString("Num1");
                            break;
                        case "2":
                            ircc_cmd = GetCommandString("Num2");
                            break;
                        case "3":
                            ircc_cmd = GetCommandString("Num3");
                            break;
                        case "4":
                            ircc_cmd = GetCommandString("Num4");
                            break;
                        case "5":
                            ircc_cmd = GetCommandString("Num5");
                            break;
                        case "6":
                            ircc_cmd = GetCommandString("Num6");
                            break;
                        case "7":
                            ircc_cmd = GetCommandString("Num7");
                            break;
                        case "8":
                            ircc_cmd = GetCommandString("Num8");
                            break;
                        case "9":
                            ircc_cmd = GetCommandString("Num9");
                            break;
                        case "0":
                            ircc_cmd = GetCommandString("Num0");
                            break;
                        case ".":
                            ircc_cmd = GetCommandString("DOT");
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
            public string CheckFullPath(string cpath)
            {
                if (cpath.StartsWith("http://"))
                {
                    //Full path already exist
                }
                else
                {
                    //Complete the Path
                    cpath = "http://" + this.IPAddress + ":" + this.Port + cpath ;
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
            public string GetServerMac()
            {
                _Log.AddMessage("Retrieving Controlling devices Mac Address. (this computer)", false);
                string mac = GetMacAddress();
                _Log.AddMessage("Mac Address retrieved: " + mac, false);
                _Log.AddMessage("Re-Parsing Mac Address. (Replace : with -)", false);
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
                _Log.AddMessage("Mac Address has been re-Parsed: " + mac, true);
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
            /// <param name="mDev">The SonyDevice to obtain the Mac Address From</param>
            /// <returns></returns>
            public string GetDeviceMac(SonyDevice mDev)
            {
                String macaddress = "";
                _Log.AddMessage("Retrieving the Mac Address from: " + mDev.Name + " at IP: " + mDev.IPAddress, true);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + mDev.IPAddress + @"/sony/system");
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
                _Log.AddMessage("Devices Mac Address: " + macaddress, true);
                return macaddress;
            }

            #endregion

            #region Build From Document

            /// <summary>
            /// Builds a SonyDevice object based on the devices Description document URL.
            /// </summary>
            /// <param name="dPath">A URI containg the URL to the device's Description XML file.</param>
            /// <returns>A Fully built and populated Sony Device</returns>
            public bool BuildFromDocument(Uri dPath)
            {
                this.DocumentUrl = dPath.ToString();
                _Log.AddMessage("Retrieving Device Description Document from URI:", false);
                _Log.AddMessage(dPath.ToString(), false);
                try
                {
                    XDocument dDoc = XDocument.Load(dPath.ToString());
                    this.BuildFromDocument(dDoc.Root.Document.ToString(), dPath.ToString());
                    return true;
                }
                catch
                {
                    _Log.AddMessage("There was an Error while Building the Device", false);
                    _Log.AddMessage("The device may not be powered on, or the Path was incorrect.", false);
                    return false;
                }
            }

            /// <summary>
            /// Builds a SonyDevice object based on the devices Description document text.
            /// </summary>
            /// <param name="Doc">A string containg the Description XML.</param>
            /// <param name="Path">A string containg the Full Path to the device's Description XML file.</param>
            /// <returns>A Fully built and populated Sony Device</returns>
            public bool BuildFromDocument(string Doc, string Path)
            {
                _Log.AddMessage("Building Device from Document: " + Path, true);
                this.DocumentUrl = Path;
                try
                {
                    _Log.AddMessage("Document Found", false);
                    Uri d = new Uri(Path);
                    this.IPAddress = d.Host;
                    this.Port = d.Port;
                    this.ServerMacAddress = this.GetServerMac();
                    this.ServerName = System.Windows.Forms.SystemInformation.ComputerName + "(SonyAPILib)";
                    XmlDocument xDoc = new XmlDocument();
                    xDoc.LoadXml(Doc);
                    XmlNode xNode = xDoc.DocumentElement.ChildNodes[1];
                    foreach (XmlNode node in xNode.ChildNodes)
                    {
                        if (node.Name == "av:X_UNR_DeviceInfo")
                        {
                            foreach (XmlNode dserv in node.ChildNodes)
                            {
                                if (dserv.Name == "av:X_CERS_ActionList_URL")
                                {
                                    _Log.AddMessage("Action List Found.", false);
                                    string alPath = dserv.FirstChild.InnerText;
                                    this.ActionListUrl = alPath;
                                    DataSet acList = new DataSet();
                                    acList.ReadXml(alPath);
                                    DataTable act = new DataTable();
                                    act = acList.Tables[0];
                                    var results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "register" select myRow;
                                    this.Actionlist.RegisterMode = Convert.ToInt16(results.ElementAt(0).ItemArray[1].ToString());
                                    _Log.AddMessage("Device has a registration Mode of: " + this.Actionlist.RegisterMode.ToString(), false);
                                    this.Actionlist.RegisterUrl = this.CheckFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                    results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getSystemInformation" select myRow;
                                    this.Actionlist.SystemInformationUrl = this.CheckFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                    results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getRemoteCommandList" select myRow;
                                    this.Actionlist.RemoteCommandListUrl = this.CheckFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                    //this.GetRemoteCommandList();
                                    results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getStatus" select myRow;
                                    this.Actionlist.StatusUrl = this.CheckFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                    results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "getText" select myRow;
                                    this.Actionlist.GetTextUrl = this.CheckFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                    results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == "sendText" select myRow;
                                    this.Actionlist.SendTextUrl = this.CheckFullPath(results.ElementAt(0).ItemArray[2].ToString());
                                }
                            }
                        }

                        if (node.Name == "av:X_ScalarWebAPI_DeviceInfo")
                        {
                            this.Actionlist.RegisterMode = 3;
                            _Log.AddMessage("Device has a registration Mode of: " + this.Actionlist.RegisterMode.ToString(), false);
                            this.MacAddress = this.GetDeviceMac(this);
                        }
                        if (node.Name == "friendlyName") { this.Name = node.FirstChild.InnerText; }
                        if (node.Name == "manufacturer") { this.Manufacture = node.FirstChild.InnerText; }
                        if (node.Name == "modelDescription") { this.ModelDescription = node.FirstChild.InnerText; }
                        if (node.Name == "modelName") { this.ModelName = node.FirstChild.InnerText; }
                        if (node.Name == "modelNumber") { this.ModelNumber = node.FirstChild.InnerText; }
                        if (node.Name == "UDN") { this.Udn = node.FirstChild.InnerText; }
                        if (node.Name == "deviceType") { this.Type = node.FirstChild.InnerText; }
                        if (node.Name == "serviceList")
                        {
                            foreach (XmlNode cnode in node.ChildNodes)
                            {
                                DeviceService dServ = new DeviceService();
                                foreach (XmlNode dserv in cnode.ChildNodes)
                                {
                                    if (dserv.Name == "serviceType")
                                    {

                                        dServ.Type = dserv.InnerText;
                                        dServ.ServiceIdentifier = dServ.Type.ChopOffBefore("service:");
                                    }
                                    if (dserv.Name == "serviceId") { dServ.ServiceID = dserv.InnerText; }
                                    if (dserv.Name == "SCPDURL") { dServ.ScpdUrl = this.CheckFullPath(dserv.InnerText); }
                                    if (dserv.Name == "controlURL") { dServ.ControlUrl = this.CheckFullPath(dserv.InnerText); }
                                    if (dserv.Name == "eventSubURL")
                                    {
                                        if (dserv.InnerText != "")
                                        {
                                            dServ.EventSubUrl = this.CheckFullPath(dserv.InnerText);
                                        }
                                    }
                                }
                                if (dServ.ServiceIdentifier == "IRCC:1")
                                {
                                    _Log.AddMessage("IRCC:1 Service discovered on this device", false);
                                    this.Ircc.ControlUrl = dServ.ControlUrl;
                                    this.Ircc.ScpdUrl = dServ.ScpdUrl;
                                    this.Ircc.EventSubUrl = dServ.EventSubUrl;
                                    this.Ircc.ServiceID = dServ.ServiceID;
                                    this.Ircc.ServiceIdentifier = dServ.ServiceIdentifier;
                                    this.Ircc.Type = dServ.Type;
                                }
                                if (dServ.ServiceIdentifier == "AVTransport:1")
                                {
                                    _Log.AddMessage("AVTransport:1 Service discovered on this device", false);
                                    this.AVTransport.ControlUrl = dServ.ControlUrl;
                                    this.AVTransport.ScpdUrl = dServ.ScpdUrl;
                                    this.AVTransport.EventSubUrl = dServ.EventSubUrl;
                                    this.AVTransport.ServiceID = dServ.ServiceID;
                                    this.AVTransport.ServiceIdentifier = dServ.ServiceIdentifier;
                                    this.AVTransport.Type = dServ.Type;
                                }
                                if (dServ.ServiceIdentifier == "RenderingControl:1")
                                {
                                    _Log.AddMessage("RenderingControl:1 Service discovered on this device", false);
                                    this.RenderingControl.ControlUrl = dServ.ControlUrl;
                                    this.RenderingControl.ScpdUrl = dServ.ScpdUrl;
                                    this.RenderingControl.EventSubUrl = dServ.EventSubUrl;
                                    this.RenderingControl.ServiceID = dServ.ServiceID;
                                    this.RenderingControl.ServiceIdentifier = dServ.ServiceIdentifier;
                                    this.RenderingControl.Type = dServ.Type;
                                }
                                if (dServ.ServiceIdentifier == "ConnectionManager:1")
                                {
                                    _Log.AddMessage("ConnectionManager:1 Service discovered on this device", false);
                                    this.ConnectionManager.ControlUrl = dServ.ControlUrl;
                                    this.ConnectionManager.ScpdUrl = dServ.ScpdUrl;
                                    this.ConnectionManager.EventSubUrl = dServ.EventSubUrl;
                                    this.ConnectionManager.ServiceID = dServ.ServiceID;
                                    this.ConnectionManager.ServiceIdentifier = dServ.ServiceIdentifier;
                                    this.ConnectionManager.Type = dServ.Type;
                                }
                                if (dServ.ServiceIdentifier == "Party:1")
                                {
                                    _Log.AddMessage("Party:1 Service discovered on this device", false);
                                    this.Party.ControlUrl = dServ.ControlUrl;
                                    this.Party.ScpdUrl = dServ.ScpdUrl;
                                    this.Party.EventSubUrl = dServ.EventSubUrl;
                                    this.Party.ServiceID = dServ.ServiceID;
                                    this.Party.ServiceIdentifier = dServ.ServiceIdentifier;
                                    this.Party.Type = dServ.Type;
                                }
                            }
                        }
                    }
                    this.CheckReg();
                    this.GetRemoteCommandList();
                    _Log.AddMessage(this.Name + " was built successfully.", true);
                    return true;
                }
                catch
                {
                    _Log.AddMessage("There was an Error while Building the Device", false);
                    _Log.AddMessage("The device may not be powered on, or the Path was incorrect.", false);
                    return false;
                }
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
                    _Log.AddMessage("Sending Wake On Lan command to device", false);
                    Byte[] datagram = new byte[102];
                    for (int i = 0; i <= 5; i++)
                    {
                        datagram[i] = 0xff;
                    }
                    string[] macDigits = null;
                    if (this.MacAddress.Contains("-"))
                    {
                        macDigits = this.MacAddress.Split('-');
                    }
                    else
                    {
                        macDigits = this.MacAddress.Split(':');
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
                    _Log.AddMessage("Send WOL command to " + this.Name, true);
                }
                else
                {
                    _Log.AddMessage("Device does not support WOL", true);
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
        public class APILogging
        {
            #region Public Variables
            /// <summary>
            /// Gets or Sets Enable API Logging
            /// True - Turns Loggin On
            /// False - Turns Loggin Off
            /// </summary>
            public bool Enable 
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
            public string Level
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
            public string Path
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
            public string Name
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
            public void AddMessage(string message, bool oride)
            {
                if (this.Path == null | this.Path == "") { this.Path = @"c:\ProgramData\Sony\"; }
                Directory.CreateDirectory(this.Path);
                if (this.Name == null | this.Name == "") { this.Name = @"SonyAPILib_LOG.txt"; }
                //if(File.Exists(this.Path + this.Name))
                //{
                    // File already there!
                //}
                //else
                //{
                //    File.Create(this.Path + this.Name);
                //}
                if (this.Level == null | this.Level == "") { this.Level = "Basic"; }
                string logPath = this.Path + this.Name;
                if (Enable == true)
                {
                    if (this.Level == "Basic")
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
            public void ClearLog(string newName)
            {
                if (newName != null)
                {
                    File.Copy(@Path + Name, @Path + newName);
                    File.Delete(@Path + Name);
                    AddMessage("Saving Log file as: " + @Path + newName, true);
                }
                else
                {
                    File.Delete(@Path + Name);
                    AddMessage("Clearing Log file: " + @Path + Name, true);
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

        #region DLNA / UPnP Services

            #region Rendering Control

            /// <summary>
            /// Service class for the RenderingControl1 (urn:schemas-upnp-org:service:RenderingControl:1) service.
            /// </summary>
            [Serializable]
            public class RenderingControl1
            {
           
                #region Public Constants

                /// <summary>
                /// Gets the service type identifier for the RenderingControl1 service.
                /// </summary>
                public const string ServiceType = "urn:schemas-upnp-org:service:RenderingControl:1";

                /// <summary>
                /// Gets the service instanceID for the RenderingControl1 service.
                /// </summary>
                public const int InstanceId = 0;

                /// <summary>
                /// Gets the service type identifier for the RenderingControl1 service.
                /// </summary>
                public const string Channel = "Master";
                #endregion

                #region Initialisation

                

                #endregion

                #region Event Handlers
                /// <summary>
                /// 
                /// </summary>
                /// <param name="EventObj"></param>
                /// <param name="eDevice"></param>
                public void ProcessEventNotifications(SonyDevice eDevice, EventObject EventObj)
                {
                    foreach (EventVariable eV in EventObj.StateVariables)
                    {
                        int sc = eV.name.ToString().IndexOf("::");
                        string pName = eV.name.Substring(sc + 2);
                        PropertyInfo pI = eDevice.RenderingControl.GetType().GetProperty(pName);
                        if(pI.PropertyType == typeof(Boolean) | pI.PropertyType == typeof(bool))
                        {
                            if(eV.value == "0")
                            {
                            eV.value = "false";
                            }
                            else
                            {
                            eV.value = "true";
                            }
                        }
                        pI.SetValue(eDevice.RenderingControl, Convert.ChangeType(eV.value,pI.PropertyType),null);
                    }

                }
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
                    if (parent.RenderingControl.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:ListPresets xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "</m:ListPresets>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 3;
                        string req = parent.RenderingControl.ControlUrl.Substring(reqi);
                        reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#ListPresets", parent.IPAddress, parent.Port) + XML;
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
                        parent.RenderingControl.LastChange = "ListPresets: " + ret;
                        parent.RenderingControl.PresetNameList = ret;
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
                public void SelectPreset(APILibrary.SonyDevice parent, string presetName)
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
                    if (parent.RenderingControl.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetMute xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "</m:GetMute>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 3;
                        string req = parent.RenderingControl.ControlUrl.Substring(reqi);
                        reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#GetMute", parent.IPAddress, parent.Port) + XML;
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
                        parent.RenderingControl.LastChange = "GetMute: " + ret.ToString();
                        parent.RenderingControl.Mute = ret;
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
                    if (parent.RenderingControl.ControlUrl != null)
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
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 3;
                        string req = parent.RenderingControl.ControlUrl.Substring(reqi);
                        reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#SetMute", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        parent.RenderingControl.LastChange = "SetMute: " + desiredMute;
                        parent.RenderingControl.Mute = desiredMute;
                        parent.RenderingControl.ChannelState = "Master";
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
                    if (parent.RenderingControl.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetVolume xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "</m:GetVolume>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 3;
                        string req = parent.RenderingControl.ControlUrl.Substring(reqi);
                        reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#GetVolume", parent.IPAddress, parent.Port) + XML;
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
                        parent.RenderingControl.LastChange = "GetVolume: " + ret.ToString();
                        parent.RenderingControl.Volume = ret;
                        parent.RenderingControl.ChannelState = "Master";
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
                    if (parent.RenderingControl.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:SetVolume xmlns:m=\"urn:schemas-upnp-org:service:RenderingControl:1\">" + Environment.NewLine;
                        XML += "<InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<Channel xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"string\">" + Channel + "</Channel>" + Environment.NewLine;
                        XML += "<DesiredVolume xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui2\">" + desiredVolume.ToString() + "</DesiredVolume>" + Environment.NewLine;
                        XML += "</m:SetVolume>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 3;
                        string req = parent.RenderingControl.ControlUrl.Substring(reqi);
                        reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:RenderingControl:1#SetVolume", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        string GG = HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                        parent.RenderingControl.LastChange = "SetVolume: " + desiredVolume;
                        parent.RenderingControl.Volume = desiredVolume;
                        parent.RenderingControl.ChannelState = "Master";
                    }
                }
                #endregion

            #endregion

                #region Public Properties
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;
                /// <summary>
                /// Gets or Sets the Service Type
                /// </summary>
                public string Type { get; set; }
                /// <summary>
                /// Gets of Sets the Friendly Service Identifier
                /// </summary>
                public string ServiceIdentifier { get; set; }
                /// <summary>
                /// Gets or sets the Service ID
                /// </summary>
                public string ServiceID { get; set; }
                /// <summary>
                /// Gets or Sets the Service Control URL
                /// </summary>
                public string ControlUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service Event URL
                /// </summary>
                public string EventSubUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service SCPD URL
                /// </summary>
                public string ScpdUrl { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Last Change.
                /// </summary>
                public string LastChange { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Preset Name List.
                /// </summary>
                public string PresetNameList { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Mute State.
                /// </summary>
                public Boolean Mute { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Volume state.
                /// </summary>
                public int Volume { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Channel(Master).
                /// </summary>
                public string ChannelState = "Master";
                /// <summary>
                /// Gets or Sets the State Variable for the Preset Name.
                /// </summary>
                public string PresetName { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Instance ID (0).
                /// </summary>
                public int InstanceID  { get; set; }
                /// <summary>
                /// Bool value of Event Subscription Status (True/False)
                /// </summary>
                public bool Subscribed { get; set; }
                #endregion
            }

            #endregion

            #region Connection Manager

            /// <summary>
            /// Service class for the ConnectionManager1 (urn:schemas-upnp-org:service:ConnectionManager:1) service.
            /// </summary>
            [Serializable]
            public class ConnectionManager1
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
                /// <summary>
                /// 
                /// </summary>
                /// <param name="EventObj"></param>
                /// <param name="eDevice"></param>
                public void ProcessEventNotifications(SonyDevice eDevice, EventObject EventObj)
                {
                    foreach (EventVariable eV in EventObj.StateVariables)
                    {
                        int sc = eV.name.ToString().IndexOf("::");
                        string pName = eV.name.Substring(sc + 2);
                        PropertyInfo pI = eDevice.ConnectionManager.GetType().GetProperty(pName);
                        if (pI.PropertyType == typeof(Boolean) | pI.PropertyType == typeof(bool))
                        {
                            if (eV.value == "0")
                            {
                                eV.value = "false";
                            }
                            else
                            {
                                eV.value = "true";
                            }
                        }
                        pI.SetValue(eDevice.ConnectionManager, Convert.ChangeType(eV.value, pI.PropertyType), null);
                    }

                }

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
                   if (parent.ConnectionManager.ControlUrl != null)
                   {
                       string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                       string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                       string XML = XMLHead;
                       XML += "<m:GetProtocolInfo xmlns:m=\"urn:schemas-upnp-org:service:ConnectionManager:1\">" + Environment.NewLine;
                       XML += "</m:GetProtocolInfo>" + XMLFoot + Environment.NewLine;
                       Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                       int reqi = parent.ConnectionManager.ControlUrl.IndexOf(":") + 3;
                       string req = parent.ConnectionManager.ControlUrl.Substring(reqi);
                       reqi = parent.ConnectionManager.ControlUrl.IndexOf(":") + 1;
                       req = req.Substring(reqi);
                       reqi = req.IndexOf("/") + 1;
                       req = req.Substring(reqi);
                       string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:ConnectionManager:1#GetProtocolInfo", parent.IPAddress, parent.Port) + XML;
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
                               if (snode.Name == "Source") { parent.ConnectionManager.ProticolSource = snode.InnerText; }
                               if (snode.Name == "Sink") { parent.ConnectionManager.ProtocolSink = snode.InnerText; }
                           }
                       }
                       parent.ConnectionManager.LastChange = "GetProtocolInfo";
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
                    if (parent.ConnectionManager.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetCurrentConnectionIDs xmlns:m=\"urn:schemas-upnp-org:service:ConnectionManager:1\">" + Environment.NewLine;
                        XML += "</m:GetCurrentConnectionIDs>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 3;
                        string req = parent.RenderingControl.ControlUrl.Substring(reqi);
                        reqi = parent.RenderingControl.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:ConnectionManager:1#GetCurrentConnectionIDs", parent.IPAddress, parent.Port) + XML;
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
                        parent.ConnectionManager.LastChange = "GetCurrentConnectionIDs: " + ret.ToString();
                        parent.ConnectionManager.ConnectionID = ret;
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
                    if (parent.ConnectionManager.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<m:GetCurrentConnectionInfo xmlns:m=\"urn:schemas-upnp-org:service:ConnectionManager:1\">" + Environment.NewLine;
                        XML += "<ConnectionID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + parent.ConnectionManager.ConnectionID.ToString() + "</ConnectionID>" + Environment.NewLine;
                        XML += "</m:GetCurrentConnectionInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.ConnectionManager.ControlUrl.IndexOf(":") + 3;
                        string req = parent.ConnectionManager.ControlUrl.Substring(reqi);
                        reqi = parent.ConnectionManager.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:ConnectionManager:1#GetCurrentConnectionInfo", parent.IPAddress, parent.Port) + XML;
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
                                if (snode.Name == "RcsID") { parent.ConnectionManager.RcsID = snode.InnerText; }
                                if (snode.Name == "AVTransportID") { parent.ConnectionManager.AVTransportID = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "PeerConnectionManager") { parent.ConnectionManager.Manager = snode.InnerText; }
                                if (snode.Name == "Status") { parent.ConnectionManager.sv_ConnectionStatus = snode.InnerText; }
                                if (snode.Name == "Direction") { parent.ConnectionManager.Direction = snode.InnerText; }
                                if (snode.Name == "PeerConnectionID") { parent.ConnectionManager.PeerConnectionID = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "ProtocolInfo") { parent.ConnectionManager.ProtocolInfo = snode.InnerText; }
                                if (snode.Name == "RcsID") { parent.ConnectionManager.Direction = snode.InnerText; }
                            }
                        }
                        parent.ConnectionManager.LastChange = "GetCurrentConnectionInfo";
                    }
                }
                #endregion

                #endregion

                #region Public Properties
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;
                /// <summary>
                /// Gets or Sets the Service Type
                /// </summary>
                public string Type { get; set; }
                /// <summary>
                /// Gets of Sets the Friendly Service Identifier
                /// </summary>
                public string ServiceIdentifier { get; set; }
                /// <summary>
                /// Gets or sets the Service ID
                /// </summary>
                public string ServiceID { get; set; }
                /// <summary>
                /// Gets or Sets the Service Control URL
                /// </summary>
                public string ControlUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service Event URL
                /// </summary>
                public string EventSubUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service SCPD URL
                /// </summary>
                public string ScpdUrl { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Last Change.
                /// </summary>
                public string LastChange { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Connection Id.
                /// </summary>
                public int ConnectionID { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Connection Status.
                /// </summary>
                public string sv_ConnectionStatus { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Connection Manager.
                /// </summary>
                public string Manager { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Direction.
                /// </summary>
                public string Direction { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the RcsID.
                /// </summary>
                public string RcsID { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Transport ID.
                /// </summary>
                public int AVTransportID { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Peer Connection ID.
                /// </summary>
                public int PeerConnectionID { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Proticol Information.
                /// </summary>
                public string ProtocolInfo { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Proticol Sink.
                /// </summary>
                public string ProtocolSink { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Proticol Source.
                /// </summary>
                public string ProticolSource { get; set; }

                #endregion
        }

            #endregion

            #region AVTransport
       
            /// <summary>
            /// Service class for the AVTransport1 (urn:schemas-upnp-org:service:AVTransport:1) service.
            /// </summary>
            [Serializable]
            public class AVTransport1
            {

                #region Public Properties
                /// <summary>
                /// Gets or Sets the Service Type
                /// </summary>
                public string Type { get; set; }
                /// <summary>
                /// Gets of Sets the Friendly Service Identifier
                /// </summary>
                public string ServiceIdentifier { get; set; }
                /// <summary>
                /// Gets or sets the Service ID
                /// </summary>
                public string ServiceID { get; set; }
                /// <summary>
                /// Gets or Sets the Service Control URL
                /// </summary>
                public string ControlUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service Event URL
                /// </summary>
                public string EventSubUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service SCPD URL
                /// </summary>
                public string ScpdUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service Last Change variable
                /// </summary>
                public string LastChange { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Transport State.
                /// </summary>
                public string TransportState { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Transport Status.
                /// </summary>
                public string TransportStatus { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Playback Storage Medium.
                /// </summary>
                public string PlayBackStorageMedium { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Record Storage Medium.
                /// </summary>
                public string RecordStorageMedium { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Possible Playback Storage Medium.
                /// </summary>
                public string PossiblePlaybackStorageMedia { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Possible Record Storage Medium.
                /// </summary>
                public string PossibleRecordStorageMedia { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current PLay Maode.
                /// </summary>
                public string CurrentPlayMode { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Transport Play Speed.
                /// </summary>
                public int TransportPlaySpeed { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Record Medium Write Status.
                /// </summary>
                public string RecordMediumWriteStatus { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Record Quality Mode.
                /// </summary>
                public string CurrentRecordQualityMode { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Possible Record Quality Modes.
                /// </summary>
                public string PossibleRecordQualityModes { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Number of Tracks.
                /// </summary>
                public int NumberOfTracks { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Track.
                /// </summary>
                public int CurrentTrack { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Track Duration.
                /// </summary>
                public string CurrentTrackDuration { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Track Meta Data.
                /// </summary>
                public string CurrentTrackMetaData { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Track URI.
                /// </summary>
                public string CurrentTrackURI { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the AVTransport URI.
                /// </summary>
                public string AVTransportURI { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the AVTransport URI Meta Data.
                /// </summary>
                public string AVTransportURIMetaData { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Next AVTransport URI.
                /// </summary>
                public string NextAVTransportURI { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Next AVTransport URI Meta Data.
                /// </summary>
                public string NextAVTransportURIMetaData { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Relative Time Position.
                /// </summary>
                public string RelativeTimePosition { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Absolute Time Position.
                /// </summary>
                public string AbsoluteTimePosition { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Relative Counter Position.
                /// </summary>
                public string RelativeCounterPosition { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Absolute Counter POsition.
                /// </summary>
                public string AbsoluteCounterPosition { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Transport Actions.
                /// </summary>
                public string CurrentTransportActions { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Relative Byte Position.
                /// </summary>
                public string X_DLNA_RelativeBytePosition { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Absolute Byte Position.
                /// </summary>
                public string X_DLNA_AbsoluteBytePosition { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Track Size.
                /// </summary>
                public string X_DLNA_CurrentTrackSize { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Seek Mode.
                /// </summary>
                public string A_ARG_TYPE_SeekMode { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Seek Target.
                /// </summary>
                public string A_ARG_TYPE_SeekTarget { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Instance ID.
                /// </summary>
                public int A_ARG_TYPE_InstanceID { get; set; }
                /// <summary>
                /// Socket Return Code
                /// </summary>
                public int ReturnCode = 0;
                #endregion

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
                /// <summary>
                /// 
                /// </summary>
                /// <param name="EventObj"></param>
                /// <param name="eDevice"></param>
                public void ProcessEventNotifications(SonyDevice eDevice, EventObject EventObj)
                {
                    foreach (EventVariable eV in EventObj.StateVariables)
                    {
                        int sc = eV.name.ToString().IndexOf("::");
                        string pName = eV.name.Substring(sc + 2);
                        PropertyInfo pI = eDevice.AVTransport.GetType().GetProperty(pName);
                        if (pI.PropertyType == typeof(Boolean) | pI.PropertyType == typeof(bool))
                        {
                            if (eV.value == "0")
                            {
                                eV.value = "false";
                            }
                            else
                            {
                                eV.value = "true";
                            }
                        }
                        pI.SetValue(eDevice.AVTransport, Convert.ChangeType(eV.value, pI.PropertyType), null);
                    }

                }
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
                    if (parent.AVTransport.ControlUrl != null)
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
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#SetAVTransportURI", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "SetAVTransportURI";
                        parent.AVTransport.AVTransportURI = currentURI;
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
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
                /// <param name="nextURI">In value for the NextURI action parameter.</param>
                /// <param name="nextURIMetaData">In value for the NextURIMetaData action parameter.</param>
                public string SetNextAVTransportURI(SonyDevice parent, String nextURI, String nextURIMetaData)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:SetNextAVTransportURI xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\">" + Environment.NewLine;
                        XML += "<InstanceID>" + InstanceID + "</InstanceID>" + Environment.NewLine;
                        XML += "<NextURI>" + nextURI.Replace(" ", "%20") + "</NextURI>" + Environment.NewLine;
                        XML += "<NextURIMetaData>" + nextURIMetaData + "</NextURIMetaData>" + Environment.NewLine;
                        XML += "</u:SetNextAVTransportURI>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#SetNextAVTransportURI", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "SetNextAVTransportURI";
                        parent.AVTransport.NextAVTransportURI = nextURI;
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
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
                public void GetMediaInfo(SonyDevice parent)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetMediaInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetMediaInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetMediaInfo", parent.IPAddress, parent.Port) + XML;
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
                                if (snode.Name == "NrTracks") { parent.AVTransport.NumberOfTracks = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "MediaDuration") { parent.AVTransport.CurrentTrackDuration = snode.InnerText; }
                                if (snode.Name == "CurrentURIMetaData") { parent.AVTransport.CurrentTrackMetaData = snode.InnerText; }
                                if (snode.Name == "CurrentURI") { parent.AVTransport.CurrentTrackURI = snode.InnerText; }
                                if (snode.Name == "NextURI") { parent.AVTransport.NextAVTransportURI = snode.InnerText; }
                                if (snode.Name == "NextURIMetaData") { parent.AVTransport.NextAVTransportURIMetaData = snode.InnerText; }
                                if (snode.Name == "PlayMedium") { parent.AVTransport.PlayBackStorageMedium = snode.InnerText; }
                                if (snode.Name == "RecordMedium") { parent.AVTransport.RecordStorageMedium = snode.InnerText; }
                                if (snode.Name == "WriteStatus") { parent.AVTransport.RecordMediumWriteStatus = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.LastChange = "GetMediaInfo";
                    }
                }
                #endregion

                #region Get Transport Information
                /// <summary>
                /// Executes the GetTransportInfo action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <remarks>Populates the following State Variables: Current Transport State, Current Transport Status, Current Play Speed</remarks>
                public void GetTransportInfo(SonyDevice parent)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetTransportInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetTransportInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo", parent.IPAddress, parent.Port) + XML;
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
                                if (snode.Name == "CurrentTransportState") { parent.AVTransport.TransportState = snode.InnerText; }
                                if (snode.Name == "CurrentTransportStatus") { parent.AVTransport.TransportStatus = snode.InnerText; }
                                if (snode.Name == "CurrentSpeed") { parent.AVTransport.TransportPlaySpeed = Convert.ToInt32(snode.InnerText); }
                            }
                        }
                        parent.AVTransport.LastChange = "GetTransportInfo";
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
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetPositionInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetPositionInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetPositionInfo", parent.IPAddress, parent.Port) + XML;
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
                                if (snode.Name == "Track") { parent.AVTransport.CurrentTrack = Convert.ToInt32(snode.InnerText); }
                                if (snode.Name == "TrackDuration") { parent.AVTransport.CurrentTrackDuration = snode.InnerText; }
                                if (snode.Name == "TrackMetaData") { parent.AVTransport.CurrentTrackMetaData = snode.InnerText; }
                                if (snode.Name == "TrackURI") { parent.AVTransport.CurrentTrackURI = snode.InnerText; }
                                if (snode.Name == "RelTime") { parent.AVTransport.RelativeTimePosition = snode.InnerText; }
                                if (snode.Name == "AbsTimne") { parent.AVTransport.AbsoluteTimePosition = snode.InnerText; }
                                if (snode.Name == "RelCount") { parent.AVTransport.RelativeCounterPosition = snode.InnerText; }
                                if (snode.Name == "AbsCount") { parent.AVTransport.AbsoluteCounterPosition = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.LastChange = "GetPosition";

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
                public void GetDeviceCapabilities(SonyDevice parent)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetTransportInfo xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetTransportInfo>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportInfo", parent.IPAddress, parent.Port) + XML;
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
                                if (snode.Name == "PlayMedia") { parent.AVTransport.PlayBackStorageMedium = snode.InnerText; }
                                if (snode.Name == "RecMedia") { parent.AVTransport.RecordStorageMedium = snode.InnerText; }
                                if (snode.Name == "RecQualityModes") { parent.AVTransport.PossibleRecordQualityModes = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.LastChange = "GetTransportInfo";
                    }
                }
                #endregion

                #region Get Transport Settings
                /// <summary>
                /// Executes the GetTransportSettings action.
                /// </summary>
                /// <param name="parent">The Parent Device object to execute this action on.</param>
                /// <remarks>Populates the following State Variables: Current Play Mode, Record Quality Mode</remarks>
                public void GetTransportSettings(SonyDevice parent)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead + "<m:GetTransportSettings xmlns:m=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID xmlns:dt=\"urn:schemas-microsoft-com:datatypes\" dt:dt=\"ui4\">" + InstanceID + "</InstanceID></m:GetTransportSettings>" + XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#GetTransportSettings", parent.IPAddress, parent.Port) + XML;
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
                                if (snode.Name == "PlayMode") { parent.AVTransport.CurrentPlayMode = snode.InnerText; }
                                if (snode.Name == "RecQualityModes") { parent.AVTransport.PossibleRecordQualityModes = snode.InnerText; }
                            }
                        }
                        parent.AVTransport.LastChange = "GetTransportSettings";
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
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Stop xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID><Speed>1</Speed></u:Stop>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#PStop", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "Stop";
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
                        parent.AVTransport.TransportPlaySpeed = 0;
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
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Play xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID><Speed>" + speed + "</Speed></u:Play>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Play", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "Play";
                        parent.AVTransport.TransportPlaySpeed = speed;
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
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
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Pause xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID></u:Pause>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Pause", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "Pause";
                        parent.AVTransport.TransportPlaySpeed = 0;
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
                        return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    return null;
                }
            #endregion

                #region Seek
                /// <summary>
                /// Executes the Seek action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="unit">In value for the Unit action parameter.</param>
                /// <param name="target">In value for the Target action parameter.</param>
                public void Seek(SonyDevice parent, int unit, String target)
                {
                //    object[] loIn = new object[3];

                //    loIn[0] = instanceID;
                //    loIn[1] = ToStringAARGTYPESeekMode(unit);
                //    loIn[2] = target;
                //    InvokeAction(csAction_Seek, loIn);

                }
                #endregion

                #region Next
            /// <summary>
            /// Executes the Next action.
            /// </summary>
            /// <param name="parent">The Device object to execute the request on</param>
            public void Next(APILibrary.SonyDevice parent)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Next xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID></u:Next>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Next", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "Next";
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
                        //return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    //return null;
                }
                #endregion

                #region Previous
                /// <summary>
                /// Executes the Previous action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                public void Previous(SonyDevice parent)
                {
                    if (parent.AVTransport.ControlUrl != null)
                    {
                        string XMLHead = "<?xml version=\"1.0\"?>" + Environment.NewLine + "<SOAP-ENV:Envelope xmlns:SOAP-ENV=\"http://schemas.xmlsoap.org/soap/envelope/\" SOAP-ENV:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">" + Environment.NewLine + "<SOAP-ENV:Body>" + Environment.NewLine;
                        string XMLFoot = "</SOAP-ENV:Body>" + Environment.NewLine + "</SOAP-ENV:Envelope>" + Environment.NewLine;
                        string XML = XMLHead;
                        XML += "<u:Previous xmlns:u=\"urn:schemas-upnp-org:service:AVTransport:1\"><InstanceID>" + InstanceID + "</InstanceID></u:Previous>" + Environment.NewLine;
                        XML += XMLFoot + Environment.NewLine;
                        Socket SocWeb = HelperDLNA.MakeSocket(parent.IPAddress, parent.Port);
                        int reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 3;
                        string req = parent.AVTransport.ControlUrl.Substring(reqi);
                        reqi = parent.AVTransport.ControlUrl.IndexOf(":") + 1;
                        req = req.Substring(reqi);
                        reqi = req.IndexOf("/") + 1;
                        req = req.Substring(reqi);
                        string Request = HelperDLNA.MakeRequest("POST", req, XML.Length, "urn:schemas-upnp-org:service:AVTransport:1#Previous", parent.IPAddress, parent.Port) + XML;
                        SocWeb.Send(UTF8Encoding.UTF8.GetBytes(Request), SocketFlags.None);
                        parent.AVTransport.LastChange = "Next";
                        parent.AVTransport.A_ARG_TYPE_InstanceID = (int)InstanceID;
                        //return HelperDLNA.ReadSocket(SocWeb, true, ref this.ReturnCode);
                    }
                    //return null;
                }
                #endregion

                #region Set Play Mode
                /// <summary>
                /// Executes the SetPlayMode action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="newPlayMode">In value for the NewPlayMode action parameter. Default value of NORMAL.</param>
                public void SetPlayMode(SonyDevice parent, CurrentPlayModeEnum newPlayMode)
                {
                //    object[] loIn = new object[2];

                //    loIn[0] = instanceID;
                //    loIn[1] = ToStringCurrentPlayMode(newPlayMode);
                //    InvokeAction(csAction_SetPlayMode, loIn);

                }
            #endregion

                #region Get Current Transport Actions
                /// <summary>
                /// Executes the GetCurrentTransportActions action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <returns>Out value for the Actions action parameter.</returns>
                public String GetCurrentTransportActions(APILibrary.SonyDevice parent)
                {
                //    object[] loIn = new object[1];

                //    loIn[0] = instanceID;
                //    object[] loOut = InvokeAction(csAction_GetCurrentTransportActions, loIn);

                    return"";
                }
            #endregion

                #region Get Operation List
                /// <summary>
                /// Executes the XGetOperationList action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <returns>Out value for the OperationList action parameter.</returns>
                public String XGetOperationList(SonyDevice parent)
                {
                //    object[] loIn = new object[1];

                    //   loIn[0] = aVTInstanceID;
                //    object[] loOut = InvokeAction(csAction_XGetOperationList, loIn);

                    return "";
                }
            #endregion

                #region Execute Operation
                /// <summary>
                /// Executes the XExecuteOperation action.
                /// </summary>
                /// <param name="parent">The Device object to execute the request on</param>
                /// <param name="actionDirective">In value for the ActionDirective action parameter.</param>
                /// <returns>Out value for the Result action parameter.</returns>
                public String XExecuteOperation(SonyDevice parent, String actionDirective)
                {
                //   object[] loIn = new object[2];

                //   loIn[0] = aVTInstanceID;
                //    loIn[1] = actionDirective;
                //    object[] loOut = InvokeAction(csAction_XExecuteOperation, loIn);

                    return "";
                }
                #endregion

            #endregion

            }

            #endregion

            #region Party

            /// <summary>
            /// Service class for the Party1 (urn:schemas-sony-com:service:Party:1) service.
            /// </summary>
            [Serializable]
            public class Party1
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
                /// <summary>
                /// 
                /// </summary>
                /// <param name="EventObj"></param>
                /// <param name="eDevice"></param>
                public void ProcessEventNotifications(SonyDevice eDevice, EventObject EventObj)
                {
                    foreach (EventVariable eV in EventObj.StateVariables)
                    {
                        int sc = eV.name.ToString().IndexOf("::");
                        string pName = eV.name.Substring(sc + 2);
                        PropertyInfo pI = eDevice.Party.GetType().GetProperty(pName);
                        if (pI.PropertyType == typeof(Boolean) | pI.PropertyType == typeof(bool))
                        {
                            if (eV.value == "0")
                            {
                                eV.value = "false";
                            }
                            else
                            {
                                eV.value = "true";
                            }
                        }
                        pI.SetValue(eDevice.Party, Convert.ChangeType(eV.value, pI.PropertyType), null);
                    }

                }



            #endregion

            #region Event Callers



            #endregion

            #region Public Methods

            /// <summary>
            /// Executes the XGetDeviceInfo action.
            /// </summary>
            /// <param name="parent">The Device object to execute the request on</param>
            /// <remarks>Populates the following State Variables: Singer Capability and Transport Port</remarks>
            public void XGetDeviceInfo(APILibrary.SonyDevice parent)
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
                public void XGetState(APILibrary.SonyDevice parent)
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
                public UInt32 XStart(APILibrary.SonyDevice parent, String partyMode, String listenerList)
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
                public void XEntry(APILibrary.SonyDevice parent, UInt32 singerSessionID, String listenerList)
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
                public void XLeave(APILibrary.SonyDevice parent, UInt32 singerSessionID, String listenerList)
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
                public void XAbort(APILibrary.SonyDevice parent, UInt32 singerSessionID)
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
                public UInt32 XInvite(APILibrary.SonyDevice parent, String partyMode, String singerUUID, UInt32 singerSessionID)
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
                public void XExit(APILibrary.SonyDevice parent, UInt32 listenerSessionID)
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
                /// <summary>
                /// Gets or Sets the Service Type
                /// </summary>
                public string Type { get; set; }
                /// <summary>
                /// Gets of Sets the Friendly Service Identifier
                /// </summary>
                public string ServiceIdentifier { get; set; }
                /// <summary>
                /// Gets or sets the Service ID
                /// </summary>
                public string ServiceID { get; set; }
                /// <summary>
                /// Gets or Sets the Service Control URL
                /// </summary>
                public string ControlUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service Event URL
                /// </summary>
                public string EventSubUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service SCPD URL
                /// </summary>
                public string ScpdUrl { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Last Change.
                /// </summary>
                public string LastChange { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Singer Capability.
                /// </summary>
                public int SingerCapability { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Transport Port.
                /// </summary>
                public int TransportPort { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Party State.
                /// </summary>
                public string PartyState { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Party Mode.
                /// </summary>
                public string PartyMode { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Party Song.
                /// </summary>
                public string PartySong { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Session ID.
                /// </summary>
                public int SessionID { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Number of Listeners.
                /// </summary>
                public int NumberOfListeners { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the ListenersList
                /// </summary>
                public string ListenersList { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the UUID.
                /// </summary>
                public string Uuid { get; set; }

                #endregion
        }

            #endregion

            #region IRCC

            /// <summary>
            /// Service class for the IRCC1 (urn:schemas-sony-com:service:IRCC:1) service.
            /// </summary>
            [Serializable]
            public class IRCC1
            {

                #region Public Properties
                /// <summary>
                /// Gets or Sets the Service Type
                /// </summary>
                public string Type { get; set; }
                /// <summary>
                /// Gets of Sets the Friendly Service Identifier
                /// </summary>
                public string ServiceIdentifier { get; set; }
                /// <summary>
                /// Gets or sets the Service ID
                /// </summary>
                public string ServiceID { get; set; }
                /// <summary>
                /// Gets or Sets the Service Control URL
                /// </summary>
                public string ControlUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service Event URL
                /// </summary>
                public string EventSubUrl { get; set; }
                /// <summary>
                /// Gets or Sets the Service SCPD URL
                /// </summary>
                public string ScpdUrl { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Last Change.
                /// </summary>
                public string LastChange { get; set; }
                /// <summary>
                /// Gets or Sets the State Variable for the Current Status
                /// </summary>
                public string CurrentStatus { get; set; }
                #endregion

                #region Public Constants

            /// <summary>
            /// Gets the service type identifier for the IRCC1 service.
            /// </summary>
            public const string ServiceType = "urn:schemas-sony-com:service:IRCC:1";

                #endregion

                #region Public Methods

                #region Send IRCC
                /// <summary>
                /// Executes the XSendIRCC action.
                /// </summary>
                /// <param name="Parent">The Parent Device to use.</param>
                /// <param name="CommandString">In value for the IRCCCode action parameter.</param>
                public string SendIRCC(SonyDevice Parent, String CommandString)
                {
                    if (Parent.Ircc.ControlUrl != null & Parent.Registered == true)
                    {
                        _Log.AddMessage("SendIrcc Command String: " + CommandString, false);
                        string response = "";
                        StringBuilder body = new StringBuilder("<?xml version=\"1.0\"?>");
                        body.Append("<s:Envelope xmlns:s=\"http://schemas.xmlsoap.org/soap/envelope/\" s:encodingStyle=\"http://schemas.xmlsoap.org/soap/encoding/\">");
                        body.Append("<s:Body>");
                        body.Append("<u:X_SendIRCC xmlns:u=\"urn:schemas-sony-com:service:IRCC:1\">");
                        body.Append("<IRCCCode>" + CommandString + "</IRCCCode>");
                        body.Append("</u:X_SendIRCC>");
                        body.Append("</s:Body>");
                        body.Append("</s:Envelope>");
                        _Log.AddMessage("Sending IRCC Command String: " + CommandString, true);
                        string Url = Parent.Ircc.ControlUrl;
                        string Parameters = body.ToString();
                        _Log.AddMessage("Creating HttpWebRequest to URL: " + Url, true);
                        HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                        _Log.AddMessage("Sending the following parameter: " + Parameters.ToString(), true);
                        byte[] bytes = System.Text.Encoding.UTF8.GetBytes(Parameters);
                        req.KeepAlive = true;
                        req.Method = "POST";
                        req.ContentType = "text/xml; charset=utf-8";
                        req.ContentLength = bytes.Length;
                        _Log.AddMessage("Setting Header Information: " + req.Host.ToString(), false);
                        if (Parent.Port != 80)
                        {
                            req.Host = Parent.IPAddress + ":" + Parent.Port;
                        }
                        else
                        {
                            req.Host = Parent.IPAddress;
                        }
                        _Log.AddMessage("Header Host: " + req.Host.ToString(), false);
                        req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                        _Log.AddMessage("Setting Header User Agent: " + req.UserAgent, false);
                        if (Parent.Actionlist.RegisterMode == 3)
                        {
                            _Log.AddMessage("Processing Auth Cookie", false);
                            req.CookieContainer = new CookieContainer();
                            List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(Parent.Cookie);
                            req.CookieContainer.Add(new Uri(@"http://" + Parent.IPAddress + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                            _Log.AddMessage("Cookie Container Count: " + req.CookieContainer.Count.ToString(), false);
                            _Log.AddMessage("Setting Header Cookie: auth=" + bal[0].Value, false);
                        }
                        else
                        {
                            _Log.AddMessage("Setting Header X-CERS-DEVICE-ID: TVSideView-" + Parent.ServerMacAddress, false);
                            req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + Parent.ServerMacAddress);
                        }
                        req.Headers.Add("SOAPAction", "\"urn:schemas-sony-com:service:IRCC:1#X_SendIRCC\"");
                        if (Parent.Actionlist.RegisterMode != 3)
                        {
                            req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                            req.Headers.Add("Accept-Encoding", "gzip, deflate");
                        }
                        else
                        {
                            req.Headers.Add("Accept-Encoding", "gzip");
                        }
                        _Log.AddMessage("Sending WebRequest", false);
                        try
                        {
                            System.IO.Stream os = req.GetRequestStream();
                            // Post data and close connection
                            os.Write(bytes, 0, bytes.Length);
                            _Log.AddMessage("Sending WebRequest Complete", false);
                            // build response object if any
                            _Log.AddMessage("Creating Web Request Response", false);
                            System.Net.HttpWebResponse resp = req.GetResponse() as HttpWebResponse;
                            Stream respData = resp.GetResponseStream();
                            StreamReader sr = new StreamReader(respData);
                            response = sr.ReadToEnd();
                            _Log.AddMessage("Response returned: " + response, false);
                            os.Close();
                            sr.Close();
                            respData.Close();
                            if (response != "")
                            {
                                _Log.AddMessage("Command WAS sent Successfully", true);
                                Thread.Sleep(1000);
                            }
                            else
                            {
                                _Log.AddMessage("Command was NOT sent successfully", true);
                            }
                            Parent.Ircc.LastChange = response;
                        }
                        catch
                        {
                            _Log.AddMessage("Error communicating with device", true);
                        }
                        return response;
                    }
                    if (Parent.Ircc.ControlUrl == null)
                    {
                        _Log.AddMessage("ERROR: controlURL for IRCC Service is NULL", true);
                    }
                    else
                    {
                        _Log.AddMessage("ERROR: Device Registration is FALSE", true);
                    }
                    _Log.AddMessage("Or, this device is not compatiable with this Service!", false);
                    return "Error";
                }

                #endregion

                #region GetStatus
                /// <summary>
                /// Executes the XGetStatus action.
                /// </summary>
                /// <param name="Parent">Parent Device object to get the Status from.</param>
                public string GetStatus(SonyDevice Parent)
                {
                    string retstatus = "";
                    if (Parent.Actionlist.RegisterMode != 3)
                    {
                        if (Parent.Actionlist.StatusUrl != null)
                        {
                            try
                            {
                                _Log.AddMessage("Checking Status of Device " + Parent.Name, false);
                                string cstatus;
                                int x;
                                //cstatus = HttpGet(parent.Actionlist.getStatus);
                                String Url = Parent.Actionlist.StatusUrl;
                                _Log.AddMessage("Creating HttpWebRequest to URL: " + Url, true);
                                HttpWebRequest req = (HttpWebRequest)WebRequest.Create(Url);
                                req.KeepAlive = true;
                                // Set our default header Info
                                _Log.AddMessage("Setting Header Information: " + req.Host.ToString(), false);
                                req.Host = Parent.IPAddress + ":" + Parent.Port;
                                req.UserAgent = "Dalvik/1.6.0 (Linux; u; Android 4.0.3; EVO Build/IML74K)";
                                req.Headers.Add("X-CERS-DEVICE-INFO", "Android4.03/TVSideViewForAndroid2.7.1/EVO");
                                req.Headers.Add("X-CERS-DEVICE-ID", "TVSideView:" + Parent.ServerMacAddress);
                                req.Headers.Add("Accept-Encoding", "gzip");
                                try
                                {
                                    _Log.AddMessage("Creating Web Request Response", false);
                                    System.Net.WebResponse resp = req.GetResponse();
                                    _Log.AddMessage("Executing StreamReader", false);
                                    System.IO.StreamReader sr = new System.IO.StreamReader(resp.GetResponseStream());
                                    cstatus = sr.ReadToEnd().Trim();
                                    _Log.AddMessage("Response returned: " + cstatus, false);
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
                                    _Log.AddMessage("Device returned a Status of: " + retstatus, true);
                                }
                                catch (Exception e)
                                {
                                    _Log.AddMessage("There was an error during the Web Request or Response! " + e.ToString(), true);
                                }
                            }
                            catch (Exception ex)
                            {
                                _Log.AddMessage("Checking Device Status for " + Parent.Name + " Failed!", true);
                                _Log.AddMessage(ex.ToString(), true);
                                retstatus = "";
                            }
                        }
                        else
                        {
                            _Log.AddMessage("ERROR: getStatusUrl is NULL", true);
                            _Log.AddMessage("This device is not compatiable with this Service!", false);
                            return "Error";
                        }
                    }
                    else
                    {
                        try
                        {
                            _Log.AddMessage("Checking Status of Device " + Parent.Name, false);
                            var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + Parent.IPAddress + @"/sony/system");
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
                            _Log.AddMessage("Device returned a Status of: " + retstatus, true);
                        }
                        catch (Exception ex)
                        {
                            _Log.AddMessage("Check Status Failed: " + ex, true);
                        }
                    }
                    Parent.Ircc.LastChange = retstatus;
                    Parent.Ircc.CurrentStatus = retstatus;
                    return retstatus;
                }

                #endregion

                #endregion
            }

        #endregion

            #region Content Directory

            #endregion

        #endregion

        #region Event Server
        /// <summary>
        /// Class for creating, starting and Stopping the Eventing Server.
        /// </summary>
        public class EventServer : INotifyPropertyChanged
        {

            #region Public Properties
            /// <summary>
            /// Represents if the Event Server Is connected
            /// </summary>
            public static ManualResetEvent connected = new ManualResetEvent(false);

            /// <summary>
            /// Gets or Sets the IP address of the TCP Listener Server.
            /// </summary>
            public string IpAddress { get; set; }

            /// <summary>
            /// Gets or Sets the Port of the TCP Listener Server
            /// </summary>
            public int Port { get; set; }
            
            /// <summary>
            /// Represents the TCP Listener Server Status. (True/False)
            /// </summary>
            public bool IsRunning = false;

            /// <summary>
            /// Contains This servers Call Back URL
            /// </summary>
            public string CallBackUrl { get; set; }
            
            /// <summary>
            /// String representation of the message received by the TCP Listener Server
            /// </summary>
            public string Output
            {
                get { return output; }
                set
                {
                    if (value != output)
                    {
                        output = value;
                        OnPropertyChanged("Output");
                    }
                }
            }
            #endregion

            #region Private Properties
            private string output = "";
            private List<EventDevice> EventedDevices = new List<EventDevice>();
            static TcpListener server;
            private EventObject cEventObj;
            private EventDevice sED = new EventDevice();
            #endregion

            #region Public Methods
            /// <summary>
            /// Event occurs when TCP Listener Server receives an Event Notification from the device
            /// </summary>
            public event PropertyChangedEventHandler PropertyChanged;

            /// <summary>
            /// Server Object for receiving device Event and Property Changes
            /// </summary>
            public EventServer()
            {
            }

            /// <summary>
            /// Executes a START command on the TCP Listener Server
            /// </summary>
            public void Start()
            {
                server = new TcpListener(IPAddress.Parse(IpAddress), Port);
                server.Start();

                _Log.AddMessage("Event Server Started at: " + IpAddress + " on Port: " + Port, true);
                new Thread(() =>
                {
                    while (true)
                    {
                        connected.Reset();
                        server.BeginAcceptTcpClient(new AsyncCallback(AcceptCallback), server);
                        connected.WaitOne();
                    }
                }).Start();
                this.IsRunning = true;
            }

            /// <summary>
            /// Executes a STOP command on the TCP Listener Server
            /// </summary>
            public void Stop()
            {
                EndAllSubscriptions();
                connected.Reset();
                server.Stop();
                this.IsRunning = false;
                _Log.AddMessage("Event Server at: " + IpAddress + " on Port: " + Port + " was STOPPED", true);
            }

            #region Event Subscription
            /// <summary>
            /// Sends the SUBSCRIBE information to the parent device for Event Handeling
            /// </summary>
            /// <param name="Parent">The Sony device to SUBSCRIBE to.</param>
            /// <param name="ServiceIdent">The Sony device UPnP/DLNA ServiceIdentifier to SUBSCRIBE to.</param>
            /// <param name="Duration">The number of seconds to keep subscription alive</param>
            public void SubscribeToEvents(SonyDevice Parent, string ServiceIdent, int Duration)
            {
                if(this.IsRunning == false)
                {
                    _Log.AddMessage("Event Server is NOT running. Please start Server before Subscribing to Events", true);
                    return;
                }
                string eventURL = GetEventUrl(Parent, ServiceIdent);
                if(string.IsNullOrEmpty(eventURL))
                {
                    _Log.AddMessage("Cant not subscribe to service: " + ServiceIdent, true);
                    return;
                }
                _Log.AddMessage("Sending Subscription Request to: " + eventURL, true);

                Uri nHost = new Uri(eventURL);
                IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, 8093);
                IPEndPoint DeviceEndPoint = new IPEndPoint(IPAddress.Parse(Parent.IPAddress), nHost.Port);
                Socket TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                TcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                TcpSocket.Bind(LocalEndPoint);
                string SearchString = "SUBSCRIBE " + eventURL +" HTTP/1.1\r\nHOST:" + Parent.IPAddress +":" + nHost.Port.ToString() + "\r\nNT:upnp:event\r\nCALLBACK: <" + CallBackUrl +"/" + ServiceIdent +">\r\nTIMEOUT:Second-" + Duration + "\r\n\r\n";
                TcpSocket.Connect(DeviceEndPoint);
                TcpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, DeviceEndPoint);
                bool Running = true;
                byte[] ReceiveBuffer = new byte[4000];
                int ReceivedBytes = 0;
                while (Running)
                {
                    if (TcpSocket.Available > 0)
                    {
                        ReceivedBytes = TcpSocket.Receive(ReceiveBuffer, SocketFlags.None);
                        if (ReceivedBytes > 0)
                        {
                            string Data = Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes);
                            string resp = Data.Split(new[] { '\r', '\n' }).FirstOrDefault();
                            string[] respItem = resp.Split(' ');
                            if (respItem[1] == "200" & respItem[2] == "OK")
                            {
                                _Log.AddMessage("Subscription request returned: " + respItem[1] + " " + respItem[2], true);
                                int x, y;
                                x = Data.IndexOf("SID");
                                Data = Data.Substring(x, Data.Length - x);
                                resp = Data.Split(new[] { '\r', '\n' }).FirstOrDefault();
                                respItem = resp.Split(' ');
                                string sid = respItem[1];
                                y = Data.IndexOf("TIMEOUT");
                                Data = Data.Substring(y, Data.Length - y);
                                resp = Data.Split(new[] { '\r', '\n' }).FirstOrDefault();
                                respItem = resp.Split(' ');
                                string timo = respItem[1];
                                x = Data.IndexOf("<?xml");
                                _Log.AddMessage("SID issued: " + sid + " - Timeout: " + timo, false);
                                EventDevice nEventedDevice = new EventDevice();
                                nEventedDevice.Device = Parent;
                                nEventedDevice.EventUrl = eventURL;
                                nEventedDevice.ID = sid;
                                nEventedDevice.Service = ServiceIdent;
                                nEventedDevice.Timeout = timo;
                                EventedDevices.Add(nEventedDevice);
                                _Log.AddMessage("The Service: " + ServiceIdent + " for device: " + Parent.Name + " was added to the EventedDevices list.", false);
                            }
                            else
                            {
                                _Log.AddMessage("Subscription request returned" + respItem[1] + " " + respItem[2], true);
                            }
                            Running = false;
                        }
                    }
                }
                TcpSocket.Shutdown(SocketShutdown.Both);
                TcpSocket.Disconnect(true);
                TcpSocket.Close();
                TcpSocket.Dispose();
            }
            #endregion

            #region Event Un-Subscription
            /// <summary>
            /// Sends the UNSUBSCRIBE information to the parent device for Event Handeling
            /// </summary>
            /// <param name="deviceName">The Sony device name to send the UNSUBSCRIBE to.</param>
            /// <param name="serviceIdent">The Sony device UPnP/DLNA ServiceIdentifier to UNSUBSCRIBE to.</param>
            public void UnSubscribeToEvents(string deviceName, string serviceIdent)
            {
                
                string eventURL = "";
                EventDevice sED = new EventDevice();
                if (this.IsRunning == false)
                {
                    _Log.AddMessage("Event Server is NOT running. Please start Server before Un-Subscribing to Events", true);
                }
                try
                {
                    sED = EventedDevices.Find(x => x.Device.Name == deviceName & x.Service == serviceIdent);
                    eventURL = sED.EventUrl;
                    if (string.IsNullOrEmpty(eventURL))
                    {
                        _Log.AddMessage("Cant not Unsubscribe to service: " + serviceIdent, true);
                        return;
                    }
                }
                catch
                {
                    _Log.AddMessage("The service: " + serviceIdent + " for device: " + deviceName + " is not currently Subscribed to the Event Server.", true);
                    return;
                }
                _Log.AddMessage("Sending UnSubscription Request to: " + eventURL, true);

                Uri nHost = new Uri(eventURL);
                IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, 8093);
                IPEndPoint DeviceEndPoint = new IPEndPoint(IPAddress.Parse(sED.Device.IPAddress), nHost.Port);
                Socket TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                TcpSocket.SetSocketOption(SocketOptionLevel.Socket, SocketOptionName.ReuseAddress, true);
                TcpSocket.Bind(LocalEndPoint);
                string SearchString = "UNSUBSCRIBE " + eventURL + " HTTP/1.1\r\nHOST:" + sED.Device.IPAddress + ":" + nHost.Port.ToString() + "\r\nSID: " + sED.ID + "\r\n\r\n";
                TcpSocket.Connect(DeviceEndPoint);
                TcpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, DeviceEndPoint);
                bool Running = true;
                byte[] ReceiveBuffer = new byte[4000];
                int ReceivedBytes = 0;
                while (Running)
                {
                    if (TcpSocket.Available > 0)
                    {
                        ReceivedBytes = TcpSocket.Receive(ReceiveBuffer, SocketFlags.None);
                        if (ReceivedBytes > 0)
                        {
                            string Data = Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes);
                            string resp = Data.Split(new[] { '\r', '\n' }).FirstOrDefault();
                            string[] respItem = resp.Split(' ');
                            if (respItem[1] == "200" & respItem[2] == "OK")
                            {
                                _Log.AddMessage("Unsubscription request returned: " + respItem[1] + " " + respItem[2], true);
                                EventedDevices.Remove(sED);
                                _Log.AddMessage("The Service: " + sED.Service + " for device: " + sED.Device.Name + " was removed from the EventedDevices list.", false);
                            }
                            else
                            {
                                _Log.AddMessage("UnSubscription request returned" + respItem[1] + " " + respItem[2], true);
                            }
                            Running = false;
                        }
                    }
                }
                TcpSocket.Shutdown(SocketShutdown.Both);
                TcpSocket.Disconnect(true);
                TcpSocket.Close();
                TcpSocket.Dispose();
                
            }
            #endregion

            #region Event Re-Subscription
            /// <summary>
            /// Sends the RESUBSCRIBE information to the parent device for Event Handeling
            /// </summary>
            /// <param name="DeviceName">The Sony device to send the RESUBSCRIBE to.</param>
            /// <param name="ServiceIdent">The Sony device UPnP/DLNA ServiceIdentifier to RESUBSCRIBE to.</param>
            public void ReSubscribeToEvents(string DeviceName, string ServiceIdent)
            {
                if (this.IsRunning == false)
                {
                    _Log.AddMessage("Event Server is NOT running. Please start Server before Re-Subscribing to Events", true);
                }
                string eventURL = "";
                EventDevice sED = new EventDevice();
                if (this.IsRunning == false)
                {
                    _Log.AddMessage("Event Server is NOT running. Please start Server before Re-Subscribing to Events", true);
                }
                try
                {
                    sED = EventedDevices.Find(x => x.Device.Name == DeviceName & x.Service == ServiceIdent);
                    eventURL = sED.EventUrl;
                    if (string.IsNullOrEmpty(eventURL))
                    {
                        _Log.AddMessage("Cant not Resubscribe to service: " + ServiceIdent, true);
                        return;
                    }
                }
                catch
                {
                    _Log.AddMessage("The service: " + ServiceIdent + " for device: " + DeviceName + " is not currently Subscribed to the Event Server.", true);
                    return;
                }
                _Log.AddMessage("Sending ReSubscription Request to: " + eventURL, true);
                int Duration = Convert.ToInt32(sED.Timeout.Substring(7));
                Uri nHost = new Uri(eventURL);
                IPEndPoint LocalEndPoint = new IPEndPoint(IPAddress.Any, 8093);
                IPEndPoint DeviceEndPoint = new IPEndPoint(IPAddress.Parse(sED.Device.IPAddress), nHost.Port);
                Socket TcpSocket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
                TcpSocket.SetSocketOption(SocketOptionLevel.Tcp, SocketOptionName.ReuseAddress, true);
                TcpSocket.Bind(LocalEndPoint);
                string SearchString = "SUBSCRIBE " + eventURL + " HTTP/1.1\r\nHOST:" + sED.Device.IPAddress + ":" + nHost.Port.ToString() + "\r\nSID: " + sED.ID + "\r\nTIMEOUT:Second-" + Duration + " >\r\n\r\n";
                if (TcpSocket.Connected == false)
                {
                    TcpSocket.Connect(DeviceEndPoint);
                }
                TcpSocket.SendTo(Encoding.UTF8.GetBytes(SearchString), SocketFlags.None, DeviceEndPoint);
                bool Running = true;
                byte[] ReceiveBuffer = new byte[4000];
                int ReceivedBytes = 0;
                while (Running)
                {
                    if (TcpSocket.Available > 0)
                    {
                        ReceivedBytes = TcpSocket.Receive(ReceiveBuffer, SocketFlags.None);
                        if (ReceivedBytes > 0)
                        {
                            string Data = Encoding.UTF8.GetString(ReceiveBuffer, 0, ReceivedBytes);
                            string resp = Data.Split(new[] { '\r', '\n' }).FirstOrDefault();
                            string[] respItem = resp.Split(' ');
                            if (respItem[1] == "200" & respItem[2] == "OK")
                            {
                                _Log.AddMessage("Resubscription request returned: " + respItem[1] + " " + respItem[2], true);
                                _Log.AddMessage("The Service: " + sED.Service + " for device: " + sED.Device.Name + " was renewed in the EventedDevices list.", false);
                                _Log.AddMessage(Data, true);
                            }
                            else
                            {
                                _Log.AddMessage("Subscription request returned" + respItem[1] + " " + respItem[2], true);
                            }
                            Running = false;
                        }
                    }
                }
                TcpSocket.Shutdown(SocketShutdown.Both);
                TcpSocket.Disconnect(true);
                TcpSocket.Close();
                TcpSocket.Dispose();
            }
            #endregion
            #endregion

            #region Private Methods
            #region On Property Change
            private void OnPropertyChanged(PropertyChangedEventArgs e)
            {
                PropertyChangedEventHandler handler = PropertyChanged;
                if (handler != null)
                    handler(this, e);
            }


            private void OnPropertyChanged(string propertyName)
            {
                OnPropertyChanged(new PropertyChangedEventArgs(propertyName));
            }
            #endregion

            #region Accept Call Back
            private void AcceptCallback(IAsyncResult ar)
            {
                TcpListener listener = (TcpListener)ar.AsyncState;
                TcpClient client = listener.EndAcceptTcpClient(ar);

                byte[] buffer = new byte[1024];
                NetworkStream ns = client.GetStream();
                if (ns.CanRead)
                {
                    ns.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallback), new object[] { ns, buffer });
                }

                connected.Set();
            }
            #endregion

            #region Read Call Back
            private void ReadCallback(IAsyncResult ar)
            {
                NetworkStream ns = (NetworkStream)((ar.AsyncState as object[])[0]);
                byte[] buffer = (byte[])((ar.AsyncState as object[])[1]);
                int n = ns.EndRead(ar);
                if (n > 0)
                {
                    //_Log.AddMessage(Encoding.ASCII.GetString(buffer, 0, n), true);
                }
                ns.BeginRead(buffer, 0, buffer.Length, new AsyncCallback(ReadCallback), new object[] { ns, buffer });
                ProcessMessage(Encoding.ASCII.GetString(buffer, 0, n));
            }
            #endregion

            #region Process Message
            private void ProcessMessage(string msg)
            {
                string data = msg;
                string resp = msg.Split(new[] { '\r', '\n' }).FirstOrDefault();
                string[] respItem = resp.Split(' ');
                if (respItem[0] == "NOTIFY")
                {
                    cEventObj = new EventObject();
                    cEventObj.ServiceID = respItem[1].Substring(1);
                    int y;
                    y = msg.IndexOf("NTS:");
                    msg = msg.Substring(y, msg.Length - y);
                    resp = msg.Split(new[] { '\r', '\n' }).FirstOrDefault();
                    respItem = resp.Split(' ');
                    cEventObj.Action = respItem[1].Substring(5);
                    y = msg.IndexOf("SID");
                    msg = msg.Substring(y, msg.Length - y);
                    resp = msg.Split(new[] { '\r', '\n' }).FirstOrDefault();
                    respItem = resp.Split(' ');
                    cEventObj.Uid = respItem[1];
                    y = msg.IndexOf("SEQ:");
                    msg = msg.Substring(y, msg.Length - y);
                    resp = msg.Split(new[] { '\r', '\n' }).FirstOrDefault();
                    respItem = resp.Split(' ');
                    cEventObj.Sequence = respItem[1];
                    sED = new EventDevice();
                    sED = EventedDevices.Find(x => x.ID == cEventObj.Uid);
                    cEventObj.DeviceName = sED.Device.Name;
                    _Log.AddMessage("Event Server received a Notification Message from " + sED.Device.Name, true);
                    _Log.AddMessage(cEventObj.ServiceID + " Event Notification received.", false);
                    _Log.AddMessage("Event type: " + cEventObj.Action, false);
                }
                else if (respItem[0] == "<?xml")
                {
                    sED = EventedDevices.Find(x => x.ID == cEventObj.Uid);
                    msg = WebUtility.HtmlDecode(msg);
                    _Log.AddMessage("XML CONTENT: \r\n" + msg, false);
                    XDocument doc = XDocument.Parse(msg);
                    foreach (XElement element in doc.Descendants("LastChange"))
                    {
                        foreach (XElement cEle in element.Descendants())
                        {
                            if (cEle.LastAttribute.ToString().Contains("val"))
                            {
                                int sc = cEle.Name.ToString().IndexOf("}");
                                EventVariable eV = new EventVariable();
                                eV.name = cEventObj.ServiceID + "::" + cEle.Name.ToString().Substring(sc + 1);
                                string aVal = cEle.LastAttribute.ToString();
                                aVal = aVal.Replace("val=", "");
                                aVal = aVal.Replace("\"", "");
                                eV.value = aVal;
                                cEventObj.StateVariables.Add(eV);
                            }
                        }
                    }
                    foreach(EventVariable e in cEventObj.StateVariables)
                    {
                        _Log.AddMessage("State Variable " + e.name + " is now set to " + e.value, false);
                    }
                    sED.Device.ProcessEventMessages(cEventObj);
                    Output = msg;
                }
            }
            #endregion

            #region End All Subscriptions
            private void EndAllSubscriptions()
            {
                List<EventDevice> endALL = new List<EventDevice>();
                foreach (EventDevice eD in EventedDevices)
                {
                    endALL.Add(eD);
                }
                    foreach (EventDevice eD in endALL)
                {
                    UnSubscribeToEvents(eD.Device.Name, eD.Service);
                }
            }
            #endregion

            #region Get Event Url from Parent Device
            private string GetEventUrl(SonyDevice parent, string servIdent)
            {
                string eventURL = "";
                switch (servIdent)
                {
                    case "IRCC:1":
                        eventURL = parent.Ircc.EventSubUrl;
                        break;

                    case "RenderingControl:1":
                        eventURL = parent.RenderingControl.EventSubUrl;
                        break;

                    case "ConnectionManager:1":
                        eventURL = parent.ConnectionManager.EventSubUrl;
                        break;

                    case "AVTransport:1":
                        eventURL = parent.AVTransport.EventSubUrl;
                        break;

                    case "Party:1":
                        eventURL = parent.Party.EventSubUrl;
                        break;
                }
                if(string.IsNullOrEmpty(eventURL))
                {
                    _Log.AddMessage("The Event URL for service " + servIdent + " is Empty!", true);
                }
                return eventURL;
            }
            #endregion
            #endregion
        }
        
        #region Subscribed Event Devices
        public class EventDevice
        {
            public SonyDevice Device { get; set; }
            public string Service { get; set; }
            public string ID { get; set; }
            public string Timeout { get; set; }
            public string EventUrl { get; set; }
        }
        #endregion

        #region EventObject
        public class EventObject
        {
            public string DeviceName { get; set; }
            public string ServiceID { get; set; }
            public string Uid { get; set; }
            public string Action { get; set; }
            public string Sequence { get; set; }
            public List<EventVariable> StateVariables = new List<EventVariable>();
        }
        #endregion

        #region EventVeriables
        [Serializable]
        ///<summary>
        /// Holds the State Variables for each Event Notification
        /// </summary>
        public class EventVariable
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
        #endregion
    }
}
