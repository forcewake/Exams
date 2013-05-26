using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task06
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length != 1)
                throw new Exception("Invalid command line arguments");

            string assemblyPath = args[0];

            Assembly assembly = Assembly.LoadFrom(assemblyPath);
            Type[] types = assembly.GetTypes();

            types = types.OrderBy(x => x.Namespace).ToArray();

            foreach (Type type in types)
            {
                Console.WriteLine(type.FullName);
            }
            Console.ReadLine();
        }
    }
}
