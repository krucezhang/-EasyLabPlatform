/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            2/08/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Collections.Generic;
using System.Security.Principal;

namespace EasyLab.Tools.Db.SqlCe
{
    public class DbTool
    {
        #region Properties

        internal CommandInfo Command { get; set; }

        #endregion 

        #region Database Tools Public Methods

        /// <summary>
        /// Lanuch the tool with the command parameters.
        /// </summary>
        /// <param name="args">command parameters</param>
        public void Start(string[] args)
        {
            Command = GetCommand(args);
            ProcessCommand(Command);
        }

        #endregion

        #region Private Method
        /// <summary>
        /// Execute the command
        /// </summary>
        /// <param name="Command"></param>
        private void ProcessCommand(CommandInfo Command)
        {
            switch (Command.Action)
            {
                case CommandActions.Install:
                    Install();
                    break;
                case CommandActions.Uninstall:
                    Uninstall();
                    break;
                case CommandActions.Config:
                    Config();
                    break;
                case CommandActions.Elevate:
                    Elevate();
                    break;
                default:
                    break;
            }
        }
        /// <summary>
        /// Parse the command parameters to command information.
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        private CommandInfo GetCommand(string[] args)
        {
            var command = new CommandInfo();
            //System.Windows.Forms.MessageBox.Show(args[3]);
            if (args != null)
            {
                for (int i = 0; i < args.Length; i++)
                {
                    switch (args[i])
                    {
                        case "-i":
                        case "/i":
                            command.Action = CommandActions.Install;
                            break;
                        case "-u":
                        case "/u":
                            command.Action = CommandActions.Uninstall;
                            break;
                        case "-c":
                        case "/c":
                            command.Action = CommandActions.Config;
                            break;
                        case "-e":
                        case "/e":
                            command.Action = CommandActions.Elevate;
                            break;
                        case "-silent":
                        case "/silent":
                            command.Silent = true;
                            break;
                        case "-path":
                        case "/path":
                            command.Path = GetArgumentValue(args, i + 1);
                            break;
                        case "-file":
                        case "/file":
                            command.Files.AddRange(ParseFiles(GetArgumentValue(args, i + 1)));
                            break;
                        case "-svr":
                        case "/svr":
                            command.DbUser.DataSource = GetArgumentValue(args, i + 1);
                            break;
                        case "-db":
                        case "/db":
                            command.DbUser.InitialCatalog = GetArgumentValue(args, i + 1);
                            break;
                        case "-pwd":
                        case "/pwd":
                            command.DbUser.Password = GetArgumentValue(args, i + 1);
                            break;
                        default:
                            break;
                    }
                }
            }

            return command;
        }
        
        /// <summary>
        /// Start Configuration mode.
        /// </summary>
        private void Config()
        {
            using (Installation cmd = new Installation())
            {
                cmd.Exec(Command);
            }
        }
        /// <summary>
        /// Start the Uninstallation mode.
        /// </summary>
        private void Uninstall()
        {
            using (Installation cmd = new Installation())
            {
                cmd.Exec(Command);
            }
        }
        /// <summary>
        /// Start the Installation mode.
        /// </summary>
        private void Install()
        {
            using (Installation cmd = new Installation())
            {
                cmd.Exec(Command);
            }
        }
        /// <summary>
        /// Start the elevate mode.
        /// </summary>
        private void Elevate()
        {
            bool isAdmin = IsAdministrator();

            if (!isAdmin)
            {
                throw new System.Exception(EasyLabRs.ElevatedError);
            }
        }
        /// <summary>
        /// Check whether the application is run as administrator
        /// </summary>
        /// <returns></returns>
        private bool IsAdministrator()
        {
            bool isAdmin;
            try
            {
                using (WindowsIdentity user = WindowsIdentity.GetCurrent())
                {
                    WindowsPrincipal principle = new WindowsPrincipal(user);
                    isAdmin = principle.IsInRole(WindowsBuiltInRole.Administrator);
                }
            }
            catch (UnauthorizedAccessException)
            {
                isAdmin = false;
            }
            catch (Exception)
            {
                isAdmin = false;
            }

            return isAdmin;
        }
        /// <summary>
        /// Get the command value from the index of the arguments.
        /// </summary>
        /// <param name="args"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        private string GetArgumentValue(string[] args, int index)
        {
            if (args == null || args.Length == 0 || index >= args.Length)
            {
                return string.Empty;
            }
            return args[index];
        }
        /// <summary>
        /// Parses the files seperated by semicolon.
        /// </summary>
        /// <param name="files">files</param>
        /// <returns></returns>
        private List<string> ParseFiles(string files)
        {
            var results = new List<string>();

            if (files != null)
            {
                results.AddRange(files.Split(new char[]{';'}, StringSplitOptions.RemoveEmptyEntries));
            }

            return results;
        }

        #endregion

    }
}
