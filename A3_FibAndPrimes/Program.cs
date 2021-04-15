using System;
using System.IO;
using System.Threading;

namespace A3_FibAndPrimes {
	static class Program {
		static void Main() {
			Console.Title = "Fibonacci and Primes";

			Thread fib = new(GenerateFibonacci);
			Thread prime = new(GeneratePrimes);
			fib.Start();
			prime.Start();
			bool fib_checked = false;
			bool prime_checked = false;
			do {
				if(fib.Join(1) && !fib_checked) {
					fib_checked = true;
					PrintFibonacci();
				}
				if(prime.Join(1) && !prime_checked) {
					prime_checked = true;
					PrintPrimes();
				}
			} while(!fib_checked || !prime_checked);

			while(Console.KeyAvailable)
				Console.ReadKey();
			Console.WriteLine("Роботу програми завершено. Натиснiть Return, щоб продовжити...");
			while(Console.ReadKey(true).Key != ConsoleKey.Enter) { }
		}
		static void PrintFibonacci() {
			Console.WriteLine("Список чисел Фiбоначчi в дiапазонi [0; 2^64):");
			StreamReader reader = File.OpenText("FIB.TXT");
			int count = 0;
			for(;;){
				string s = reader.ReadLine();
				if(s == null) break;
				count++;
				Console.WriteLine(s);
			}
			Console.WriteLine($"Всього {count} чисел Фiбоначчi.");
			Console.WriteLine();
			reader.Close();
			reader.Dispose();
		}
		static void PrintPrimes() {
			Console.WriteLine("Список простих чисел у дiапазонi [0; 2^16):");
			BinaryReader reader = new(File.OpenRead("PRIMES.DAT"));
			int count = 0;
			for(;;){
				try {
					Console.Write(reader.ReadUInt16());
					Console.Write(',');
					count++;
				} catch {
					break;
				}
			}
			Console.WriteLine();
			Console.WriteLine($"Всього {count} простих чисел.");
			Console.WriteLine();
			reader.Close();
			reader.Dispose();
		}

		static void GenerateFibonacci() {
			StreamWriter writer = new(File.Create("FIB.TXT"));
			writer.WriteLine(0);
			writer.WriteLine(1);
			ulong a;
			ulong b = 0;
			ulong c = 1;
			for(;;){
				a = b;
				b = c;
				try {
					// Перевіряти обчислення на переповнення.
					c = checked(a + b);
					writer.WriteLine(c);
				} catch {
					// Переповнення — завершити цикл.
					break;
				}
			}
			writer.Flush();
			writer.Close();
			writer.Dispose();
		}
		static void GeneratePrimes() {
			BinaryWriter writer = new(File.Create("PRIMES.DAT"));
			writer.Write((ushort)2);
			for(int i = 3;i <= ushort.MaxValue;i += 2)
				if(i.IsPrime())
					writer.Write((ushort)i);
			writer.Flush();
			writer.Close();
			writer.Dispose();
		}
		static bool IsPrime(this int number) {
			if(number == int.MinValue)
				return false;
			number = Math.Abs(number);
			if(number is 0 or 1)
				return false;  // Числа 0 та 1 не є простими.
			int bound = (int)Math.Sqrt(number);
			for(int i = 2;i <= bound;i++)
				if(number % i == 0)
					return false;
			return true;
		}
	}
}
