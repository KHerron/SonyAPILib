namespace Sony_Forms_Example
{
    partial class Form2
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
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.Save_but = new System.Windows.Forms.Button();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.Load_but = new System.Windows.Forms.Button();
            this.DName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DIP = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DActionList = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.DControl = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.Generation = new System.Windows.Forms.DataGridViewTextBoxColumn();
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
            this.DActionList,
            this.DControl,
            this.Generation});
            this.dataGridView1.Location = new System.Drawing.Point(15, 62);
            this.dataGridView1.Name = "dataGridView1";
            this.dataGridView1.Size = new System.Drawing.Size(649, 150);
            this.dataGridView1.TabIndex = 0;
            // 
            // OK_but
            // 
            this.OK_but.Location = new System.Drawing.Point(488, 227);
            this.OK_but.Name = "OK_but";
            this.OK_but.Size = new System.Drawing.Size(75, 23);
            this.OK_but.TabIndex = 1;
            this.OK_but.Text = "OK";
            this.toolTip1.SetToolTip(this.OK_but, "Save and Continue");
            this.OK_but.UseVisualStyleBackColor = true;
            this.OK_but.Click += new System.EventHandler(this.OK_but_Click);
            // 
            // Cancel_but
            // 
            this.Cancel_but.Location = new System.Drawing.Point(407, 227);
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
            this.Delete_But.Location = new System.Drawing.Point(96, 227);
            this.Delete_But.Name = "Delete_But";
            this.Delete_But.Size = new System.Drawing.Size(75, 23);
            this.Delete_But.TabIndex = 5;
            this.Delete_But.Text = "Delete";
            this.toolTip1.SetToolTip(this.Delete_But, "Delete the selected Device");
            this.Delete_But.UseVisualStyleBackColor = true;
            this.Delete_But.Click += new System.EventHandler(this.Delete_but_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 28);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(220, 13);
            this.label1.TabIndex = 6;
            this.label1.Text = "Enter the Information Below for Each Device.";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.ForeColor = System.Drawing.Color.Red;
            this.label2.Location = new System.Drawing.Point(23, 46);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(228, 13);
            this.label2.TabIndex = 7;
            this.label2.Text = "Device Name and IP Address are REQUIRED!";
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(295, 1);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(268, 58);
            this.label3.TabIndex = 8;
            this.label3.Text = "To increase Performance, include the Action List and Control URL\'s. If Gen3, ente" +
    "r a Single space for the  Action List or Include full URL: ie http://192.168.1.1" +
    "00:8080/sony/IRCC";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(12, 9);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(155, 13);
            this.label4.TabIndex = 9;
            this.label4.Text = "Click Add to add a new device.";
            // 
            // Save_but
            // 
            this.Save_but.Location = new System.Drawing.Point(177, 227);
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
            this.Load_but.Location = new System.Drawing.Point(258, 227);
            this.Load_but.Name = "Load_but";
            this.Load_but.Size = new System.Drawing.Size(75, 23);
            this.Load_but.TabIndex = 11;
            this.Load_but.Text = "Load";
            this.toolTip1.SetToolTip(this.Load_but, "Loads a Saved Configuration");
            this.Load_but.UseVisualStyleBackColor = true;
            this.Load_but.Click += new System.EventHandler(this.Load_but_Click);
            // 
            // DName
            // 
            this.DName.HeaderText = "Device Name";
            this.DName.Name = "DName";
            // 
            // DIP
            // 
            this.DIP.HeaderText = "Device IP";
            this.DIP.Name = "DIP";
            // 
            // DActionList
            // 
            this.DActionList.HeaderText = "Action List URL";
            this.DActionList.Name = "DActionList";
            this.DActionList.Width = 150;
            // 
            // DControl
            // 
            this.DControl.HeaderText = "Control URL";
            this.DControl.Name = "DControl";
            this.DControl.Width = 150;
            // 
            // Generation
            // 
            this.Generation.HeaderText = "Generation";
            this.Generation.Name = "Generation";
            // 
            // Form2
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(676, 262);
            this.Controls.Add(this.Load_but);
            this.Controls.Add(this.Save_but);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.Delete_But);
            this.Controls.Add(this.Add_but);
            this.Controls.Add(this.Cancel_but);
            this.Controls.Add(this.OK_but);
            this.Controls.Add(this.dataGridView1);
            this.Name = "Form2";
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
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Button Save_but;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.Button Load_but;
        private System.Windows.Forms.DataGridViewTextBoxColumn DName;
        private System.Windows.Forms.DataGridViewTextBoxColumn DIP;
        private System.Windows.Forms.DataGridViewTextBoxColumn DActionList;
        private System.Windows.Forms.DataGridViewTextBoxColumn DControl;
        private System.Windows.Forms.DataGridViewTextBoxColumn Generation;
    }
}