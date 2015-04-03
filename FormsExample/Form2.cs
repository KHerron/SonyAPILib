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
using System.IO;

namespace Sony_Forms_Example
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            if (dataGridView1.Rows.Count > 0)
            {   
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    SonyAPI_Lib.SonyDevice nDev = new SonyAPI_Lib.SonyDevice();
                    if (r.Cells[0].Value == null)
                    {
                        break;
                    }
                    if (r.Cells[1].Value == null)
                    {
                        break;
                    }
                    nDev.Name = r.Cells[0].Value.ToString();
                    nDev.Device_IP_Address = r.Cells[1].Value.ToString();
                    if (r.Cells[2].Value != null)
                    {
                        nDev.actionList_URL = r.Cells[2].Value.ToString();
                    }
                    if (r.Cells[3].Value != null)
                    {
                        nDev.control_URL = r.Cells[3].Value.ToString();
                    }
                    Program.fDev.Add(nDev);
                }
            }
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void button3_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Refresh();
            int r = dataGridView1.RowCount - 1;
            dataGridView1.CurrentCell = dataGridView1.Rows[r].Cells[0];
            dataGridView1.BeginEdit(true);
        }

        private void button5_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
        }

        private void Form2_Load(object sender, EventArgs e)
        {

        }

        private void button4_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            List<Program.devConfig> sDev = new List<Program.devConfig>();
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    Program.devConfig nDev = new Program.devConfig();
                    if (r.Cells[0].Value == null)
                    {
                        break;
                    }
                    if (r.Cells[1].Value == null)
                    {
                        break;
                    }
                    nDev.dName = r.Cells[0].Value.ToString();
                    nDev.dIP = r.Cells[1].Value.ToString();
                    if (r.Cells[2].Value != null)
                    {
                        nDev.dAction = r.Cells[2].Value.ToString();
                    }
                    if (r.Cells[3].Value != null)
                    {
                        nDev.dControl = r.Cells[3].Value.ToString();
                    }
                    sDev.Add(nDev);
                    string dir = Application.StartupPath;
                    string serializationFile = Path.Combine(dir, "devConfig.bin");

                    //serialize
                    using (Stream stream = File.Open(serializationFile, FileMode.Create))
                    {
                        var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                        bformatter.Serialize(stream, sDev);
                    }
                }
            }
        }

        private void button6_Click(object sender, EventArgs e)
        {
            List<Program.devConfig> sDev = new List<Program.devConfig>();
            string dir = Application.StartupPath;
            string serializationFile = Path.Combine(dir, "devConfig.bin");
            //deserialize
            using (Stream stream = File.Open(serializationFile, FileMode.Open))
            {
                var bformatter = new System.Runtime.Serialization.Formatters.Binary.BinaryFormatter();

                sDev = (List<Program.devConfig>)bformatter.Deserialize(stream);
            }
            while (dataGridView1.Rows.Count > 0)
            {
                dataGridView1.Rows.RemoveAt(0);
            }
            Int16 i = 0;
            foreach (Program.devConfig d in sDev)
            {
                dataGridView1.Refresh();
                dataGridView1.Rows.Add();
                dataGridView1.Refresh();
                dataGridView1.Rows[i].Cells[0].Value = d.dName;
                dataGridView1.Rows[i].Cells[1].Value = d.dIP;
                dataGridView1.Rows[i].Cells[2].Value = d.dAction;
                dataGridView1.Rows[i].Cells[3].Value = d.dControl;
                i++;
            }
        }

        private void Form2_Load_1(object sender, EventArgs e)
        {
            if (Program.fDev.Count > 0)
            {
                int i = 0;
                foreach (SonyAPI_Lib.SonyDevice d in Program.fDev)
                {
                    dataGridView1.Refresh();
                    dataGridView1.Rows.Add();
                    dataGridView1.Refresh();
                    dataGridView1.Rows[i].Cells[0].Value = d.Name;
                    dataGridView1.Rows[i].Cells[1].Value = d.Device_IP_Address;
                    dataGridView1.Rows[i].Cells[2].Value = d.actionList_URL;
                    dataGridView1.Rows[i].Cells[3].Value = d.control_URL;
                    i++;
                }
            }
        }
    }
}
