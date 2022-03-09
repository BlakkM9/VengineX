using OpenTK.Graphics.OpenGL4;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace VengineX.Debugging.Logging
{
    /// <summary>
    /// Static logging class, handles all the logging for the game.
    /// </summary>
    public static class Logger
    {
        /// <summary>
        /// Configuration for this logger.
        /// </summary>
        public static LoggerConfiguration Configuration { get; set; } = LoggerConfiguration.DEFAULT;

        private static StreamWriter? _currentLogFileStream;



        /// <summary>
        /// Initializes the logger.
        /// </summary>
        public static void Initialize()
        {
            // Set OpenGL debug message callback
            GL.DebugMessageCallback(DebugProc, IntPtr.Zero);
        }


        // TODO implementation.
        /// <summary>
        /// OpenGL debug message callback.
        /// </summary>
        private static void DebugProc(DebugSource source, DebugType type, int id, DebugSeverity severity, int length, IntPtr message, IntPtr userParam)
        {
            Console.WriteLine("DebugProc called!");
        }


        /// <summary>
        /// Overload for <see cref="LogRaw(Severity, string, string, string, string, int)"/>, without tag and severity (defaults to info).
        /// </summary>
        public static void Log(string message,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => LogRaw(Severity.Info, string.Empty, message, DetermineCallerName(), memberName, sourceLineNumber);


        /// <summary>
        /// Overload for <see cref="LogRaw(Severity, string, string, string, string, int)"/>, without tag.
        /// </summary>
        public static void Log(Severity severity, string message,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => LogRaw(severity, string.Empty, message, DetermineCallerName(), memberName, sourceLineNumber);

        /// <summary>
        /// Overload for <see cref="LogRaw(Severity, string, string, string, string, int)"/>, using <see cref="Severity.Info"/>.
        /// </summary>
        public static void Log(Tag tag, string message,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => LogRaw(Severity.Info, tag.ToString(), message, DetermineCallerName(), memberName, sourceLineNumber);

        /// <summary>
        /// Overload for <see cref="LogRaw(Severity, string, string, string, string, int)"/>, taking <see cref="Tag"/> instead of string for tag.
        /// </summary>
        public static void Log(Severity severity, Tag tag, string message,
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
            => LogRaw(severity, tag.ToString(), message, DetermineCallerName(), memberName, sourceLineNumber);


        /// <summary>
        /// Logs given message as given severity based on configuration.
        /// </summary>
        public static void LogRaw(Severity severity, string tag, string message,
            string caller = "",
            [CallerMemberName] string memberName = "",
            [CallerLineNumber] int sourceLineNumber = 0)
        {
            // Ignore if not logging to console or file
            if (!(Configuration.LogToConsole || Configuration.LogToFile)) { return; }
            // Ignore if severity is filtered.
            if (Configuration.SeverityFilter.Contains(severity)) { return; }
            // Ignore if tag is filtered.
            if (Configuration.TagFilter.Contains(tag)) { return; }

            string logMessage = string.Empty;


            // Add time.
            if (Configuration.LogTime)
            {
                logMessage += CreateTimeTag();
            }


            // Add severity.
            if (Configuration.LogSeverity)
            {
                logMessage += CreateSeverityTag(severity);
            }


            // Add tag
            if (!string.IsNullOrWhiteSpace(tag))
            {
                logMessage += CreateTag(tag);
            }


            // Add caller
            if (Configuration.LogCaller)
            {
                caller = caller == string.Empty ? DetermineCallerName() : caller;
                logMessage += CreateCallerTag(caller, memberName, sourceLineNumber);
            }


            // Add message
            logMessage += message;


            // Log to console.
            if (Configuration.LogToConsole)
            {
                LogToConsole(severity, logMessage);
            }


            // Log to file.
            if (Configuration.LogToFile)
            {
                LogToFile(logMessage);
            }
        }


        /// <summary>
        /// Creates the time tag for the log message.
        /// </summary>
        private static string CreateTimeTag() => $"<{DateTime.Now:O}> ";


        /// <summary>
        /// Creates the severity tag for the log message.
        /// </summary>
        private static string CreateSeverityTag(Severity severity)
        {
            return severity switch
            {
                Severity.Fatal =>   "[ FATAL ] ",
                Severity.Error =>   "[ ERROR ] ",
                Severity.Warning => "[ WARNING ] ",
                Severity.Info =>    "[ INFO ] ",
                Severity.Debug =>   "[ DEBUG ] ",
                _ => throw new ArgumentException($"Severity {severity} is unknown to logger.")
            };
        }


        /// <summary>
        /// Creates a tag for the log message.
        /// </summary>
        private static string CreateTag(string tag) => $"[ {tag} ] ";


        /// <summary>
        /// Creates a tag for the method caller.
        /// </summary>
        private static string CreateCallerTag(string caller, string memberName, int sourceLineNumber) => $"[ {caller}:{memberName}:{sourceLineNumber} ] ";


        /// <summary>
        /// Determines the caller of the function.
        /// </summary>
        /// <returns>Empty string if failed to determine.</returns>
        public static string DetermineCallerName()
        {
            string? fullName;
            Type? declaringType;
            int skipFrames = 2;
            do
            {
                MethodBase? method = new StackFrame(skipFrames, false).GetMethod();
                declaringType = method?.DeclaringType;
                if (declaringType == null)
                {
                    return method?.Name ?? string.Empty;
                }
                skipFrames++;
                fullName = declaringType.FullName;
            }
            while (declaringType.Module.Name.Equals("mscorlib.dll", StringComparison.OrdinalIgnoreCase));

            return fullName ?? string.Empty;
        }

        /// <summary>
        /// Logs to console.
        /// </summary>
        private static void LogToConsole(Severity severity, string message)
        {

            Console.ForegroundColor = severity switch
            {
                Severity.Fatal => ConsoleColor.DarkRed,
                Severity.Error => ConsoleColor.Red,
                Severity.Warning => ConsoleColor.Yellow,
                Severity.Info => ConsoleColor.White,
                Severity.Debug => ConsoleColor.Gray,
                _ => throw new ArgumentException($"Severity {severity} is unknown to logger.")
            };

            Console.WriteLine(message);
            Console.ForegroundColor = ConsoleColor.White;
        }


        /// <summary>
        /// Logs to file.<br/>
        /// Also creates a stream writer to the log file if needed.
        /// </summary>
        /// <param name="message"></param>
        private static void LogToFile(string message)
        {
            // Create new log file for this session if not done already.
            if (_currentLogFileStream == null) { CreateLogStream(); }

            _currentLogFileStream?.WriteLine(message);
            _currentLogFileStream?.Flush();
        }


        /// <summary>
        /// Creates the stream writer to the log file.
        /// </summary>
        private static void CreateLogStream()
        {
            if (!Directory.Exists(Configuration.LogFolder))
            {
                Directory.CreateDirectory(Configuration.LogFolder);
            }

            _currentLogFileStream = new StreamWriter($"{Configuration.LogFolder}/{DateTime.Now:yyyyMMdd'T'HHmmss}.log");
        }


        /// <summary>
        /// Closes the current log file stream.<br/>
        /// This should be called whenever the game is closed.
        /// </summary>
        public static void CloseCurrenLogFileStream()
        {
            if (_currentLogFileStream != null)
            {
                Log(Severity.Info, Tag.Unloading, "Closing LogFile Stream");
                _currentLogFileStream.Close();
            }
        }
    }
}
