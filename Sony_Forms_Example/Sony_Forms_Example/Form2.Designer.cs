namespace Sony_Forms_Example
{
    partial class Devices
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
            this.components = new System.ComponentModel.Container();
            this.dataGridView1 = new System.Windows.Forms.DataGridView();
            this.OK_but = new System.Windows.Forms.Button();
            this.Cancel_but = new System.Windows.Forms.Button();
            this.Add_but = new System.Windows.Forms.Button();
            this.Delete_But = new System.Windows.Forms.Button();
            this.Save_but = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Load_but = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.BuildBut = new System.Windows.Forms.Button();
            this.DName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Generation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DocumentUrl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).BeginInit();
            this.SuspendLayout();
            // 
            // dataGridView1
            // 
            this.dataGridView1.AllowUserToAddRows = false;
            this.dataGridView1.AllowUserToDeleteRows = false;
            this.dataGridView1.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridView1.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.DName,
            this.DIP,
            this.Generation,
            this.DocumentUrl});
            this.dataGridView1.Location = new System.Drawing.Point(15, 12);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(649, 200);
            this.dataGridView1.TabIndex = 0;
            // 
            // OK_but
            // 
            this.OK_but.Enabled = false;
            this.OK_but.Location = new System.Drawing.Point(589, 285);
            this.OK_but.Name = "OK_but";
            this.OK_but.Size = new System.Drawing.Size(75, 23);
            this.OK_but.TabIndex = 1;
            this.OK_but.Text = "Close";
            this.toolTip1.SetToolTip(this.OK_but, "Save and Continue");
            this.OK_but.UseVisualStyleBackColor = true;
            this.OK_but.Click += new System.EventHandler(this.OK_but_Click);
            // 
            // Cancel_but
            // 
            this.Cancel_but.Location = new System.Drawing.Point(589, 314);
            this.Cancel_but.Name = "Cancel_but";
            this.Cancel_but.Size = new System.Drawing.Size(75, 23);
            this.Cancel_but.TabIndex = 2;
            this.Cancel_but.Text = "Cancel";
            this.toolTip1.SetToolTip(this.Cancel_but, "Cancel");
            this.Cancel_but.UseVisualStyleBackColor = true;
            this.Cancel_but.Click += new System.EventHandler(this.Cancel_but_Click);
            // 
            // Add_but
            // 
            this.Add_but.Location = new System.Drawing.Point(15, 227);
            this.Add_but.Name = "Add_but";
            this.Add_but.Size = new System.Drawing.Size(75, 23);
            this.Add_but.TabIndex = 3;
            this.Add_but.Text = "Add";
            this.toolTip1.SetToolTip(this.Add_but, "Add a new Device");
            this.Add_but.UseVisualStyleBackColor = true;
            this.Add_but.Click += new System.EventHandler(this.Add_but_Click);
            // 
            // Delete_But
            // 
            this.Delete_But.Enabled = false;
            this.Delete_But.Location = new System.Drawing.Point(15, 256);
            this.Delete_But.Name = "Delete_But";
            this.Delete_But.Size = new System.Drawing.Size(75, 23);
            this.Delete_But.TabIndex = 5;
            this.Delete_But.Text = "Delete";
            this.toolTip1.SetToolTip(this.Delete_But, "Delete the selected Device");
            this.Delete_But.UseVisualStyleBackColor = true;
            this.Delete_But.Click += new System.EventHandler(this.Delete_but_Click);
            // 
            // Save_but
            // 
            this.Save_but.Enabled = false;
            this.Save_but.Location = new System.Drawing.Point(15, 285);
            this.Save_but.Name = "Save_but";
            this.Save_but.Size = new System.Drawing.Size(75, 23);
            this.Save_but.TabIndex = 10;
            this.Save_but.Text = "Save";
            this.toolTip1.SetToolTip(this.Save_but, "Saves this configuration");
            this.Save_but.UseVisualStyleBackColor = true;
            this.Save_but.Click += new System.EventHandler(this.Save_but_Click);
            // 
            // Load_but
            // 
            this.Load_but.Enabled = false;
            this.Load_but.Location = new System.Drawing.Point(15, 314);
            this.Load_but.Name = "Load_but";
            this.Load_but.Size = new System.Drawing.Size(75, 23);
            this.Load_but.TabIndex = 11;
            this.Load_but.Text = "Load";
            this.toolTip1.SetToolTip(this.Load_but, "Loads a Saved Configuration");
            this.Load_but.UseVisualStyleBackColor = true;
            this.Load_but.Click += new System.EventHandler(this.Load_but_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(96, 232);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(0, 13);
            this.label5.TabIndex = 13;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(102, 232);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(135, 13);
            this.label6.TabIndex = 14;
            this.label6.Text = "Adds a new row to the Grid";
            // 
            // label7
            // 
            this.label7.AutoSize = true;
            this.label7.Location = new System.Drawing.Point(102, 261);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(187, 13);
            this.label7.TabIndex = 15;
            this.label7.Text = "Deletes the selected row from the Grid";
            // 
            // label8
            // 
            this.label8.AutoSize = true;
            this.label8.Location = new System.Drawing.Point(102, 290);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(177, 13);
            this.label8.TabIndex = 16;
            this.label8.Text = "Saves the Selected Device to a File";
            // 
            // label9
            // 
            this.label9.AutoSize = true;
            this.label9.Location = new System.Drawing.Point(102, 319);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 13);
            this.label9.TabIndex = 17;
            this.label9.Text = "Loads a Device file to the Grid";
            // 
            // BuildBut
            // 
            this.BuildBut.Location = new System.Drawing.Point(323, 227);
            this.BuildBut.Name = "BuildBut";
            this.BuildBut.Size = new System.Drawing.Size(75, 23);
            this.BuildBut.TabIndex = 18;
            this.BuildBut.Text = "Build";
            this.toolTip1.SetToolTip(this.BuildBut, "Add a new Device");
            this.BuildBut.UseVisualStyleBackColor = true;
            this.BuildBut.Click += new System.EventHandler(this.Buildbut_Click);
            // 
            // DName
            // 
            this.DName.HeaderText = "Device Name";
            this.DName.Name = "DName";
            this.DName.ReadOnly = true;
            this.DName.Width = 200;
            // 
            // DIP
            // 
            this.DIP.HeaderText = "Device IP";
            this.DIP.Name = "DIP";
            this.DIP.ReadOnly = true;
            // 
            // Generation
            // 
            this.Generation.HeaderText = "Generation";
            this.Generation.Name = "Generation";
            this.Generation.ReadOnly = true;
            // 
            // DocumentUrl
            // 
            this.DocumentUrl.HeaderText = "Document URL";
            this.DocumentUrl.Name = "DocumentUrl";
            this.DocumentUrl.Width = 200;
            // 
            // Devices
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 351);
            this.Controls.Add(this.BuildBut);
            this.Controls.Add(this.label9);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.Load_but);
            this.Controls.Add(this.Save_but);
            this.Controls.Add(this.Delete_But);
            this.Controls.Add(this.Add_but);
            this.Controls.Add(this.Cancel_but);
            this.Controls.Add(this.OK_but);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Devices";
            this.Text = "Form2";
            this.Load += new System.EventHandler(this.Form2_Load);
            ((System.ComponentModel.ISupportInitialize)(this.dataGridView1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView dataGridView1;
        private System.Windows.Forms.Button OK_but;
        private System.Windows.Forms.Button Cancel_but;
        private System.Windows.Forms.Button Add_but;
        private System.Windows.Forms.Button Delete_But;
        private System.Windows.Forms.Button Save_but;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button Load_but;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Button BuildBut;
        private System.Windows.Forms.DataGridViewTextBoxColumn DName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn Generation;
        private System.Windows.Forms.DataGridViewTextBoxColumn DocumentUrl;
    }
}