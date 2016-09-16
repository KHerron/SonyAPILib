using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using SonyAPILib;


namespace DLNA_Control_Panel
{
    public partial class Form1 : Form
    {
        public static APILibrary mySonyLib = new APILibrary();
        public static List<APILibrary.SonyDevice> fDev = new List<APILibrary.SonyDevice>();
        public static APILibrary.SonyDevice curDev = new APILibrary.SonyDevice();
        
        public Form1()
        {
            InitializeComponent();
            mySonyLib.Log.Enable = true;
            mySonyLib.Log.Level = "All";
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoggEnabled.Text = mySonyLib.Log.Enable.ToString();
            LogLevel.Text = mySonyLib.Log.Level.ToString();
            LogName.Text = mySonyLib.Log.Name.ToString();
            LogPath.Text = mySonyLib.Log.Path.ToString();
            UpdateLog(mySonyLib.Log.Path.ToString(), mySonyLib.Log.Name.ToString());
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
                curDev.BuildFromDocument(new Uri(bDocURL.Text));
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
            foundDevices = mySonyLib.Locator.LocateDevices();
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
                mySonyLib.Log.ClearLog(null);
            }
            else
            {
                mySonyLib.Log.ClearLog(saveLogName.Text);
            }
            UpdateLog(LogPath.Text, LogName.Text);
        }

        private void LogWriteBut_Click(object sender, EventArgs e)
        {
            mySonyLib.Log.AddMessage(LogLine.Text, true);
            UpdateLog(LogPath.Text, LogName.Text);
        }

        private void UpdateDevice()
        {
            devName.Text = curDev.Name;
            devIP.Text = curDev.IPAddress;
            devPort.Text = curDev.Port.ToString();
            devMac.Text = curDev.MacAddress;
            devServName.Text = curDev.ServerName;
            devServMac.Text =  curDev.ServerMacAddress;
            devManuf.Text =  curDev.Manufacture;
            devModel.Text =  curDev.ModelName;
            devModnum.Text =  curDev.ModelNumber;
            devUDN.Text =  curDev.Udn;
            devActionList.Text =  curDev.ActionListUrl;
            devRegistered.Text =  curDev.Registered.ToString();
            devType.Text =  curDev.Type;
            devMDesc.Text =  curDev.ModelDescription;
            devDocumentUrl.Text =  curDev.DocumentUrl;
            devRegMode.Text =  curDev.Actionlist.RegisterMode.ToString();
            devCookie.Text =  curDev.Cookie;
            UpdateIRCC();
            UpdateRenderingControl();
            UpdateAVTransport();
        }

        private void UpdateIRCC()
        {
            if ( curDev.Ircc.ControlUrl != null)
            {
                irccservControl.Text =  curDev.Ircc.ControlUrl;
                irccservEvent.Text =  curDev.Ircc.EventSubUrl;
                irccservSCPD.Text =  curDev.Ircc.ScpdUrl;
                irccservType.Text =  curDev.Ircc.Type;
                irccservId.Text =  curDev.Ircc.ServiceID;
                irccservIdent.Text =  curDev.Ircc.ServiceIdentifier;
                irccservLastChange.Text =  curDev.Ircc.LastChange;
                irccservCurStatus.Text =  curDev.Ircc.CurrentStatus;
                irccservCmdCount.Text =  curDev.Commands.Count().ToString();
                irccservCmds.DataSource =  curDev.Commands;
                irccservCmds.DisplayMember = "Name";
                irccservCmds.ValueMember = "Value";
            }
        }

        private void UpdateRenderingControl()
        {
            if (curDev.RenderingControl.ControlUrl != null)
            {
                rendservControl.Text = curDev.RenderingControl.ControlUrl;
                rendservEvent.Text = curDev.RenderingControl.EventSubUrl;
                rendservSCPD.Text = curDev.RenderingControl.ScpdUrl;
                rendservType.Text = curDev.RenderingControl.Type;
                rendservId.Text = curDev.RenderingControl.ServiceID;
                rendservIdent.Text = curDev.RenderingControl.ServiceIdentifier;
                rendservLastChange.Text = curDev.RenderingControl.LastChange;
                rendservPreList.Text = curDev.RenderingControl.PresetNameList;
                rendservMute.Text = curDev.RenderingControl.Mute.ToString();
                rendservVolume.Text = curDev.RenderingControl.Volume.ToString();
                rendservInstId.Text = curDev.RenderingControl.InstanceID.ToString();
                rendservPreName.Text = curDev.RenderingControl.PresetName;
                rendservChan.Text = curDev.RenderingControl.ChannelState;
            }
        }

