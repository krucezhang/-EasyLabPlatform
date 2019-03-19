/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            2/08/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace EasyLab.Tools.Db.SqlCe
{
    static class Program
    {
        [DllImport("Kernel32.dll")]
        public static extern bool AttachConsole(int processId);

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            var tool = new DbTool();

            try
            {
                AttachConsole(-1);
                tool.Start(args);
                //System.Windows.Forms.Application.Run(new Installation());
            }
            catch (Exception ex)
            {
                ex.GetInnerExceptionMessage().Write(tool.Command.Silent, icon: MessageBoxIcon.Error);
                Environment.Exit(-1);
            }
        }
    }
}
