/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/12/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using System.IO;
using System.Security.AccessControl;

namespace EasyLab.Server.Common.Extensions
{
    public static class FileDirectorySecurity
    {
        public static void AddDirectorySecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            //Create a new DirectoryInfo Object
            DirectoryInfo dInfo = new DirectoryInfo(FileName);

            //Get a DirecctorySecurity object that represents the current security settings
            DirectorySecurity dSecurity = dInfo.GetAccessControl();

            //Add the FileSystemAccessRule to the security settings
            dSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

            //Set the new access settings
            dInfo.SetAccessControl(dSecurity);
        }
        /// <summary>
        /// It adds security rights to a file
        /// </summary>
        /// <param name="FileName"></param>
        /// <param name="Account"></param>
        /// <param name="Rights"></param>
        /// <param name="ControlType"></param>
        public static void AddFileSecurity(string FileName, string Account, FileSystemRights Rights, AccessControlType ControlType)
        {
            //Create a new FileInfo instance
            FileInfo fInfo = new FileInfo(FileName);

            FileSecurity fSecurity = fInfo.GetAccessControl();

            fSecurity.AddAccessRule(new FileSystemAccessRule(Account, Rights, ControlType));

            fInfo.SetAccessControl(fSecurity);
        }
    }
}
