using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EasyLab.Tools.Db
{
    /// <summary>
    /// Represents the command information
    /// </summary>
    public class CommandInfo
    {
        private string path;

        /// <summary>
        /// Get or Set the command type, Default is Install
        /// </summary>
        public CommandActions Action { get; set; }

        /// <summary>
        /// Get or Set the path script files, Default is empty which stands for current folder.
        /// </summary>
        public string Path
        {
            get
            {
                return path;
            }
            set
            {
                path = (value ?? string.Empty);
            }
        }
        /// <summary>
        /// Get or Set the silent mode which doesn't show the forms, Default is false.
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        /// Get or set the configuration files to update M1 connection string in config command, or sql script files  in install and uninstall commands. Default is empty.
        /// </summary>
        public List<string> Files { get; private set; }

        /// <summary>
        /// Get or Set the database connection to excute scripts in install/uninstall commands or user to connection to EasyLab database. Default is empty.
        /// </summary>
        public SqlConnectionStringBuilder DbUser { get; internal set; }

        public CommandInfo()
        {
            Action = CommandActions.Install;
            Path = String.Empty;
            Silent = false;
            Files = new List<string>();
            DbUser = new SqlConnectionStringBuilder();
        }
    }
}
