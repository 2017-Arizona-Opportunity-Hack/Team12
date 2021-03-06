﻿namespace MatthewsCrossingFoodBank
{
    partial class MainPage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainPage));
            this.btnProcess = new System.Windows.Forms.Button();
            this.txtFilenamePath = new System.Windows.Forms.TextBox();
            this.btnOpenFile = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.lblOpenFile = new System.Windows.Forms.Label();
            this.dataGridViewEntries = new System.Windows.Forms.DataGridView();
            this.btnDeleteEntry = new System.Windows.Forms.Button();
            this.colFirstName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colLastName = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colEmail = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDate = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonationType = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colDonationValue = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colAddress = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colApartment = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colCityTown = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colStateProvince = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colZipcode = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.colSalutation = new System.Windows.Forms.DataGridViewTextBoxColumn();
            this.progressBarEntries = new System.Windows.Forms.ProgressBar();
            this.lblLoadedInfo = new System.Windows.Forms.Label();
            this.btnDataAnalytics = new System.Windows.Forms.Button();
            this.txtKeywords = new System.Windows.Forms.TextBox();
            this.lblKeywords = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntries)).BeginInit();
            this.SuspendLayout();
            // 
            // btnProcess
            // 
            this.btnProcess.BackColor = System.Drawing.Color.Lime;
            this.btnProcess.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnProcess.Location = new System.Drawing.Point(573, 540);
            this.btnProcess.Name = "btnProcess";
            this.btnProcess.Size = new System.Drawing.Size(84, 23);
            this.btnProcess.TabIndex = 0;
            this.btnProcess.Text = "Process";
            this.btnProcess.UseVisualStyleBackColor = false;
            this.btnProcess.Click += new System.EventHandler(this.btnSendEmails_Click);
            // 
            // txtFilenamePath
            // 
            this.txtFilenamePath.BackColor = System.Drawing.SystemColors.Window;
            this.txtFilenamePath.Location = new System.Drawing.Point(80, 198);
            this.txtFilenamePath.Name = "txtFilenamePath";
            this.txtFilenamePath.ReadOnly = true;
            this.txtFilenamePath.Size = new System.Drawing.Size(257, 20);
            this.txtFilenamePath.TabIndex = 1;
            // 
            // btnOpenFile
            // 
            this.btnOpenFile.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnOpenFile.Location = new System.Drawing.Point(343, 196);
            this.btnOpenFile.Name = "btnOpenFile";
            this.btnOpenFile.Size = new System.Drawing.Size(75, 23);
            this.btnOpenFile.TabIndex = 2;
            this.btnOpenFile.Text = "Open File";
            this.btnOpenFile.UseVisualStyleBackColor = true;
            this.btnOpenFile.Click += new System.EventHandler(this.btnOpenFile_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Zoom;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(218, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(381, 128);
            this.pictureBox1.TabIndex = 4;
            this.pictureBox1.TabStop = false;
            // 
            // lblOpenFile
            // 
            this.lblOpenFile.AutoSize = true;
            this.lblOpenFile.Location = new System.Drawing.Point(80, 179);
            this.lblOpenFile.Name = "lblOpenFile";
            this.lblOpenFile.Size = new System.Drawing.Size(132, 13);
            this.lblOpenFile.TabIndex = 5;
            this.lblOpenFile.Text = "Chose a CSV file to import:";
            // 
            // dataGridViewEntries
            // 
            this.dataGridViewEntries.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.dataGridViewEntries.Columns.AddRange(new System.Windows.Forms.DataGridViewColumn[] {
            this.colFirstName,
            this.colLastName,
            this.colEmail,
            this.colDate,
            this.colDonationType,
            this.colDonationValue,
            this.colAddress,
            this.colApartment,
            this.colCityTown,
            this.colStateProvince,
            this.colZipcode,
            this.colSalutation});
            this.dataGridViewEntries.Location = new System.Drawing.Point(83, 257);
            this.dataGridViewEntries.Name = "dataGridViewEntries";
            this.dataGridViewEntries.Size = new System.Drawing.Size(644, 239);
            this.dataGridViewEntries.TabIndex = 6;
            this.dataGridViewEntries.CellContentClick += new System.Windows.Forms.DataGridViewCellEventHandler(this.dataGridViewEntries_CellContentClick);
            // 
            // btnDeleteEntry
            // 
            this.btnDeleteEntry.BackColor = System.Drawing.Color.Tomato;
            this.btnDeleteEntry.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDeleteEntry.Location = new System.Drawing.Point(357, 540);
            this.btnDeleteEntry.Name = "btnDeleteEntry";
            this.btnDeleteEntry.Size = new System.Drawing.Size(83, 23);
            this.btnDeleteEntry.TabIndex = 7;
            this.btnDeleteEntry.Text = "Delete Entry";
            this.btnDeleteEntry.UseVisualStyleBackColor = false;
            this.btnDeleteEntry.Click += new System.EventHandler(this.btnDeleteEntry_Click);
            // 
            // colFirstName
            // 
            this.colFirstName.HeaderText = "First name";
            this.colFirstName.Name = "colFirstName";
            // 
            // colLastName
            // 
            this.colLastName.HeaderText = "Last Name";
            this.colLastName.Name = "colLastName";
            // 
            // colEmail
            // 
            this.colEmail.HeaderText = "Email";
            this.colEmail.Name = "colEmail";
            // 
            // colDate
            // 
            this.colDate.HeaderText = "Date";
            this.colDate.Name = "colDate";
            // 
            // colDonationType
            // 
            this.colDonationType.HeaderText = "Donation Type";
            this.colDonationType.Name = "colDonationType";
            // 
            // colDonationValue
            // 
            this.colDonationValue.HeaderText = "Donation Value";
            this.colDonationValue.Name = "colDonationValue";
            // 
            // colAddress
            // 
            this.colAddress.HeaderText = "Address";
            this.colAddress.Name = "colAddress";
            // 
            // colApartment
            // 
            this.colApartment.HeaderText = "Apartment";
            this.colApartment.Name = "colApartment";
            // 
            // colCityTown
            // 
            this.colCityTown.HeaderText = "City/Town";
            this.colCityTown.Name = "colCityTown";
            // 
            // colStateProvince
            // 
            this.colStateProvince.HeaderText = "State/Province";
            this.colStateProvince.Name = "colStateProvince";
            // 
            // colZipcode
            // 
            this.colZipcode.HeaderText = "Zipcode";
            this.colZipcode.Name = "colZipcode";
            // 
            // colSalutation
            // 
            this.colSalutation.HeaderText = "Salutation";
            this.colSalutation.Name = "colSalutation";
            // 
            // progressBarEntries
            // 
            this.progressBarEntries.Location = new System.Drawing.Point(83, 502);
            this.progressBarEntries.Name = "progressBarEntries";
            this.progressBarEntries.Size = new System.Drawing.Size(644, 23);
            this.progressBarEntries.TabIndex = 8;
            // 
            // lblLoadedInfo
            // 
            this.lblLoadedInfo.AutoSize = true;
            this.lblLoadedInfo.Location = new System.Drawing.Point(252, 223);
            this.lblLoadedInfo.Name = "lblLoadedInfo";
            this.lblLoadedInfo.Size = new System.Drawing.Size(0, 13);
            this.lblLoadedInfo.TabIndex = 9;
            // 
            // btnDataAnalytics
            // 
            this.btnDataAnalytics.BackColor = System.Drawing.SystemColors.MenuHighlight;
            this.btnDataAnalytics.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnDataAnalytics.Location = new System.Drawing.Point(150, 539);
            this.btnDataAnalytics.Name = "btnDataAnalytics";
            this.btnDataAnalytics.Size = new System.Drawing.Size(90, 23);
            this.btnDataAnalytics.TabIndex = 10;
            this.btnDataAnalytics.Text = "Data Analytics";
            this.btnDataAnalytics.UseVisualStyleBackColor = false;
            this.btnDataAnalytics.Click += new System.EventHandler(this.btnDataAnalytics_Click);
            // 
            // txtKeywords
            // 
            this.txtKeywords.Location = new System.Drawing.Point(469, 198);
            this.txtKeywords.Name = "txtKeywords";
            this.txtKeywords.Size = new System.Drawing.Size(258, 20);
            this.txtKeywords.TabIndex = 11;
            this.txtKeywords.Text = "frys\'s,safeway,walmart,winco,bashas,albertsons,starbucks";
            // 
            // lblKeywords
            // 
            this.lblKeywords.AutoSize = true;
            this.lblKeywords.Location = new System.Drawing.Point(466, 179);
            this.lblKeywords.Name = "lblKeywords";
            this.lblKeywords.Size = new System.Drawing.Size(266, 13);
            this.lblKeywords.TabIndex = 12;
            this.lblKeywords.Text = "Enter business keywords to filter out comma separated:";
            // 
            // MainPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ControlLightLight;
            this.ClientSize = new System.Drawing.Size(767, 579);
            this.Controls.Add(this.lblKeywords);
            this.Controls.Add(this.txtKeywords);
            this.Controls.Add(this.btnDataAnalytics);
            this.Controls.Add(this.lblLoadedInfo);
            this.Controls.Add(this.progressBarEntries);
            this.Controls.Add(this.btnDeleteEntry);
            this.Controls.Add(this.dataGridViewEntries);
            this.Controls.Add(this.lblOpenFile);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnOpenFile);
            this.Controls.Add(this.txtFilenamePath);
            this.Controls.Add(this.btnProcess);
            this.Name = "MainPage";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.dataGridViewEntries)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnProcess;
        private System.Windows.Forms.TextBox txtFilenamePath;
        private System.Windows.Forms.Button btnOpenFile;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Label lblOpenFile;
        private System.Windows.Forms.DataGridView dataGridViewEntries;
        private System.Windows.Forms.Button btnDeleteEntry;
        private System.Windows.Forms.DataGridViewTextBoxColumn colFirstName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colLastName;
        private System.Windows.Forms.DataGridViewTextBoxColumn colEmail;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDate;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonationType;
        private System.Windows.Forms.DataGridViewTextBoxColumn colDonationValue;
        private System.Windows.Forms.DataGridViewTextBoxColumn colAddress;
        private System.Windows.Forms.DataGridViewTextBoxColumn colApartment;
        private System.Windows.Forms.DataGridViewTextBoxColumn colCityTown;
        private System.Windows.Forms.DataGridViewTextBoxColumn colStateProvince;
        private System.Windows.Forms.DataGridViewTextBoxColumn colZipcode;
        private System.Windows.Forms.DataGridViewTextBoxColumn colSalutation;
        private System.Windows.Forms.ProgressBar progressBarEntries;
        private System.Windows.Forms.Label lblLoadedInfo;
        private System.Windows.Forms.Button btnDataAnalytics;
        private System.Windows.Forms.TextBox txtKeywords;
        private System.Windows.Forms.Label lblKeywords;
    }
}