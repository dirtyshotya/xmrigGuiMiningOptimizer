using System;
using System.IO;
using System.Windows.Forms;

namespace XmrigLauncher
{
    public static class Logger
    {
        private static string logFilePath;
        private static readonly object lockObject = new object();

        static Logger()
        {
            // Create log file in application directory
            string appDirectory = Application.StartupPath;
            logFilePath = Path.Combine(appDirectory, "xmrig_launcher.log");
            
            // Write startup message
            Log("=== XMRig Launcher Started ===");
            Log($"Log file: {logFilePath}");
            Log($"Time: {DateTime.Now:yyyy-MM-dd HH:mm:ss}");
        }

        public static void Log(string message)
        {
            lock (lockObject)
            {
                try
                {
                    string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss.fff");
                    string logMessage = $"[{timestamp}] {message}";
                    
                    File.AppendAllText(logFilePath, logMessage + Environment.NewLine);
                    
                    // Also show in console if debugging
                    #if DEBUG
                    Console.WriteLine(logMessage);
                    #endif
                }
                catch (Exception ex)
                {
                    // If logging fails, try to write to event log or just ignore
                    #if DEBUG
                    Console.WriteLine($"LOGGER ERROR: {ex.Message}");
                    #endif
                }
            }
        }

        public static void LogError(string message, Exception ex = null)
        {
            string errorMessage = $"ERROR: {message}";
            if (ex != null)
            {
                errorMessage += $"\nException: {ex.Message}\nStack: {ex.StackTrace}";
            }
            Log(errorMessage);
        }

        public static string GetLogFilePath()
        {
            return logFilePath;
        }

        public static void OpenLogFile()
        {
            try
            {
                if (File.Exists(logFilePath))
                {
                    System.Diagnostics.Process.Start("notepad.exe", logFilePath);
                }
                else
                {
                    MessageBox.Show("Log file not found yet. It will be created when the application runs.", 
                                  "Log File", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Could not open log file: {ex.Message}", "Error", 
                              MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}