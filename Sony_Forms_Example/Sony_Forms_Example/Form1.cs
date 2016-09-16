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
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            Program.mySonyLib.Log.Enable = false;  // This application will NOT use the API logging
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<string> fDevices = Program.mySonyLib.Locator.LocateDevices();
            if (fDevices.Count > 0)
            {
                foreach (string doc in fDevices)
                {
                    APILibrary.SonyDevice nDev = new APILibrary.SonyDevice();
                    nDev.DocumentUrl = doc;
                    nDev.BuildFromDocument(new Uri(nDev.DocumentUrl));
                    Program.fDev.Add(nDev);
                }
                Form dRemote = new Form3();
                dRemote.ShowDialog();
            }
            else
            {
                MessageBox.Show("No Devices were found!", "No Devices", MessageBoxButtons.OK);
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form mInit = new Devices();
            mInit.ShowDialog();
            if (mInit.DialogResult != System.Windows.Forms.DialogResult.Cancel)
            {
                if (Program.fDev.Count > 0)
                {
                    Form dRemote = new Form3();
                    dRemote.ShowDialog();
                }
            }
        }
    }
}
