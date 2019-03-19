namespace EasyLab.Tools.Db.SqlCe
{
    partial class Installation
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
            this.gbDatabase = new System.Windows.Forms.GroupBox();
            this.btnShowPassword = new System.Windows.Forms.Button();
            this.btnSelectFile = new System.Windows.Forms.Button();
            this.txtPassword = new System.Windows.Forms.TextBox();
            this.txtFilePath = new System.Windows.Forms.TextBox();
            this.lblPassword = new System.Windows.Forms.Label();
            this.lblFileName = new System.Windows.Forms.Label();
            this.gbFiles = new System.Windows.Forms.GroupBox();
            this.btnUnSelectAll = new System.Windows.Forms.Button();
            this.btnSelectAll = new System.Windows.Forms.Button();
            this.listViewFilePath = new System.Windows.Forms.ListView();
            this.colName = ((System.Windows.Forms.ColumnHeader)(new System.Windows.Forms.ColumnHeader()));
            this.btnChangeFolder = new System.Windows.Forms.Button();
            this.txtChangeFolder = new System.Windows.Forms.TextBox();
            this.btnOK = new System.Windows.Forms.Button();
            this.dlgSaveFile = new System.Windows.Forms.SaveFileDialog();
            this.dlgBrowseFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.gbDatabase.SuspendLayout();
            this.gbFiles.SuspendLayout();
            this.SuspendLayout();
            // 
            // gbDatabase
            // 
            this.gbDatabase.Controls.Add(this.btnShowPassword);
            this.gbDatabase.Controls.Add(this.btnSelectFile);
            this.gbDatabase.Controls.Add(this.txtPassword);
            this.gbDatabase.Controls.Add(this.txtFilePath);
            this.gbDatabase.Controls.Add(this.lblPassword);
            this.gbDatabase.Controls.Add(this.lblFileName);
            this.gbDatabase.Location = new System.Drawing.Point(27, 13);
            this.gbDatabase.Name = "gbDatabase";
            this.gbDatabase.Size = new System.Drawing.Size(506, 171);
            this.gbDatabase.TabIndex = 0;
            this.gbDatabase.TabStop = false;
            this.gbDatabase.Text = "Database";
            this.gbDatabase.Visible = false;
            // 
            // btnShowPassword
            // 
            this.btnShowPassword.Location = new System.Drawing.Point(377, 93);
            this.btnShowPassword.Name = "btnShowPassword";
            this.btnShowPassword.Size = new System.Drawing.Size(119, 23);
            this.btnShowPassword.TabIndex = 5;
            this.btnShowPassword.Text = "Show Password";
            this.btnShowPassword.UseVisualStyleBackColor = true;
            this.btnShowPassword.MouseDown += new System.Windows.Forms.MouseEventHandler(this.btnShowPassword_MouseDown);
            this.btnShowPassword.MouseUp += new System.Windows.Forms.MouseEventHandler(this.btnShowPassword_MouseUp);
            // 
            // btnSelectFile
            // 
            this.btnSelectFile.Location = new System.Drawing.Point(377, 32);
            this.btnSelectFile.Name = "btnSelectFile";
            this.btnSelectFile.Size = new System.Drawing.Size(119, 23);
            this.btnSelectFile.TabIndex = 4;
            this.btnSelectFile.Text = "Select File";
            this.btnSelectFile.UseVisualStyleBackColor = true;
            this.btnSelectFile.Click += new System.EventHandler(this.btnSelectFile_Click);
            // 
            // txtPassword
            // 
            this.txtPassword.Location = new System.Drawing.Point(102, 93);
            this.txtPassword.Name = "txtPassword";
            this.txtPassword.PasswordChar = '*';
            this.txtPassword.Size = new System.Drawing.Size(269, 22);
            this.txtPassword.TabIndex = 3;
            // 
            // txtFilePath
            // 
            this.txtFilePath.Location = new System.Drawing.Point(103, 32);
            this.txtFilePath.Name = "txtFilePath";
            this.txtFilePath.ReadOnly = true;
            this.txtFilePath.Size = new System.Drawing.Size(268, 22);
            this.txtFilePath.TabIndex = 2;
            // 
            // lblPassword
            // 
            this.lblPassword.AutoSize = true;
            this.lblPassword.Location = new System.Drawing.Point(28, 96);
            this.lblPassword.Name = "lblPassword";
            this.lblPassword.Size = new System.Drawing.Size(68, 14);
            this.lblPassword.TabIndex = 1;
            this.lblPassword.Text = "Password: ";
            // 
            // lblFileName
            // 
            this.lblFileName.AutoSize = true;
            this.lblFileName.Location = new System.Drawing.Point(28, 32);
            this.lblFileName.Name = "lblFileName";
            this.lblFileName.Size = new System.Drawing.Size(68, 14);
            this.lblFileName.TabIndex = 0;
            this.lblFileName.Text = "File Name: ";
            // 
            // gbFiles
            // 
            this.gbFiles.Controls.Add(this.btnUnSelectAll);
            this.gbFiles.Controls.Add(this.btnSelectAll);
            this.gbFiles.Controls.Add(this.listViewFilePath);
            this.gbFiles.Controls.Add(this.btnChangeFolder);
            this.gbFiles.Controls.Add(this.txtChangeFolder);
            this.gbFiles.Location = new System.Drawing.Point(27, 191);
            this.gbFiles.Name = "gbFiles";
            this.gbFiles.Size = new System.Drawing.Size(506, 182);
            this.gbFiles.TabIndex = 0;
            this.gbFiles.TabStop = false;
            this.gbFiles.Text = "Files";
            this.gbFiles.Visible = false;
            // 
            // btnUnSelectAll
            // 
            this.btnUnSelectAll.Location = new System.Drawing.Point(381, 130);
            this.btnUnSelectAll.Name = "btnUnSelectAll";
            this.btnUnSelectAll.Size = new System.Drawing.Size(115, 23);
            this.btnUnSelectAll.TabIndex = 4;
            this.btnUnSelectAll.Text = "UnSelect All";
            this.btnUnSelectAll.UseVisualStyleBackColor = true;
            this.btnUnSelectAll.Click += new System.EventHandler(this.btnUnSelectAll_Click);
            // 
            // btnSelectAll
            // 
            this.btnSelectAll.Location = new System.Drawing.Point(381, 80);
            this.btnSelectAll.Name = "btnSelectAll";
            this.btnSelectAll.Size = new System.Drawing.Size(115, 23);
            this.btnSelectAll.TabIndex = 3;
            this.btnSelectAll.Text = "Select All";
            this.btnSelectAll.UseVisualStyleBackColor = true;
            this.btnSelectAll.Click += new System.EventHandler(this.btnSelectAll_Click);
            // 
            // listViewFilePath
            // 
            this.listViewFilePath.CheckBoxes = true;
            this.listViewFilePath.Columns.AddRange(new System.Windows.Forms.ColumnHeader[] {
            this.colName});
            this.listViewFilePath.GridLines = true;
            this.listViewFilePath.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.listViewFilePath.Location = new System.Drawing.Point(31, 71);
            this.listViewFilePath.Name = "listViewFilePath";
            this.listViewFilePath.Size = new System.Drawing.Size(340, 97);
            this.listViewFilePath.TabIndex = 2;
            this.listViewFilePath.UseCompatibleStateImageBehavior = false;
            this.listViewFilePath.View = System.Windows.Forms.View.Details;
            this.listViewFilePath.ItemChecked += new System.Windows.Forms.ItemCheckedEventHandler(this.listViewFilePath_ItemChecked);
            // 
            // colName
            // 
            this.colName.Width = 300;
            // 
            // btnChangeFolder
            // 
            this.btnChangeFolder.Location = new System.Drawing.Point(381, 32);
            this.btnChangeFolder.Name = "btnChangeFolder";
            this.btnChangeFolder.Size = new System.Drawing.Size(115, 23);
            this.btnChangeFolder.TabIndex = 1;
            this.btnChangeFolder.Text = "Change Folder";
            this.btnChangeFolder.UseVisualStyleBackColor = true;
            this.btnChangeFolder.Click += new System.EventHandler(this.btnChangeFolder_Click);
            // 
            // txtChangeFolder
            // 
            this.txtChangeFolder.Location = new System.Drawing.Point(31, 33);
            this.txtChangeFolder.Name = "txtChangeFolder";
            this.txtChangeFolder.ReadOnly = true;
            this.txtChangeFolder.Size = new System.Drawing.Size(340, 22);
            this.txtChangeFolder.TabIndex = 0;
            this.txtChangeFolder.TextChanged += new System.EventHandler(this.txtChangeFolder_TextChanged);
            // 
            // btnOK
            // 
            this.btnOK.Location = new System.Drawing.Point(408, 390);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(125, 29);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // Installation
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 14F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(557, 429);
            this.Controls.Add(this.btnOK);
            this.Controls.Add(this.gbFiles);
            this.Controls.Add(this.gbDatabase);
            this.Font = new System.Drawing.Font("Cambria", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "Installation";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Database Installation";
            this.Load += new System.EventHandler(this.Installation_Load);
            this.gbDatabase.ResumeLayout(false);
            this.gbDatabase.PerformLayout();
            this.gbFiles.ResumeLayout(false);
            this.gbFiles.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox gbDatabase;
        private System.Windows.Forms.GroupBox gbFiles;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Label lblFileName;
        private System.Windows.Forms.Label lblPassword;
        private System.Windows.Forms.TextBox txtFilePath;
        private System.Windows.Forms.TextBox txtPassword;
        private System.Windows.Forms.Button btnSelectFile;
        private System.Windows.Forms.Button btnShowPassword;
        private System.Windows.Forms.Button btnChangeFolder;
        private System.Windows.Forms.TextBox txtChangeFolder;
        private System.Windows.Forms.ListView listViewFilePath;
        private System.Windows.Forms.Button btnUnSelectAll;
        private System.Windows.Forms.Button btnSelectAll;
        private System.Windows.Forms.SaveFileDialog dlgSaveFile;
        private System.Windows.Forms.FolderBrowserDialog dlgBrowseFolder;
        private System.Windows.Forms.ColumnHeader colName;
    }
}

