using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Threading;
using SonyAPILib;


namespace DLNA_Control_Panel
{
    public partial class Form1 : Form
    {
        public static SonyAPI_Lib mySonyLib = new SonyAPI_Lib();
        public static List<SonyAPI_Lib.SonyDevice> fDev = new List<SonyAPI_Lib.SonyDevice>();
        public static SonyAPI_Lib.SonyDevice curDev = new SonyAPI_Lib.SonyDevice();
        
        public Form1()
        {
            InitializeComponent();
            mySonyLib.LOG.Enable = true;
            mySonyLib.LOG.Level = "All";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoggEnabled.Text = mySonyLib.LOG.Enable.ToString();
            LogLevel.Text = mySonyLib.LOG.Level.ToString();
            LogName.Text = mySonyLib.LOG.Name.ToString();
            LogPath.Text = mySonyLib.LOG.Path.ToString();
            UpdateLog(mySonyLib.LOG.Path.ToString(), mySonyLib.LOG.Name.ToString());
        }

        private void UpdateLog(String Path, String Name)
        {
            string dir = @Path + Name;
            System.IO.StreamReader myFile = new System.IO.StreamReader(dir);
            string myString = myFile.ReadToEnd();
            myFile.Close();
            LogDetails.Text = myString;
        }

        private void ShowWait1()
        {
            pWait1.Visible = true;
            pWait1.Invalidate();
            pWait1.Update();
            Cursor.Current = Cursors.WaitCursor;
        }

        private void HideWait1()
        {
            pWait1.Visible = false;
            Cursor.Current = Cursors.Default;
        }

        private void LoadBut_Click(object sender, EventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = Application.StartupPath;
            ofd.RestoreDirectory = true;
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                ShowWait1();
                //deserialize
                string fpth = ofd.FileName;
                curDev = mySonyLib.Locator.DeviceLoad(fpth);
                ofd.Dispose();
                fDev.Add(curDev);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
                UpdateDeviceList();
                SaveBut.Enabled = true;
                HideWait1();
            }
        }

        private void BuildBut_Click(object sender, EventArgs e)
        {
            if (bDocURL.Text != "")
            {
                ShowWait1();
                curDev.buildFromDocument(new Uri(bDocURL.Text));
                fDev.Add(curDev);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
                UpdateDeviceList();
                SaveBut.Enabled = true;
                HideWait1();
            }
            else
            {
                MessageBox.Show("Device Document URL is Blank. Please enter URL.", "Error - No Device Document URL");
            }
        }

        private void LocateBut_Click(object sender, EventArgs e)
        {
            ShowWait1();
            List<string> foundDevices = new List<string>();
            foundDevices = mySonyLib.Locator.locateDevices();
            foreach (string d in foundDevices)
            {
                LocatedDevices.Text = LocatedDevices.Text + d + Environment.NewLine;
                UpdateLog(LogPath.Text, LogName.Text);
            }
            HideWait1();
        }

        private void ClearLogBut_Click(object sender, EventArgs e)
        {
            if (saveLogName.Text == "")
            {
                mySonyLib.LOG.clearLog(null);
            }
            else
            {
                mySonyLib.LOG.clearLog(saveLogName.Text);
            }
            UpdateLog(LogPath.Text, LogName.Text);
        }

        private void LogWriteBut_Click(object sender, EventArgs e)
        {
            mySonyLib.LOG.writetolog(LogLine.Text, true);
            UpdateLog(LogPath.Text, LogName.Text);
        }

        private void UpdateDevice()
        {
            devName.Text = curDev.Name;
            devIP.Text = curDev.Device_IP_Address;
            devPort.Text = curDev.Device_Port.ToString();
            devMac.Text = curDev.Device_Macaddress;
            devServName.Text = curDev.Server_Name;
            devServMac.Text =  curDev.Server_Macaddress;
            devManuf.Text =  curDev.Manufacture;
            devModel.Text =  curDev.ModelName;
            devModnum.Text =  curDev.ModelNumber;
            devUDN.Text =  curDev.UDN;
            devActionList.Text =  curDev.Actionlist_Url;
            devRegistered.Text =  curDev.Registered.ToString();
            devType.Text =  curDev.DeviceType;
            devMDesc.Text =  curDev.ModelDescription;
            devDocumentUrl.Text =  curDev.DocumentURL;
            devRegMode.Text =  curDev.Actionlist.RegisterMode.ToString();
            devCookie.Text =  curDev.Cookie;

            UpdateIRCC();
            UpdateRenderingControl();
            UpdateAVTransport();
        }

