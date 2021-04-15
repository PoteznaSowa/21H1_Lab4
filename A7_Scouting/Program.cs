using System;
using System.Collections.Generic;
using System.Threading;

namespace A7_Scouting {
	class Program {
		static void Main() {
			Console.Title = "Scouting";

			Map map = new();
			Console.WriteLine($"Створюється мапа розмiром {map.Width} на {map.Height}.");
			Console.WriteLine("Кiлькiсть цiлей невiдома, вiдправляємо розвiдникiв...");

			List<Thread> threads = new();
			foreach(Direction d in Enum.GetValues<Direction>()) {
				threads.Add(new Thread(new Scout(map, d).FindTargets));
				threads[^1].Start();
			}

			foreach(Thread t in threads)
				t.Join();

			while(Console.KeyAvailable)
				Console.ReadKey();
			Console.WriteLine("Роботу програми завершено. Натиснiть Return, щоб продовжити...");
			while(Console.ReadKey(true).Key != ConsoleKey.Enter) { }
		}
	}
}
