using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Debugging.Logging
{
    public class LoggerConfiguration
    {
        /// <summary>
        /// Default logger configuration.
        /// </summary>
        public static readonly LoggerConfiguration DEFAULT = new LoggerConfiguration()
        {
            LogFolder = "log",
            LogToFile = true,
            LogToConsole = true,
            LogTime = true,
            LogSeverity = true,
            LogCaller = true,
        };

        /// <summary>
        /// The folder path of all the log files.
        /// </summary>
        public string LogFolder { get; set; } = string.Empty;

        /// <summary>
        /// Holds all severities that should not be logged.
        /// </summary>
        public HashSet<Severity> SeverityFilter { get; } = new HashSet<Severity>();

        /// <summary>
        /// Holds all tags (as string) that should not be logged.
        /// </summary>
        public HashSet<string> TagFilter { get; } = new HashSet<string>();

        /// <summary>
        /// Wether or not to log to file.
        /// </summary>
        public bool LogToFile { get; set; }

        /// <summary>
        /// Wether or not to log to console.
        /// </summary>
        public bool LogToConsole { get; set; }

        /// <summary>
        /// Wether or not to add a (sortable) timestamp to the log.
        /// </summary>
        public bool LogTime { get; set; }

        /// <summary>
        /// Wether or not to add a severity tag to the log.
        /// </summary>
        public bool LogSeverity { get; set; }

        /// <summary>
        /// Wether or not to add the caller to the log.
        /// </summary>
        public bool LogCaller { get; set; }
    }
}