        private void UpdateAVTransport()
        {
            if (curDev.AVTransport.ControlUrl != null)
            {
                avtransservLastChange.Text = curDev.AVTransport.LastChange;
                avtransservControl.Text = curDev.AVTransport.ControlUrl;
                avtransservEvent.Text = curDev.AVTransport.EventSubUrl;
                avtransservSCPD.Text = curDev.AVTransport.ScpdUrl;
                avtransservType.Text = curDev.AVTransport.Type;
                avtransservId.Text = curDev.AVTransport.ServiceID;
                avtransservIdent.Text = curDev.AVTransport.ServiceIdentifier;
                avtransservState.Text = curDev.AVTransport.TransportState;
                avtransservStatus.Text = curDev.AVTransport.TransportStatus;
                avtransservPlaybackStorage.Text = curDev.AVTransport.PlayBackStorageMedium;
                avtransservRecordStorage.Text = curDev.AVTransport.RecordStorageMedium;
                avtransservPossiblePlayback.Text = curDev.AVTransport.PossiblePlaybackStorageMedia;
                avtransservPossibleRecord.Text = curDev.AVTransport.PossibleRecordStorageMedia;
                avtransservPlayMode.Text = curDev.AVTransport.CurrentPlayMode;
                avtransservPlaySpeed.Text = curDev.AVTransport.TransportPlaySpeed.ToString();
                avtransservRecStatus.Text = curDev.AVTransport.RecordMediumWriteStatus;
                avtransservRecQuality.Text = curDev.AVTransport.CurrentRecordQualityMode;
                avtransservPossibleQuality.Text = curDev.AVTransport.PossibleRecordQualityModes;
                avtransservTracks.Text = curDev.AVTransport.NumberOfTracks.ToString();
                if (curDev.AVTransport.CurrentTrackDuration != null)
                {
                    avtransservTrackDuration.Text = curDev.AVTransport.CurrentTrackDuration.ToString();
                }
                avtransservTrackMeta.Text = curDev.AVTransport.CurrentTrackMetaData;
                avtransservURI.Text = curDev.AVTransport.CurrentTrackURI;
                avtransservURIMeta.Text = curDev.AVTransport.AVTransportURIMetaData;
                avtransservNextURI.Text = curDev.AVTransport.NextAVTransportURI;
                avtransservNextURIMeta.Text = curDev.AVTransport.NextAVTransportURIMetaData;
                avtransservRelativeTime.Text = curDev.AVTransport.RelativeTimePosition;
                avtransservAbsoluteTime.Text = curDev.AVTransport.AbsoluteTimePosition;
                avtransservRelativePosition.Text = curDev.AVTransport.RelativeCounterPosition;
                avtransservAbsolutiePosition.Text = curDev.AVTransport.AbsoluteCounterPosition;
                avtransservActions.Text = curDev.AVTransport.CurrentTransportActions;
                avtransservAbsBytePosition.Text = curDev.AVTransport.X_DLNA_AbsoluteBytePosition;
                avtransservRelBytePosition.Text = curDev.AVTransport.X_DLNA_RelativeBytePosition;
                avtransservTrackSize.Text = curDev.AVTransport.X_DLNA_CurrentTrackSize;
                avtransservSeekMode.Text = curDev.AVTransport.A_ARG_TYPE_SeekMode;
                avtransservSeekTarget.Text = curDev.AVTransport.A_ARG_TYPE_SeekTarget;
                avtransservInstanceId.Text = "0";
            }
        }

        private void UpdateDeviceList()
        {
            devList.Items. Clear();
            foreach (APILibrary.SonyDevice dev in fDev)
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
            mySonyLib.Log.Enable = Convert.ToBoolean(LoggEnabled.Text);
        }

        private void logLsetBut_Click(object sender, EventArgs e)
        {
            logLchgBut.Visible = true;
            LogLevel.Enabled = false;
            logLsetBut.Visible = false;
            mySonyLib.Log.Level = LogLevel.Text;
        }

        private void logNsetBut_Click(object sender, EventArgs e)
        {
            logNchgBut.Visible = true;
            LogName.Enabled = false;
            logNsetBut.Visible = false;
            mySonyLib.Log.Name = LogName.Text;
        }

        private void logPsetBut_Click(object sender, EventArgs e)
        {
            logPchgBut.Visible = true;
            LogPath.Enabled = false;
            logPsetBut.Visible = false;
            mySonyLib.Log.Path = LogPath.Text;
        }

        private void irccservCmds_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void getCmdStrBut_Click(object sender, EventArgs e)
        {
            if (irccservCmds.Text != "")
            {
                irccservcmdValue.Text = curDev.GetCommandString(irccservCmds.Text);
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
                curDev.Ircc.SendIRCC(curDev, irccservcmdValue.Text);
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
                curDev.Ircc.GetStatus(curDev);
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
                curDev.RenderingControl.ListPresets(curDev);
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
                curDev.RenderingControl.GetMute(curDev);
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
                    curDev.RenderingControl.SetMute(curDev, Convert.ToBoolean(rendsetMute.Text));
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
                    curDev.RenderingControl.SetVolume(curDev, Convert.ToInt32(rendVolLevel.Text));
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
                curDev.RenderingControl.GetVolume(curDev);
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
            curDev.AVTransport.GetPosition(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtranGetMediaBut_Click(object sender, EventArgs e)
        {
            curDev.AVTransport.GetMediaInfo(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtransGetTransportBut_Click(object sender, EventArgs e)
        {
            curDev.AVTransport.GetTransportInfo(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtransCapabilitiesBut_Click(object sender, EventArgs e)
        {
            curDev.AVTransport.GetDeviceCapabilities(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

        private void avtrasGetTransportSettingsBut_Click(object sender, EventArgs e)
        {
            curDev.AVTransport.GetTransportSettings(curDev);
            UpdateLog(LogPath.Text, LogName.Text);
            UpdateDevice();
        }

    }
}
