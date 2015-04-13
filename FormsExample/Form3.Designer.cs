namespace Sony_Forms_Example
{
    partial class Form3
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.Device = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.devName = new System.Windows.Forms.Label();
            this.devIP = new System.Windows.Forms.Label();
            this.devPort = new System.Windows.Forms.Label();
            this.devMac = new System.Windows.Forms.Label();
            this.devGen = new System.Windows.Forms.Label();
            this.devCommands = new System.Windows.Forms.Label();
            this.devSName = new System.Windows.Forms.Label();
            this.devSMac = new System.Windows.Forms.Label();
            this.devReg = new System.Windows.Forms.Label();
            this.devAction = new System.Windows.Forms.Label();
            this.devControl = new System.Windows.Forms.Label();
            this.Commands = new System.Windows.Forms.ComboBox();
            this.button1 = new System.Windows.Forms.Button();
            this.label13 = new System.Windows.Forms.Label();
            this.Chanlabel = new System.Windows.Forms.Label();
            this.button2 = new System.Windows.Forms.Button();
            this.Display = new System.Windows.Forms.Button();
            this.Stop = new System.Windows.Forms.Button();
            this.Play = new System.Windows.Forms.Button();
            this.Pause = new System.Windows.Forms.Button();
            this.FastForw = new System.Windows.Forms.Button();
            this.FastRew = new System.Windows.Forms.Button();
            this.SkipForw = new System.Windows.Forms.Button();
            this.SkipBack = new System.Windows.Forms.Button();
            this.Input = new System.Windows.Forms.Button();
            this.Mute = new System.Windows.Forms.Button();
            this.ChanDown = new System.Windows.Forms.Button();
            this.VolDown = new System.Windows.Forms.Button();
            this.ChanUp = new System.Windows.Forms.Button();
            this.VolUp = new System.Windows.Forms.Button();
            this.Options = new System.Windows.Forms.Button();
            this.Return = new System.Windows.Forms.Button();
            this.Home = new System.Windows.Forms.Button();
            this.Guide = new System.Windows.Forms.Button();
            this.Enter = new System.Windows.Forms.Button();
            this.Num0 = new System.Windows.Forms.Button();
            this.Dot = new System.Windows.Forms.Button();
            this.Num9 = new System.Windows.Forms.Button();
            this.Num8 = new System.Windows.Forms.Button();
            this.Num7 = new System.Windows.Forms.Button();
            this.Num6 = new System.Windows.Forms.Button();
            this.Num5 = new System.Windows.Forms.Button();
            this.Num4 = new System.Windows.Forms.Button();
            this.Num3 = new System.Windows.Forms.Button();
            this.Num2 = new System.Windows.Forms.Button();
            this.Num1 = new System.Windows.Forms.Button();
            this.Yellow = new System.Windows.Forms.Button();
            this.Blue = new System.Windows.Forms.Button();
            this.Red = new System.Windows.Forms.Button();
            this.Green = new System.Windows.Forms.Button();
            this.Right = new System.Windows.Forms.Button();
            this.Left = new System.Windows.Forms.Button();
            this.Down = new System.Windows.Forms.Button();
            this.Confirm = new System.Windows.Forms.Button();
            this.UP = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.regButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // Device
            // 
            this.Device.FormattingEnabled = true;
            this.Device.Location = new System.Drawing.Point(92, 6);
            this.Device.Name = "Device";
            this.Device.Size = new System.Drawing.Size(157, 21);
            this.Device.TabIndex = 0;
            this.Device.SelectedIndexChanged += new System.EventHandler(this.Device_SelectedIndexChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 9);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(74, 13);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select Device";
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(2, 136);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(90, 13);
            this.label2.TabIndex = 3;
            this.label2.Text = "Name:";
            this.label2.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(2, 159);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 13);
            this.label3.TabIndex = 4;
            this.label3.Text = "IP Address:";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(2, 182);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(90, 13);
            this.label4.TabIndex = 5;
            this.label4.Text = "Port:";
            this.label4.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(2, 228);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(90, 13);
            this.label5.TabIndex = 6;
            this.label5.Text = "Generation:";
            this.label5.TextAlign = System.Drawing.ContentAlignment.TopRight;
            this.label5.UseCompatibleTextRendering = true;
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(121, 33);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(90, 13);
            this.label6.TabIndex = 7;
            this.label6.Text = "Command Count";
            this.label6.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(2, 272);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(90, 13);
            this.label7.TabIndex = 8;
            this.label7.Text = "Server Name:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(2, 316);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(90, 13);
            this.label8.TabIndex = 9;
            this.label8.Text = "Registered:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label9
            // 
            this.label9.Location = new System.Drawing.Point(2, 205);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(90, 13);
            this.label9.TabIndex = 10;
            this.label9.Text = "Device MAC:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label10
            // 
            this.label10.Location = new System.Drawing.Point(2, 294);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(90, 13);
            this.label10.TabIndex = 11;
            this.label10.Text = "Server MAC:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label11
            // 
            this.label11.Location = new System.Drawing.Point(2, 338);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(90, 13);
            this.label11.TabIndex = 12;
            this.label11.Text = "Action List:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // label12
            // 
            this.label12.Location = new System.Drawing.Point(2, 361);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(90, 13);
            this.label12.TabIndex = 13;
            this.label12.Text = "Control URL:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // devName
            // 
            this.devName.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devName.Location = new System.Drawing.Point(98, 136);
            this.devName.Name = "devName";
            this.devName.Size = new System.Drawing.Size(200, 13);
            this.devName.TabIndex = 14;
            // 
            // devIP
            // 
            this.devIP.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devIP.Location = new System.Drawing.Point(98, 159);
            this.devIP.Name = "devIP";
            this.devIP.Size = new System.Drawing.Size(200, 13);
            this.devIP.TabIndex = 15;
            // 
            // devPort
            // 
            this.devPort.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devPort.Location = new System.Drawing.Point(98, 182);
            this.devPort.Name = "devPort";
            this.devPort.Size = new System.Drawing.Size(200, 13);
            this.devPort.TabIndex = 16;
            // 
            // devMac
            // 
            this.devMac.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devMac.Location = new System.Drawing.Point(98, 205);
            this.devMac.Name = "devMac";
            this.devMac.Size = new System.Drawing.Size(200, 13);
            this.devMac.TabIndex = 17;
            // 
            // devGen
            // 
            this.devGen.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devGen.Location = new System.Drawing.Point(98, 228);
            this.devGen.Name = "devGen";
            this.devGen.Size = new System.Drawing.Size(200, 13);
            this.devGen.TabIndex = 18;
            // 
            // devCommands
            // 
            this.devCommands.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devCommands.Location = new System.Drawing.Point(217, 33);
            this.devCommands.Name = "devCommands";
            this.devCommands.Size = new System.Drawing.Size(32, 13);
            this.devCommands.TabIndex = 19;
            // 
            // devSName
            // 
            this.devSName.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devSName.Location = new System.Drawing.Point(98, 272);
            this.devSName.Name = "devSName";
            this.devSName.Size = new System.Drawing.Size(200, 13);
            this.devSName.TabIndex = 20;
            // 
            // devSMac
            // 
            this.devSMac.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devSMac.Location = new System.Drawing.Point(98, 294);
            this.devSMac.Name = "devSMac";
            this.devSMac.Size = new System.Drawing.Size(200, 13);
            this.devSMac.TabIndex = 21;
            // 
            // devReg
            // 
            this.devReg.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devReg.Location = new System.Drawing.Point(98, 316);
            this.devReg.Name = "devReg";
            this.devReg.Size = new System.Drawing.Size(200, 13);
            this.devReg.TabIndex = 22;
            // 
            // devAction
            // 
            this.devAction.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devAction.Location = new System.Drawing.Point(98, 338);
            this.devAction.Name = "devAction";
            this.devAction.Size = new System.Drawing.Size(250, 13);
            this.devAction.TabIndex = 23;
            // 
            // devControl
            // 
            this.devControl.BackColor = System.Drawing.SystemColors.HighlightText;
            this.devControl.Location = new System.Drawing.Point(98, 361);
            this.devControl.Name = "devControl";
            this.devControl.Size = new System.Drawing.Size(250, 13);
            this.devControl.TabIndex = 24;
            // 
            // Commands
            // 
            this.Commands.FormattingEnabled = true;
            this.Commands.Location = new System.Drawing.Point(121, 59);
            this.Commands.Name = "Commands";
            this.Commands.Size = new System.Drawing.Size(174, 21);
            this.Commands.TabIndex = 25;
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(247, 86);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(48, 23);
            this.button1.TabIndex = 26;
            this.button1.Text = "Run";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label13.Location = new System.Drawing.Point(346, 221);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(69, 17);
            this.label13.TabIndex = 56;
            this.label13.Text = "Volume";
            this.label13.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // Chanlabel
            // 
            this.Chanlabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Chanlabel.Location = new System.Drawing.Point(409, 221);
            this.Chanlabel.Name = "Chanlabel";
            this.Chanlabel.Size = new System.Drawing.Size(69, 17);
            this.Chanlabel.TabIndex = 57;
            this.Chanlabel.Text = "Channel";
            this.Chanlabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(829, 351);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(75, 23);
            this.button2.TabIndex = 67;
            this.button2.Text = "Exit";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Display
            // 
            this.Display.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.display;
            this.Display.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Display.Location = new System.Drawing.Point(468, 168);
            this.Display.Name = "Display";
            this.Display.Size = new System.Drawing.Size(65, 45);
            this.Display.TabIndex = 68;
            this.Display.UseVisualStyleBackColor = true;
            this.Display.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Display_MouseDown);
            this.Display.MouseLeave += new System.EventHandler(this.Display_MouseLeave);
            this.Display.MouseHover += new System.EventHandler(this.Display_MouseHover);
            this.Display.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Display_MouseUp);
            // 
            // Stop
            // 
            this.Stop.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.stop;
            this.Stop.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Stop.Location = new System.Drawing.Point(830, 163);
            this.Stop.Name = "Stop";
            this.Stop.Size = new System.Drawing.Size(50, 50);
            this.Stop.TabIndex = 66;
            this.Stop.UseVisualStyleBackColor = true;
            this.Stop.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Stop_MouseDown);
            this.Stop.MouseLeave += new System.EventHandler(this.Stop_MouseLeave);
            this.Stop.MouseHover += new System.EventHandler(this.Stop_MouseHover);
            this.Stop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Stop_MouseUp);
            // 
            // Play
            // 
            this.Play.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.play;
            this.Play.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Play.Location = new System.Drawing.Point(774, 163);
            this.Play.Name = "Play";
            this.Play.Size = new System.Drawing.Size(50, 50);
            this.Play.TabIndex = 65;
            this.Play.UseVisualStyleBackColor = true;
            this.Play.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Play_MouseDown);
            this.Play.MouseLeave += new System.EventHandler(this.Play_MouseLeave);
            this.Play.MouseHover += new System.EventHandler(this.Play_MouseHover);
            this.Play.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Play_MouseUp);
            // 
            // Pause
            // 
            this.Pause.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.pause;
            this.Pause.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Pause.Location = new System.Drawing.Point(718, 163);
            this.Pause.Name = "Pause";
            this.Pause.Size = new System.Drawing.Size(50, 50);
            this.Pause.TabIndex = 64;
            this.Pause.UseVisualStyleBackColor = true;
            this.Pause.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Pause_MouseDown);
            this.Pause.MouseLeave += new System.EventHandler(this.Pause_MouseLeave);
            this.Pause.MouseHover += new System.EventHandler(this.Pause_MouseHover);
            this.Pause.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Pause_MouseUp);
            // 
            // FastForw
            // 
            this.FastForw.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.fastforward;
            this.FastForw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FastForw.Location = new System.Drawing.Point(799, 104);
            this.FastForw.Name = "FastForw";
            this.FastForw.Size = new System.Drawing.Size(50, 50);
            this.FastForw.TabIndex = 63;
            this.FastForw.UseVisualStyleBackColor = true;
            this.FastForw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FastForw_MouseDown);
            this.FastForw.MouseLeave += new System.EventHandler(this.FastForw_MouseLeave);
            this.FastForw.MouseHover += new System.EventHandler(this.FastForw_MouseHover);
            this.FastForw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FastForw_MouseUp);
            // 
            // FastRew
            // 
            this.FastRew.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.fastbackwards;
            this.FastRew.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.FastRew.Location = new System.Drawing.Point(743, 104);
            this.FastRew.Name = "FastRew";
            this.FastRew.Size = new System.Drawing.Size(50, 50);
            this.FastRew.TabIndex = 62;
            this.FastRew.UseVisualStyleBackColor = true;
            this.FastRew.MouseDown += new System.Windows.Forms.MouseEventHandler(this.FastRew_MouseDown);
            this.FastRew.MouseLeave += new System.EventHandler(this.FastRew_MouseLeave);
            this.FastRew.MouseHover += new System.EventHandler(this.FastRew_MouseHover);
            this.FastRew.MouseUp += new System.Windows.Forms.MouseEventHandler(this.FastRew_MouseUp);
            // 
            // SkipForw
            // 
            this.SkipForw.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.skipforward;
            this.SkipForw.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SkipForw.Location = new System.Drawing.Point(855, 104);
            this.SkipForw.Name = "SkipForw";
            this.SkipForw.Size = new System.Drawing.Size(50, 50);
            this.SkipForw.TabIndex = 61;
            this.SkipForw.UseVisualStyleBackColor = true;
            this.SkipForw.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SkipForw_MouseDown);
            this.SkipForw.MouseLeave += new System.EventHandler(this.SkipForw_MouseLeave);
            this.SkipForw.MouseHover += new System.EventHandler(this.SkipForw_MouseHover);
            this.SkipForw.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SkipForw_MouseUp);
            // 
            // SkipBack
            // 
            this.SkipBack.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.skipbackwards;
            this.SkipBack.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.SkipBack.Location = new System.Drawing.Point(687, 104);
            this.SkipBack.Name = "SkipBack";
            this.SkipBack.Size = new System.Drawing.Size(50, 50);
            this.SkipBack.TabIndex = 60;
            this.SkipBack.UseVisualStyleBackColor = true;
            this.SkipBack.MouseDown += new System.Windows.Forms.MouseEventHandler(this.SkipBack_MouseDown);
            this.SkipBack.MouseLeave += new System.EventHandler(this.SkipBack_MouseLeave);
            this.SkipBack.MouseHover += new System.EventHandler(this.SkipBack_MouseHover);
            this.SkipBack.MouseUp += new System.Windows.Forms.MouseEventHandler(this.SkipBack_MouseUp);
            // 
            // Input
            // 
            this.Input.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.input;
            this.Input.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Input.Location = new System.Drawing.Point(618, 239);
            this.Input.Name = "Input";
            this.Input.Size = new System.Drawing.Size(63, 50);
            this.Input.TabIndex = 59;
            this.Input.UseVisualStyleBackColor = true;
            this.Input.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Input_MouseDown);
            this.Input.MouseLeave += new System.EventHandler(this.Input_MouseLeave);
            this.Input.MouseHover += new System.EventHandler(this.Input_MouseHover);
            this.Input.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Input_MouseUp);
            // 
            // Mute
            // 
            this.Mute.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.muting;
            this.Mute.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Mute.Location = new System.Drawing.Point(537, 239);
            this.Mute.Name = "Mute";
            this.Mute.Size = new System.Drawing.Size(63, 50);
            this.Mute.TabIndex = 58;
            this.Mute.UseVisualStyleBackColor = true;
            this.Mute.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Mute_MouseDown);
            this.Mute.MouseLeave += new System.EventHandler(this.Mute_MouseLeave);
            this.Mute.MouseHover += new System.EventHandler(this.Mute_MouseHover);
            this.Mute.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Mute_MouseUp);
            // 
            // ChanDown
            // 
            this.ChanDown.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.minus;
            this.ChanDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ChanDown.Location = new System.Drawing.Point(415, 239);
            this.ChanDown.Name = "ChanDown";
            this.ChanDown.Size = new System.Drawing.Size(50, 50);
            this.ChanDown.TabIndex = 55;
            this.ChanDown.UseVisualStyleBackColor = true;
            this.ChanDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChanDown_MouseDown);
            this.ChanDown.MouseLeave += new System.EventHandler(this.ChanDown_MouseLeave);
            this.ChanDown.MouseHover += new System.EventHandler(this.ChanDown_MouseHover);
            this.ChanDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChanDown_MouseUp);
            // 
            // VolDown
            // 
            this.VolDown.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.minus;
            this.VolDown.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VolDown.Location = new System.Drawing.Point(359, 239);
            this.VolDown.Name = "VolDown";
            this.VolDown.Size = new System.Drawing.Size(50, 50);
            this.VolDown.TabIndex = 54;
            this.VolDown.UseVisualStyleBackColor = true;
            this.VolDown.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VolDown_MouseDown);
            this.VolDown.MouseLeave += new System.EventHandler(this.VolDown_MouseLeave);
            this.VolDown.MouseHover += new System.EventHandler(this.VolDown_MouseHover);
            this.VolDown.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VolDown_MouseUp);
            // 
            // ChanUp
            // 
            this.ChanUp.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.plus;
            this.ChanUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.ChanUp.Location = new System.Drawing.Point(415, 168);
            this.ChanUp.Name = "ChanUp";
            this.ChanUp.Size = new System.Drawing.Size(50, 50);
            this.ChanUp.TabIndex = 53;
            this.ChanUp.UseVisualStyleBackColor = true;
            this.ChanUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.ChanUp_MouseDown);
            this.ChanUp.MouseLeave += new System.EventHandler(this.ChanUp_MouseLeave);
            this.ChanUp.MouseHover += new System.EventHandler(this.ChanUp_MouseHover);
            this.ChanUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.ChanUp_MouseUp);
            // 
            // VolUp
            // 
            this.VolUp.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.plus;
            this.VolUp.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.VolUp.Location = new System.Drawing.Point(359, 168);
            this.VolUp.Name = "VolUp";
            this.VolUp.Size = new System.Drawing.Size(50, 50);
            this.VolUp.TabIndex = 52;
            this.VolUp.UseVisualStyleBackColor = true;
            this.VolUp.MouseDown += new System.Windows.Forms.MouseEventHandler(this.VolUp_MouseDown);
            this.VolUp.MouseLeave += new System.EventHandler(this.VolUp_MouseLeave);
            this.VolUp.MouseHover += new System.EventHandler(this.VolUp_MouseHover);
            this.VolUp.MouseUp += new System.Windows.Forms.MouseEventHandler(this.VolUp_MouseUp);
            // 
            // Options
            // 
            this.Options.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.options;
            this.Options.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Options.Location = new System.Drawing.Point(468, 117);
            this.Options.Name = "Options";
            this.Options.Size = new System.Drawing.Size(65, 50);
            this.Options.TabIndex = 51;
            this.Options.UseVisualStyleBackColor = true;
            this.Options.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Options_MouseDown);
            this.Options.MouseLeave += new System.EventHandler(this.Options_MouseLeave);
            this.Options.MouseHover += new System.EventHandler(this.Options_MouseHover);
            this.Options.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Options_MouseUp);
            // 
            // Return
            // 
            this.Return.BackgroundImage = global::Sony_Forms_Example.Properties.Resources._return;
            this.Return.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Return.Location = new System.Drawing.Point(359, 117);
            this.Return.Name = "Return";
            this.Return.Size = new System.Drawing.Size(65, 50);
            this.Return.TabIndex = 50;
            this.Return.UseVisualStyleBackColor = true;
            this.Return.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Return_MouseDown);
            this.Return.MouseLeave += new System.EventHandler(this.Return_MouseLeave);
            this.Return.MouseHover += new System.EventHandler(this.Return_MouseHover);
            this.Return.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Return_MouseUp);
            // 
            // Home
            // 
            this.Home.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.home;
            this.Home.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Home.Location = new System.Drawing.Point(468, 17);
            this.Home.Name = "Home";
            this.Home.Size = new System.Drawing.Size(65, 50);
            this.Home.TabIndex = 49;
            this.Home.UseVisualStyleBackColor = true;
            this.Home.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Home_MouseDown);
            this.Home.MouseLeave += new System.EventHandler(this.Home_MouseLeave);
            this.Home.MouseHover += new System.EventHandler(this.Home_MouseHover);
            this.Home.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Home_MouseUp);
            // 
            // Guide
            // 
            this.Guide.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.guide;
            this.Guide.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Guide.Location = new System.Drawing.Point(359, 17);
            this.Guide.Name = "Guide";
            this.Guide.Size = new System.Drawing.Size(65, 50);
            this.Guide.TabIndex = 48;
            this.Guide.UseVisualStyleBackColor = true;
            this.Guide.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Guide_MouseDown);
            this.Guide.MouseLeave += new System.EventHandler(this.Guide_MouseLeave);
            this.Guide.MouseHover += new System.EventHandler(this.Guide_MouseHover);
            this.Guide.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Guide_MouseUp);
            // 
            // Enter
            // 
            this.Enter.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.enter;
            this.Enter.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Enter.Location = new System.Drawing.Point(631, 163);
            this.Enter.Name = "Enter";
            this.Enter.Size = new System.Drawing.Size(50, 50);
            this.Enter.TabIndex = 47;
            this.Enter.UseVisualStyleBackColor = true;
            this.Enter.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Enter_MouseDown);
            this.Enter.MouseLeave += new System.EventHandler(this.Enter_MouseLeave);
            this.Enter.MouseHover += new System.EventHandler(this.Enter_MouseHover);
            this.Enter.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Enter_MouseUp);
            // 
            // Num0
            // 
            this.Num0.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num0;
            this.Num0.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num0.Location = new System.Drawing.Point(584, 163);
            this.Num0.Name = "Num0";
            this.Num0.Size = new System.Drawing.Size(50, 50);
            this.Num0.TabIndex = 46;
            this.Num0.UseVisualStyleBackColor = true;
            this.Num0.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num0_MouseDown);
            this.Num0.MouseLeave += new System.EventHandler(this.Num0_MouseLeave);
            this.Num0.MouseHover += new System.EventHandler(this.Num0_MouseHover);
            this.Num0.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num0_MouseUp);
            // 
            // Dot
            // 
            this.Dot.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.dot;
            this.Dot.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Dot.Location = new System.Drawing.Point(537, 163);
            this.Dot.Name = "Dot";
            this.Dot.Size = new System.Drawing.Size(50, 50);
            this.Dot.TabIndex = 45;
            this.Dot.UseVisualStyleBackColor = true;
            this.Dot.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Dot_MouseDown);
            this.Dot.MouseLeave += new System.EventHandler(this.Dot_MouseLeave);
            this.Dot.MouseHover += new System.EventHandler(this.Dot_MouseHover);
            this.Dot.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Dot_MouseUp);
            // 
            // Num9
            // 
            this.Num9.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num9;
            this.Num9.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num9.Location = new System.Drawing.Point(631, 116);
            this.Num9.Name = "Num9";
            this.Num9.Size = new System.Drawing.Size(50, 50);
            this.Num9.TabIndex = 44;
            this.Num9.UseVisualStyleBackColor = true;
            this.Num9.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num9_MouseDown);
            this.Num9.MouseLeave += new System.EventHandler(this.Num9_MouseLeave);
            this.Num9.MouseHover += new System.EventHandler(this.Num9_MouseHover);
            this.Num9.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num9_MouseUp);
            // 
            // Num8
            // 
            this.Num8.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num8;
            this.Num8.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num8.Location = new System.Drawing.Point(584, 116);
            this.Num8.Name = "Num8";
            this.Num8.Size = new System.Drawing.Size(50, 50);
            this.Num8.TabIndex = 43;
            this.Num8.UseVisualStyleBackColor = true;
            this.Num8.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num8_MouseDown);
            this.Num8.MouseLeave += new System.EventHandler(this.Num8_MouseLeave);
            this.Num8.MouseHover += new System.EventHandler(this.Num8_MouseHover);
            this.Num8.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num8_MouseUp);
            // 
            // Num7
            // 
            this.Num7.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num7;
            this.Num7.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num7.Location = new System.Drawing.Point(537, 116);
            this.Num7.Name = "Num7";
            this.Num7.Size = new System.Drawing.Size(50, 50);
            this.Num7.TabIndex = 42;
            this.Num7.UseVisualStyleBackColor = true;
            this.Num7.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num7_MouseDown);
            this.Num7.MouseLeave += new System.EventHandler(this.Num7_MouseLeave);
            this.Num7.MouseHover += new System.EventHandler(this.Num7_MouseHover);
            this.Num7.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num7_MouseUp);
            // 
            // Num6
            // 
            this.Num6.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num6;
            this.Num6.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num6.Location = new System.Drawing.Point(631, 67);
            this.Num6.Name = "Num6";
            this.Num6.Size = new System.Drawing.Size(50, 50);
            this.Num6.TabIndex = 41;
            this.Num6.UseVisualStyleBackColor = true;
            this.Num6.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num6_MouseDown);
            this.Num6.MouseLeave += new System.EventHandler(this.Num6_MouseLeave);
            this.Num6.MouseHover += new System.EventHandler(this.Num6_MouseHover);
            this.Num6.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num6_MouseUp);
            // 
            // Num5
            // 
            this.Num5.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num5;
            this.Num5.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num5.Location = new System.Drawing.Point(584, 67);
            this.Num5.Name = "Num5";
            this.Num5.Size = new System.Drawing.Size(50, 50);
            this.Num5.TabIndex = 40;
            this.Num5.UseVisualStyleBackColor = true;
            this.Num5.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num5_MouseDown);
            this.Num5.MouseLeave += new System.EventHandler(this.Num5_MouseLeave);
            this.Num5.MouseHover += new System.EventHandler(this.Num5_MouseHover);
            this.Num5.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num5_MouseUp);
            // 
            // Num4
            // 
            this.Num4.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num4;
            this.Num4.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num4.Location = new System.Drawing.Point(537, 67);
            this.Num4.Name = "Num4";
            this.Num4.Size = new System.Drawing.Size(50, 50);
            this.Num4.TabIndex = 39;
            this.Num4.UseVisualStyleBackColor = true;
            this.Num4.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num4_MouseDown);
            this.Num4.MouseLeave += new System.EventHandler(this.Num4_MouseLeave);
            this.Num4.MouseHover += new System.EventHandler(this.Num4_MouseHover);
            this.Num4.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num4_MouseUp);
            // 
            // Num3
            // 
            this.Num3.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num3;
            this.Num3.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num3.Location = new System.Drawing.Point(631, 16);
            this.Num3.Name = "Num3";
            this.Num3.Size = new System.Drawing.Size(50, 50);
            this.Num3.TabIndex = 38;
            this.Num3.UseVisualStyleBackColor = true;
            this.Num3.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num3_MouseDown);
            this.Num3.MouseLeave += new System.EventHandler(this.Num3_MouseLeave);
            this.Num3.MouseHover += new System.EventHandler(this.Num3_MouseHover);
            this.Num3.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num3_MouseUp);
            // 
            // Num2
            // 
            this.Num2.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num2;
            this.Num2.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num2.Location = new System.Drawing.Point(584, 16);
            this.Num2.Name = "Num2";
            this.Num2.Size = new System.Drawing.Size(50, 50);
            this.Num2.TabIndex = 37;
            this.Num2.UseVisualStyleBackColor = true;
            this.Num2.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num2_MouseDown);
            this.Num2.MouseLeave += new System.EventHandler(this.Num2_MouseLeave);
            this.Num2.MouseHover += new System.EventHandler(this.Num2_MouseHover);
            this.Num2.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num2_MouseUp);
            // 
            // Num1
            // 
            this.Num1.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.num1;
            this.Num1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Num1.Location = new System.Drawing.Point(537, 16);
            this.Num1.Name = "Num1";
            this.Num1.Size = new System.Drawing.Size(50, 50);
            this.Num1.TabIndex = 36;
            this.Num1.UseVisualStyleBackColor = true;
            this.Num1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Num1_MouseDown);
            this.Num1.MouseLeave += new System.EventHandler(this.Num1_MouseLeave);
            this.Num1.MouseHover += new System.EventHandler(this.Num1_MouseHover);
            this.Num1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Num1_MouseUp);
            // 
            // Yellow
            // 
            this.Yellow.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.yellow;
            this.Yellow.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Yellow.Location = new System.Drawing.Point(686, 14);
            this.Yellow.Name = "Yellow";
            this.Yellow.Size = new System.Drawing.Size(50, 50);
            this.Yellow.TabIndex = 35;
            this.Yellow.UseVisualStyleBackColor = true;
            this.Yellow.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Yellow_MouseDown);
            this.Yellow.MouseLeave += new System.EventHandler(this.Yellow_MouseLeave);
            this.Yellow.MouseHover += new System.EventHandler(this.Yellow_MouseHover);
            this.Yellow.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Yellow_MouseUp);
            // 
            // Blue
            // 
            this.Blue.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.blue;
            this.Blue.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Blue.Location = new System.Drawing.Point(742, 14);
            this.Blue.Name = "Blue";
            this.Blue.Size = new System.Drawing.Size(50, 50);
            this.Blue.TabIndex = 34;
            this.Blue.UseVisualStyleBackColor = true;
            this.Blue.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Blue_MouseDown);
            this.Blue.MouseLeave += new System.EventHandler(this.Blue_MouseLeave);
            this.Blue.MouseHover += new System.EventHandler(this.Blue_MouseHover);
            this.Blue.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Blue_MouseUp);
            // 
            // Red
            // 
            this.Red.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.red;
            this.Red.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Red.Location = new System.Drawing.Point(798, 14);
            this.Red.Name = "Red";
            this.Red.Size = new System.Drawing.Size(50, 50);
            this.Red.TabIndex = 33;
            this.Red.UseVisualStyleBackColor = true;
            this.Red.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Red_MouseDown);
            this.Red.MouseLeave += new System.EventHandler(this.Red_MouseLeave);
            this.Red.MouseHover += new System.EventHandler(this.Red_MouseHover);
            this.Red.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Red_MouseUp);
            // 
            // Green
            // 
            this.Green.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.green;
            this.Green.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Green.Location = new System.Drawing.Point(854, 14);
            this.Green.Name = "Green";
            this.Green.Size = new System.Drawing.Size(50, 50);
            this.Green.TabIndex = 32;
            this.Green.UseVisualStyleBackColor = true;
            this.Green.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Green_MouseDown);
            this.Green.MouseLeave += new System.EventHandler(this.Green_MouseLeave);
            this.Green.MouseHover += new System.EventHandler(this.Green_MouseHover);
            this.Green.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Green_MouseUp);
            // 
            // Right
            // 
            this.Right.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.arrowr;
            this.Right.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Right.Location = new System.Drawing.Point(468, 67);
            this.Right.Name = "Right";
            this.Right.Size = new System.Drawing.Size(65, 50);
            this.Right.TabIndex = 31;
            this.Right.UseVisualStyleBackColor = true;
            this.Right.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Right_MouseDown);
            this.Right.MouseLeave += new System.EventHandler(this.Right_MouseLeave);
            this.Right.MouseHover += new System.EventHandler(this.Right_MouseHover);
            this.Right.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Right_MouseUp);
            // 
            // Left
            // 
            this.Left.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.arrowl;
            this.Left.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Left.Location = new System.Drawing.Point(359, 67);
            this.Left.Name = "Left";
            this.Left.Size = new System.Drawing.Size(65, 50);
            this.Left.TabIndex = 30;
            this.Left.UseVisualStyleBackColor = true;
            this.Left.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Left_MouseDown);
            this.Left.MouseLeave += new System.EventHandler(this.Left_MouseLeave);
            this.Left.MouseHover += new System.EventHandler(this.Left_MouseHover);
            this.Left.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Left_MouseUp);
            // 
            // Down
            // 
            this.Down.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.arrowd;
            this.Down.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Down.Location = new System.Drawing.Point(421, 117);
            this.Down.Name = "Down";
            this.Down.Size = new System.Drawing.Size(50, 50);
            this.Down.TabIndex = 29;
            this.Down.UseVisualStyleBackColor = true;
            this.Down.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Down_MouseDown);
            this.Down.MouseLeave += new System.EventHandler(this.Down_MouseLeave);
            this.Down.MouseHover += new System.EventHandler(this.Down_MouseHover);
            this.Down.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Down_MouseUp);
            // 
            // Confirm
            // 
            this.Confirm.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.confirm;
            this.Confirm.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.Confirm.Location = new System.Drawing.Point(421, 67);
            this.Confirm.Name = "Confirm";
            this.Confirm.Size = new System.Drawing.Size(50, 50);
            this.Confirm.TabIndex = 28;
            this.Confirm.UseVisualStyleBackColor = true;
            this.Confirm.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Confirm_MouseDown);
            this.Confirm.MouseLeave += new System.EventHandler(this.Confirm_MouseLeave);
            this.Confirm.MouseHover += new System.EventHandler(this.Confirm_MouseHover);
            this.Confirm.MouseUp += new System.Windows.Forms.MouseEventHandler(this.Confirm_MouseUp);
            // 
            // UP
            // 
            this.UP.BackgroundImage = global::Sony_Forms_Example.Properties.Resources.arrowu;
            this.UP.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.UP.Location = new System.Drawing.Point(421, 17);
            this.UP.Name = "UP";
            this.UP.Size = new System.Drawing.Size(50, 50);
            this.UP.TabIndex = 27;
            this.UP.UseVisualStyleBackColor = true;
            this.UP.MouseDown += new System.Windows.Forms.MouseEventHandler(this.UP_MouseDown);
            this.UP.MouseLeave += new System.EventHandler(this.UP_MouseLeave);
            this.UP.MouseHover += new System.EventHandler(this.UP_MouseHover);
            this.UP.MouseUp += new System.Windows.Forms.MouseEventHandler(this.UP_MouseUp);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(15, 33);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(100, 100);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // regButton
            // 
            this.regButton.Location = new System.Drawing.Point(371, 311);
            this.regButton.Name = "regButton";
            this.regButton.Size = new System.Drawing.Size(94, 23);
            this.regButton.TabIndex = 69;
            this.regButton.Text = "Register";
            this.regButton.UseVisualStyleBackColor = true;
            this.regButton.Click += new System.EventHandler(this.regButton_Click);
            // 
            // Form3
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(916, 402);
            this.Controls.Add(this.regButton);
            this.Controls.Add(this.Display);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.Stop);
            this.Controls.Add(this.Play);
            this.Controls.Add(this.Pause);
            this.Controls.Add(this.FastForw);
            this.Controls.Add(this.FastRew);
            this.Controls.Add(this.SkipForw);
            this.Controls.Add(this.SkipBack);
            this.Controls.Add(this.Input);
            this.Controls.Add(this.Mute);
            this.Controls.Add(this.Chanlabel);
            this.Controls.Add(this.label13);
            this.Controls.Add(this.ChanDown);
            this.Controls.Add(this.VolDown);
            this.Controls.Add(this.ChanUp);
            this.Controls.Add(this.VolUp);
            this.Controls.Add(this.Options);
            this.Controls.Add(this.Return);
            this.Controls.Add(this.Home);
            this.Controls.Add(this.Guide);
            this.Controls.Add(this.Enter);
            this.Controls.Add(this.Num0);
            this.Controls.Add(this.Dot);
            this.Controls.Add(this.Num9);
            this.Controls.Add(this.Num8);
            this.Controls.Add(this.Num7);
            this.Controls.Add(this.Num6);
            this.Controls.Add(this.Num5);
            this.Controls.Add(this.Num4);
            this.Controls.Add(this.Num3);
            this.Controls.Add(this.Num2);
            this.Controls.Add(this.Num1);
            this.Controls.Add(this.Yellow);
            this.Controls.Add(this.Blue);
            this.Controls.Add(this.Red);
            this.Controls.Add(this.Green);
            this.Controls.Add(this.Right);
            this.Controls.Add(this.Left);
            this.Controls.Add(this.Down);
            this.Controls.Add(this.Confirm);
            this.Controls.Add(this.UP);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.Commands);
            this.Controls.Add(this.devControl);
            this.Controls.Add(this.devAction);
            this.Controls.Add(this.devReg);
            this.Controls.Add(this.devSMac);
            this.Controls.Add(this.devSName);
            this.Controls.Add(this.devCommands);
            this.Controls.Add(this.devGen);
            this.Controls.Add(this.devMac);
            this.Controls.Add(this.devPort);
            this.Controls.Add(this.devIP);
            this.Controls.Add(this.devName);
            this.Controls.Add(this.label12);
            this.Controls.Add(this.label11);
            this.Controls.Add(this.label10);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Device);
            this.Name = "Form3";
            this.Text = "Form3";
            this.Load += new System.EventHandler(this.Form3_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.ComboBox Device;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label devName;
        private System.Windows.Forms.Label devIP;
        private System.Windows.Forms.Label devPort;
        private System.Windows.Forms.Label devMac;
        private System.Windows.Forms.Label devGen;
        private System.Windows.Forms.Label devCommands;
        private System.Windows.Forms.Label devSName;
        private System.Windows.Forms.Label devSMac;
        private System.Windows.Forms.Label devReg;
        private System.Windows.Forms.Label devAction;
        private System.Windows.Forms.Label devControl;
        private System.Windows.Forms.ComboBox Commands;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button UP;
        private System.Windows.Forms.Button Confirm;
        private System.Windows.Forms.Button Down;
        private System.Windows.Forms.Button Left;
        private System.Windows.Forms.Button Right;
        private System.Windows.Forms.Button Green;
        private System.Windows.Forms.Button Red;
        private System.Windows.Forms.Button Blue;
        private System.Windows.Forms.Button Yellow;
        private System.Windows.Forms.Button Num1;
        private System.Windows.Forms.Button Num2;
        private System.Windows.Forms.Button Num3;
        private System.Windows.Forms.Button Num6;
        private System.Windows.Forms.Button Num5;
        private System.Windows.Forms.Button Num4;
        private System.Windows.Forms.Button Num9;
        private System.Windows.Forms.Button Num8;
        private System.Windows.Forms.Button Num7;
        private System.Windows.Forms.Button Enter;
        private System.Windows.Forms.Button Num0;
        private System.Windows.Forms.Button Dot;
        private System.Windows.Forms.Button Guide;
        private System.Windows.Forms.Button Home;
        private System.Windows.Forms.Button Return;
        private System.Windows.Forms.Button Options;
        private System.Windows.Forms.Button VolUp;
        private System.Windows.Forms.Button ChanUp;
        private System.Windows.Forms.Button ChanDown;
        private System.Windows.Forms.Button VolDown;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label Chanlabel;
        private System.Windows.Forms.Button Mute;
        private System.Windows.Forms.Button Input;
        private System.Windows.Forms.Button SkipBack;
        private System.Windows.Forms.Button SkipForw;
        private System.Windows.Forms.Button FastRew;
        private System.Windows.Forms.Button FastForw;
        private System.Windows.Forms.Button Pause;
        private System.Windows.Forms.Button Play;
        private System.Windows.Forms.Button Stop;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button Display;
        private System.Windows.Forms.Button regButton;
    }
}