using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task04
{
    /// <summary>
    /// A Logging class implementing the Singleton pattern and an internal Queue to be flushed perdiodically
    /// </summary>
    public class LogWriter
    {
        private static LogWriter _instance;
        private static ConcurrentQueue<Log> _logQueue;
        private const string LogDir = @"D:\";
        private const string LogFile = "log.txt";
        private const int MaxLogAge = 60;
        private const int QueueSize = 10;
        private static DateTime _lastFlushed = DateTime.Now;

        private static readonly Lazy<LogWriter> Lazy = new Lazy<LogWriter>(() => new LogWriter());
        private readonly object _locker;
        private string _logPath;

        /// <summary>
        /// Private constructor to prevent instance creation
        /// </summary>
        private LogWriter()
        {
            _logQueue = new ConcurrentQueue<Log>();
            _locker = new object();
            _logPath = LogDir + LogFile;
        }

        /// <summary>
        /// An LogWriter instance that exposes a single instance
        /// </summary>
        public static LogWriter Instance
        {
            get { return Lazy.Value; }
        }

        /// <summary>
        /// The single instance method that writes to the log file
        /// </summary>
        /// <param name="message">The message to write to the log</param>
        public void Write(string message)
        {
            // Create the entry and push to the Queue
            Log logEntry = new Log(message);
            _logQueue.Enqueue(logEntry);


            // If we have reached the Queue Size then flush the Queue
            if (_logQueue.Count >= QueueSize || DoPeriodicFlush())
            {
                lock (_locker)
                {
                    FlushLog(_logQueue.ToList());     
                    _logQueue = new ConcurrentQueue<Log>();
                }
            }

        }

        private bool DoPeriodicFlush()
        {
            TimeSpan logAge = DateTime.Now - _lastFlushed;
            if (logAge.TotalSeconds >= MaxLogAge)
            {
                _lastFlushed = DateTime.Now;
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Flushes the Queue to the physical log file
        /// </summary>
        private void FlushLog(List<Log> logs)
        {
            using (FileStream fs = File.Open(_logPath, FileMode.Append, FileAccess.Write))
            {
                using (StreamWriter streamWriter = new StreamWriter(fs))
                {
                    streamWriter.Write(string.Join(Environment.NewLine,
                                                   logs.Select(log => string.Format("{0}\t{1}", log.LogTime, log.Message))));
                }
            }
        }
    }
}
