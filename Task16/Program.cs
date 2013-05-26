using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task16
{
    /*
Создать на языке C# обобщенный (generic-) класс DynamicList<T>, который:
- реализует динамический массив с помощью обычного массива T[];
- имеет свойство Count, показывающее количество элементов; 
- имеет свойство Items для доступа к элементам по индексу; 
- имеет методы Add, Remove, RemoveAt, Clear для соответственно добавления, удаления, удаления по индексу и удаления всех элементов;
- реализует интерфейс IEnumerable<T>.
Реализовать простейший пример использования класса DynamicList<T> на языке C#.
     */
    class Program
    {
        static void Main(string[] args)
        {
            DynamicList<int> test = new DynamicList<int>();
            for (int i = 0; i < 1000; i++)
            {
                test.Add(i * i);
            }
            for (int i = 0; i < 1000; i++)
            {
                Console.Write(test[i] + "\t");
            }
            Console.ReadLine();
        }
    }
}
