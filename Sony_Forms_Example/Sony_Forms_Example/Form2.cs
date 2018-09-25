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
    public partial class Devices : Form
    {
        public Devices()
        {
            InitializeComponent();
        }

        private void OK_but_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.Close();
            
        }

        private void Cancel_but_Click(object sender, EventArgs e)
        {
            this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.Close();
        }

        private void Add_but_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.Add();
            dataGridView1.Refresh();
            int r = dataGridView1.RowCount - 1;
            dataGridView1.CurrentCell = dataGridView1.Rows[r].Cells[0];
            dataGridView1.BeginEdit(true);
            if (dataGridView1.Rows.Count > 0)
            {
                Save_but.Enabled = true;
                Delete_But.Enabled = true;
                OK_but.Enabled = true;
            }
            else
            {
                Save_but.Enabled = false;
                Delete_But.Enabled = false;
                OK_but.Enabled = false;
            }
        }

        private void Delete_but_Click(object sender, EventArgs e)
        {
            dataGridView1.Rows.RemoveAt(dataGridView1.CurrentCell.RowIndex);
            if (dataGridView1.Rows.Count > 0)
            {
                Save_but.Enabled = true;
                OK_but.Enabled = true;
            }
            else
            {
                Save_but.Enabled = false;
                Delete_But.Enabled = false;
                OK_but.Enabled = false;
            }
        }

        private void Save_but_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            APILibrary.SonyDevice sDev = new APILibrary.SonyDevice();
            if (dataGridView1.Rows.Count > 0)
            {
                DataGridViewRow ro = dataGridView1.CurrentRow;
                if (Program.fDev.Count > 0)
                {
                    sDev = Program.fDev[ro.Index];
                    SaveFileDialog sfd = new SaveFileDialog();
                    sfd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
                    sfd.FilterIndex = 1;
                    sfd.InitialDirectory = Application.StartupPath;
                    sfd.RestoreDirectory = true;
                    sfd.FileName = sDev.Name + ".xml";
                    if (sfd.ShowDialog() == DialogResult.OK)
                    {
                        string fpth = sfd.FileName;
                        sfd.Dispose();
                        try
                        {
                            Program.mySonyLib.Locator.DeviceSave(fpth, sDev);
                        }
                        catch
                        {
                           MessageBox.Show("Error. File Not Saved!", "File Error", MessageBoxButtons.OK);
                        }
                    }
                }
                else
                {
                    MessageBox.Show("Error. No Device to Save!", "File Error", MessageBoxButtons.OK);
                }
            }
        }

        private void Load_but_Click(object sender, EventArgs e)
        {
            APILibrary.SonyDevice sDev = new APILibrary.SonyDevice();
            DataGridViewRow ro = dataGridView1.CurrentRow;
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.Filter = "xml files (*.xml)|*.xml|All files (*.*)|*.*";
            ofd.FilterIndex = 1;
            ofd.InitialDirectory = Application.StartupPath;
            ofd.RestoreDirectory = true;
            DataGridViewRow nr = new DataGridViewRow();
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                //deserialize
                string fpth = ofd.FileName;           
                sDev = Program.mySonyLib.Locator.DeviceLoad(fpth);
                ofd.Dispose();
                dataGridView1.Refresh();
                if (dataGridView1.Rows.Count < 1)
                {
                    dataGridView1.Rows.Insert(0, 1);
                    nr = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                    dataGridView1.Rows.RemoveAt(0);
                }
                else
                {
                    nr = (DataGridViewRow)dataGridView1.Rows[0].Clone();
                }
                nr.Cells[0].Value = sDev.Name;
                nr.Cells[1].Value = sDev.IPAddress;
                nr.Cells[2].Value = sDev.Actionlist.RegisterMode;
                nr.Cells[3].Value = sDev.DocumentUrl;
                dataGridView1.Rows.Add(nr);
                dataGridView1.Refresh();
                Program.fDev.Add(sDev);
            }
            if (dataGridView1.Rows.Count > 0)
            {
                Delete_But.Enabled = true;
                Save_but.Enabled = true;
                OK_but.Enabled = true;
            }
            else
            {
                Delete_But.Enabled = false; ;
                Save_but.Enabled = false; ;
                OK_but.Enabled = false;
            }
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            if (Program.fDev.Count > 0)
            {
                int i = 0;
                foreach (APILibrary.SonyDevice d in Program.fDev)
                {
                    dataGridView1.Refresh();
                    dataGridView1.Rows.Add();
                    dataGridView1.Refresh();
                    dataGridView1.Rows[i].Cells[0].Value = d.Name;
                    dataGridView1.Rows[i].Cells[1].Value = d.IPAddress;
                    dataGridView1.Rows[i].Cells[2].Value = d.Actionlist.RegisterMode.ToString();
                    dataGridView1.Rows[i].Cells[3].Value = d.DocumentUrl;
                    i++;
                }
                Delete_But.Enabled = true;
                OK_but.Enabled = true;
                Save_but.Enabled = true;
            }
            else
            {
                Delete_But.Enabled = false;
                
                OK_but.Enabled = false;
            }
            string dir = Application.StartupPath;
            string serializationFile = Path.Combine(dir, "devConfig.bin");
            if (File.Exists(serializationFile))
            {
                Load_but.Enabled = true;
            }
            else
            {
                //Load button is not being enabled and user can't choose to load an xml file they may already have
                //We comment out this line to let user choose an xml file and load it
                //Added by jrodriguez142514
                
                //Load_but.Enabled = false;
                
                //Added by jrodriguez142514
            }
        }

        private void Buildbut_Click(object sender, EventArgs e)
        {
            dataGridView1.Refresh();
            if (dataGridView1.Rows.Count > 0)
            {
                foreach (DataGridViewRow r in dataGridView1.Rows)
                {
                    if (r.Cells[3].Value != null)
                    {
                        if (r.Cells[0].Value == null)
                        {
                            APILibrary.SonyDevice nDev = new APILibrary.SonyDevice();
                            if (r.Cells[3].Value != null)
                            {
                                nDev.DocumentUrl = r.Cells[3].Value.ToString();
                                nDev.BuildFromDocument(new Uri(r.Cells[3].Value.ToString()));
                                r.Cells[0].Value = nDev.Name;
                                r.Cells[1].Value = nDev.IPAddress;
                                r.Cells[2].Value = nDev.Actionlist.RegisterMode;
                                Program.fDev.Add(nDev);
                            }
                        }
                    }
                }
            }
        }
    }
}
