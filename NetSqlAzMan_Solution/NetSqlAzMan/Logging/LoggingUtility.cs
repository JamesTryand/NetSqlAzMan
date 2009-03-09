using System;
using System.Threading;
using System.Security.Principal;
using System.Diagnostics;
using System.Collections.Generic;
using System.Text;
using NetSqlAzMan;
using NetSqlAzMan.Interfaces;
using NetSqlAzMan.LINQ;
using System.Linq;

namespace NetSqlAzMan.Logging
{
    /// <summary>
    /// Utility for message logging.
    /// </summary>
    public class LoggingUtility
    {
        private const string EVENTSOURCE = "NetSqlAzMan";
        /// <summary>
        /// Initializes a new instance of the <see cref="LoggingUtility"/> class.
        /// </summary>
        public LoggingUtility()
        {
        }

        /// <summary>
        /// Writes the event.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="message">The message.</param>
        /// <param name="type">The type.</param>
        private void WriteEvent(IAzManStorage storage, string message, EventLogEntryType type)
        {
            try
            {
                SqlAzManStorage typedStorage = storage as SqlAzManStorage;
                bool logOnDb = false;
                bool logOnEventLog = true;
                string connectionString = String.Empty;
                Guid instanceGuid = Guid.Empty;
                Guid transactionGuid = Guid.Empty;
                int operationCounter = 0;
                DateTime now = DateTime.Now;
                if (typedStorage != null)
                {
                    logOnDb = typedStorage.LogOnDb;
                    logOnEventLog = typedStorage.LogOnEventLog;
                    connectionString = typedStorage.db.Connection.ConnectionString;
                    instanceGuid = typedStorage.instanceGuid;
                    transactionGuid = typedStorage.transactionGuid;
                    operationCounter = ++typedStorage.operationCounter;
                }
                string originalMessage = message;
                if (logOnDb)
                {
                    string ENSType = "User Log";
                    if (message.StartsWith("ENS Event: ", StringComparison.CurrentCultureIgnoreCase))
                    {
                        ENSType = message.Substring(message.IndexOf("ENS Event: ") + 11);
                        ENSType = ENSType.Substring(0, ENSType.IndexOf("\r\n"));
                        message = message.Substring(message.IndexOf("\r\n") + 2);
                        message = message.TrimStart('\r').TrimStart('\n');
                    }
                    while (message.IndexOf("\r\n\r\n") != -1)
                    {
                        message = message.Replace("\r\n\r\n", "\r\n");
                    }
                    message = message.Replace("\r\n", ";");
                    string logType;
                    switch (type)
                    {
                        case EventLogEntryType.Error: logType = "E"; break;
                        case EventLogEntryType.Warning: logType = "W"; break;
                        default:
                            logType = "I"; break;
                    }
                    NTAccount nta = new NTAccount(WindowsIdentity.GetCurrent().Name);
                    SecurityIdentifier sid = (SecurityIdentifier)nta.Translate(typeof(SecurityIdentifier));
                    string winIdentity = String.Format("{0} ({1}", nta.Value, sid.Value);
                    NetSqlAzManStorageDataContext db = new NetSqlAzManStorageDataContext(connectionString);
                    NetSqlAzMan.LINQ.LogTable log = new LogTable();
                    log.LogDateTime = now;
                    log.WindowsIdentity = winIdentity;
                    log.MachineName = Environment.MachineName;
                    log.InstanceGuid = instanceGuid;
                    log.TransactionGuid = transactionGuid == Guid.Empty ? null : new Guid?();
                    log.OperationCounter = operationCounter;
                    log.ENSType = ENSType;
                    log.ENSDescription = message;
                    log.LogType = logType;
                    db.LogTables.InsertOnSubmit(log);
                    db.SubmitChanges();
                }
                if (logOnEventLog)
                {
                    if (!EventLog.SourceExists(LoggingUtility.EVENTSOURCE))
                    {
                        EventLog.CreateEventSource(LoggingUtility.EVENTSOURCE, "Application");
                    }
                    originalMessage += String.Format("\r\nInstance Guid: {0}\r\nTransaction Guid: {1}\r\nOperation Counter: {2}", instanceGuid.ToString(), transactionGuid.ToString(), operationCounter);
                    EventLog.WriteEntry(LoggingUtility.EVENTSOURCE, originalMessage, type);
                }
            }
            catch
            { 
                //Ignore if error occur during logging
            }
        }

        /// <summary>
        /// Writes the info.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="message">The message.</param>
        public void WriteInfo(IAzManStorage storage, string message)
        {
            if (storage != null && storage.LogInformations)
                this.WriteEvent(storage, message, EventLogEntryType.Information);
        }

        /// <summary>
        /// Writes the warning.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="message">The message.</param>
        public void WriteWarning(IAzManStorage storage, string message)
        {
            if (storage != null && storage.LogWarnings)
                this.WriteEvent(storage, message, EventLogEntryType.Warning);
        }

        /// <summary>
        /// Writes the error.
        /// </summary>
        /// <param name="storage">The storage.</param>
        /// <param name="message">The message.</param>
        public void WriteError(IAzManStorage storage, string message)
        {
            if (storage != null && storage.LogErrors)
                this.WriteEvent(storage, message, EventLogEntryType.Error);
        }
    }
}
