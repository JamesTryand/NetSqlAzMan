//#define TEST

using System;
using System.Configuration;
using System.Diagnostics;
using System.ServiceModel;
using System.ServiceProcess;
using System.Security;

namespace NetSqlAzMan.Cache.Service
{
    /// <summary>
    /// .NET Sql Authorization Manager Cache Service class.
    /// </summary>
    public partial class WindowsCacheService : ServiceBase
    {
        private ServiceHost serviceHost = null;
        private DateTime nextExecution = DateTime.Now;
        private bool faultState = false;
        /// <summary>
        /// Initializes a new instance of the <see cref="WindowsCacheService"/> class.
        /// </summary>
        public WindowsCacheService()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Called when [start internal].
        /// </summary>
        [PreEmptive.Attributes.Setup(CustomEndpoint = "so-s.info/PreEmptive.Web.Services.Messaging/MessagingServiceV2.asmx")]
        [PreEmptive.Attributes.Teardown()]
        internal void OnStartInternal()
        {
            try
            {
                //Start serviceHost
                if (this.serviceHost != null)
                {
                    this.serviceHost.Close();
                }

                // Create a ServiceHost for the CacheService type and 
                // provide the base address.
                this.serviceHost = new ServiceHost(typeof(CacheService));

                // Open the ServiceHostBase to create listeners and start 
                // listening for messages.
                this.serviceHost.Open();

                //Check storage connection string
                using (SqlAzManStorage s = new SqlAzManStorage(Properties.Settings.Default.NetSqlAzManStorageCacheConnectionString))
                {
                    s.OpenConnection();
                    s.CloseConnection();
                }

                //Start cache building in a new thread
                this.setNextExecution();
                CacheService.startStorageBuildCache();
#if TEST
            while (true)
            {
                System.Threading.Thread.Sleep(500);
                System.Windows.Forms.Application.DoEvents();
            }
#endif
                this.faultState = false;
            }
            catch (Exception ex)
            {
                WindowsCacheService.writeEvent(ex.Message, EventLogEntryType.Error);
                this.faultState = true;
                this.timer1.Interval = 60000; //Retry after 1 minute
                this.timer1.Start();
            }
        }

        /// <summary>
        /// Sets the next execution.
        /// </summary>
        private void setNextExecution()
        {
            try
            {
                this.timer1.Stop();
                string expirationValue = ConfigurationManager.AppSettings["expirationValue"].Trim();
                TimeSpan tsFreq = new TimeSpan(int.Parse(expirationValue.Split(' ')[0]), int.Parse(expirationValue.Split(' ')[1]), int.Parse(expirationValue.Split(' ')[2]), int.Parse(expirationValue.Split(' ')[3]));
                this.nextExecution = DateTime.Now.Add(tsFreq);
                TimeSpan delta = this.nextExecution.Subtract(DateTime.Now);
                double d = delta.TotalMilliseconds;
                if (d > int.MaxValue)
                {
                    this.timer1.Interval = int.MaxValue;
                }
                else
                {
                    if (d >= 500)
                        this.timer1.Interval = d;
                    else
                        this.timer1.Interval = 500D;
                }
                if (this.faultState)
                    this.timer1.Interval = 60000; //Retry after 1 minute if faulted
            }
            catch (Exception ex)
            {
                WindowsCacheService.writeEvent(ex.Message, EventLogEntryType.Error);
            }
            finally
            {
                this.timer1.Start();
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Start command is sent to the service by the Service Control Manager (SCM) or when the operating system starts (for a service that starts automatically). Specifies actions to take when the service starts.
        /// </summary>
        /// <param name="args">Data passed by the start command.</param>
        [PreEmptive.Attributes.Feature("WCF Cache Service Started")]
        [SecurityCritical()]
        protected override void OnStart(string[] args)
        {
            this.OnStartInternal();
        }

        /// <summary>
        /// Called when [stop internal].
        /// </summary>
        internal void OnStopInternal()
        {
            if (this.serviceHost != null)
            {
                this.serviceHost.Close();
                this.serviceHost = null;
            }
        }

        /// <summary>
        /// When implemented in a derived class, executes when a Stop command is sent to the service by the Service Control Manager (SCM). Specifies actions to take when a service stops running.
        /// </summary>
        protected override void OnStop()
        {
            this.OnStopInternal();
        }

        /// <summary>
        /// Releases all resources used by the <see cref="T:System.ComponentModel.Component"/>.
        /// </summary>
        public new void Dispose()
        {
            this.OnStopInternal();
            base.Dispose();
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Handles the Elapsed event of the timer1 control.
        /// </summary>
        /// <param name="sender">The source of the event.</param>
        /// <param name="e">The <see cref="System.Timers.ElapsedEventArgs"/> instance containing the event data.</param>
        private void timer1_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            try
            {
                this.timer1.Stop();
                if (DateTime.Now < this.nextExecution && !this.faultState)
                {
                    TimeSpan delta = this.nextExecution.Subtract(DateTime.Now);
                    double d = delta.TotalMilliseconds;
                    if (d > int.MaxValue)
                        this.timer1.Interval = int.MaxValue;
                    else
                    {
                        if (d > 0)
                        {
                            this.timer1.Interval = d;
                        }
                    }
                }
                else if (DateTime.Now >= this.nextExecution || this.faultState)
                {
                    //Check storage connection string
                    using (SqlAzManStorage s = new SqlAzManStorage(Properties.Settings.Default.NetSqlAzManStorageCacheConnectionString))
                    {
                        s.OpenConnection();
                        s.CloseConnection();
                    }
                    CacheService.startStorageBuildCache();
                    this.faultState = false;
                    this.setNextExecution();
                }
            }
            catch (Exception ex)
            {
                WindowsCacheService.writeEvent(ex.Message, EventLogEntryType.Error);
                this.faultState = true;
                this.timer1.Interval = 60000; //Retry after 1 minute
            }
            finally
            {
                this.timer1.Start();
            }
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        internal static void writeEvent(string message, EventLogEntryType type)
        {
            string eventSource = ".NET Sql Authorization Manager Cache Service";
            try
            {
                if (!EventLog.SourceExists(eventSource))
                {
                    EventLog.CreateEventSource(eventSource, "Application");
                }
                EventLog.WriteEntry(eventSource, message, type);
            }
            catch
            {
                //Ignore if error occur during logging
            }
        }
    }
}