        private void UpdateIRCC()
        {
            if ( curDev.IRCC.controlURL != null)
            {
                irccservControl.Text =  curDev.IRCC.controlURL;
                irccservEvent.Text =  curDev.IRCC.eventSubURL;
                irccservSCPD.Text =  curDev.IRCC.SCPDURL;
                irccservType.Text =  curDev.IRCC.serviceType;
                irccservId.Text =  curDev.IRCC.serviceID;
                irccservIdent.Text =  curDev.IRCC.friendlyServiceIdentifier;
                irccservLastChange.Text =  curDev.IRCC.sv_LastChange;
                irccservCurStatus.Text =  curDev.IRCC.sv_CurrentStatus;
                irccservCmdCount.Text =  curDev.Commands.Count().ToString();
                irccservCmds.DataSource =  curDev.Commands;
                irccservCmds.DisplayMember = "Name";
                irccservCmds.ValueMember = "Value";
            }
        }

        private void UpdateRenderingControl()
        {
            if (curDev.RenderingControl.controlURL != null)
            {
                rendservControl.Text = curDev.RenderingControl.controlURL;
                rendservEvent.Text = curDev.RenderingControl.eventSubURL;
                rendservSCPD.Text = curDev.RenderingControl.SCPDURL;
                rendservType.Text = curDev.RenderingControl.serviceType;
                rendservId.Text = curDev.RenderingControl.serviceID;
                rendservIdent.Text = curDev.RenderingControl.friendlyServiceIdentifier;
                rendservLastChange.Text = curDev.RenderingControl.sv_LastChange;
                rendservPreList.Text = curDev.RenderingControl.sv_PresetNameList;
                rendservMute.Text = curDev.RenderingControl.sv_Mute.ToString();
                rendservVolume.Text = curDev.RenderingControl.sv_Volume.ToString();
                rendservInstId.Text = curDev.RenderingControl.sv_InstanceID.ToString();
                rendservPreName.Text = curDev.RenderingControl.sv_PresetName;
                rendservChan.Text = curDev.RenderingControl.sv_Channel;
            }
        }

