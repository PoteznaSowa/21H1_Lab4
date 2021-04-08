using System;
using System.Threading;

namespace A6_Hurdling {
	class Program {
		static void Main() {
			Console.Title = "Hurdling";

			int rnum = random.Next(5, 21);

			Console.WriteLine($"У змаганнi беруть участь {rnum} бiгунiв.");
			Console.WriteLine($"Довжина траси {_tracklen}. Ймовiрнiсть перешкод 1%.");

			Thread[] runners = new Thread[rnum];

			for(int i = 0;i < rnum;i++) {
				runners[i] = new Thread(Run);
				runners[i].Start(i + 1);
			}
		}

		static readonly Random random = new();
		static readonly int _tracklen = random.Next(100000);
		static readonly object _conlock = new();

		static void Run(object num) {
			const int bound = int.MaxValue / 100;

			for(int i = _tracklen;i != 0;i--) {
				int rnd;
				lock(random)  // Генератор випадкових чисел не є thread-safe.
					rnd = random.Next();
				if(rnd < bound)
					Thread.Sleep(1);
			}
			lock(_conlock)
				Console.WriteLine($"Бiгун #{num} досяг фiнiшу!");
		}
	}
}
