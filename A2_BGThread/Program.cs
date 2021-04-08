using System;
using System.Threading;

namespace A2_BGThread {
	class Program {
		static void Main() {
			Console.Title = "Background Thread";

			double x = (double)new Random().Next() / int.MaxValue * (2 * Math.PI);
			Console.WriteLine($"x= {x}");
			Console.WriteLine("W= 1+sin(x)-2*cos(x)+4*sin2(x)-8*cos2(x)+...-...=");
			Console.Write("= 1");

			Console.CancelKeyPress += CtrlBrec;
			ThreadPool.QueueUserWorkItem(Calculate, x, true);
			Console.ReadKey(true);
			if(!_exit) {
				_exit = true;
				lock(_con) Console.WriteLine();
			}
		}

		static void CtrlBrec(object sender, ConsoleCancelEventArgs e) {
			_exit = true;
			lock(_con) Console.WriteLine();
		}

		static readonly object _con = new();
		static volatile bool _exit = false;

		static void Calculate(double x) {
			double result = 1;
			for(int i = 0;;i++) {
				if((i & 1) == 0)
					result += (1 << i) * Math.Sin(x);
				else
					result -= (1 << i) * Math.Cos(x);
				if(_exit) return;
				lock(_con) {
					Console.CursorLeft = 2;
					Console.Write($"{result,-21}");
				}
				Thread.Sleep(17);
			}
		}
	}
}
