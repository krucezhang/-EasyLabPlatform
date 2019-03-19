/*************************************************************
* © 2015 EasyLab Industries Corporation All rights reserved
* Author:          Kruce.Zhang
* Date:            1/28/2015
* Revision:        0.1       Draft
*                  
************************************************************/
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Resources;
using System;
using System.IO;
using System.Net;
using System.Net.NetworkInformation;
using System.Text;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Json;
using System.Security.Cryptography;

namespace EasyLab.Server.Business
{
    internal class Helper
    {

        #region Check network connection

        public static bool IsNetworkAvailable
        {
            get
            {
                return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            }
        }
        /// <summary>
        /// Check whether can make a connection to server
        /// </summary>
        /// <param name="serverName">server machine name</param>
        /// <returns></returns>
        public static bool CanConnectServer(string serverName)
        {
            //if the server name is empty, the connection doesn't make sense.
            if (string.IsNullOrWhiteSpace(serverName))
            {
                return false;
            }
            //if REST server is local machine, the connection should always be available
            if (ServerIsLocalMachine(serverName))
            {
                return true;
            }
            //if REST server is not local machine, check whether there is network connection.
            return IsNetworkAvailable;
        }

        

        /// <summary>
        /// Get whether the server name represents a local machine.
        /// </summary>
        /// <param name="serverName"></param>
        /// <returns>true if the server is local machine, otherwise, false</returns>
        private static bool ServerIsLocalMachine(string serverName)
        {
            foreach (string machineName in GetLocalMachineNames())
            {
                if (string.Equals(serverName, machineName, StringComparison.OrdinalIgnoreCase))
                {
                    return true;
                }
            }

            return false;
        }
        /// <summary>
        /// Get the possible machine name of the local machine.
        /// </summary>
        /// <returns></returns>
        private static string[] GetLocalMachineNames()
        {
            return new string[] { "localhost", Environment.MachineName, System.Net.Dns.GetHostEntry(string.Empty).HostName };
        }

        #endregion

        public static string ReadResponseStream(HttpWebResponse response)
        {
            using (Stream stream = response.GetResponseStream())
            {
                StreamReader reader = new StreamReader(stream, Encoding.UTF8);

                return reader.ReadToEnd();
            }
        }

        public static T ResponseJsonSerializer<T>(string responseJson)
        {

            DataContractJsonSerializer ser = new DataContractJsonSerializer(typeof(T));

            MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(responseJson));

            T jsonObject = (T)ser.ReadObject(ms);

            ms.Close();

            return jsonObject;
        }

        public static T Deserialize<T>(string json)
        {
            T obj = Activator.CreateInstance<T>();
            using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(json)))
            {
                DataContractJsonSerializer serializer = new DataContractJsonSerializer(obj.GetType());
                return (T)serializer.ReadObject(ms);
            }
        }  

        #region Password


        public static string MD5Encrypt(string encryptString)
        {
            if (String.IsNullOrWhiteSpace(encryptString))
            {
                return string.Empty;
            }

            byte[] byteResult = Encoding.Default.GetBytes(encryptString);

            MD5 md = new MD5CryptoServiceProvider();

            byte[] output = md.ComputeHash(byteResult);

            return BitConverter.ToString(output).Replace("-", "");
        }
        /// <summary>
        /// Validate whether the plain password and encrypted password are the same.
        /// </summary>
        /// <param name="plainPassword">plain password without encryption</param>
        /// <param name="encryptedPassword">encrypted password</param>
        public static void ValidatePassword(string plainPassword, string encryptedPassword)
        {
            if (!MatchPasswords(plainPassword, encryptedPassword))
            {
                throw new EasyLabException(System.Net.HttpStatusCode.Conflict, Rs.INVALID_PASSWORD);
            }
        }
        /// <summary>
        /// Get whether the plain password and encrypted password are the same.
        /// </summary>
        /// <param name="plainPassword">plain password without encryption</param>
        /// <param name="encryptedPassword">encrypted password</param>
        /// <returns>true if they are the same; otherwise, false.</returns>
        public static bool MatchPasswords(string plainPassword, string encryptedPassword)
        {
            return string.Equals(DecryptPassword(encryptedPassword), plainPassword);
        }
        /// <summary>
        /// Encrypte a string.
        /// </summary>
        /// <param name="plainPassword">plain password</param>
        /// <returns>encrypted password</returns>
        public static string EncryptPassword(string plainPassword)
        {
            if (string.IsNullOrEmpty(plainPassword))
            {
                return string.Empty;
            }
            return new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Encrypt(plainPassword);
        }
        /// <summary>
        /// Decrypte an encrypted password to plain password.
        /// </summary>
        /// <param name="encryptedPassword">encrypted password</param>
        /// <returns>plain password</returns>
        public static string DecryptPassword(string encryptedPassword)
        {
            if (string.IsNullOrEmpty(encryptedPassword))
            {
                return encryptedPassword;
            }

            return new EasyLab.Server.Common.Extensions.RijndaelEnhanced().Decrypt(encryptedPassword);
        }

        #endregion
    }
}
