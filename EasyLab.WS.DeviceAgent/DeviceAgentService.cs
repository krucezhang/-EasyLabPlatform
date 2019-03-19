using EasyLab.Server.Business.Interface;
using EasyLab.Server.Common.Errors;
using EasyLab.WS.DeviceAgent.App_Start;
using EasyLab.WS.DeviceAgent.Models;
using NLog;
using System;
using System.Configuration;
using System.Net;
using System.ServiceProcess;
using System.Threading;
using System.Web.Http.SelfHost;

namespace EasyLab.WS.DeviceAgent
{
    public partial class DeviceAgentService : ServiceBase
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        internal const string Id = "EasyLabDeviceAgent";

        private const string EasyLab_MessageTimer_Interval = "easylab_messageTimer_interval";

        /// <summary>
        /// default interval 60 seconds for Auditlog
        /// </summary>
        private const int EasyLab_Default_Auditlog_Interval = 60;

        private const int EasyLab_Default_MessageTimer_Interval = 120;

        private HttpSelfHostServer server;

        private DependencyResolver resolver;

        private Timer messageTimer;

        /// <summary>
        /// The timer to retry messages.
        /// </summary>
        private Timer retryTimer;

        private Timer reserveQueueTimer;

        private Timer onlineTimer;

        private Timer processExpireQueueTimer;

        //private Timer timeStampTimer;

        public DeviceAgentService()
        {
            InitializeComponent();

            this.ServiceName = Id;
            this.resolver = new DependencyResolver();
            //Start Timer
            InitaliazeTimer();
        }

        private void InitaliazeTimer()
        {
            this.messageTimer = new Timer(ProcessMessages, TimerTypes.Normal, Timeout.Infinite, Timeout.Infinite);
            this.retryTimer = new Timer(ProcessMessages, TimerTypes.Retry, Timeout.Infinite, Timeout.Infinite);
            this.reserveQueueTimer = new Timer(ProcessReserveQueue, TimerTypes.ReserveQueue, Timeout.Infinite, Timeout.Infinite);
            this.onlineTimer = new Timer(CheckOnlineClient, TimerTypes.OnlineTimer, Timeout.Infinite, Timeout.Infinite);
            this.processExpireQueueTimer = new Timer(ProcessUpdateQueue, TimerTypes.UpdateQueueTimer, Timeout.Infinite, Timeout.Infinite);

            logger.Info("InitaliazeTimer");
        }

        
        protected override void OnStart(string[] args)
        {
            try
            {
                logger.Info(EasyLabRs.LOG_DEVICE_SERVICE_START);

                var machineName = Dns.GetHostEntry(string.Empty).HostName;
                var portSetting = resolver.Resolve<IGlobalSettingAppSvc>().Get("agent", "devicePort");

                if (portSetting == null || string.IsNullOrEmpty(portSetting.OptionValue))
                {
                    throw new EasyLabException(EasyLabRs.DEVICE_PORT_NOT_SET);
                }
                var addressFormat = ConfigurationManager.AppSettings["easylab_device_service_address"];
                var address = string.Format(addressFormat, machineName, portSetting.OptionValue);

                logger.Info(EasyLabRs.LOG_REST_SERVICE_START);

                var config = new HttpSelfHostConfiguration(address);
                config.MessageHandlers.Add(new LogHandler());

                AttributeRoutingConfig.RegisterRoutes(config);
                FilterConfig.Register(config);

                server = new HttpSelfHostServer(config);
                server.OpenAsync().Wait();

                logger.Info(EasyLabRs.LOG_REST_SERVICE_STARTED_WITH_BASE_ADDRESS, ((HttpSelfHostConfiguration)server.Configuration).BaseAddress.AbsoluteUri);

                StartTimers(TimerTypes.All);

                logger.Info(EasyLabRs.LOG_DEVICE_SERVICE_STARTED);
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(EasyLabRs.FAILED_START_DEVICE_SERVICE_WITH_EXCEPTION, ex);
                throw;
            }
        }

        protected override void OnStop()
        {
            logger.Info(EasyLabRs.LOG_BEGIN_STOP_DEVICE_SERVICE);

            server.CloseAsync().Wait();
            server.Dispose();

            StopTimers(TimerTypes.All);
            logger.Info(EasyLabRs.LOG_END_STOP_DEVICE_SERVICE);
        }

        #region Timer Operation