        private void UpdateAVTransport()
        {
            if (curDev.AVTransport.controlURL != null)
            {
                avtransservLastChange.Text = curDev.AVTransport.sv_LastChange;
                avtransservControl.Text = curDev.AVTransport.controlURL;
                avtransservEvent.Text = curDev.AVTransport.eventSubURL;
                avtransservSCPD.Text = curDev.AVTransport.SCPDURL;
                avtransservType.Text = curDev.AVTransport.serviceType;
                avtransservId.Text = curDev.AVTransport.serviceID;
                avtransservIdent.Text = curDev.AVTransport.friendlyServiceIdentifier;
                avtransservState.Text = curDev.AVTransport.sv_TransportState;
                avtransservStatus.Text = curDev.AVTransport.sv_TransportStatus;
                avtransservPlaybackStorage.Text = curDev.AVTransport.sv_PlayBackStorageMedium;
                avtransservRecordStorage.Text = curDev.AVTransport.sv_RecordStorageMedium;
                avtransservPossiblePlayback.Text = curDev.AVTransport.sv_PossiblePlaybackStorageMedia;
                avtransservPossibleRecord.Text = curDev.AVTransport.sv_PossibleRecordStorageMedia;
                avtransservPlayMode.Text = curDev.AVTransport.sv_CurrentPlayMode;
                avtransservPlaySpeed.Text = curDev.AVTransport.sv_TransportPlaySpeed.ToString();
                avtransservRecStatus.Text = curDev.AVTransport.sv_RecordMediumWriteStatus;
                avtransservRecQuality.Text = curDev.AVTransport.sv_CurrentRecordQualityMode;
                avtransservPossibleQuality.Text = curDev.AVTransport.sv_PossibleRecordQualityModes;
                avtransservTracks.Text = curDev.AVTransport.sv_NumberOfTracks.ToString();
                if (curDev.AVTransport.sv_CurrentTrackDuration != null)
                {
                    avtransservTrackDuration.Text = curDev.AVTransport.sv_CurrentTrackDuration.ToString();
                }
                avtransservTrackMeta.Text = curDev.AVTransport.sv_CurrentTrackMetaData;
                avtransservURI.Text = curDev.AVTransport.sv_CurrentTrackURI;
                avtransservURIMeta.Text = curDev.AVTransport.sv_AVTransportURIMetaData;
                avtransservNextURI.Text = curDev.AVTransport.sv_NextAVTransportURI;
                avtransservNextURIMeta.Text = curDev.AVTransport.sv_NextAVTransportURIMetaData;
                avtransservRelativeTime.Text = curDev.AVTransport.sv_RelativeTimePosition;
                avtransservAbsoluteTime.Text = curDev.AVTransport.sv_AbsoluteTimePosition;
                avtransservRelativePosition.Text = curDev.AVTransport.sv_RelativeCounterPosition;
                avtransservAbsolutiePosition.Text = curDev.AVTransport.sv_AbsoluteCounterPosition;
                avtransservActions.Text = curDev.AVTransport.sv_CurrentTransportActions;
                avtransservAbsBytePosition.Text = curDev.AVTransport.sv_X_DLNA_AbsoluteBytePosition;
                avtransservRelBytePosition.Text = curDev.AVTransport.sv_X_DLNA_RelativeBytePosition;
                avtransservTrackSize.Text = curDev.AVTransport.sv_X_DLNA_CurrentTrackSize;
                avtransservSeekMode.Text = curDev.AVTransport.sv_A_ARG_TYPE_SeekMode;
                avtransservSeekTarget.Text = curDev.AVTransport.sv_A_ARG_TYPE_SeekTarget;
                avtransservInstanceId.Text = "0";
            }
        }

        private void UpdateDeviceList()
        {
            devList.Items. Clear();
            foreach (SonyAPI_Lib.SonyDevice dev in fDev)
            {
                devList.Items.Add(dev.Name);
            }
            devList.SelectedIndex = (devList.Items.Count - 1);
        }

        private void logEchgBut_Click(object sender, EventArgs e)
        {
            LoggEnabled.Enabled = true;
            logEchgBut.Visible = false;
            logEsetBut.Visible = true;

        }

        private void logLchgBut_Click(object sender, EventArgs e)
        {
            LogLevel.Enabled = true;
            logLchgBut.Visible = false;
            logLsetBut.Visible = true;
        }

        private void logNchgBut_Click(object sender, EventArgs e)
        {
            LogName.Enabled = true;
            logNchgBut.Visible = false;
            logNsetBut.Visible = true;
        }

        private void logPchgBut_Click(object sender, EventArgs e)
        {
            LogPath.Enabled = true;
            logPchgBut.Visible = false;
            logPsetBut.Visible = true;
        }

        private void logEsetBut_Click(object sender, EventArgs e)
        {
            logEchgBut.Visible = true;
            LoggEnabled.Enabled = false;
            logEsetBut.Visible = false;
             mySonyLib.LOG.Enable = Convert.ToBoolean(LoggEnabled.Text);
        }

        private void logLsetBut_Click(object sender, EventArgs e)
        {
            logLchgBut.Visible = true;
            LogLevel.Enabled = false;
            logLsetBut.Visible = false;
             mySonyLib.LOG.Level = LogLevel.Text;
        }

        private void logNsetBut_Click(object sender, EventArgs e)
        {
            logNchgBut.Visible = true;
            LogName.Enabled = false;
            logNsetBut.Visible = false;
             mySonyLib.LOG.Name = LogName.Text;
        }

        private void logPsetBut_Click(object sender, EventArgs e)
        {
            logPchgBut.Visible = true;
            LogPath.Enabled = false;
            logPsetBut.Visible = false;
            mySonyLib.LOG.Path = LogPath.Text;
        }

