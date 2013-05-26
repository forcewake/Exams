using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task07
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            Base test = null;
            for (int i = 0; i < 100; i++)
            {
                test = new MyClass();
            }

            Base bBase = new Base();

            Console.WriteLine(test.Count);
            Console.WriteLine(bBase.Count);

            test = null;

            GC.Collect(3, GCCollectionMode.Forced);

            Console.ReadLine();

            Console.WriteLine(bBase.Count);

            Console.ReadLine();
        }

        public class Base
        {
            private static int _count;

            static Base()
            {
                _count = 0;
            }

            public Base()
            {
                if (typeof(Base) != GetType())
                {
                    Interlocked.Increment(ref _count);
                }
            }

            ~Base()
            {
                if (typeof(Base) != GetType())
                {
                    Interlocked.Decrement(ref _count);
                }
            }

            public int Count
            {
                get
                {
                    lock (_locker)
                    {
                        return _count;
                    }
                }
            }

            private readonly object _locker = new object();
        }

        public class MyClass : Base
        {

        }
    }
}
