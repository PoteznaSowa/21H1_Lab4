using System;
using System.Threading;

namespace A5_SorterThreads {
	class Program {
		static void Main() {
			Console.Title = "SorterThreads";

			Thread t1 = new(AscendingSort);
			Thread t2 = new(DescendingSort);
			t1.Start();
			t2.Start();

			t1.Join();
			t2.Join();

			while(Console.KeyAvailable)
				Console.ReadKey();
			Console.WriteLine("Роботу програми завершено. Натиснiть Return, щоб продовжити...");
			while(Console.ReadKey(true).Key != ConsoleKey.Enter) { }
		}

		static readonly object _conlock = new();

		static void AscendingSort() {
			int[] a = GetRandomInts();
			ShowArray(a, "Вмiст масива 1:");

			for(int i = 0;i < a.Length - 1;i++) {
				int min_idx = i;
				for(int j = i + 1;j < a.Length;j++) {
					if(a[j] < a[min_idx])
						min_idx = j;
				}
				if(min_idx != i) {
					int swap = a[i];
					a[i] = a[min_idx];
					a[min_idx] = swap;
					lock(_conlock)
						Console.WriteLine($"Масив 1: {i} <-> {min_idx}");
					Thread.Sleep(1);
				}
			}
			ShowArray(a, "Масив 1 вiдсортовано:");
		}
		static void DescendingSort() {
			int[] a = GetRandomInts();
			ShowArray(a, "Вмiст масива 2:");

			for(int i = 0;i < a.Length - 1;i++) {
				int max_idx = i;
				for(int j = i + 1;j < a.Length;j++) {
					if(a[j] > a[max_idx])
						max_idx = j;
				}
				if(max_idx != i) {
					int swap = a[i];
					a[i] = a[max_idx];
					a[max_idx] = swap;
					lock(_conlock)
						Console.WriteLine($"Масив 2: {i} <-> {max_idx}");
					Thread.Sleep(1);
				}
			}
			ShowArray(a, "Масив 2 вiдсортовано:");
		}
		static int[] GetRandomInts() {
			Random random = new();
			int[] a = new int[random.Next(2, 31)];
			for(int i = 0;i < a.Length;i++)
				a[i] = random.Next(-99, 100);
			return a;
		}
		static void ShowArray(int[] array, string desc) {
			lock(_conlock) {
				Console.WriteLine(desc);
				foreach(int i in array)
					Console.Write($"{i} ");
				Console.WriteLine();
			}
		}
	}
}
