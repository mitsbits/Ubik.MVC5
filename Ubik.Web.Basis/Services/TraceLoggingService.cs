using System;
using System.Diagnostics;
using Ubik.Infra;
using Ubik.Infra.Contracts;

namespace Ubik.Web.Basis.Services
{
    public class TraceLoggingService : ILoggingService
    {
        #region Implementation of ILoggingService

        public void LogMessage(string message)
        {
            Trace.WriteLine(message);
        }

        public void LogMessage(string message, LogEntryType entryType)
        {
            WriteToTraceLevel(message, entryType);
        }

        public void LogException(Exception ex)
        {
            Trace.TraceError(ex.ToTraceString());
        }

        public void LogException(Exception ex, LogEntryType entryType)
        {
            Trace.TraceInformation("Error level: {0}\r\n{1}", entryType, ex.ToTraceString());
        }

        /* Handler designed to use the appropriate tracing tool depending upon the intent of the end-user */

        private static void WriteToTraceLevel(string message, LogEntryType entryType)
        {
            switch (entryType)
            {
                case LogEntryType.Error: //Designed to fall-through to the failure case
                case LogEntryType.Failure:
                    Trace.TraceError(message);
                    break;

                case LogEntryType.Information:
                    Trace.TraceInformation(message);
                    break;

                case LogEntryType.Warning:
                    Trace.TraceWarning(message);
                    break;

                default:
                    Trace.WriteLine(message, entryType.ToString());
                    break;
            }
        }

        #endregion Implementation of ILoggingService
    }

    /// <summary>
    /// Helper class for condensing exceptions down into single strings
    /// </summary>
    internal static class ExceptionHelpers
    {
        /// <summary>
        /// Condenses an exception down into a string that is of an appropriate size for System.Trace
        /// </summary>
        /// <param name="ex">The exception to trace</param>
        /// <returns>A string containing the Exception message, Source, and Trace</returns>
        public static string ToTraceString(this Exception ex)
        {
            return string.Format("Exception: {0}\r\n Source:{1}\r\n Trace:{2}", ex.Message, ex.Source, ex.StackTrace);
        }
    }
}