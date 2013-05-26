using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task04
{
    class Program
    {
        static void Main(string[] args)
        {
            LoggerTester loggerTester = new LoggerTester(LogWriter.Instance);

            loggerTester.Test(2, 19);

        }
    }

    public class LoggerTester
    {
        private readonly LogWriter _logger;
        private readonly List<Thread> _threads;
        private int _messagesCount;

        public LoggerTester(LogWriter logger)
        {
            _logger = logger;
            _threads = new List<Thread>();
        }

        public void Test(int threadCount, int messagesCount)
        {
            _messagesCount = messagesCount;
            for (int i = 0; i < threadCount; i++)
            {
                var thread = new Thread(Routine);
                _threads.Add(thread);
            }
            foreach (Thread t in _threads)
                t.Start();
            foreach (Thread t in _threads)
                t.Join();
        }

        private void Routine(object o)
        {
            for (int i = 0; i < _messagesCount; i++)
            {
                _logger.Write(string.Format("Thread: {0} Routine: {1} DateTime.Now.Millisecond: {2}",
                                                 Thread.CurrentThread.ManagedThreadId, i, DateTime.Now.Millisecond));
            }
        }
    }
}
