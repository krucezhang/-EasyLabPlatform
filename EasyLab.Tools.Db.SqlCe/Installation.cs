using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlServerCe;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Windows.Forms;

namespace EasyLab.Tools.Db.SqlCe
{
    public partial class Installation : Form
    {
        public Installation()
        {
            InitializeComponent();
        }

        #region Fields

        static Regex SqlCeCommandRegex = new Regex(@"\s*(?<Action>(\s*\w+)+)\s+\[(?<TableName>\w+)\]?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        static Regex AlterContraintOrColumnRegex = new Regex(@"\s*ALTER\s+TABLE\s+\[(?<TableName>\w+)\]\s+(?<Action>\w+(\s+\w+)?)\s+\[(?<ContraintOrColumnName>\w+)\]?", RegexOptions.IgnoreCase | RegexOptions.Compiled);
        // CANNOT PARSE ALL INSERT INTO STATEMENTS
        static Regex InsertIntoRegex = new Regex(@"\s*INSERT\s+INTO\s+\[(?<TableName>\w+)\]\s+\((?<Columns>[\w|\[|\]|,]+)\)\s+VALUES\s+\((?<Values>[\w|\[|\]|,|\']+)\)", RegexOptions.IgnoreCase | RegexOptions.Compiled);

        #endregion

        #region Properties

        internal CommandInfo Command { get; private set; }

        private string ExtFilter
        {
            get
            {
                if (Command.Action == CommandActions.Config)
                {
                    return "*.config";
                }
                else
                {
                    return "*.sqlce";
                }
            }
        }

        #endregion

        #region Form Application Event Function

        private void txtChangeFolder_TextChanged(object sender, EventArgs e)
        {
            LoadFiles(txtChangeFolder.Text);
        }

        private void btnOK_Click(object sender, EventArgs e)
        {
            CollectData();
            try
            {
                ValidateInput();
                Save();
                ShowSuccessMessage();
                this.Close();
            }
            catch (DbToolException ex)
            {
                MessageBox.Show(ex.Message, this.Text, MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.GetInnerExceptionMessage(), this.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void btnChangeFolder_Click(object sender, EventArgs e)
        {
            //Open the folder browser dialog.
            var result = dlgBrowseFolder.ShowDialog();
            //change the log path to the selected folder.
            if (result == DialogResult.OK)
            {
                txtChangeFolder.Text = dlgBrowseFolder.SelectedPath;
            }
        }

        private void btnSelectAll_Click(object sender, EventArgs e)
        {
            UpdateFilesCheck(true);
        }

        private void btnUnSelectAll_Click(object sender, EventArgs e)
        {
            UpdateFilesCheck(false);
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (dlgSaveFile.ShowDialog() == DialogResult.OK)
            {
                txtFilePath.Text = dlgSaveFile.FileName;
            }
        }

        private void btnShowPassword_MouseDown(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = Constants.EmptyPasswordChar;
        }

        private void btnShowPassword_MouseUp(object sender, MouseEventArgs e)
        {
            txtPassword.PasswordChar = Constants.PasswordChar;
        }

        private void listViewFilePath_ItemChecked(object sender, ItemCheckedEventArgs e)
        {
            UpdateButtonsUI();
        }

        private void Installation_Load(object sender, EventArgs e)
        {
            Init();

            btnOK_Click(sender, e);
        }

        #endregion
        
        #region Method

        public void Exec(CommandInfo command)
        {
            if (command == null)
            {
                throw new ArgumentNullException("command");
            }
            Command = command;
            if (Command.Silent)
            {
                ExecSilentCommand();
            }
            else
            {
                ExecNormalCommand();
            }
        }

        private void ExecSilentCommand()
        {
            Save();
            ShowSuccessMessage();
        }

        private void Save()
        {
            switch (Command.Action)
            {
                case CommandActions.Install:
                    CreateDatabase();
                    RunDatabaseScripts();
                    break;
                case CommandActions.Uninstall:
                    RunDatabaseScripts();
                    break;
                case CommandActions.Config:
                    UpdateConnectionStrings();
                    break;
                default:
                    break;
            }
        }

        private void ExecNormalCommand()
        {
            ShowDialog();
        }

        private void RunDatabaseScripts()
        {
            //TestConnection(Command.DbUser.ConnectionString);
            ExecuteScripts(Command.DbUser.ConnectionString);
        }

        private void TestConnection(string connectionString)
        {
            using (var connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                connection.Close();
            }
        }

        private void ExecuteScripts(string connectionString)
        {
            if (Command.Files.Count == 0)
            {
                return;
            }
            var files = Command.Files.OrderBy(o => o.ToLower()).ToFullPath(Command.Path);

            string objectName, action, contraintOrColumnName;

            using (var connection = new SqlCeConnection(connectionString))
            {
                connection.Open();
                var cmd = new SqlCeCommand();
                cmd.Connection = connection;

                foreach (var file in files)
                {
                    string sql = String.Empty;

                    using (var reader = new StreamReader(file))
                    {
                        sql = reader.ReadToEnd();
                    }
                    var statements = sql.Split(new string[] { Constants.StatementSeparator }, StringSplitOptions.RemoveEmptyEntries);

                    foreach (var statement in statements)
                    {
                        string trimedStatement = statement.Trim();
                        ExtractCmd(trimedStatement, out objectName, out action);
                        switch (action)
                        {
                            case "CREATE TABLE":
                                //Skip it if already created special table
                                if (connection.TableExists(objectName))
                                {
                                    continue;
                                }
                                break;
                            case "ALTER TABLE":
                                if (ExtractContraintOrColumn(sql, out objectName, out action, out contraintOrColumnName))
                                {
                                    switch (action)
                                    {
                                        case "ADD CONSTRAINT":
                                            if (connection.IndexExists(contraintOrColumnName))
                                            {
                                                continue;
                                            }
                                            break;
                                        case "ADD": //Add column
                                            if (connection.ColumnExists(objectName, contraintOrColumnName))
                                            {
                                                continue;
                                            }
                                            break;
                                        case "ALTER COLUMN":
                                            if (!connection.CanAlterColumn(objectName, contraintOrColumnName))
                                            {
                                                continue;
                                            }
                                            break;
                                        default:
                                            break;
                                    }
                                }
                                break;
                            case "CREATE UNIQUE INDEX":
                                //Skip if index already created.
                                if (connection.IndexExists(objectName))
                                {
                                    continue;
                                }
                                break;
                            default:
                                break;
                        }
                        try
                        {
                            ExecuteScript(cmd, trimedStatement);
                        }
                        catch (SqlCeException ex)
                        {
                            if (ex.NativeError != 25016)
                            {
                                throw;
                            }
                        }
                    }
                }
            }
        }

        private void ExecuteScript(SqlCeCommand cmd, string statement)
        {
            if (!String.IsNullOrWhiteSpace(statement))
            {
                cmd.CommandText = statement;
                cmd.ExecuteNonQuery();
            }
        }

        private bool ExtractContraintOrColumn(string sql, out string objectName, out string action, out string contraintOrColumnName)
        {
            objectName = action = contraintOrColumnName = string.Empty;

            if (!String.IsNullOrWhiteSpace(sql))
            {
                var matchCmd = AlterContraintOrColumnRegex.Match(sql);
                if (matchCmd.Success)
                {
                    objectName = matchCmd.Groups["TableName"].Value.Trim();
                    action = matchCmd.Groups["Action"].Value.Trim();
                    contraintOrColumnName = matchCmd.Groups["ContraintOrColumnName"].Value.Trim();
                    return true;
                }
            }

            return false;
        }

        private void CreateDatabase()
        {
            if (!File.Exists(Command.DbUser.DataSource))
            {
                var directoryName = Path.GetDirectoryName(Command.DbUser.DataSource);

                if (!Directory.Exists(directoryName))
                {
                    Directory.CreateDirectory(directoryName);
                }
                using (SqlCeEngine engine = new SqlCeEngine(Command.DbUser.ConnectionString))
                {
                    engine.CreateDatabase();
                }
            }
        }

        private bool ExtractCmd(string sql, out string objectName, out string action)
        {
            objectName = action = string.Empty;

            if (!string.IsNullOrWhiteSpace(sql))
            {
                var matchCmd = SqlCeCommandRegex.Match(sql);
                if (matchCmd.Success)
                {
                    objectName = matchCmd.Groups["TableName"].Value.Trim();
                    action = matchCmd.Groups["Action"].Value.ToUpper().Trim();
                    return true;
                }
            }
            return false;
        }

        private void Init()
        {
            switch (Command.Action)
            {
                case CommandActions.Install:
                    this.Text = EasyLabRs.DatabaseInstallation;
                    this.gbFiles.Text = EasyLabRs.SqlScripts;
                    this.txtFilePath.Text = Command.DbUser.DataSource;
                    this.txtPassword.Text = Command.DbUser.Password;
                    break;
                case CommandActions.Uninstall:
                    this.Text = EasyLabRs.DatabaseUninstallation;
                    this.gbFiles.Text = EasyLabRs.SqlScripts;
                    this.txtFilePath.Text = Command.DbUser.DataSource;
                    this.txtPassword.Text = Command.DbUser.Password;
                    break;
                case CommandActions.Config:
                    this.Text = EasyLabRs.DatabaseConfiguration;
                    this.gbFiles.Text = EasyLabRs.SqlScripts;
                    this.txtFilePath.Text = Command.DbUser.DataSource;
                    this.txtPassword.Text = Command.DbUser.Password;
                    break;
                default:
                    break;
            }
            if (string.IsNullOrWhiteSpace(Command.Path))
            {
                Command.Path = Environment.CurrentDirectory;
            }
            txtChangeFolder.Text = Command.Path;
            dlgBrowseFolder.SelectedPath = GetFullPath(Command.Path);

            LoadFiles(Command.Path);
            CheckFiles(Command.Files);
            UpdateButtonsUI();
        }

        private void CheckFiles(List<string> files)
        {
            foreach (ListViewItem item in listViewFilePath.Items)
            {
                if (files.Exists(o => string.Compare(o, item.Text, true) == 0))
                {
                    item.Checked = true;
                }
            }
        }

        private void LoadFiles(string folder)
        {
            listViewFilePath.Items.Clear();

            folder = GetFullPath(folder);

            var directory = new DirectoryInfo(folder);

            var files = directory.GetFiles(ExtFilter).OrderBy(o => o.Name.ToLower());

            foreach (var file in files)
            {
                listViewFilePath.Items.Add(file.Name);
            }
            UpdateFilesButtons();
        }

        private void ShowSuccessMessage()
        {
            string message = string.Empty;
            switch (Command.Action)
            {
                case CommandActions.Install:
                    message = EasyLabRs.InstallDatabaseOK;
                    break;
                case CommandActions.Uninstall:
                    message = EasyLabRs.UninstallDatabaseOK;
                    break;
                case CommandActions.Config:
                    message = EasyLabRs.ConfigDatabaseOK;
                    break;
                default:
                    break;
            }
            message.Write(Command.Silent);
        }

        private void UpdateConnectionStrings()
        {
            TestConnection(Command.DbUser.ConnectionString);
            WriteConnection(Command.DbUser.ConnectionString);
        }

        private void WriteConnection(string connectionString)
        {
            List<string> errors = new List<string>();

            foreach (string file in Command.Files.ToFullPath(Command.Path))
            {
                try
                {
                    WriteConnectionToConfig(connectionString, file);
                }
                catch (Exception ex)
                {
                    errors.Add(string.Format(EasyLabRs.ConfigFileUpdateError, file, ex.Message));
                }
            }
            if (errors.Count > 0)
            {
                throw new DbToolException(errors.ToMessage());
            }
        }

        private void WriteConnectionToConfig(string connectionString, string configFile)
        {
            var file = ConfigurationManager.OpenExeConfiguration(configFile.ToExeFile());
            if (file.ConnectionStrings.ConnectionStrings["EasyLabDb"] == null)
            {
                file.ConnectionStrings.ConnectionStrings.Add(new ConnectionStringSettings("EasyLabDb", connectionString, "System.Data.SqlServerCe.3.5"));
            }
            else
            {
                file.ConnectionStrings.ConnectionStrings["EasyLabDb"].ConnectionString = connectionString;
                file.ConnectionStrings.ConnectionStrings["EasyLabDb"].ProviderName = "System.Data.SqlServerCe.3.5";
            }
            file.Save(ConfigurationSaveMode.Minimal);
        }

        private string GetFullPath(string directory)
        {
            if (!String.IsNullOrWhiteSpace(directory))
            {
                var folder = new DirectoryInfo(directory);
                if (folder.Exists)
                {
                    return folder.FullName;
                }
            }
            return Environment.CurrentDirectory;
        }
        /// <summary>
        /// Update [Select All] and [UnSelect All] button's state
        /// </summary>
        private void UpdateFilesButtons()
        {
            btnSelectAll.Enabled = listViewFilePath.Items.Count > 0;
            btnUnSelectAll.Enabled = listViewFilePath.Items.Count > 0;
        }

        private void ValidateInput()
        {
            if (string.IsNullOrWhiteSpace(txtFilePath.Text))
            {
                throw new DbToolException(EasyLabRs.DataBaseNotSet);
            }
            if (Command.Files.Count == 0)
            {
                throw new DbToolException(EasyLabRs.CommandFileNotSet);
            }
        }

        private void UpdateFilesCheck(bool checkAll)
        {
            foreach (ListViewItem item in listViewFilePath.Items)
            {
                item.Checked = checkAll;
            }
        }

        private void UpdateButtonsUI()
        {
            UpdateFilesButtons();
        }

        private void CollectData()
        {
            Command.DbUser.DataSource = txtFilePath.Text;
            Command.DbUser.Password = txtPassword.Text;
            Command.Files.Clear();
            Command.Path = txtChangeFolder.Text;
            foreach (ListViewItem item in listViewFilePath.Items)
            {
                if (item.Checked)
                {
                    Command.Files.Add(item.Text);
                }
            }
        }

        #endregion

    }
}
