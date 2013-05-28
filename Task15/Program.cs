using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Task15
{
	/*
	 Создать на языке C# статический метод Sum статического класса ArrayHelper, который: 
 - является unsafe-методом;
 - суммирует все элементы целочисленного массива без проверки на выход за границы диапазона массива;
 - в случае возникновения переполнения создает исключение класса OverflowException.
 Реализовать простейший пример использования статического метода Sum на языке C#.
	 */
	class Program
	{
		static void Main(string[] args)
		{
			int sum = 0;
			int[] arr = new int[1000000];
			InitArray(arr);

			try
			{
				sum = ArrayHelper.Sum(arr);
			}
			catch (OverflowException exc)
			{
				Console.WriteLine(exc.Message);
			}

			Console.WriteLine(sum);
			Console.ReadKey();
		}

		public static void InitArray(int[] arr)
		{
			for (int i = 0; i < arr.Length; i++)
			{
				arr[i] = i;
			}
		}
	}

	public static class ArrayHelper
	{
		public unsafe static int Sum(int[] array)
		{
			int sum = 0;

			fixed (int* arrayPointer = array)
			{
				for (int i = 0; i < array.Length; i++)
				{
					checked
					{
						sum += *(arrayPointer + i);
					}
				}
			}

			return sum;
		}
	}
}
