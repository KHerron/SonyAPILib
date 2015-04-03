using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SonyAPILib;

namespace Sony_Forms_Example
{
    public partial class Form3 : Form
    {
        public SonyAPI_Lib.SonyDevice curDev = new SonyAPI_Lib.SonyDevice();
        public string devType;
        public Form3()
        {
            InitializeComponent();
            Device.DataSource = Program.fDev;
            Device.DisplayMember = "Name";
            Device.ValueMember = "Name";
            devName.Text = "";
            devIP.Text = "";
            devPort.Text = "";
            devMac.Text = "";
            devGen.Text = "";
            devCommands.Text = "";
            devSName.Text = "";
            devSMac.Text = "";
            devReg.Text = "";
            devAction.Text = "";
            devControl.Text = "";
            pictureBox1.Image = null;
            Commands.DataSource = null;
            Commands.DisplayMember = "Name";
            Commands.ValueMember = "Name";
            if (Program.fDev.Count > 0)
            {
                curDev = Program.fDev[0];
                curDev.initialize();
                showDevice(curDev);
            }
        }

        private void showDevice(SonyAPI_Lib.SonyDevice cDev)
        {
            devName.Text = cDev.Name;
            devIP.Text = cDev.Device_IP_Address;
            devPort.Text = cDev.Device_Port.ToString();
            devMac.Text = cDev.Device_Macaddress;
            devGen.Text = cDev.Generation.ToString();
            devCommands.Text = cDev.Commands.Count().ToString();
            devSName.Text = cDev.Server_Name;
            devSMac.Text = cDev.Server_Macaddress;
            devReg.Text = cDev.Registered.ToString();
            devAction.Text = cDev.actionList_URL;
            devControl.Text = cDev.control_URL;
            Commands.DataSource = cDev.Commands;
            Commands.DisplayMember = "Name";
            Commands.ValueMember = "Name";
            string hCH = curDev.getIRCCcommandString("ChannelUp");
            if (hCH != "")
            {
                devType = "TV";
                pictureBox1.Image = Sony_Forms_Example.Properties.Resources.TV_icon1;
                ChanUp.Visible = true;
                ChanDown.Visible = true;
                Chanlabel.Visible = true;
                Guide.Visible = true;
                Yellow.Visible = true;
                Red.Visible = true;
                Blue.Visible = true;
                Green.Visible = true;
            }
            else
            {
                devType = "other";
                pictureBox1.Image = Sony_Forms_Example.Properties.Resources.Stereo_Icon;
                ChanUp.Visible = false;
                ChanDown.Visible = false;
                Chanlabel.Visible = false;
                Guide.Visible = false;
                Yellow.Visible = false;
                Red.Visible = false;
                Blue.Visible = false;
                Green.Visible = false;
            }
            if(curDev.Registered == true)
            {
                regButton.Visible = false;
            }
            else
            {
                regButton.Visible = true;
            }
        }

        private void Device_SelectedIndexChanged(object sender, EventArgs e)
        {
            curDev = Program.fDev[Device.SelectedIndex];
            curDev.initialize();
            showDevice(curDev);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string cmd = curDev.getIRCCcommandString(Commands.Text);
            string rslt = curDev.send_ircc(cmd);
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void Green_MouseHover(object sender, EventArgs e)
        {
            Green.Image = Sony_Forms_Example.Properties.Resources.green_hover;
        }

        private void Green_MouseLeave(object sender, EventArgs e)
        {
            Green.Image = Sony_Forms_Example.Properties.Resources.green;
        }

        private void Green_MouseDown(object sender, MouseEventArgs e)
        {
            Green.Image = Sony_Forms_Example.Properties.Resources.green_down;
            curDev.send_ircc(curDev.getIRCCcommandString("Green"));
        }

        private void Green_MouseUp(object sender, MouseEventArgs e)
        {
            Green.Image = Sony_Forms_Example.Properties.Resources.green_hover;
        }

        private void VolUp_MouseHover(object sender, EventArgs e)
        {
            VolUp.Image = Sony_Forms_Example.Properties.Resources.plus_hover;
        }

        private void VolUp_MouseLeave(object sender, EventArgs e)
        {
            VolUp.Image = Sony_Forms_Example.Properties.Resources.plus;
        }

        private void VolUp_MouseDown(object sender, MouseEventArgs e)
        {
            VolUp.Image = Sony_Forms_Example.Properties.Resources.plus_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("VolumeUp"));
        }

