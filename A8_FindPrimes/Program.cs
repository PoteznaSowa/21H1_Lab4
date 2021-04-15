using System;
using System.Collections.Generic;
using System.Threading;

namespace A8_FindPrimes {
	class Program {
		static void Main() {
			Console.Title = "FindPrimes";

			const int range_offset = 20000;
			int cpus = Environment.ProcessorCount;
			Console.WriteLine($"Пошук простих чисел у дiапазонi [0; {range_offset * cpus})...");

			ranges = new (List<int>, Thread)[cpus];
			int range = 0;
			for(int i = 0;i < cpus;i++) {
				ranges[i].range = new List<int>();
				ranges[i].thread = new Thread(FindPrimes) {
					Priority = ThreadPriority.Lowest
				};
				ranges[i].thread.Start(new PrimeFinder(ranges[i].range, range, range + range_offset));
				range += range_offset;
			}

			int count = 0;
			foreach((var list, var thread) in ranges) {
				thread.Join();  // Чекати, поки потік не завершить свою роботу.
				count += list.Count;
				foreach(int i in list)
					Console.Write($"{i},");
			}
			Console.WriteLine();
			Console.WriteLine($"Всього було знайдено {count} простих чисел.");

			while(Console.KeyAvailable)
				Console.ReadKey();
			Console.WriteLine("Роботу програми завершено. Натиснiть Return, щоб продовжити...");
			while(Console.ReadKey(true).Key != ConsoleKey.Enter) { }
		}

		static (List<int> range, Thread thread)[] ranges;

		static int GcdOf(int x, int y) {
			if(x < 1)
				throw new ArgumentOutOfRangeException(nameof(x));
			if(y < 1)
				throw new ArgumentOutOfRangeException(nameof(x));
			while(x != y) {
				if(x < y)
					y -= x;
				else
					x -= y;
			}
			return x;
		}

		static void FindPrimes(object input) {
			PrimeFinder finder = input as PrimeFinder;
			int begin = finder.RangeBegin;
			int end = finder.RangeEnd;
			for(int i = begin;i < end;i++) {
				if(IsPrime(i))
					finder.List.Add(i);
			}
		}
		static bool IsPrime(int number) {
			if(number == int.MinValue)
				return false;
			number = Math.Abs(number);
			if(number < 2)
				return false;
			if(number == 2)
				return true;
			int bound = (int)Math.Sqrt(number);
			int i = 1;
			foreach((var list, var thread) in ranges) {
				if(Thread.CurrentThread.ManagedThreadId != thread.ManagedThreadId && thread.IsAlive)
					break;
				foreach(int prime in list) {
					i = prime;
					if(i > bound)
						return true;
					if(GcdOf(i, number) != 1)
						return false;
				}
			}
			do {
				if(GcdOf(++i, number) != 1)
					return false;
			} while(i <= bound);
			return true;
		}
	}
}
