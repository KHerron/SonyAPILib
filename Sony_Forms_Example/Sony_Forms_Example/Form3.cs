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

        public APILibrary.SonyDevice curDev = new APILibrary.SonyDevice();

        public string devType;

        public Form3()

        {

            InitializeComponent();

            foreach (APILibrary.SonyDevice d in Program.fDev)

            {

                Device.Items.Add(d);

            }

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

            pictureBox1.Image = null;

            Commands.DataSource = null;

            Commands.DisplayMember = "Name";

            Commands.ValueMember = "Name";

        }



        private void showDevice(APILibrary.SonyDevice cDev)

        {

            devName.Text = cDev.Name;

            devIP.Text = cDev.IPAddress;

            devPort.Text = cDev.Port.ToString();

            devMac.Text = cDev.MacAddress;

            devGen.Text = cDev.Actionlist.RegisterMode.ToString();

            devCommands.Text = cDev.Commands.Count().ToString();

            devSName.Text = cDev.ServerName;

            devSMac.Text = cDev.ServerMacAddress;

            devReg.Text = cDev.Registered.ToString();

            Commands.DataSource = cDev.Commands;

            Commands.DisplayMember = "Name";

            Commands.ValueMember = "Name";

            string hCH = curDev.GetCommandString("ChannelUp");

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

            this.Refresh();

        }



        private void Device_SelectedIndexChanged(object sender, EventArgs e)

        {

            curDev = Program.fDev[Device.SelectedIndex];

            showDevice(curDev);

            this.Refresh();

        }



        private void button1_Click(object sender, EventArgs e)

        {

            string cmd = curDev.GetCommandString(Commands.Text);

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void button2_Click(object sender, EventArgs e)

        {

            this.Close();

        }



        private void Green_MouseHover(object sender, EventArgs e)

        {

            Green.Image = Properties.Resources.green_hover;

        }



        private void Green_MouseLeave(object sender, EventArgs e)

        {

            Green.Image = Properties.Resources.green;

        }



        private void Green_MouseDown(object sender, MouseEventArgs e)

        {

            Green.Image = Properties.Resources.green_down;

            string cmd = curDev.GetCommandString("Green");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void Green_MouseUp(object sender, MouseEventArgs e)

        {

            Green.Image = Properties.Resources.green_hover;

        }



        private void VolUp_MouseHover(object sender, EventArgs e)

        {

            VolUp.Image = Properties.Resources.plus_hover;

        }



        private void VolUp_MouseLeave(object sender, EventArgs e)

        {

            VolUp.Image = Properties.Resources.plus;

        }



        private void VolUp_MouseDown(object sender, MouseEventArgs e)

        {

            VolUp.Image = Properties.Resources.plus_down;

            string cmd = curDev.GetCommandString("VolumeUp");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void VolUp_MouseUp(object sender, MouseEventArgs e)

        {

            VolUp.Image = Properties.Resources.plus_hover;

        }



        private void VolDown_MouseHover(object sender, EventArgs e)

        {

            VolDown.Image = Properties.Resources.minus_hover;

        }



        private void VolDown_MouseDown(object sender, MouseEventArgs e)

        {

            VolDown.Image = Properties.Resources.minus_down;

            string cmd = curDev.GetCommandString("VolumeDown");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void VolDown_MouseLeave(object sender, EventArgs e)

        {

            VolDown.Image = Properties.Resources.minus;

        }



        private void VolDown_MouseUp(object sender, MouseEventArgs e)

        {

            VolDown.Image = Properties.Resources.minus_hover;

        }



        private void ChanUp_MouseDown(object sender, MouseEventArgs e)

        {

            ChanUp.Image = Properties.Resources.plus_down;

            string cmd = curDev.GetCommandString("ChannelUp");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void ChanUp_MouseHover(object sender, EventArgs e)

        {

            ChanUp.Image = Properties.Resources.plus_hover;

        }



        private void ChanUp_MouseLeave(object sender, EventArgs e)

        {

            ChanUp.Image = Properties.Resources.plus;

        }



        private void ChanUp_MouseUp(object sender, MouseEventArgs e)

        {

            ChanUp.Image = Properties.Resources.plus_hover;

        }



        private void ChanDown_MouseDown(object sender, MouseEventArgs e)

        {

            ChanDown.Image = Properties.Resources.minus_down;

            string cmd = curDev.GetCommandString("ChannelDown");

            string rslts =curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void ChanDown_MouseHover(object sender, EventArgs e)

        {

            ChanDown.Image = Properties.Resources.minus_hover;

        }



        private void ChanDown_MouseLeave(object sender, EventArgs e)

        {

            ChanDown.Image = Properties.Resources.minus;

        }



        private void ChanDown_MouseUp(object sender, MouseEventArgs e)

        {

            ChanDown.Image = Properties.Resources.minus_hover;

        }



        private void Guide_MouseDown(object sender, MouseEventArgs e)

        {

            Guide.Image = Properties.Resources.guide_down;

            string cmd = curDev.GetCommandString("Guide");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void Guide_MouseHover(object sender, EventArgs e)

        {

            Guide.Image = Properties.Resources.guide_hover;

        }



        private void Guide_MouseLeave(object sender, EventArgs e)

        {

            Guide.Image = Properties.Resources.guide;

        }



        private void Guide_MouseUp(object sender, MouseEventArgs e)

        {

            Guide.Image = Properties.Resources.guide_hover;

        }



        private void UP_MouseDown(object sender, MouseEventArgs e)

        {

            UPbut.Image = Properties.Resources.arrowu_down;

            string cmd = curDev.GetCommandString("Up");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void UP_MouseHover(object sender, EventArgs e)

        {

            UPbut.Image = Properties.Resources.arrowu_hover;

        }



        private void UP_MouseLeave(object sender, EventArgs e)

        {

            UPbut.Image = Properties.Resources.arrowu;

        }



        private void UP_MouseUp(object sender, MouseEventArgs e)

        {

            UPbut.Image = Properties.Resources.arrowu_hover;

        }



        private void Home_MouseDown(object sender, MouseEventArgs e)

        {

            Home.Image = Properties.Resources.home_down;

            string cmd = curDev.GetCommandString("Home");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void Home_MouseHover(object sender, EventArgs e)

        {

            Home.Image = Properties.Resources.home_hover;

        }



        private void Home_MouseLeave(object sender, EventArgs e)

        {

            Home.Image = Properties.Resources.home;

        }



        private void Home_MouseUp(object sender, MouseEventArgs e)

        {

            Home.Image = Properties.Resources.home_hover;

        }



        private void Left_MouseDown(object sender, MouseEventArgs e)

        {

            Leftbut.Image = Properties.Resources.arrowl_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Left"));

        }



        private void Left_MouseHover(object sender, EventArgs e)

        {

            Leftbut.Image = Properties.Resources.arrowl_hover;

        }



        private void Left_MouseLeave(object sender, EventArgs e)

        {

            Leftbut.Image = Properties.Resources.arrowl;

        }



        private void Left_MouseUp(object sender, MouseEventArgs e)

        {

            Leftbut.Image = Properties.Resources.arrowl_hover;

        }



        private void Confirm_MouseDown(object sender, MouseEventArgs e)

        {

            Confirm.Image = Properties.Resources.confirm_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Confirm"));

        }



        private void Confirm_MouseHover(object sender, EventArgs e)

        {

            Confirm.Image = Properties.Resources.confirm_hover;

        }



        private void Confirm_MouseLeave(object sender, EventArgs e)

        {

            Confirm.Image = Properties.Resources.confirm;

        }



        private void Confirm_MouseUp(object sender, MouseEventArgs e)

        {

            Confirm.Image = Properties.Resources.confirm_hover;

        }



        private void Right_MouseDown(object sender, MouseEventArgs e)

        {

            Rightbut.Image = Properties.Resources.arrowr_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Right"));

        }



        private void Right_MouseHover(object sender, EventArgs e)

        {

            Rightbut.Image = Properties.Resources.arrowr_hover;

        }



        private void Right_MouseLeave(object sender, EventArgs e)

        {

            Rightbut.Image = Properties.Resources.arrowr;

        }



        private void Right_MouseUp(object sender, MouseEventArgs e)

        {

            Rightbut.Image = Properties.Resources.arrowr_hover;

        }



        private void Return_MouseDown(object sender, MouseEventArgs e)

        {

            Return.Image = Properties.Resources.return_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Return"));

        }



        private void Return_MouseHover(object sender, EventArgs e)

        {

            Return.Image = Properties.Resources.return_hover;

        }



        private void Return_MouseLeave(object sender, EventArgs e)

        {

            Return.Image = Properties.Resources._return;

        }



        private void Return_MouseUp(object sender, MouseEventArgs e)

        {

            Return.Image = Properties.Resources.return_hover;

        }



        private void Down_MouseDown(object sender, MouseEventArgs e)

        {

            Downbut.Image = Properties.Resources.arrowd_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Down"));

        }



        private void Down_MouseHover(object sender, EventArgs e)

        {

            Downbut.Image = Properties.Resources.arrowd_hover;

        }



        private void Down_MouseLeave(object sender, EventArgs e)

        {

            Downbut.Image = Properties.Resources.arrowd;

        }



        private void Down_MouseUp(object sender, MouseEventArgs e)

        {

            Downbut.Image = Properties.Resources.arrowd_hover;

        }



        private void Options_MouseDown(object sender, MouseEventArgs e)

        {

            Options.Image = Properties.Resources.options_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Options"));

        }



        private void Options_MouseHover(object sender, EventArgs e)

        {

            Options.Image = Properties.Resources.options_hover;

        }



        private void Options_MouseLeave(object sender, EventArgs e)

        {

            Options.Image = Properties.Resources.options;

        }



        private void Options_MouseUp(object sender, MouseEventArgs e)

        {

            Options.Image = Properties.Resources.options_hover;

        }



        private void Display_MouseDown(object sender, MouseEventArgs e)

        {

            Display.Image = Properties.Resources.display_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Display"));

        }



        private void Display_MouseHover(object sender, EventArgs e)

        {

            Display.Image = Properties.Resources.display_hover;

        }



        private void Display_MouseLeave(object sender, EventArgs e)

        {

            Display.Image = Properties.Resources.display;

        }



        private void Display_MouseUp(object sender, MouseEventArgs e)

        {

            Display.Image = Properties.Resources.display_hover;

        }



        private void Num1_MouseDown(object sender, MouseEventArgs e)

        {

            Num1.Image = Properties.Resources.num1_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num1"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num1"));

            }

        }



        private void Num1_MouseHover(object sender, EventArgs e)

        {

            Num1.Image = Properties.Resources.num1_hover;

        }



        private void Num1_MouseLeave(object sender, EventArgs e)

        {

            Num1.Image = Properties.Resources.num1;

        }



        private void Num1_MouseUp(object sender, MouseEventArgs e)

        {

            Num1.Image = Properties.Resources.num1_hover;

        }



        private void Num2_MouseDown(object sender, MouseEventArgs e)

        {

            Num2.Image = Properties.Resources.num2_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num2"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num2"));

            }

        }



        private void Num2_MouseHover(object sender, EventArgs e)

        {

            Num2.Image = Properties.Resources.num2_hover;

        }



        private void Num2_MouseLeave(object sender, EventArgs e)

        {

            Num2.Image = Properties.Resources.num2;

        }



        private void Num2_MouseUp(object sender, MouseEventArgs e)

        {

            Num2.Image = Properties.Resources.num2_hover;

        }



        private void Num3_MouseDown(object sender, MouseEventArgs e)

        {

            Num3.Image = Properties.Resources.num3_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num3"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num3"));

            }

        }



        private void Num3_MouseHover(object sender, EventArgs e)

        {

            Num3.Image = Properties.Resources.num3_hover;

        }



        private void Num3_MouseLeave(object sender, EventArgs e)

        {

            Num3.Image = Properties.Resources.num3;

        }



        private void Num3_MouseUp(object sender, MouseEventArgs e)

        {

            Num3.Image = Properties.Resources.num3_hover;

        }



        private void Num4_MouseDown(object sender, MouseEventArgs e)

        {

            Num4.Image = Properties.Resources.num4_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num4"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num4"));

            }

        }



        private void Num4_MouseHover(object sender, EventArgs e)

        {

            Num4.Image = Properties.Resources.num4_hover;

        }



        private void Num4_MouseLeave(object sender, EventArgs e)

        {

            Num4.Image = Properties.Resources.num4;

        }



        private void Num4_MouseUp(object sender, MouseEventArgs e)

        {

            Num4.Image = Properties.Resources.num4_hover;

        }



        private void Num5_MouseDown(object sender, MouseEventArgs e)

        {

            Num5.Image = Properties.Resources.num5_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num5"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num5"));

            }

        }



        private void Num5_MouseHover(object sender, EventArgs e)

        {

            Num5.Image = Properties.Resources.num5_hover;

        }



        private void Num5_MouseLeave(object sender, EventArgs e)

        {

            Num5.Image = Properties.Resources.num5;

        }



        private void Num5_MouseUp(object sender, MouseEventArgs e)

        {

            Num5.Image = Properties.Resources.num5_hover;

        }



        private void Num6_MouseDown(object sender, MouseEventArgs e)

        {

            Num6.Image = Properties.Resources.num6_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num6"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num6"));

            }

        }



        private void Num6_MouseHover(object sender, EventArgs e)

        {

            Num6.Image = Properties.Resources.num6_hover;

        }



        private void Num6_MouseLeave(object sender, EventArgs e)

        {

            Num6.Image = Properties.Resources.num6;

        }



        private void Num6_MouseUp(object sender, MouseEventArgs e)

        {

            Num6.Image = Properties.Resources.num6_hover;

        }



        private void Num7_MouseDown(object sender, MouseEventArgs e)

        {

            Num7.Image = Properties.Resources.num7_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num7"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num7"));

            }

        }



        private void Num7_MouseHover(object sender, EventArgs e)

        {

            Num7.Image = Properties.Resources.num7_hover;

        }



        private void Num7_MouseLeave(object sender, EventArgs e)

        {

            Num7.Image = Properties.Resources.num7;

        }



        private void Num7_MouseUp(object sender, MouseEventArgs e)

        {

            Num7.Image = Properties.Resources.num7_hover;

        }



        private void Num8_MouseDown(object sender, MouseEventArgs e)

        {

            Num8.Image = Properties.Resources.num8_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num8"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num8"));

            }

        }



        private void Num8_MouseHover(object sender, EventArgs e)

        {

            Num8.Image = Properties.Resources.num8_hover;

        }



        private void Num8_MouseLeave(object sender, EventArgs e)

        {

            Num8.Image = Properties.Resources.num8;

        }



        private void Num8_MouseUp(object sender, MouseEventArgs e)

        {

            Num8.Image = Properties.Resources.num8_hover;

        }



        private void Num9_MouseDown(object sender, MouseEventArgs e)

        {

            Num9.Image = Properties.Resources.num9_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num9"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num9"));

            }

        }



        private void Num9_MouseHover(object sender, EventArgs e)

        {

            Num9.Image = Properties.Resources.num9_hover;

        }



        private void Num9_MouseLeave(object sender, EventArgs e)

        {

            Num9.Image = Properties.Resources.num9;

        }



        private void Num9_MouseUp(object sender, MouseEventArgs e)

        {

            Num9.Image = Properties.Resources.num9_hover;

        }



        private void Dot_MouseDown(object sender, MouseEventArgs e)

        {

            Dot.Image = Properties.Resources.dot_down;

            string rslts =curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Dot"));

        }



        private void Dot_MouseHover(object sender, EventArgs e)

        {

            Dot.Image = Properties.Resources.dot_hover;

        }



        private void Dot_MouseLeave(object sender, EventArgs e)

        {

            Dot.Image = Properties.Resources.dot;

        }



        private void Dot_MouseUp(object sender, MouseEventArgs e)

        {

            Dot.Image = Properties.Resources.dot_hover;

        }



        private void Num0_MouseDown(object sender, MouseEventArgs e)

        {

            Num0.Image = Properties.Resources.num0_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Num0"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:Num0"));

            }

        }



        private void Num0_MouseHover(object sender, EventArgs e)

        {

            Num0.Image = Properties.Resources.num0_hover;

        }



        private void Num0_MouseLeave(object sender, EventArgs e)

        {

            Num0.Image = Properties.Resources.num0;

        }



        private void Num0_MouseUp(object sender, MouseEventArgs e)

        {

            Num0.Image = Properties.Resources.num0_hover;

        }



        private void Enter_MouseDown(object sender, MouseEventArgs e)

        {

            Enterbut.Image = Properties.Resources.enter_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Enter"));

        }



        private void Enter_MouseHover(object sender, EventArgs e)

        {

            Enterbut.Image = Properties.Resources.enter_hover;

        }



        private void Enter_MouseLeave(object sender, EventArgs e)

        {

            Enterbut.Image = Properties.Resources.enter;

        }



        private void Enter_MouseUp(object sender, MouseEventArgs e)

        {

            Enterbut.Image = Properties.Resources.enter_hover;

        }



        private void Mute_MouseDown(object sender, MouseEventArgs e)

        {

            Mute.Image = Properties.Resources.muting_down;

            string cmd = curDev.GetCommandString("Mute");

            string rslts = curDev.Ircc.SendIRCC(curDev, cmd);

        }



        private void Mute_MouseHover(object sender, EventArgs e)

        {

            Mute.Image = Properties.Resources.muting_hover;

        }



        private void Mute_MouseLeave(object sender, EventArgs e)

        {

            Mute.Image = Properties.Resources.muting;

        }



        private void Mute_MouseUp(object sender, MouseEventArgs e)

        {

            Mute.Image = Properties.Resources.muting_hover;

        }



        private void Input_MouseDown(object sender, MouseEventArgs e)

        {

            Input.Image = Properties.Resources.input_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Input"));

        }



        private void Input_MouseHover(object sender, EventArgs e)

        {

            Input.Image = Sony_Forms_Example.Properties.Resources.input_hover;

        }



        private void Input_MouseLeave(object sender, EventArgs e)

        {

            Input.Image = Properties.Resources.input;

        }



        private void Input_MouseUp(object sender, MouseEventArgs e)

        {

            Input.Image = Properties.Resources.input_hover;

        }



        private void Yellow_MouseDown(object sender, MouseEventArgs e)

        {

            Yellow.Image = Properties.Resources.yellow_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Yellow"));

        }



        private void Yellow_MouseHover(object sender, EventArgs e)

        {

            Yellow.Image = Properties.Resources.yellow_hover;

        }



        private void Yellow_MouseLeave(object sender, EventArgs e)

        {

            Yellow.Image = Properties.Resources.yellow;

        }



        private void Yellow_MouseUp(object sender, MouseEventArgs e)

        {

            Yellow.Image = Properties.Resources.yellow_hover;

        }



        private void Blue_MouseDown(object sender, MouseEventArgs e)

        {

            Blue.Image = Properties.Resources.blue_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Blue"));

        }



        private void Blue_MouseHover(object sender, EventArgs e)

        {

            Blue.Image = Properties.Resources.blue_hover;

        }



        private void Blue_MouseLeave(object sender, EventArgs e)

        {

            Blue.Image = Properties.Resources.blue;

        }



        private void Blue_MouseUp(object sender, MouseEventArgs e)

        {

            Blue.Image = Properties.Resources.blue_hover;

        }



        private void Red_MouseDown(object sender, MouseEventArgs e)

        {

            Red.Image = Properties.Resources.red_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Red"));

        }



        private void Red_MouseHover(object sender, EventArgs e)

        {

            Red.Image = Properties.Resources.red_hover;

        }



        private void Red_MouseLeave(object sender, EventArgs e)

        {

            Red.Image = Properties.Resources.red;

        }



        private void Red_MouseUp(object sender, MouseEventArgs e)

        {

            Red.Image = Properties.Resources.red_hover;

        }



        private void SkipBack_MouseDown(object sender, MouseEventArgs e)

        {

            SkipBack.Image = Properties.Resources.skipbackwards_down;

            string rslts =curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Prev"));

        }



        private void SkipBack_MouseHover(object sender, EventArgs e)

        {

            SkipBack.Image = Properties.Resources.skipbackwards_hover;

        }



        private void SkipBack_MouseLeave(object sender, EventArgs e)

        {

            SkipBack.Image = Properties.Resources.skipbackwards;

        }



        private void SkipBack_MouseUp(object sender, MouseEventArgs e)

        {

            SkipBack.Image = Properties.Resources.skipbackwards_hover;

        }



        private void SkipForw_MouseDown(object sender, MouseEventArgs e)

        {

            SkipForw.Image = Properties.Resources.skipforward_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Next"));

        }



        private void SkipForw_MouseHover(object sender, EventArgs e)

        {

            SkipForw.Image = Properties.Resources.skipforward_hover;

        }



        private void SkipForw_MouseLeave(object sender, EventArgs e)

        {

            SkipForw.Image = Properties.Resources.skipforward;

        }



        private void SkipForw_MouseUp(object sender, MouseEventArgs e)

        {

            SkipForw.Image = Properties.Resources.skipforward_hover;

        }



        private void FastRew_MouseDown(object sender, MouseEventArgs e)

        {

            FastRew.Image = Properties.Resources.fastbackwards_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Rewind"));

            }

            else

            {

                string rslts =curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:FR"));

            }

        }



        private void FastRew_MouseHover(object sender, EventArgs e)

        {

            FastRew.Image = Properties.Resources.fastbackwards_hover;

        }



        private void FastRew_MouseLeave(object sender, EventArgs e)

        {

            FastRew.Image = Properties.Resources.fastbackwards;

        }



        private void FastRew_MouseUp(object sender, MouseEventArgs e)

        {

            FastRew.Image = Properties.Resources.fastbackwards_hover;

        }



        private void FastForw_MouseDown(object sender, MouseEventArgs e)

        {

            FastForw.Image = Properties.Resources.fastforward_down;

            if (devType == "TV")

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Forward"));

            }

            else

            {

                string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("STR:FF"));

            }

        }



        private void FastForw_MouseHover(object sender, EventArgs e)

        {

            FastForw.Image = Properties.Resources.fastforward_hover;

        }



        private void FastForw_MouseLeave(object sender, EventArgs e)

        {

            FastForw.Image = Properties.Resources.fastforward;

        }



        private void FastForw_MouseUp(object sender, MouseEventArgs e)

        {

            FastForw.Image = Properties.Resources.fastforward_hover;

        }



        private void Pause_MouseDown(object sender, MouseEventArgs e)

        {

            Pause.Image = Properties.Resources.pause_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Pause"));

        }



        private void Pause_MouseHover(object sender, EventArgs e)

        {

            Pause.Image = Properties.Resources.pause_hover;

        }



        private void Pause_MouseLeave(object sender, EventArgs e)

        {

            Pause.Image = Properties.Resources.pause;

        }



        private void Pause_MouseUp(object sender, MouseEventArgs e)

        {

            Pause.Image = Properties.Resources.pause_hover;

        }



        private void Play_MouseDown(object sender, MouseEventArgs e)

        {

            Play.Image = Properties.Resources.play_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Play"));

        }



        private void Play_MouseHover(object sender, EventArgs e)

        {

            Play.Image = Properties.Resources.play_hover;

        }



        private void Play_MouseLeave(object sender, EventArgs e)

        {

            Play.Image = Properties.Resources.play;

        }



        private void Play_MouseUp(object sender, MouseEventArgs e)

        {

            Play.Image = Properties.Resources.play_hover;

        }



        private void Stop_MouseDown(object sender, MouseEventArgs e)

        {

            Stop.Image = Properties.Resources.stop_down;

            string rslts = curDev.Ircc.SendIRCC(curDev, curDev.GetCommandString("Stop"));

        }



        private void Stop_MouseHover(object sender, EventArgs e)

        {

            Stop.Image = Properties.Resources.stop_hover;

        }



        private void Stop_MouseLeave(object sender, EventArgs e)

        {

            Stop.Image = Properties.Resources.stop;

        }



        private void Stop_MouseUp(object sender, MouseEventArgs e)

        {

            Stop.Image = Properties.Resources.stop_hover;

        }



        private void regButton_Click(object sender, EventArgs e)

        {

            bool devReg = curDev.Register();

            if (curDev.Registered == false)

            {

                //Check if Generaton 3. If yes, prompt for pin code

                if (curDev.Actionlist.RegisterMode == 3)

                {

                    Form4 ePin = new Form4();

                    ePin.ShowDialog();

                    string Pin = ePin.PinCode.Text;

                    devReg = curDev.SendAuth(Pin);

                    ePin.Dispose();

                }

               //Added by jrodriguez142514

               //Before the below code was added, the application would register a button click but never register a 

               //Generation 4 device.  Now it will open the new Form4 and show a dialog to enter pin.

               //Check if Generaton 4. If yes, prompt for pin code

                else if (curDev.Actionlist.RegisterMode == 4)

                {

                    Form4 ePin = new Form4();

                    ePin.ShowDialog();

                    string Pin = ePin.PinCode.Text;

                    devReg = curDev.SendAuth(Pin);

                    ePin.Dispose();

                    devReg = true;

                }



             }

             else

                 {

                   devReg = true;                

                 }

               //Added by jrodriguez142514

            }

        }



        private void Form3_Load(object sender, EventArgs e)

        {

            Device.SelectedIndex = 0;

        }



    }

}
