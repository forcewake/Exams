using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Task11
{
    /*
Создать на языке C# пользовательский атрибут с именем ExportClass, применимый только к классам, и реализовать консольную программу, которая: 
- принимает в параметре командной строки путь к сборке .NET (EXE- или DLL-файлу);
- загружает указанную сборку в память;
- выводит на экран полные имена всех public-типов данных этой сборки, помеченные атрибутом ExportClass.
     */

    internal class Program
    {
        private static void Main(string[] args)
        {
            Assembly a = Assembly.Load("Mscorlib.dll");

            var userType = typeof (ObsoleteAttribute);

            List<Type> types = a.GetTypesWithHelpAttribute(userType).ToList();
            foreach (var type in types)
            {
                Console.WriteLine("Type is {0}", type);
            }
            Console.WriteLine("{0} types found", types.Count());
            Console.ReadLine();
        }
    }

    public static class ReflectionExtensions
    {
        public static IEnumerable<Type> GetTypesWithHelpAttribute(this Assembly assembly, Type typeForFind)
        {
            foreach (Type type in assembly.GetTypes())
            {
                if (type.GetCustomAttributes(typeForFind, true).Length > 0)
                {
                    yield return type;
                }
            }

        }
    }
}
