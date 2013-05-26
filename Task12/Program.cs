using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task12
{
    /*
Создать на языке C# статический метод Where статического класса StringListEnumerator, который: 
- имеет вид:
public static IEnumerable<string> Where(this IEnumerable<string> source, Func<string, bool> predicate);
- реализует шаблон Enumerator и возвращает только те элементы переданного виртуального списка source, для которых делегат predicate возвращает значение true.
Реализовать простейший пример использования метода StringListEnumerator.Where. 
    */

    class Program
    {
        static void Main(string[] args)
        {
            List<int> list = new List<int>();
            const int max = 100;
            var rnd = new Random(max / 10);
            int rn = 0;
            for (int i = 1; i < max; i++)
            {
                rn = rnd.Next(i);
                list.Add(rn);
            }

            var enumerable = EnumerableExtension.Where(list, x => x < 20);

            foreach (var i in enumerable)
            {
                Console.WriteLine(i);
            }

            Console.ReadLine();
        }
    }

    internal static class EnumerableExtension
    {
        public static IEnumerable<T> Where<T>(this IEnumerable<T> source, Func<T, bool> predicate)
        {
            foreach (var item in source)
                if (predicate(item))
                    yield return item;
        }
    }
}