        private void irccservCmds_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void getCmdStrBut_Click(object sender, EventArgs e)
        {
            if (irccservCmds.Text != "")
            {
                irccservcmdValue.Text = curDev.getIRCCcommandString(irccservCmds.Text);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
            else
            {
                MessageBox.Show("Command is blank. Please choose a Command", "Error - No Command Selected");
            }
        }

        private void XsendIRCC_but_Click(object sender, EventArgs e)
        {
            if (irccservcmdValue.Text != "")
            {
                mySonyLib.ircc1.XSendIRCC(curDev, irccservcmdValue.Text);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
            else
            {
                MessageBox.Show("Command String Value is blank. Please click \"Get Command String Value\" button!", "Error - No Command Value");
            }
        }

        private void XGetStatus_but_Click(object sender, EventArgs e)
        {
            if (curDev.Name != null)
            {
                mySonyLib.ircc1.XGetStatus(curDev);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
            else
            {
                MessageBox.Show("Please Load or Build a device before executing", "Error - No Device Loaded");
            }
        }

        private void rendListPreBut_Click(object sender, EventArgs e)
        {
            if (curDev.Name != null)
            {
                mySonyLib.renderingcontrol1.ListPresets(curDev);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
            else
            {
                MessageBox.Show("Please Load or Build a device before executing", "Error - No Device Loaded");
            }
        }

        private void rendSelctPreBut_Click(object sender, EventArgs e)
        {
            
        }

        private void rendGetMuteBut_Click(object sender, EventArgs e)
        {
            if (curDev.Name != null)
            {
                mySonyLib.renderingcontrol1.GetMute(curDev);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
            else
            {
                MessageBox.Show("Please Load or Build a device before executing", "Error - No Device Loaded");
            }
        }

        private void rendSetMuteBut_Click(object sender, EventArgs e)
        {
            if (curDev.Name != null)
            {
                if (rendsetMute.Text != "")
                {
                    mySonyLib.renderingcontrol1.SetMute(curDev, Convert.ToBoolean(rendsetMute.Text));
                    UpdateLog(LogPath.Text, LogName.Text);
                    UpdateDevice();
                }
                else
                {
                    MessageBox.Show("Mute Level is not Specified. Please choose Mute Level.", "Error - Must not Specified");
                }
            }
            else
            {
                MessageBox.Show("Please Load or Build a device before executing", "Error - No Device Loaded");
            }
        }

        private void rendSetVolumeBut_Click(object sender, EventArgs e)
        {
            if (curDev.Name != null)
            {
                if (rendservVolume.Text != "0")
                {
                    mySonyLib.renderingcontrol1.SetVolume(curDev, Convert.ToInt32(rendVolLevel.Text));
                    UpdateLog(LogPath.Text, LogName.Text);
                    UpdateDevice();
                }
                else
                {
                    MessageBox.Show("No Volume Level specified. Please enter Volume Level", "Error - No Volume Level");
                }
            }
            else
            {
                MessageBox.Show("Please Load or Build a device before executing", "Error - No Device Loaded");
            }
        }

        private void rendGetVolumeBut_Click(object sender, EventArgs e)
        {
            if (curDev.Name != null)
            {
                mySonyLib.renderingcontrol1.GetVolume(curDev);
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
            else
            {
                MessageBox.Show("Please Load or Build a device before executing", "Error - No Device Loaded");
            }
        }

        private void irccservCmds_SelectedIndexChanged_1(object sender, EventArgs e)
        {
            irccservcmdValue.Text = "";
        }

        private void devList_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (fDev.Count > 0)
            {
                curDev = fDev[devList.SelectedIndex];
                UpdateLog(LogPath.Text, LogName.Text);
                UpdateDevice();
            }
        }

        private void avtransGetPositionBut_Click(object sender, EventArgs e)
        {
            mySonyLib.avtransport1.GetPosition(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtranGetMediaBut_Click(object sender, EventArgs e)
        {
            mySonyLib.avtransport1.GetMediaInfo(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtransGetTransportBut_Click(object sender, EventArgs e)
        {
            mySonyLib.avtransport1.GetTransportInfo(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtransCapabilitiesBut_Click(object sender, EventArgs e)
        {
            mySonyLib.avtransport1.GetDeviceCapabilities(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtrasGetTransportSettingsBut_Click(object sender, EventArgs e)
        {
            mySonyLib.avtransport1.GetTransportSettings(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

    }
}
