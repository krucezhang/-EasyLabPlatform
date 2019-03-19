using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace EasyLab.Tools.Db
{
    public static class Extensions
    {
        /// <summary>
        /// Combine the files with a path.
        /// </summary>
        /// <param name="files"></param>
        /// <param name="path"></param>
        /// <returns></returns>
        public static IEnumerable<string> ToFullPath(this IEnumerable<string> files, string path)
        {
            if (String.IsNullOrWhiteSpace(path))
            {
                return files;
            }

            return files.Select(o => Path.Combine(path, o)).ToList();
        }

        /// <summary>
        /// Convert a collection of errors to one error message.
        /// </summary>
        /// <param name="errors"></param>
        /// <returns></returns>
        public static string ToMessage(this IEnumerable<string> errors)
        {
            return string.Join(Environment.NewLine, errors.ToArray());
        }
        /// <summary>
        /// Get the .exe file name from .config file name.
        /// </summary>
        /// <param name="configFile"></param>
        /// <returns></returns>
        public static string GetExeFile(this string configFile)
        {
            if (string.IsNullOrWhiteSpace(configFile))
            {
                return configFile;
            }

            int index = configFile.LastIndexOf(".config", StringComparison.OrdinalIgnoreCase);

            if (index >= 0)
            {
                return configFile.Remove(index);
            }

            return configFile;
        }
        /// <summary>
        /// Get any item is checked in the list view.
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public static bool AnyChecked(this ListView list)
        {
            foreach (ListViewItem item in list.Items)
            {
                if (item.Checked)
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Get Inner Message of an Exception.
        /// </summary>
        /// <param name="ex"></param>
        /// <returns></returns>
        public static string GetInnerExceptionMessage(this Exception ex)
        {
            if (ex == null)
            {
                return string.Empty;
            }
            if (ex.InnerException == null)
            {
                return ex.Message;
            }

            return GetInnerExceptionMessage(ex.InnerException);
        }

        public static string ToExeFile(this string configFile)
        {
            if (string.IsNullOrWhiteSpace(configFile))
            {
                return configFile;
            }
            int index = configFile.LastIndexOf("*.config", StringComparison.OrdinalIgnoreCase);
            if (index >= 0)
            {
                return configFile.Remove(index);
            }
            return configFile;
        }

        /// <summary>
        /// Write a message to Console or MessageBox
        /// </summary>
        /// <param name="message"></param>
        /// <param name="silent"></param>
        /// <param name="caption"></param>
        /// <param name="icon"></param>
        public static void Write(this string message, bool silent = false, string caption = "", MessageBoxIcon icon = MessageBoxIcon.Information)
        {
            if (silent)
            {
                Console.WriteLine(message);
                Console.WriteLine();
            }
            else
            {
                //MessageBox.Show(message, caption, MessageBoxButtons.OK, icon);
            }
        }
    }
}
