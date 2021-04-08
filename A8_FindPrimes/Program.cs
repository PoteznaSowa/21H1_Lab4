using System;
using System.Collections.Generic;
using System.Threading;

namespace A8_FindPrimes {
	class Program {
		static void Main() {
			int cpus = Environment.ProcessorCount;
			List<int>[] ranges = new List<int>[cpus];
			Thread[] threads = new Thread[cpus];
			int range = 0;
			const int range_offset = 20000;
			for(int i = 0;i < cpus;i++) {
				ranges[i] = new List<int>();
				threads[i] = new Thread(FindPrimes) {
					Priority = ThreadPriority.Lowest
				};
				threads[i].Start(new PrimeFinder(ranges[i], range, range + range_offset));
				range += range_offset;
			}

		Loop:
			Thread.Sleep(1);
			foreach(Thread thread in threads)
				if(thread.IsAlive)
					goto Loop;

			foreach(List<int> list in ranges)
				foreach(int i in list)
					Console.Write($"{i},");
			Console.WriteLine();
			while(Console.KeyAvailable)
				Console.ReadKey(true);
			Console.ReadKey(true);
		}

		static int GcdOf(int x, int y) {
			if(x < 1)
				throw new ArgumentOutOfRangeException(nameof(x));
			if(y < 1)
				throw new ArgumentOutOfRangeException(nameof(y));
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
			number = Math.Abs(number);
			if(number < 2)
				return false;
			if(number == 2)
				return true;
			if((number & 1) == 0)
				return false;
			int bound = (int)Math.Sqrt(number);
			for(int i = 3;i<=bound;i += 2) {
				if(GcdOf(i, number) != 1)
					return false;
			}
			return true;
		}
	}

	class PrimeFinder {
		public IList<int> List {
			get;
		}
		public int RangeBegin {
			get;
		}
		public int RangeEnd {
			get;
		}

		public PrimeFinder(IList<int> list, int rangeBegin, int rangeEnd) {
			List = list;
			RangeBegin = rangeBegin;
			RangeEnd = rangeEnd;
		}
	}
}
