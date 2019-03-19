using System;
using System.Collections.Generic;
using System.DirectoryServices;
using System.Linq;
using NLog;
using EasyLab.Server.Common.Errors;
using EasyLab.Server.Common.ExceptionExtensions;
using EasyLab.Server.Common.Extensions;
using EasyLab.Server.Business.Interface;
using EasyLab.Server.DTOs;

namespace EasyLab.WS.DeviceAgent.Models
{
    class LabInSyncSettings
    {
        #region Static Members

        private static Logger logger = LogManager.GetCurrentClassLogger();

        #endregion

        private DependencyResolver resolver;

        private bool IsNetworkAvailable
        {
            get
            {
                return System.Net.NetworkInformation.NetworkInterface.GetIsNetworkAvailable();
            }
        }

        public LabInSyncSettings(DependencyResolver resolver)
        {
            if (resolver == null)
            {
                throw new ArgumentNullException("resolver");
            }

            this.resolver = resolver;
        }

        /// <summary>
        /// Download Lab and Instruments list from server.
        /// </summary>
        public void Synchronize()
        {
            logger.Info(EasyLabRs.LOG_LDAP_SYNC_SETTINGS_BEGIN_SYNC, DateTime.Now);

            if (IsNetworkAvailable)
            {
                
            }
        }
    }
}
