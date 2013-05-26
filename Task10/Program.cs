using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task10
{
    /*
Создать на языке C# статический метод класса Parallel.WaitAll, который: 
- принимает в параметрах массив делегатов;
- выполняет все указанные делегаты параллельно с помощью пула потоков;
- дожидается окончания выполнения всех делегатов.
Реализовать простейший пример использования метода Parallel.WaitAll.

     */
    class Program
    {
        static void Main(string[] args)
        {
            for (int i = 0; i < 100; i++)
            {
                ParallelsExtensions.WaitAll(() =>
                    {
                        Thread.Sleep(i);
                        Console.WriteLine("i -> " + i + " thread -> " + Thread.CurrentThread.ManagedThreadId);
                    });
            }

            Console.ReadLine();
        }
    }

    public static class ParallelsExtensions
    {
        /// <summary>
        /// Executes a set of methods in parallel and returns the results
        /// from each in an array when all threads have completed.  The methods
        /// must take no parameters and have no return value.
        /// </summary>
        /// <param name="m"></param>
        /// <param name="actions"></param>
        /// <returns></returns>
        public static void WaitAll(params Action[] actions)
        {
            // Initialize the reset events to keep track of completed threads
            var resetEvents = new ManualResetEvent[actions.Length];

            // Launch each method in it's own thread
            for (int i = 0; i < actions.Length; i++)
            {
                resetEvents[i] = new ManualResetEvent(false);
                ThreadPool.QueueUserWorkItem(index =>
                    {
                        var actionIndex = (int)index;

                        // Execute the method
                        actions[actionIndex]();

                        // Tell the calling thread that we're done
                        resetEvents[actionIndex].Set();
                    }, i);
            }

            // Wait for all threads to execute
            WaitHandle.WaitAll(resetEvents);
        }
    }
}
