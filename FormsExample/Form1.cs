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
            Program.mySonyLib.LOG.enableLogging = false;  // This application will NOT use the API logging
        }

        private void button4_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Program.fDev = Program.mySonyLib.API.sonyDiscover(null);
            if (Program.fDev.Count > 0)
            {
                Form dRemote = new Form3();
                dRemote.ShowDialog();
            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            Form mInit = new Form2();
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