        private void VolUp_MouseUp(object sender, MouseEventArgs e)
        {
            VolUp.Image = Sony_Forms_Example.Properties.Resources.plus_hover;
        }

        private void VolDown_MouseHover(object sender, EventArgs e)
        {
            VolDown.Image = Sony_Forms_Example.Properties.Resources.minus_hover;
        }

        private void VolDown_MouseDown(object sender, MouseEventArgs e)
        {
            VolDown.Image = Sony_Forms_Example.Properties.Resources.minus_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("VolumeDown"));
        }

        private void VolDown_MouseLeave(object sender, EventArgs e)
        {
            VolDown.Image = Sony_Forms_Example.Properties.Resources.minus;
        }

        private void VolDown_MouseUp(object sender, MouseEventArgs e)
        {
            VolDown.Image = Sony_Forms_Example.Properties.Resources.minus_hover;
        }

        private void ChanUp_MouseDown(object sender, MouseEventArgs e)
        {
            ChanUp.Image = Sony_Forms_Example.Properties.Resources.plus_down;
            curDev.send_ircc(curDev.getIRCCcommandString("ChannelUp"));
        }

        private void ChanUp_MouseHover(object sender, EventArgs e)
        {
            ChanUp.Image = Sony_Forms_Example.Properties.Resources.plus_hover;
        }

        private void ChanUp_MouseLeave(object sender, EventArgs e)
        {
            ChanUp.Image = Sony_Forms_Example.Properties.Resources.plus;
        }

        private void ChanUp_MouseUp(object sender, MouseEventArgs e)
        {
            ChanUp.Image = Sony_Forms_Example.Properties.Resources.plus_hover;
        }

