using ManagedUPnP;
using ManagedUPnP.Descriptions;
using Newtonsoft.Json;
using System;
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
        public Sony_API API { get { return _API; } set { _API = value; } }
        /// <summary>
        /// Gets or Sets the Device Object
        /// </summary>
        public SonyDevice Device { get { return _Device; } set { _Device = value; } }
        /// <summary>
        /// Gets or Sets the UPnP Services discovered
        /// </summary>
        public Services mySony { get { return  _mySony; } set { _mySony = value; } }
        /// <summary>
        /// API Logging Object
        /// </summary>
        static APILogging _Log = new APILogging();
        /// <summary>
        /// API Object
        /// </summary>
        static Sony_API _API = new Sony_API();
        /// <summary>
        /// Device Object
        /// </summary>
        static SonyDevice _Device = new SonyDevice();
        /// <summary>
        /// UPnP Discovered Services
        /// </summary>
        static Services _mySony = _API.getAllServices();
        #endregion

        #region API Library
        // Sony API Library Class
        /// <summary>
        /// Class used to Discover Sony devices with the IRCC service via a LAN connection
        /// </summary>
        public partial class Sony_API
        {
            // Sony API Variables
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

            #region sonyDiscover

            /// <summary>
            /// sonyDiscover is used to scan and locate all compatiable devices
            /// </summary>
            /// <param name="service">Sends null as default. Use urn:schemas to find other service</param>
            /// <returns>A list of SonyDevices found</returns>
            [STAThread]
            public List<SonyDevice> sonyDiscover(string service = null)
            {
                //_mySony = getAllServices();
                if (service == null) { service = "IRCC:1"; }
                _Log.writetolog("UPnP is Discovering devices with a service of: " + service, true);
                List<SonyDevice> foundDevices = new List<SonyDevice>();
                foreach (Service mySonyServ in _mySony)
                {
                    SonyDevice fdev = new SonyDevice();
                    fdev.Name = mySonyServ.Device.FriendlyName;
                    string serName = mySonyServ.FriendlyServiceTypeIdentifier.ToString();
                    if (serName == service)
                    {
                        _Log.writetolog("Found Device: " + fdev.Name, false);
                        fdev.Device_IP_Address = mySonyServ.Device.RootHostAddresses[0].ToString();
                        foundDevices.Add(fdev);
                        _Log.writetolog(mySonyServ.Device.FriendlyName + " Added", true);
                    }
                }
                _Log.writetolog("Devices Discovered: " + foundDevices.Count.ToString(), true);
                return foundDevices;
            }
            #endregion

            #region getAllServices

            /// <summary>
            /// This method discovers all UPnP services on the LAN.
            /// </summary>
            /// <returns>All Services found</returns>
            public Services getAllServices()
            {
                Services lsServices = Discovery.FindServices(AddressFamilyFlags.IPvBoth, false);
                return lsServices;
            }
            #endregion            
        }
        #endregion

        #region Sony Device Class
        /// <summary>
        /// Sony Device Object used to Register and Control via Lan connection
        /// </summary>
        public partial class SonyDevice
        {
            #region Variables
            /// <summary>
            /// Gets or Sets the Sony Device Object Name
            /// </summary>
            public string Name { get; set; }
            /// <summary>
            /// Gets or Sets the Sony Device Object Generation 1, 2 or 3
            /// </summary>
            public int Generation { get; set; }
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
            /// Gets or Sets the IRCC UPnP service to send Commands to device
            /// </summary>
            public Service ircc { get; set; }
            /// <summary>
            /// This vairable will contain the retrieved X_XERS_ActionList_URL used in registration and IRCC commands.
            /// </summary>
            public string actionList_URL;
            /// <summary>
            /// This vairable will contain the IRCC controlURL
            /// </summary>
            public string control_URL;
            /// <summary>
            /// This vairable will contain the TV's Current Channel set with the setChannel method.
            /// </summary>
            public string current_Channel;
            /// <summary>
            /// This vairable will contain the TV's Current Volume set with the setVolume method.
            /// </summary>
            public string current_Volume;
            /// <summary>
            /// List of Device Commands
            /// </summary>
            SonyCommandList dataSet = new SonyCommandList();
            /// <summary>
            /// Socket object used with Gen3 devices
            /// </summary>
            Socket _socket = null;
            /// <summary>
            /// Default PIN code used with Gen3 devices
            /// </summary>
            public string pincode = "0000";
            /// <summary>
            /// Cookie container for Gen3 Devices
            /// </summary>
            CookieContainer allcookies = new CookieContainer();

            #endregion

            #region getAllServices

            /// <summary>
            /// This method discovers all UPnP services on the LAN.
            /// </summary>
            /// <returns>All Services found</returns>
            static Services getAllServices()
            {
                Services lsServices = Discovery.FindServices(AddressFamilyFlags.IPvBoth, false);
                return lsServices;
            }
            #endregion

            #region initialize
            /// <summary>
            /// Initializes a NEW Sony Device object with settings manually entered in to each Property.
            /// </summary>
            /// <remarks>Must set at least the Device Name and IP Address befotre executing initialize()</remarks>
            public void initialize()
            {
                if(this.Name == null | this.Device_IP_Address == null)
                {
                    // Throw Exception if NO Device Name or IP Address
                    throw new Exception("Initialization is Missing Information. Please set Device name and IP Address before executing");
                }
                // Initialize and set Device Variables
                if (this.actionList_URL == null)
                {
                    this.getActionlist_URL(this.Name);
                }
                if (this.Generation == 0)
                {
                    int x = Convert.ToInt32(getRegistrationMode());
                    this.Generation = x;
                    if (this.Device_Port == 0)
                    {
                        if (this.Generation <= 2)
                        {
                            string devIP = this.actionList_URL;
                            devIP = devIP.Replace("http://", "");
                            string s1 = ":";
                            int endIP = devIP.IndexOf(s1);
                            string port = devIP.Substring(endIP + 1);
                            devIP = devIP.Substring(0, endIP);
                            s1 = "/";
                            int endPort = port.IndexOf(s1);
                            port = port.Substring(0, endPort);
                            this.Device_Port = Convert.ToInt32(port);
                        }
                        else
                        {
                            this.Device_Port = 80;
                        }
                    }
                }
                else
                {
                    if (this.Device_Port == 0)
                    {
                        if (this.Generation <= 2)
                        {
                            string devIP = this.actionList_URL;
                            devIP = devIP.Replace("http://", "");
                            string s1 = ":";
                            int endIP = devIP.IndexOf(s1);
                            string port = devIP.Substring(endIP + 1);
                            devIP = devIP.Substring(0, endIP);
                            s1 = "/";
                            int endPort = port.IndexOf(s1);
                            port = port.Substring(0, endPort);
                            this.Device_Port = Convert.ToInt32(port);
                        }
                        else
                        {
                            this.Device_Port = 80;
                        }
                    }
                }
                _Log.writetolog("Initializing Device: " + this.Name + " @ " + this.Device_IP_Address + ":" + this.Device_Port.ToString(), true);
                if (this.Server_Name == null)
                {
                    this.Server_Name = System.Windows.Forms.SystemInformation.ComputerName + "(SonyAPILib)";
                }
                string service = "IRCC:1";
                foreach (Service mySonyServ in _mySony)
                {
                    if (mySonyServ.Device.FriendlyName == this.Name)
                    {
                        string serName = mySonyServ.FriendlyServiceTypeIdentifier.ToString();
                        if (serName == service)
                        {
                            this.ircc = mySonyServ;
                        }
                    }
                }
                if (this.Server_Macaddress == null)
                {
                    this.Server_Macaddress = getControlMac();
                }
                if (this.Generation == 3)
                {
                    if (this.Device_Macaddress == null)
                    {
                        this.Device_Macaddress = findDevMac().ToString();
                    }
                }
                this.getControlURL(this.Name);
                this.get_remote_command_list();
                this.checkReg();
                _Log.writetolog("Device Initialized: " + this.Name, true);
            }
            
            
            /// <summary>
            /// Initializes a NEW Sony Device object with settings from a device retrieved from sonyDiscover(device).
            /// </summary>
            /// <param name="device">A Sony Device object selected from the list obtained with sonyDiscover(device) method.</param>
            public void initialize(SonyDevice device)
            {
                // Initialize and set Device Variables based on another Sony Device Object.
                
                // Name is Required
                this.Name = device.Name;
                // Name is Required
                this.Device_IP_Address = device.Device_IP_Address;
                
                if (this.Name == null | this.Device_IP_Address == null)
                {
                    // Throw Exception if NO Device Name or IP Address
                    throw new Exception("Initialization is Missing Information. Please set Device name and IP Address before executing");
                }
                // Set Action List URL
                if (device.actionList_URL == null)
                {
                    this.getActionlist_URL(this.Name);
                }
                else
                {
                    this.actionList_URL = device.actionList_URL;
                }
                // Set Generation
                if (device.Generation == 0)
                {
                    int x = Convert.ToInt32(getRegistrationMode());
                    this.Generation = x;
                    // Set port if not Provided
                    if (device.Device_Port == 0)
                    {
                        if (x <= 2)
                        {
                            string devIP = this.actionList_URL;
                            devIP = devIP.Replace("http://", "");
                            string s1 = ":";
                            int endIP = devIP.IndexOf(s1);
                            string port = devIP.Substring(endIP + 1);
                            devIP = devIP.Substring(0, endIP);
                            s1 = "/";
                            int endPort = port.IndexOf(s1);
                            port = port.Substring(0, endPort);
                            this.Device_Port = Convert.ToInt32(port);
                        }
                        else
                        {
                            this.Device_Port = 80;
                        }
                    }
                    else
                    {
                        this.Device_Port = device.Device_Port;
                    }
                }
                else
                {
                    this.Generation = device.Generation;
                    if (device.Device_Port == 0)
                    {
                        if (this.Generation <= 2)
                        {
                            string devIP = this.actionList_URL;
                            devIP = devIP.Replace("http://", "");
                            string s1 = ":";
                            int endIP = devIP.IndexOf(s1);
                            string port = devIP.Substring(endIP + 1);
                            devIP = devIP.Substring(0, endIP);
                            s1 = "/";
                            int endPort = port.IndexOf(s1);
                            port = port.Substring(0, endPort);
                            this.Device_Port = Convert.ToInt32(port);
                        }
                        else
                        {
                            this.Device_Port = 80;
                        }
                    }
                    else
                    {
                        this.Device_Port = device.Device_Port;
                    }
                }
                _Log.writetolog("Initializing Device: " + this.Name + " @ " + this.Device_IP_Address + ":" + this.Device_Port.ToString(), true);
                if (device.Server_Name == null)
                {
                    this.Server_Name = System.Windows.Forms.SystemInformation.ComputerName + "(SonyAPILib)";
                }
                string service = "IRCC:1";
                foreach (Service mySonyServ in _mySony)
                {
                    if(mySonyServ.Device.FriendlyName == this.Name)
                    {
                        string serName = mySonyServ.FriendlyServiceTypeIdentifier.ToString();
                        if (serName == service)
                        {
                            this.ircc = mySonyServ;
                        }
                    }
                }
                this.Server_Macaddress = getControlMac();
                if (this.Generation == 3) 
                { 
                    this.Device_Macaddress = findDevMac().ToString();
                }
                this.getControlURL(this.Name);
                this.get_remote_command_list();
                this.checkReg();
                _Log.writetolog(" Device Initialized: " + this.Name, true);
            }
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
                if (this.Generation == 1)
                {
                    reg = HttpGet(getActionlist("register", actionList_URL) + "?" + args1);
                    _Log.writetolog("Registering Generation 2 Sony Device", false);
                }
                else if (this.Generation == 2)
                {
                    reg = HttpGet(getActionlist("register", actionList_URL) + "?" + args2);
                    _Log.writetolog("Registering Generation 1 Sony Device", false);
                }
                else if (this.Generation == 3)
                {
                    _Log.writetolog("Registering Generation 3 Sony Sevice", false);
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
                            httpWebRequest.GetResponse();
                        }
                        catch { }
                        _Log.writetolog("Must run Method: SendAuth(pincode)", true);
                        reg = "Gen3 Pin Code Required";
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
                        reg = true;
                    }
                    string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + this.Device_IP_Address + "/sony/appControl")));
                    System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
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
                if (this.Generation <= 2)
                {
                    _Log.writetolog(this.Name + " is Retrieving Generation:" + this.Generation + " Remote Command List", false);
                    cmdList = HttpGet(getActionlist("getRemoteCommandList", actionList_URL));
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
                    _Log.writetolog(this.Name + " is Retrieving Generation:" + this.Generation + " Remote Command List", false);
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
                //Url = "http://" + Host + ":" + Port + Url;
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
            private string HttpPost(string Url, String Parameters)
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
                if (this.Generation == 3)
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
                if (this.Generation != 3)
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
            private bool checkReg()
            {
                bool results = false;
                
                // Gen 1 or 2 Devices
                if (this.Generation <= 2)
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
                        _Log.writetolog("Checking for Generation 3 Cookie", false);
                        System.IO.StreamReader myFile = new System.IO.StreamReader("cookie.json");
                        string myString = myFile.ReadToEnd();
                        myFile.Close();
                        List<SonyCookie> bal = JsonConvert.DeserializeObject<List<SonyCookie>>(myString);
                        
                        // Check if cookie has expired
                        _Log.writetolog(this.Name + ": Checking if Cookie is Expired: " + bal[0].Expires, false);
                        DateTime CT = DateTime.Now;
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
                                //string authInfo = "" + ":" + pincode;
                                //authInfo = Convert.ToBase64String(Encoding.Default.GetBytes(authInfo));
                                //httpWebRequest2.Headers["Authorization"] = "Basic " + authInfo;
                                var httpResponse = (HttpWebResponse)httpWebRequest2.GetResponse();
                                using (var streamReader = new StreamReader(httpResponse.GetResponseStream()))
                                {
                                    var responseText = streamReader.ReadToEnd();
                                    _Log.writetolog("Registration response: " + responseText, false);
                                }
                                string answerCookie = JsonConvert.SerializeObject(httpWebRequest2.CookieContainer.GetCookies(new Uri("http://" + this.Device_IP_Address + "/sony/appControl")));
                                System.IO.StreamWriter file = new System.IO.StreamWriter("cookie.json");
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
                            allcookies.Add(new Uri(@"http://" + this.Device_IP_Address + bal[0].Path), new Cookie(bal[0].Name, bal[0].Value));
                            this.Cookie = myString;
                            this.Registered = true;
                            results = true;
                            _Log.writetolog(this.Name + ": Cookie Found: auth=" + this.Cookie, false);
                        }
                    }
                    catch 
                    {
                        _Log.writetolog("No Cookie was found", false);
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
                if (this.Generation != 3)
                {
                    try
                    {
                        _Log.writetolog("Checking Status of Device " + this.Name, false);
                        string cstatus;
                        int x;
                        cstatus = HttpGet(getActionlist("getStatus", actionList_URL));
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

            #region getControlURL
            /// <summary>
            /// This method gets the IRCC Service Control URL from the description file.
            /// </summary>
            /// <param name="devname">A string containing the name of the device to retreive the URL from</param>
            /// <returns>Returns the URL used to send IRCC commands to device</returns>
            private void getControlURL(string devname)
            {
                string foundVal = "";
                bool fnd = false;
                _Log.writetolog("Retrieving Control URL from Device", true);
                foreach (Service mySonyServ in _mySony)
                {
                    if (mySonyServ.Device.FriendlyName == devname)
                    {
                        if (mySonyServ.FriendlyServiceTypeIdentifier.ToString() == "IRCC:1")
                        {
                            _Log.writetolog("Found UPnP IRCC:1 on device:" + devname, false);
                            DeviceServiceDescription ldsdDescription = mySonyServ.DeviceServiceDescription(mySonyServ.Device.RootDeviceDescription());
                            foundVal = ldsdDescription.GetPropertyString("controlURL");
                            _Log.writetolog("Found Control URL: " + foundVal, false);
                            if (foundVal.StartsWith("http://"))
                            {
                                //Full path already exist
                                _Log.writetolog("Control URL is complete: " + foundVal, false);
                                this.control_URL = foundVal;
                            }
                            else
                            {
                                //Complete the Path
                                _Log.writetolog("Completeing Control URL", false);
                                string docURL = mySonyServ.Device.DocumentURL.ToString();
                                string devIP = docURL;
                                devIP = devIP.Replace("http://", "");
                                string s1 = ":";
                                int endIP = devIP.IndexOf(s1);
                                string port = devIP.Substring(endIP + 1);
                                devIP = devIP.Substring(0, endIP);
                                s1 = "/";
                                int endPort = port.IndexOf(s1);
                                port = port.Substring(0, endPort);
                                foundVal = @"http://" + this.Device_IP_Address + ":" + port + foundVal;
                                this.control_URL = foundVal;
                                _Log.writetolog("Control URL is Complete: " + control_URL, false);
                            }
                            fnd = true;
                        }
                    }
                    if (fnd == true) { break; }
                }
            }
            #endregion

            #region getActionList

            /// <summary>
            /// This method retrieves the DLNA Device Action List XML.
            /// </summary>
            /// <param name="lAction">This is a string containing the Action name to retrieve</param>
            /// <param name="actionList_URL">URL For this devices Action List</param>
            /// <returns></returns>
            /// 
            private string getActionlist(string lAction, string actionList_URL)
            {
                string foundVal = "";
                if (actionList_URL != "")
                {
                    DataSet acList = new DataSet();
                    //string actionURL = "http://" + Host + ":" + Port + actionList_URL.ToString();
                    string actionURL = actionList_URL.ToString();
                    acList.ReadXml(actionURL);
                    DataTable act = new DataTable();
                    act = acList.Tables[0];
                    var results = from DataRow myRow in act.Rows where myRow.Field<string>("name") == lAction select myRow;
                    foundVal = results.ElementAt(0).Field<string>("url");
                }
                return foundVal;
            }
            #endregion

            #region getActionList_URL

            /// <summary>
            /// This method sets the Action List URL found in the DLNA Device Action List XML.
            /// </summary>
            /// <param name="devName">This is the Name of the Device to get the infromation for</param>
            /// 
            private void getActionlist_URL(string devName)
            {
                string foundVal = "";
                bool fnd = false;
                _Log.writetolog("Retrieving Action List URL from device description file: " + devName, false);
                foreach (Service mySonyServ in _mySony)
                {
                    string DevN = mySonyServ.Device.FriendlyName.ToString();
                    if (devName == DevN)
                    {
                        Device gDev = mySonyServ.Device;
                        IEnumerable<KeyValuePair<string, string>> lhsUnused = gDev.GetDescription(gDev.RootDeviceDescription()).GetUnusedProperties();
                        foreach (KeyValuePair<string, string> prop in lhsUnused)
                        {
                            if (prop.Key == "av:X_CERS_ActionList_URL")
                            {
                                foundVal = prop.Value.ToString();
                                fnd = true;
                                _Log.writetolog("Action List URL found: " + foundVal, false);
                                break;
                            }
                        }
                    }
                    if (fnd == true) { break; }
                }
                if (foundVal == "") { _Log.writetolog("Action List URL was NOT found!", false); }
                this.actionList_URL = foundVal;
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
                if (actionList_URL != "")
                {
                    acList.ReadXml(actionList_URL);
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

            #region Get Device Mac
            /// <summary>
            /// Method used to retrieve Gen3 Devices Mac Address
            /// </summary>
            /// <returns></returns>
            private string findDevMac()
            {
                String macaddress = "";
                _Log.writetolog("Retrieving the Mac Address from: " + this.Name + " at IP: " + this.Device_IP_Address, true);
                var httpWebRequest = (HttpWebRequest)WebRequest.Create(@"http://" + this.Device_IP_Address + @"/sony/system");
                httpWebRequest.ContentType = "text/json";
                httpWebRequest.Method = "POST";
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

            #region getControlMac

            /// <summary>
            /// getControlMac retrieves the MAC address from the static method GetMacAddress().
            /// </summary>
            /// <returns>A string containing a processed MAC address. 
            /// For example: Actual Mac 01:02:03:04:05:06 returns 01-02-03-04-05-06</returns>
            private string getControlMac()
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

            #region Send IRCC commands
            /// <summary>
            /// This method sends an IRCC command value via an HTTP POST command using SOAP encoding
            /// </summary>
            /// <param name="irccCode">A string containing a valid IRCC command value retrieved from get_remote_commnd_list(), or getIRCCCommandString(command name)</param>
            /// <returns>Returns the device response as a string</returns>
            public string send_ircc(string irccCode)
            {
                _Log.writetolog("Recieved IRCC Command: " + irccCode, false);
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
                response = HttpPost(this.control_URL, body.ToString());
                if (response != "")
                {
                    _Log.writetolog("Command WAS sent Successfully", true);
                }
                else
                {
                    _Log.writetolog("Command was NOT sent successfully", true);
                }
                return response;
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
                if (this.Generation < 3)
                {
                    _Log.writetolog("Sending TEXT to device", false);
                    response = HttpGet(getActionlist("sendText", actionList_URL) + "?text=" + sendtext);
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
                string response = HttpGet(getActionlist("getText", actionList_URL));
                return response;
            }
            #endregion

            #region Convert mdf commands

            private void send_ircc_mdf(Int32 manu, Int32 device, Int32 function)
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
                send_ircc(pLEncoded);
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
                send_ircc(getIRCCcommandString("VolumeUp"));
            }
            #endregion

            #region Volume Down
            /// <summary>
            /// Sends IRCC value for Volume Down
            /// </summary>
            public void volume_down()
            {
                send_ircc(getIRCCcommandString("VolumeDown"));
            }
            #endregion

            #region Channel Up
            /// <summary>
            /// Sends IRCC value for Channel Up
            /// </summary>
            public void channel_up()
            {
                send_ircc(getIRCCcommandString("ChannelUp"));
            }
            #endregion

            #region Channel Down
            /// <summary>
            /// Sends IRCC value for Channel Down
            /// </summary>
            public void channel_down()
            {
                send_ircc(getIRCCcommandString("ChannelDown"));
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
                    send_ircc(ircc_cmd);
                    System.Threading.Thread.Sleep(250);
                }
                this.current_Channel = channel;
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
            #region Variables
            /// <summary>
            /// Gets or Sets Enabling cerDevice API Logging
            /// True - Turns Loggin On
            /// False - Turns Loggin Off
            /// </summary>
            public bool enableLogging { get; set; }

            /// <summary>
            /// Gets or Sets Enabling cerDevice API Logging Level
            /// Basic - for only basic entries
            /// All - for all entries
            /// </summary>
            public string enableLogginglev { get; set; }

            /// <summary>
            /// Gets or Sets the cerDevice API logging path
            /// Destination Folder MUST exist.
            /// Must be Full Path. ex: c:\programdata\sony\
            /// </summary>
            public string loggingPath { get; set; }

            /// <summary>
            /// Gets or Sets the cerDevice API logging file name
            /// Must be a .txt file
            /// default is cerDevice_LOG.txt
            /// </summary>
            public string loggingName { get; set; }
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
    }
}