        private void StopTimers(TimerTypes timerType)
        {
            if (timerType.HasValue(TimerTypes.Normal))
            {
                messageTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (timerType.HasValue(TimerTypes.ReserveQueue))
            {
                reserveQueueTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (timerType.HasValue(TimerTypes.Retry))
            {
                retryTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (timerType.HasValue(TimerTypes.OnlineTimer))
            {
                onlineTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }

            if (timerType.HasValue(TimerTypes.UpdateQueueTimer))
            {
                processExpireQueueTimer.Change(Timeout.Infinite, Timeout.Infinite);
            }
        }

        private void StartTimers(TimerTypes timerType)
        {
            var interval = GetNumberFromAppConfig(EasyLab_MessageTimer_Interval, EasyLab_Default_MessageTimer_Interval) * 1000;

            if (timerType.HasValue(TimerTypes.Normal))
            {
                messageTimer.Change(interval, interval);
                //logger.Trace(RS.LOG_NORMAL_TIMER_STARTED_WITH_INFOR, interval, MilliSecondsInOneMinute);
            }

            if (timerType.HasValue(TimerTypes.Retry))
            {
                retryTimer.Change(interval, interval);
            }

            if (timerType.HasValue(TimerTypes.ReserveQueue))
            {
                reserveQueueTimer.Change(interval, interval);
            }

            if (timerType.HasValue(TimerTypes.OnlineTimer))
            {
                onlineTimer.Change(interval, interval);
            }

            if (timerType.HasValue(TimerTypes.UpdateQueueTimer))
            {
                processExpireQueueTimer.Change(interval, interval);
            }
        }

        private void DisposeTimers()
        {
            DisposeTimer(ref messageTimer);

            DisposeTimer(ref reserveQueueTimer);

            DisposeTimer(ref retryTimer);

            DisposeTimer(ref onlineTimer);

            DisposeTimer(ref processExpireQueueTimer);
        }

        private void DisposeTimer(ref Timer timer)
        {
            if (timer != null)
            {
                timer.Dispose();
                timer = null;
            }
        }

        #endregion

        public void ProcessUpdateQueue(object state)
        {
             logger.Info("Process Update Reserve Queue");

            var timerType = (TimerTypes)state;

            StopTimers(timerType);

            try
            {
                var syncReserveQueue = resolver.Resolve<IReserveQueueSvc>();

                logger.Info("Start Update Reserve Queue");

                if (timerType.HasValue(TimerTypes.UpdateQueueTimer))
                {
                    if (!syncReserveQueue.ProcessUpdateQueue())
                    {
                        logger.Info("Update Reserve Error!!!");
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(EasyLabRs.LOG_MESSAGE_FAILED_PROCESS_WITH_EXCEPTION, ex);
            }
            finally
            {
                StartTimers(timerType);
            }
        }

        public void CheckOnlineClient(object state)
        {
            logger.Info("Online Client");

            var timerType = (TimerTypes)state;

            StopTimers(timerType);

            try
            {
                //Get current instrument Id
                var deviceSetting = resolver.Resolve<IDeviceSettingAppSvc>();
                var deviceValues = deviceSetting.Get("common", "InstrumentId");

                logger.Info("It is get instrument's id value from DeviceSettings tables of database");

                var onlineResolve = resolver.Resolve<IGlobalSettingAppSvc>();
                var syncInstrumentInfo = resolver.Resolve<ILabInstrumentAppSvc>();
                var syncUserList = resolver.Resolve<IUserAppSvc>();
                if (timerType.HasValue(TimerTypes.OnlineTimer) && deviceValues != null)
                {
                    onlineResolve.CheckOnlineClient(deviceValues.OptionValue);
                    syncInstrumentInfo.syncInstrumentInfo(deviceValues.OptionValue);
                    syncUserList.GetUserInitListByInstrument(deviceValues.OptionValue);
                }
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(EasyLabRs.LOG_MESSAGE_FAILED_PROCESS_WITH_EXCEPTION, ex);
            }
            finally
            {
                StartTimers(timerType);
            }
        }

        public void ProcessReserveQueue(object state)
        {
            logger.Info("reserveQueueTimer");

            var timerType = (TimerTypes)state;

            StopTimers(timerType);

            try
            {
                //Get current instrument Id
                var deviceSetting = resolver.Resolve<IDeviceSettingAppSvc>();
                var deviceValues = deviceSetting.Get("common", "InstrumentId");

                logger.Info("It is get instrument's id value from DeviceSettings tables of database");

                var reserveQueueServer = resolver.Resolve<IReserveQueueSvc>();

                if (timerType.HasValue(TimerTypes.ReserveQueue) && deviceValues != null)
                {
                    reserveQueueServer.AddOrUpdateReserveQueue(deviceValues.OptionValue);

                    logger.Info("About reserve queue is has already latest!");

                    //reserveQueueServer.AutoUpdateReserveQueueState(deviceValues.OptionValue);
                }
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(EasyLabRs.LOG_MESSAGE_FAILED_PROCESS_WITH_EXCEPTION, ex);
            }
            finally
            {
                StartTimers(timerType);
            }
        }

        public void ProcessMessages(object state)
        {
            logger.Info("ProcessMessages");

            var timerType = (TimerTypes)state;

            StopTimers(timerType);

            try
            {
                var messageService = resolver.Resolve<IDeviceMessageAppSvc>();

                logger.Info<TimerTypes>(EasyLabRs.LOG_MESSAGE_TYPE_IS_PROCESS, timerType);

                if (timerType.HasValue(TimerTypes.Normal))
                {
                    messageService.Process();
                }
                if (timerType.HasValue(TimerTypes.Retry))
                {
                    messageService.Retry();
                }
            }
            catch (Exception ex)
            {
                logger.Error<Exception>(EasyLabRs.LOG_MESSAGE_FAILED_PROCESS_WITH_EXCEPTION, ex);
            }
            finally
            {
                //resume the timer at last
                StartTimers(timerType);
            }
        }

        private int GetNumberFromAppConfig(string key, int defaultValue)
        {
            var value = ConfigurationManager.AppSettings[key];

            int intValue;

            if (int.TryParse(value, out intValue) && intValue > 0)
            {
                return intValue;   
            }
            return defaultValue;
        }
    }
}
