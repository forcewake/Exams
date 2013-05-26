using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Task02
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var pool = new Pool(5))
            {
                var random = new Random();
                Action<int> randomizer = (index =>
                {
                    Console.WriteLine("{0}: Working on index {1}", Thread.CurrentThread.Name, index);
                    Thread.Sleep(random.Next(20, 400));
                    Console.WriteLine("{0}: Ending {1}", Thread.CurrentThread.Name, index);
                });

                for (var i = 0; i < 40; ++i)
                {
                    var i1 = i;
                    pool.QueueTask(() => randomizer(i1));
                }
            }

            Console.ReadLine();
        }
    }
}
