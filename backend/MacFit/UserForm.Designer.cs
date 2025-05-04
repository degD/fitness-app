namespace MacFit
{
    partial class UserForm
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
            this.panel1 = new System.Windows.Forms.Panel();
            this.guna2Button1 = new Guna.UI2.WinForms.Guna2Button();
            this.GOdemelerBtn = new Guna.UI2.WinForms.Guna2Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.KaloriBtn = new Guna.UI2.WinForms.Guna2Button();
            this.YagOraniBtn = new Guna.UI2.WinForms.Guna2Button();
            this.WorkoutBtn = new Guna.UI2.WinForms.Guna2Button();
            this.SeansBtn = new Guna.UI2.WinForms.Guna2Button();
            this.UyelikBtn = new Guna.UI2.WinForms.Guna2Button();
            this.appointmentsDataGridView = new System.Windows.Forms.DataGridView();
            this.lblHeight = new System.Windows.Forms.Label();
            this.lblWeight = new System.Windows.Forms.Label();
            this.lblBodyFat = new System.Windows.Forms.Label();
            this.lblBMI = new System.Windows.Forms.Label();
            this.lblIdealWeight = new System.Windows.Forms.Label();
            this.lblMetabolicAge = new System.Windows.Forms.Label();
            this.panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentsDataGridView)).BeginInit();
            this.SuspendLayout();
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.panel1.Controls.Add(this.guna2Button1);
            this.panel1.Controls.Add(this.GOdemelerBtn);
            this.panel1.Controls.Add(this.pictureBox1);
            this.panel1.Controls.Add(this.KaloriBtn);
            this.panel1.Controls.Add(this.YagOraniBtn);
            this.panel1.Controls.Add(this.WorkoutBtn);
            this.panel1.Controls.Add(this.SeansBtn);
            this.panel1.Controls.Add(this.UyelikBtn);
            this.panel1.Location = new System.Drawing.Point(0, 1);
            this.panel1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(271, 858);
            this.panel1.TabIndex = 0;
            // 
            // guna2Button1
            // 
            this.guna2Button1.BackColor = System.Drawing.Color.DimGray;
            this.guna2Button1.BorderRadius = 21;
            this.guna2Button1.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.guna2Button1.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.guna2Button1.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.guna2Button1.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            this.guna2Button1.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.guna2Button1.ForeColor = System.Drawing.Color.White;
            this.guna2Button1.Location = new System.Drawing.Point(16, 730);
            this.guna2Button1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.guna2Button1.Name = "guna2Button1";
            this.guna2Button1.Size = new System.Drawing.Size(240, 55);
            this.guna2Button1.TabIndex = 4;
            this.guna2Button1.Text = "Profil Bilgileri";
            this.guna2Button1.Click += new System.EventHandler(this.guna2Button1_Click);
            // 
            // GOdemelerBtn
            // 
            this.GOdemelerBtn.BackColor = System.Drawing.Color.DimGray;
            this.GOdemelerBtn.BorderRadius = 1000;
            this.GOdemelerBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.GOdemelerBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.GOdemelerBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.GOdemelerBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.GOdemelerBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            this.GOdemelerBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.GOdemelerBtn.ForeColor = System.Drawing.Color.White;
            this.GOdemelerBtn.Location = new System.Drawing.Point(16, 647);
            this.GOdemelerBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.GOdemelerBtn.Name = "GOdemelerBtn";
            this.GOdemelerBtn.Size = new System.Drawing.Size(240, 55);
            this.GOdemelerBtn.TabIndex = 3;
            this.GOdemelerBtn.Text = "Geçmiş Ödemeler";
            this.GOdemelerBtn.Click += new System.EventHandler(this.GOdemelerBtn_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackColor = System.Drawing.Color.White;
            this.pictureBox1.Image = global::MacFit.Properties.Resources.macfit_black;
            this.pictureBox1.Location = new System.Drawing.Point(16, 27);
            this.pictureBox1.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(227, 78);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 1;
            this.pictureBox1.TabStop = false;
            // 
            // KaloriBtn
            // 
            this.KaloriBtn.BackColor = System.Drawing.Color.DimGray;
            this.KaloriBtn.BorderRadius = 1000;
            this.KaloriBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.KaloriBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.KaloriBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.KaloriBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.KaloriBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            this.KaloriBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.KaloriBtn.ForeColor = System.Drawing.Color.White;
            this.KaloriBtn.Location = new System.Drawing.Point(16, 556);
            this.KaloriBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.KaloriBtn.Name = "KaloriBtn";
            this.KaloriBtn.Size = new System.Drawing.Size(240, 55);
            this.KaloriBtn.TabIndex = 2;
            this.KaloriBtn.Text = "Kalori İhtiyacı Hesapla";
            this.KaloriBtn.Click += new System.EventHandler(this.KaloriBtn_Click);
            // 
            // YagOraniBtn
            // 
            this.YagOraniBtn.BackColor = System.Drawing.Color.DimGray;
            this.YagOraniBtn.BorderRadius = 1000;
            this.YagOraniBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.YagOraniBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.YagOraniBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.YagOraniBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.YagOraniBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            this.YagOraniBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.YagOraniBtn.ForeColor = System.Drawing.Color.White;
            this.YagOraniBtn.Location = new System.Drawing.Point(16, 465);
            this.YagOraniBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.YagOraniBtn.Name = "YagOraniBtn";
            this.YagOraniBtn.Size = new System.Drawing.Size(240, 55);
            this.YagOraniBtn.TabIndex = 2;
            this.YagOraniBtn.Text = "Yağ Oranı Hesapla";
            this.YagOraniBtn.Click += new System.EventHandler(this.YagOraniBtn_Click);
            // 
            // WorkoutBtn
            // 
            this.WorkoutBtn.BackColor = System.Drawing.Color.DimGray;
            this.WorkoutBtn.BorderRadius = 1000;
            this.WorkoutBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.WorkoutBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.WorkoutBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.WorkoutBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.WorkoutBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            this.WorkoutBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.WorkoutBtn.ForeColor = System.Drawing.Color.White;
            this.WorkoutBtn.Location = new System.Drawing.Point(16, 374);
            this.WorkoutBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.WorkoutBtn.Name = "WorkoutBtn";
            this.WorkoutBtn.Size = new System.Drawing.Size(240, 55);
            this.WorkoutBtn.TabIndex = 2;
            this.WorkoutBtn.Text = "Workout Plan";
            this.WorkoutBtn.Click += new System.EventHandler(this.WorkoutBtn_Click);
            // 
            // SeansBtn
            // 
            this.SeansBtn.BackColor = System.Drawing.Color.DimGray;
            this.SeansBtn.BorderRadius = 1000;
            this.SeansBtn.DisabledState.BorderColor = System.Drawing.Color.DarkGray;
            this.SeansBtn.DisabledState.CustomBorderColor = System.Drawing.Color.DarkGray;
            this.SeansBtn.DisabledState.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(169)))), ((int)(((byte)(169)))), ((int)(((byte)(169)))));
            this.SeansBtn.DisabledState.ForeColor = System.Drawing.Color.FromArgb(((int)(((byte)(141)))), ((int)(((byte)(141)))), ((int)(((byte)(141)))));
            this.SeansBtn.FillColor = System.Drawing.Color.FromArgb(((int)(((byte)(0)))), ((int)(((byte)(0)))), ((int)(((byte)(4)))), ((int)(((byte)(68)))));
            this.SeansBtn.Font = new System.Drawing.Font("Segoe UI", 9F);
            this.SeansBtn.ForeColor = System.Drawing.Color.White;
            this.SeansBtn.Location = new System.Drawing.Point(16, 283);
            this.SeansBtn.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.SeansBtn.Name = "SeansOdemeBtn";
            this.SeansBtn.Size = new System.Drawing.Size(240, 55);
            this.SeansBtn.TabIndex = 2;
            this.SeansBtn.Text = "Seans ve Ödeme";
            this.SeansBtn.Click += new System.EventHandler(this.SeansOdemeBtn_Click);
            // 
            // appointmentsDataGridView
            // 
            this.appointmentsDataGridView.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.appointmentsDataGridView.Location = new System.Drawing.Point(200, 50);
            this.appointmentsDataGridView.Margin = new System.Windows.Forms.Padding(2);
            this.appointmentsDataGridView.Name = "appointmentsDataGridView";
            this.appointmentsDataGridView.Size = new System.Drawing.Size(300, 650);
            this.appointmentsDataGridView.TabIndex = 1;
            this.appointmentsDataGridView.Visible = false;
            this.appointmentsDataGridView.CellClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.AppointmentsDataGridView_CellClick);
            // 
            // lblHeight
            // 
            this.lblHeight.AutoSize = true;
            this.lblHeight.Location = new System.Drawing.Point(10, 10);
            this.lblHeight.Name = "lblHeight";
            this.lblHeight.Size = new System.Drawing.Size(46, 17);
            this.lblHeight.TabIndex = 0;
            this.lblHeight.Text = "Height: ";
            // 
            // lblWeight
            // 
            this.lblWeight.AutoSize = true;
            this.lblWeight.Location = new System.Drawing.Point(10, 40);
            this.lblWeight.Name = "lblWeight";
            this.lblWeight.Size = new System.Drawing.Size(51, 17);
            this.lblWeight.TabIndex = 0;
            this.lblWeight.Text = "Weight: ";
            // 
            // lblBodyFat
            // 
            this.lblBodyFat.AutoSize = true;
            this.lblBodyFat.Location = new System.Drawing.Point(10, 70);
            this.lblBodyFat.Name = "lblBodyFat";
            this.lblBodyFat.Size = new System.Drawing.Size(126, 17);
            this.lblBodyFat.TabIndex = 0;
            this.lblBodyFat.Text = "Body Fat Percentage: ";
            // 
            // lblBMI
            // 
            this.lblBMI.AutoSize = true;
            this.lblBMI.Location = new System.Drawing.Point(10, 100);
            this.lblBMI.Name = "lblBMI";
            this.lblBMI.Size = new System.Drawing.Size(35, 17);
            this.lblBMI.TabIndex = 0;
            this.lblBMI.Text = "BMI: ";
            // 
            // lblIdealWeight
            // 
            this.lblIdealWeight.AutoSize = true;
            this.lblIdealWeight.Location = new System.Drawing.Point(10, 130);
            this.lblIdealWeight.Name = "lblIdealWeight";
            this.lblIdealWeight.Size = new System.Drawing.Size(85, 17);
            this.lblIdealWeight.TabIndex = 0;
            this.lblIdealWeight.Text = "Ideal Weight: ";
            // 
            // lblMetabolicAge
            // 
            this.lblMetabolicAge.AutoSize = true;
            this.lblMetabolicAge.Location = new System.Drawing.Point(10, 160);
            this.lblMetabolicAge.Name = "lblMetabolicAge";
            this.lblMetabolicAge.Size = new System.Drawing.Size(102, 17);
            this.lblMetabolicAge.TabIndex = 0;
            this.lblMetabolicAge.Text = "Metabolic Age: ";
            // 
            // UserForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(224)))), ((int)(((byte)(224)))), ((int)(((byte)(224)))));
            this.ClientSize = new System.Drawing.Size(1753, 900);
            this.Controls.Add(this.panel1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.Fixed3D;
            this.Margin = new System.Windows.Forms.Padding(4, 4, 4, 4);
            this.Name = "UserForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "MacFit";
            this.panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.appointmentsDataGridView)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panel1;
        private Guna.UI2.WinForms.Guna2Button KaloriBtn;
        private Guna.UI2.WinForms.Guna2Button YagOraniBtn;
        private Guna.UI2.WinForms.Guna2Button WorkoutBtn;
        private Guna.UI2.WinForms.Guna2Button SeansBtn;
        private Guna.UI2.WinForms.Guna2Button UyelikBtn;
        private System.Windows.Forms.PictureBox pictureBox1;
        private Guna.UI2.WinForms.Guna2Button GOdemelerBtn;
        private System.Windows.Forms.DataGridView appointmentsDataGridView;
        private System.Windows.Forms.Label lblHeight;
        private System.Windows.Forms.Label lblWeight;
        private System.Windows.Forms.Label lblBodyFat;
        private System.Windows.Forms.Label lblBMI;
        private System.Windows.Forms.Label lblIdealWeight;
        private System.Windows.Forms.Label lblMetabolicAge;
        private Guna.UI2.WinForms.Guna2Button guna2Button1;
    }
}
