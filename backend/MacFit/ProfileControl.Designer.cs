namespace MacFit
{
    partial class ProfileControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.EmailBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.PhoneBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.WBox = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.HBox = new Guna.UI2.WinForms.Guna2NumericUpDown();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.NewPassBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.OldPassBox = new Guna.UI2.WinForms.Guna2TextBox();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.guna2Button2 = new Guna.UI2.WinForms.Guna2Button();
            this.IdLbl = new System.Windows.Forms.Label();
            this.BirthLbl = new System.Windows.Forms.Label();
            this.PointsLbl = new System.Windows.Forms.Label();
            this.NameLbl = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.WBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBox)).BeginInit();
            this.SuspendLayout();
            // 
            // EmailBox
            // 
            this.EmailBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.EmailBox.DefaultText = "";
            this.EmailBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.EmailBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.EmailBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.EmailBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.EmailBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.EmailBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.EmailBox.ForeColor = System.Drawing.Color.Blue;
            this.EmailBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.EmailBox.Location = new System.Drawing.Point(74, 12);
            this.EmailBox.Name = "EmailBox";
            this.EmailBox.PlaceholderText = "enter new email...";
            this.EmailBox.SelectedText = "";
            this.EmailBox.Size = new System.Drawing.Size(200, 36);
            this.EmailBox.TabIndex = 1;
            this.EmailBox.TextChanged += new System.EventHandler(this.EmailBox_TextChanged);
            // 
            // PhoneBox
            // 
            this.PhoneBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.PhoneBox.DefaultText = "";
            this.PhoneBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.PhoneBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.PhoneBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.PhoneBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.PhoneBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.PhoneBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.PhoneBox.ForeColor = System.Drawing.Color.Blue;
            this.PhoneBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.PhoneBox.Location = new System.Drawing.Point(74, 54);
            this.PhoneBox.Name = "PhoneBox";
            this.PhoneBox.PlaceholderText = "enter new phone...";
            this.PhoneBox.SelectedText = "";
            this.PhoneBox.Size = new System.Drawing.Size(200, 36);
            this.PhoneBox.TabIndex = 3;
            // 
            // WBox
            // 
            this.WBox.BackColor = System.Drawing.Color.Transparent;
            this.WBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.WBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.WBox.Location = new System.Drawing.Point(78, 96);
            this.WBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.WBox.Name = "WBox";
            this.WBox.Size = new System.Drawing.Size(100, 36);
            this.WBox.TabIndex = 4;
            // 
            // HBox
            // 
            this.HBox.BackColor = System.Drawing.Color.Transparent;
            this.HBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.HBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.HBox.Location = new System.Drawing.Point(78, 138);
            this.HBox.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.HBox.Name = "HBox";
            this.HBox.Size = new System.Drawing.Size(100, 36);
            this.HBox.TabIndex = 6;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(13, 24);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(31, 13);
            this.label1.TabIndex = 7;
            this.label1.Text = "email";
            this.label1.Click += new System.EventHandler(this.label1_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(13, 65);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(37, 13);
            this.label2.TabIndex = 8;
            this.label2.Text = "phone";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(13, 107);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 9;
            this.label3.Text = "weight (kg)";
            this.label3.Click += new System.EventHandler(this.label3_Click);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(13, 150);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(59, 13);
            this.label4.TabIndex = 10;
            this.label4.Text = "height (cm)";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 298);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 13);
            this.label5.TabIndex = 14;
            this.label5.Text = "new pass";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(16, 255);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(46, 13);
            this.label6.TabIndex = 13;
            this.label6.Text = "old pass";
            // 
            // NewPassBox
            // 
            this.NewPassBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.NewPassBox.DefaultText = "";
            this.NewPassBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.NewPassBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.NewPassBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.NewPassBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.NewPassBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.NewPassBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.NewPassBox.ForeColor = System.Drawing.Color.Blue;
            this.NewPassBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.NewPassBox.Location = new System.Drawing.Point(74, 286);
            this.NewPassBox.Name = "NewPassBox";
            this.NewPassBox.PasswordChar = '*';
            this.NewPassBox.PlaceholderText = "enter new password...";
            this.NewPassBox.SelectedText = "";
            this.NewPassBox.Size = new System.Drawing.Size(200, 36);
            this.NewPassBox.TabIndex = 12;
            // 
            // OldPassBox
            // 
            this.OldPassBox.Cursor = System.Windows.Forms.Cursors.IBeam;
            this.OldPassBox.DefaultText = "";
            this.OldPassBox.DisabledState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(208)))), ((int)(((byte)(208)))), ((int)(((byte)(208)))));
            this.OldPassBox.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(226)))), ((int)(((byte)(226)))), ((int)(((byte)(226)))));
            this.OldPassBox.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.OldPassBox.DisabledState.PlaceholderForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(138)))), ((int)(((byte)(138)))), ((int)(((byte)(138)))));
            this.OldPassBox.FocusedState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.OldPassBox.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.OldPassBox.ForeColor = System.Drawing.Color.Blue;
            this.OldPassBox.HoverState.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(94)))), ((int)(((byte)(148)))), ((int)(((byte)(255)))));
            this.OldPassBox.Location = new System.Drawing.Point(74, 244);
            this.OldPassBox.Name = "OldPassBox";
            this.OldPassBox.PasswordChar = '*';
            this.OldPassBox.PlaceholderText = "enter old password...";
            this.OldPassBox.SelectedText = "";
            this.OldPassBox.Size = new System.Drawing.Size(200, 36);
            this.OldPassBox.TabIndex = 11;
            // 
            // guna2Button1
            // 
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(74, 180);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(180, 45);
            this.guna2Button1.TabIndex = 15;
            this.guna2Button1.Text = "update profile";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // guna2Button2
            // 
            this.guna2Button2.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button2.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button2.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button2.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button2.ForeColor = System.Drawing.Color.White;
            this.guna2Button2.Location = new System.Drawing.Point(74, 328);
            this.guna2Button2.Name = "guna2Button2";
            this.guna2Button2.Size = new System.Drawing.Size(180, 45);
            this.guna2Button2.TabIndex = 16;
            this.guna2Button2.Text = "update password";
            this.guna2Button2.Click += new System.EventHandler(this.guna2Button2_Click);
            // 
            // IdLbl
            // 
            this.IdLbl.AutoSize = true;
            this.IdLbl.Location = new System.Drawing.Point(13, 392);
            this.IdLbl.Name = "IdLbl";
            this.IdLbl.Size = new System.Drawing.Size(35, 13);
            this.IdLbl.TabIndex = 17;
            this.IdLbl.Text = "label7";
            // 
            // BirthLbl
            // 
            this.BirthLbl.AutoSize = true;
            this.BirthLbl.Location = new System.Drawing.Point(13, 459);
            this.BirthLbl.Name = "BirthLbl";
            this.BirthLbl.Size = new System.Drawing.Size(35, 13);
            this.BirthLbl.TabIndex = 18;
            this.BirthLbl.Text = "label8";
            this.BirthLbl.Click += new System.EventHandler(this.label8_Click);
            // 
            // PointsLbl
            // 
            this.PointsLbl.AutoSize = true;
            this.PointsLbl.Location = new System.Drawing.Point(13, 489);
            this.PointsLbl.Name = "PointsLbl";
            this.PointsLbl.Size = new System.Drawing.Size(35, 13);
            this.PointsLbl.TabIndex = 19;
            this.PointsLbl.Text = "label9";
            this.PointsLbl.Click += new System.EventHandler(this.label9_Click);
            // 
            // NameLbl
            // 
            this.NameLbl.AutoSize = true;
            this.NameLbl.Location = new System.Drawing.Point(13, 426);
            this.NameLbl.Name = "NameLbl";
            this.NameLbl.Size = new System.Drawing.Size(41, 13);
            this.NameLbl.TabIndex = 20;
            this.NameLbl.Text = "label10";
            // 
            // ProfileControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.Controls.Add(this.NameLbl);
            this.Controls.Add(this.PointsLbl);
            this.Controls.Add(this.BirthLbl);
            this.Controls.Add(this.IdLbl);
            this.Controls.Add(this.guna2Button2);
            this.Controls.Add(this.guna2Button1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.NewPassBox);
            this.Controls.Add(this.OldPassBox);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.HBox);
            this.Controls.Add(this.WBox);
            this.Controls.Add(this.PhoneBox);
            this.Controls.Add(this.EmailBox);
            this.Name = "ProfileControl";
            this.Size = new System.Drawing.Size(739, 545);
            ((System.ComponentModel.ISupportInitialize)(this.WBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.HBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private Guna.UI2.WinForms.Guna2TextBox EmailBox;
        private Guna.UI2.WinForms.Guna2TextBox PhoneBox;
        private Guna.UI2.WinForms.Guna2NumericUpDown WBox;
        private Guna.UI2.WinForms.Guna2NumericUpDown HBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label6;
        private Guna.UI2.WinForms.Guna2TextBox NewPassBox;
        private Guna.UI2.WinForms.Guna2TextBox OldPassBox;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
        private Guna.UI2.WinForms.Guna2Button guna2Button2;
        private System.Windows.Forms.Label IdLbl;
        private System.Windows.Forms.Label BirthLbl;
        private System.Windows.Forms.Label PointsLbl;
        private System.Windows.Forms.Label NameLbl;
    }
}
