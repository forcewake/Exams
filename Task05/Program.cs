using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task05
{
	class Program
	{
		//
		// Maximum number of events that can be used by 
		// WaitHandle.WaitAll() method equals 64 :(
		//
		private const int _taskCount = 63;

		public static void Main(string[] args)
		{
			ManualResetEvent[] doneEvents = new ManualResetEvent[_taskCount];

			for (int i = 0; i < _taskCount; i++)
			{
				doneEvents[i] = new ManualResetEvent(false);

				ThreadPool.QueueUserWorkItem(state =>
					{
						CustomMutex.Lock();

						int index = (int)state;
						Console.WriteLine(index);
						doneEvents[index].Set();

						CustomMutex.UnLock();
					}, i);
			}

			WaitHandle.WaitAll(doneEvents);

			Console.ReadKey();
		}
	}

	public class CustomMutex
	{
		//
		// _mutexState:
		// 	1 - mutex is locked
		//   0 - mutex is free
		//
		private static int _mutexState;

		static CustomMutex()
		{
			_mutexState = 0;
		}

		public static void Lock()
		{
			while (Interlocked.Equals(_mutexState, 1))
			{ }
			Interlocked.Exchange(ref _mutexState, 1);
		}

		public static void UnLock()
		{
			Interlocked.Exchange(ref _mutexState, 0);
		}
	}
}