        private void ChanDown_MouseDown(object sender, MouseEventArgs e)
        {
            ChanDown.Image = Sony_Forms_Example.Properties.Resources.minus_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("ChannelDown"));
        }

        private void ChanDown_MouseHover(object sender, EventArgs e)
        {
            ChanDown.Image = Sony_Forms_Example.Properties.Resources.minus_hover;
        }

        private void ChanDown_MouseLeave(object sender, EventArgs e)
        {
            ChanDown.Image = Sony_Forms_Example.Properties.Resources.minus;
        }

        private void ChanDown_MouseUp(object sender, MouseEventArgs e)
        {
            ChanDown.Image = Sony_Forms_Example.Properties.Resources.minus_hover;
        }

        private void Guide_MouseDown(object sender, MouseEventArgs e)
        {
            Guide.Image = Sony_Forms_Example.Properties.Resources.guide_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Guide"));
        }

        private void Guide_MouseHover(object sender, EventArgs e)
        {
            Guide.Image = Sony_Forms_Example.Properties.Resources.guide_hover;
        }

        private void Guide_MouseLeave(object sender, EventArgs e)
        {
            Guide.Image = Sony_Forms_Example.Properties.Resources.guide;
        }

        private void Guide_MouseUp(object sender, MouseEventArgs e)
        {
            Guide.Image = Sony_Forms_Example.Properties.Resources.guide_hover;
        }

        private void UP_MouseDown(object sender, MouseEventArgs e)
        {
            UP.Image = Sony_Forms_Example.Properties.Resources.arrowu_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Up"));
        }

        private void UP_MouseHover(object sender, EventArgs e)
        {
            UP.Image = Sony_Forms_Example.Properties.Resources.arrowu_hover;
        }

        private void UP_MouseLeave(object sender, EventArgs e)
        {
            UP.Image = Sony_Forms_Example.Properties.Resources.arrowu;
        }

        private void UP_MouseUp(object sender, MouseEventArgs e)
        {
            UP.Image = Sony_Forms_Example.Properties.Resources.arrowu_hover;
        }

        private void Home_MouseDown(object sender, MouseEventArgs e)
        {
            Home.Image = Sony_Forms_Example.Properties.Resources.home_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Home"));
        }

        private void Home_MouseHover(object sender, EventArgs e)
        {
            Home.Image = Sony_Forms_Example.Properties.Resources.home_hover;
        }

        private void Home_MouseLeave(object sender, EventArgs e)
        {
            Home.Image = Sony_Forms_Example.Properties.Resources.home;
        }

        private void Home_MouseUp(object sender, MouseEventArgs e)
        {
            Home.Image = Sony_Forms_Example.Properties.Resources.home_hover;
        }

        private void Left_MouseDown(object sender, MouseEventArgs e)
        {
            Left.Image = Sony_Forms_Example.Properties.Resources.arrowl_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Left"));
        }

        private void Left_MouseHover(object sender, EventArgs e)
        {
            Left.Image = Sony_Forms_Example.Properties.Resources.arrowl_hover;
        }

        private void Left_MouseLeave(object sender, EventArgs e)
        {
            Left.Image = Sony_Forms_Example.Properties.Resources.arrowl;
        }

        private void Left_MouseUp(object sender, MouseEventArgs e)
        {
            Left.Image = Sony_Forms_Example.Properties.Resources.arrowl_hover;
        }

        private void Confirm_MouseDown(object sender, MouseEventArgs e)
        {
            Confirm.Image = Sony_Forms_Example.Properties.Resources.confirm_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Confirm"));
        }

        private void Confirm_MouseHover(object sender, EventArgs e)
        {
            Confirm.Image = Sony_Forms_Example.Properties.Resources.confirm_hover;
        }

        private void Confirm_MouseLeave(object sender, EventArgs e)
        {
            Confirm.Image = Sony_Forms_Example.Properties.Resources.confirm;
        }

        private void Confirm_MouseUp(object sender, MouseEventArgs e)
        {
            Confirm.Image = Sony_Forms_Example.Properties.Resources.confirm_hover;
        }

        private void Right_MouseDown(object sender, MouseEventArgs e)
        {
            Right.Image = Sony_Forms_Example.Properties.Resources.arrowr_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Right"));
        }

        private void Right_MouseHover(object sender, EventArgs e)
        {
            Right.Image = Sony_Forms_Example.Properties.Resources.arrowr_hover;
        }

        private void Right_MouseLeave(object sender, EventArgs e)
        {
            Right.Image = Sony_Forms_Example.Properties.Resources.arrowr;
        }

        private void Right_MouseUp(object sender, MouseEventArgs e)
        {
            Right.Image = Sony_Forms_Example.Properties.Resources.arrowr_hover;
        }

        private void Return_MouseDown(object sender, MouseEventArgs e)
        {
            Return.Image = Sony_Forms_Example.Properties.Resources.return_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Return"));
        }

        private void Return_MouseHover(object sender, EventArgs e)
        {
            Return.Image = Sony_Forms_Example.Properties.Resources.return_hover;
        }

        private void Return_MouseLeave(object sender, EventArgs e)
        {
            Return.Image = Sony_Forms_Example.Properties.Resources._return;
        }

        private void Return_MouseUp(object sender, MouseEventArgs e)
        {
            Return.Image = Sony_Forms_Example.Properties.Resources.return_hover;
        }

        private void Down_MouseDown(object sender, MouseEventArgs e)
        {
            Down.Image = Sony_Forms_Example.Properties.Resources.arrowd_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Down"));
        }

        private void Down_MouseHover(object sender, EventArgs e)
        {
            Down.Image = Sony_Forms_Example.Properties.Resources.arrowd_hover;
        }

        private void Down_MouseLeave(object sender, EventArgs e)
        {
            Down.Image = Sony_Forms_Example.Properties.Resources.arrowd;
        }

        private void Down_MouseUp(object sender, MouseEventArgs e)
        {
            Down.Image = Sony_Forms_Example.Properties.Resources.arrowd_hover;
        }

        private void Options_MouseDown(object sender, MouseEventArgs e)
        {
            Options.Image = Sony_Forms_Example.Properties.Resources.options_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Options"));
        }

        private void Options_MouseHover(object sender, EventArgs e)
        {
            Options.Image = Sony_Forms_Example.Properties.Resources.options_hover;
        }

        private void Options_MouseLeave(object sender, EventArgs e)
        {
            Options.Image = Sony_Forms_Example.Properties.Resources.options;
        }

        private void Options_MouseUp(object sender, MouseEventArgs e)
        {
            Options.Image = Sony_Forms_Example.Properties.Resources.options_hover;
        }

        private void Display_MouseDown(object sender, MouseEventArgs e)
        {
            Display.Image = Sony_Forms_Example.Properties.Resources.display_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Display"));
        }

        private void Display_MouseHover(object sender, EventArgs e)
        {
            Display.Image = Sony_Forms_Example.Properties.Resources.display_hover;
        }

        private void Display_MouseLeave(object sender, EventArgs e)
        {
            Display.Image = Sony_Forms_Example.Properties.Resources.display;
        }

        private void Display_MouseUp(object sender, MouseEventArgs e)
        {
            Display.Image = Sony_Forms_Example.Properties.Resources.display_hover;
        }

        private void Num1_MouseDown(object sender, MouseEventArgs e)
        {
            Num1.Image = Sony_Forms_Example.Properties.Resources.num1_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num1"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num1"));
            }
        }

        private void Num1_MouseHover(object sender, EventArgs e)
        {
            Num1.Image = Sony_Forms_Example.Properties.Resources.num1_hover;
        }

        private void Num1_MouseLeave(object sender, EventArgs e)
        {
            Num1.Image = Sony_Forms_Example.Properties.Resources.num1;
        }

        private void Num1_MouseUp(object sender, MouseEventArgs e)
        {
            Num1.Image = Sony_Forms_Example.Properties.Resources.num1_hover;
        }

        private void Num2_MouseDown(object sender, MouseEventArgs e)
        {
            Num2.Image = Sony_Forms_Example.Properties.Resources.num2_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num2"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num2"));
            }
        }

        private void Num2_MouseHover(object sender, EventArgs e)
        {
            Num2.Image = Sony_Forms_Example.Properties.Resources.num2_hover;
        }

        private void Num2_MouseLeave(object sender, EventArgs e)
        {
            Num2.Image = Sony_Forms_Example.Properties.Resources.num2;
        }

        private void Num2_MouseUp(object sender, MouseEventArgs e)
        {
            Num2.Image = Sony_Forms_Example.Properties.Resources.num2_hover;
        }

        private void Num3_MouseDown(object sender, MouseEventArgs e)
        {
            Num3.Image = Sony_Forms_Example.Properties.Resources.num3_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num3"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num3"));
            }
        }

        private void Num3_MouseHover(object sender, EventArgs e)
        {
            Num3.Image = Sony_Forms_Example.Properties.Resources.num3_hover;
        }

        private void Num3_MouseLeave(object sender, EventArgs e)
        {
            Num3.Image = Sony_Forms_Example.Properties.Resources.num3;
        }

        private void Num3_MouseUp(object sender, MouseEventArgs e)
        {
            Num3.Image = Sony_Forms_Example.Properties.Resources.num3_hover;
        }

        private void Num4_MouseDown(object sender, MouseEventArgs e)
        {
            Num4.Image = Sony_Forms_Example.Properties.Resources.num4_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num4"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num4"));
            }
        }

        private void Num4_MouseHover(object sender, EventArgs e)
        {
            Num4.Image = Sony_Forms_Example.Properties.Resources.num4_hover;
        }

        private void Num4_MouseLeave(object sender, EventArgs e)
        {
            Num4.Image = Sony_Forms_Example.Properties.Resources.num4;
        }

        private void Num4_MouseUp(object sender, MouseEventArgs e)
        {
            Num4.Image = Sony_Forms_Example.Properties.Resources.num4_hover;
        }

        private void Num5_MouseDown(object sender, MouseEventArgs e)
        {
            Num5.Image = Sony_Forms_Example.Properties.Resources.num5_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num5"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num5"));
            }
        }

        private void Num5_MouseHover(object sender, EventArgs e)
        {
            Num5.Image = Sony_Forms_Example.Properties.Resources.num5_hover;
        }

        private void Num5_MouseLeave(object sender, EventArgs e)
        {
            Num5.Image = Sony_Forms_Example.Properties.Resources.num5;
        }

        private void Num5_MouseUp(object sender, MouseEventArgs e)
        {
            Num5.Image = Sony_Forms_Example.Properties.Resources.num5_hover;
        }

        private void Num6_MouseDown(object sender, MouseEventArgs e)
        {
            Num6.Image = Sony_Forms_Example.Properties.Resources.num6_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num6"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num6"));
            }
        }

        private void Num6_MouseHover(object sender, EventArgs e)
        {
            Num6.Image = Sony_Forms_Example.Properties.Resources.num6_hover;
        }

        private void Num6_MouseLeave(object sender, EventArgs e)
        {
            Num6.Image = Sony_Forms_Example.Properties.Resources.num6;
        }

        private void Num6_MouseUp(object sender, MouseEventArgs e)
        {
            Num6.Image = Sony_Forms_Example.Properties.Resources.num6_hover;
        }

        private void Num7_MouseDown(object sender, MouseEventArgs e)
        {
            Num7.Image = Sony_Forms_Example.Properties.Resources.num7_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num7"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num7"));
            }
        }

        private void Num7_MouseHover(object sender, EventArgs e)
        {
            Num7.Image = Sony_Forms_Example.Properties.Resources.num7_hover;
        }

        private void Num7_MouseLeave(object sender, EventArgs e)
        {
            Num7.Image = Sony_Forms_Example.Properties.Resources.num7;
        }

        private void Num7_MouseUp(object sender, MouseEventArgs e)
        {
            Num7.Image = Sony_Forms_Example.Properties.Resources.num7_hover;
        }

        private void Num8_MouseDown(object sender, MouseEventArgs e)
        {
            Num8.Image = Sony_Forms_Example.Properties.Resources.num8_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num8"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num8"));
            }
        }

        private void Num8_MouseHover(object sender, EventArgs e)
        {
            Num8.Image = Sony_Forms_Example.Properties.Resources.num8_hover;
        }

        private void Num8_MouseLeave(object sender, EventArgs e)
        {
            Num8.Image = Sony_Forms_Example.Properties.Resources.num8;
        }

        private void Num8_MouseUp(object sender, MouseEventArgs e)
        {
            Num8.Image = Sony_Forms_Example.Properties.Resources.num8_hover;
        }

        private void Num9_MouseDown(object sender, MouseEventArgs e)
        {
            Num9.Image = Sony_Forms_Example.Properties.Resources.num9_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num9"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num9"));
            }
        }

        private void Num9_MouseHover(object sender, EventArgs e)
        {
            Num9.Image = Sony_Forms_Example.Properties.Resources.num9_hover;
        }

        private void Num9_MouseLeave(object sender, EventArgs e)
        {
            Num9.Image = Sony_Forms_Example.Properties.Resources.num9;
        }

        private void Num9_MouseUp(object sender, MouseEventArgs e)
        {
            Num9.Image = Sony_Forms_Example.Properties.Resources.num9_hover;
        }

        private void Dot_MouseDown(object sender, MouseEventArgs e)
        {
            Dot.Image = Sony_Forms_Example.Properties.Resources.dot_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Dot"));
        }

        private void Dot_MouseHover(object sender, EventArgs e)
        {
            Dot.Image = Sony_Forms_Example.Properties.Resources.dot_hover;
        }

        private void Dot_MouseLeave(object sender, EventArgs e)
        {
            Dot.Image = Sony_Forms_Example.Properties.Resources.dot;
        }

        private void Dot_MouseUp(object sender, MouseEventArgs e)
        {
            Dot.Image = Sony_Forms_Example.Properties.Resources.dot_hover;
        }

        private void Num0_MouseDown(object sender, MouseEventArgs e)
        {
            Num0.Image = Sony_Forms_Example.Properties.Resources.num0_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Num0"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:Num0"));
            }
        }

        private void Num0_MouseHover(object sender, EventArgs e)
        {
            Num0.Image = Sony_Forms_Example.Properties.Resources.num0_hover;
        }

        private void Num0_MouseLeave(object sender, EventArgs e)
        {
            Num0.Image = Sony_Forms_Example.Properties.Resources.num0;
        }

        private void Num0_MouseUp(object sender, MouseEventArgs e)
        {
            Num0.Image = Sony_Forms_Example.Properties.Resources.num0_hover;
        }

        private void Enter_MouseDown(object sender, MouseEventArgs e)
        {
            Enter.Image = Sony_Forms_Example.Properties.Resources.enter_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Enter"));
        }

        private void Enter_MouseHover(object sender, EventArgs e)
        {
            Enter.Image = Sony_Forms_Example.Properties.Resources.enter_hover;
        }

        private void Enter_MouseLeave(object sender, EventArgs e)
        {
            Enter.Image = Sony_Forms_Example.Properties.Resources.enter;
        }

        private void Enter_MouseUp(object sender, MouseEventArgs e)
        {
            Enter.Image = Sony_Forms_Example.Properties.Resources.enter_hover;
        }

        private void Mute_MouseDown(object sender, MouseEventArgs e)
        {
            Mute.Image = Sony_Forms_Example.Properties.Resources.muting_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Mute"));
        }

        private void Mute_MouseHover(object sender, EventArgs e)
        {
            Mute.Image = Sony_Forms_Example.Properties.Resources.muting_hover;
        }

        private void Mute_MouseLeave(object sender, EventArgs e)
        {
            Mute.Image = Sony_Forms_Example.Properties.Resources.muting;
        }

        private void Mute_MouseUp(object sender, MouseEventArgs e)
        {
            Mute.Image = Sony_Forms_Example.Properties.Resources.muting_hover;
        }

        private void Input_MouseDown(object sender, MouseEventArgs e)
        {
            Input.Image = Sony_Forms_Example.Properties.Resources.input_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Input"));
        }

        private void Input_MouseHover(object sender, EventArgs e)
        {
            Input.Image = Sony_Forms_Example.Properties.Resources.input_hover;
        }

        private void Input_MouseLeave(object sender, EventArgs e)
        {
            Input.Image = Sony_Forms_Example.Properties.Resources.input;
        }

        private void Input_MouseUp(object sender, MouseEventArgs e)
        {
            Input.Image = Sony_Forms_Example.Properties.Resources.input_hover;
        }

        private void Yellow_MouseDown(object sender, MouseEventArgs e)
        {
            Yellow.Image = Sony_Forms_Example.Properties.Resources.yellow_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Yellow"));
        }

        private void Yellow_MouseHover(object sender, EventArgs e)
        {
            Yellow.Image = Sony_Forms_Example.Properties.Resources.yellow_hover;
        }

        private void Yellow_MouseLeave(object sender, EventArgs e)
        {
            Yellow.Image = Sony_Forms_Example.Properties.Resources.yellow;
        }

        private void Yellow_MouseUp(object sender, MouseEventArgs e)
        {
            Yellow.Image = Sony_Forms_Example.Properties.Resources.yellow_hover;
        }

        private void Blue_MouseDown(object sender, MouseEventArgs e)
        {
            Blue.Image = Sony_Forms_Example.Properties.Resources.blue_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Blue"));
        }

        private void Blue_MouseHover(object sender, EventArgs e)
        {
            Blue.Image = Sony_Forms_Example.Properties.Resources.blue_hover;
        }

        private void Blue_MouseLeave(object sender, EventArgs e)
        {
            Blue.Image = Sony_Forms_Example.Properties.Resources.blue;
        }

        private void Blue_MouseUp(object sender, MouseEventArgs e)
        {
            Blue.Image = Sony_Forms_Example.Properties.Resources.blue_hover;
        }

        private void Red_MouseDown(object sender, MouseEventArgs e)
        {
            Red.Image = Sony_Forms_Example.Properties.Resources.red_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Red"));
        }

        private void Red_MouseHover(object sender, EventArgs e)
        {
            Red.Image = Sony_Forms_Example.Properties.Resources.red_hover;
        }

        private void Red_MouseLeave(object sender, EventArgs e)
        {
            Red.Image = Sony_Forms_Example.Properties.Resources.red;
        }

        private void Red_MouseUp(object sender, MouseEventArgs e)
        {
            Red.Image = Sony_Forms_Example.Properties.Resources.red_hover;
        }

        private void SkipBack_MouseDown(object sender, MouseEventArgs e)
        {
            SkipBack.Image = Sony_Forms_Example.Properties.Resources.skipbackwards_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Prev"));
        }

        private void SkipBack_MouseHover(object sender, EventArgs e)
        {
            SkipBack.Image = Sony_Forms_Example.Properties.Resources.skipbackwards_hover;
        }

        private void SkipBack_MouseLeave(object sender, EventArgs e)
        {
            SkipBack.Image = Sony_Forms_Example.Properties.Resources.skipbackwards;
        }

        private void SkipBack_MouseUp(object sender, MouseEventArgs e)
        {
            SkipBack.Image = Sony_Forms_Example.Properties.Resources.skipbackwards_hover;
        }

        private void SkipForw_MouseDown(object sender, MouseEventArgs e)
        {
            SkipForw.Image = Sony_Forms_Example.Properties.Resources.skipforward_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Next"));
        }

        private void SkipForw_MouseHover(object sender, EventArgs e)
        {
            SkipForw.Image = Sony_Forms_Example.Properties.Resources.skipforward_hover;
        }

        private void SkipForw_MouseLeave(object sender, EventArgs e)
        {
            SkipForw.Image = Sony_Forms_Example.Properties.Resources.skipforward;
        }

        private void SkipForw_MouseUp(object sender, MouseEventArgs e)
        {
            SkipForw.Image = Sony_Forms_Example.Properties.Resources.skipforward_hover;
        }

        private void FastRew_MouseDown(object sender, MouseEventArgs e)
        {
            FastRew.Image = Sony_Forms_Example.Properties.Resources.fastbackwards_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Rewind"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:FR"));
            }
        }

        private void FastRew_MouseHover(object sender, EventArgs e)
        {
            FastRew.Image = Sony_Forms_Example.Properties.Resources.fastbackwards_hover;
        }

        private void FastRew_MouseLeave(object sender, EventArgs e)
        {
            FastRew.Image = Sony_Forms_Example.Properties.Resources.fastbackwards;
        }

        private void FastRew_MouseUp(object sender, MouseEventArgs e)
        {
            FastRew.Image = Sony_Forms_Example.Properties.Resources.fastbackwards_hover;
        }

        private void FastForw_MouseDown(object sender, MouseEventArgs e)
        {
            FastForw.Image = Sony_Forms_Example.Properties.Resources.fastforward_down;
            if (devType == "TV")
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Forward"));
            }
            else
            {
                string rslts = curDev.send_ircc(curDev.getIRCCcommandString("STR:FF"));
            }
        }

        private void FastForw_MouseHover(object sender, EventArgs e)
        {
            FastForw.Image = Sony_Forms_Example.Properties.Resources.fastforward_hover;
        }

        private void FastForw_MouseLeave(object sender, EventArgs e)
        {
            FastForw.Image = Sony_Forms_Example.Properties.Resources.fastforward;
        }

        private void FastForw_MouseUp(object sender, MouseEventArgs e)
        {
            FastForw.Image = Sony_Forms_Example.Properties.Resources.fastforward_hover;
        }

        private void Pause_MouseDown(object sender, MouseEventArgs e)
        {
            Pause.Image = Sony_Forms_Example.Properties.Resources.pause_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Pause"));
        }

        private void Pause_MouseHover(object sender, EventArgs e)
        {
            Pause.Image = Sony_Forms_Example.Properties.Resources.pause_hover;
        }

        private void Pause_MouseLeave(object sender, EventArgs e)
        {
            Pause.Image = Sony_Forms_Example.Properties.Resources.pause;
        }

        private void Pause_MouseUp(object sender, MouseEventArgs e)
        {
            Pause.Image = Sony_Forms_Example.Properties.Resources.pause_hover;
        }

        private void Play_MouseDown(object sender, MouseEventArgs e)
        {
            Play.Image = Sony_Forms_Example.Properties.Resources.play_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Play"));
        }

        private void Play_MouseHover(object sender, EventArgs e)
        {
            Play.Image = Sony_Forms_Example.Properties.Resources.play_hover;
        }

        private void Play_MouseLeave(object sender, EventArgs e)
        {
            Play.Image = Sony_Forms_Example.Properties.Resources.play;
        }

        private void Play_MouseUp(object sender, MouseEventArgs e)
        {
            Play.Image = Sony_Forms_Example.Properties.Resources.play_hover;
        }

        private void Stop_MouseDown(object sender, MouseEventArgs e)
        {
            Stop.Image = Sony_Forms_Example.Properties.Resources.stop_down;
            string rslts = curDev.send_ircc(curDev.getIRCCcommandString("Stop"));
        }

        private void Stop_MouseHover(object sender, EventArgs e)
        {
            Stop.Image = Sony_Forms_Example.Properties.Resources.stop_hover;
        }

        private void Stop_MouseLeave(object sender, EventArgs e)
        {
            Stop.Image = Sony_Forms_Example.Properties.Resources.stop;
        }

        private void Stop_MouseUp(object sender, MouseEventArgs e)
        {
            Stop.Image = Sony_Forms_Example.Properties.Resources.stop_hover;
        }

        private void regButton_Click(object sender, EventArgs e)
        {
            bool devReg = curDev.register();
            if (curDev.Registered == false)
            {
                //Check if Generaton 3. If yes, prompt for pin code
                if (curDev.Generation == 3)
                {
                    Form4 ePin = new Form4();
                    ePin.ShowDialog();
                    string Pin = ePin.PinCode.Text;
                    devReg = curDev.sendAuth(Pin);
                    ePin.Dispose();
                }
            }
        }
    }
}
