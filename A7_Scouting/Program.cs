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

		Loop:
			Thread.Sleep(1);
			foreach(Thread thread in threads)
				if(thread.IsAlive)
					goto Loop;

			while(Console.KeyAvailable)
				Console.ReadKey(true);
			Console.WriteLine("Роботу програми завершено. Натиснiть будь-яку клавiшу...");
			Console.ReadKey(true);
		}
	}

	class Map {
		static readonly Random random = new();
		readonly bool[,] _targets;
		public (int X, int Y) ScoutSpawnPoint {
			get;
		}
		public int Width {
			get;
		}
		public int Height {
			get;
		}

		public Map() {
			Width = random.Next(5, 100);
			Height = random.Next(5, 100);
			_targets = new bool[Width, Height];
			ScoutSpawnPoint = (random.Next(1, Width - 1), random.Next(1, Height - 1));
			for(int i = 0;i < random.Next(Width * Height - 1);i++) {
				int x, y;
				do {
					x = random.Next(Width);
					y = random.Next(Height);
				} while(_targets[x, y] || ScoutSpawnPoint == (x, y));
				_targets[x, y] = true;
			}
		}
		public bool this[int x, int y, Direction d] {
			get {
				(int sp_x, int sp_y) = ScoutSpawnPoint;
				return d switch {
					Direction.Right => _targets[sp_x + 1 + x, sp_y + y],
					Direction.Down => _targets[sp_x - y, sp_y + 1 + x],
					Direction.Left => _targets[sp_x - 1 - x, sp_y - y],
					Direction.Up => _targets[sp_x + y, sp_y - 1 - x],
					_ => _targets[x, y]
				};
			}
		}

		// Мапа розмiру 5x5 умовно дiлиться таким чином:
		/*
		 * 33444
		 * 33444
		 * 33#11
		 * 22211
		 * 22211

		 * де 1, 2, 3 i 4 — позначення областi, яку розвiдник N буде розвiдувати,
		 * # — точка появи розвiдника.
		 */

		public (int w, int h) GetBounds(Direction d) {
			(int sp_x, int sp_y) = ScoutSpawnPoint;
			/*
			Console.WriteLine(sp_x);
			Console.WriteLine(sp_y);
			Console.WriteLine(Width);
			Console.WriteLine(Height);
			Console.WriteLine((Width - sp_x - 1, Height - sp_y));
			Console.WriteLine((Height - sp_y - 1, sp_x + 1));
			Console.WriteLine((sp_x, sp_y + 1));
			Console.WriteLine((sp_y, Width - sp_x));
			Console.WriteLine("===");
			*/
			return d switch {
				Direction.Right => (Width - sp_x - 1, Height - sp_y),
				Direction.Down => (Height - sp_y - 1, sp_x),
				Direction.Left => (sp_x, sp_y + 1),
				Direction.Up => (sp_y, Width - sp_x),
				_ => (0, 0),
			};
		}

		public static (int x, int y) GetOffset(Direction direction)
			=> direction switch {
				Direction.Right => (1, 0),
				Direction.Down => (0, 1),
				Direction.Left => (-1, 0),
				Direction.Up => (0, -1),
				_ => throw new ArgumentException(null, nameof(direction))
			};
	}

	class Scout {
		static int _scount = 0;
		readonly int _id;
		readonly Direction _dir;
		readonly Map _map;

		public Scout(Map map, Direction direction) {
			_dir = direction;
			_id = _scount++;
			_map = map;
		}

		public void FindTargets() {
			(int w, int h) = _map.GetBounds(_dir);
			int count = 0;
			for(int i = 0;i < h;i++) {
				for(int j = 0;j < w;j++) {
					if(_map[j, i, _dir])
						count++;
				}
			}

			Console.WriteLine($"Розвiдник #{_id} знайшов {count} цiлей.");
		}
	}

	enum Direction {
		Right,
		Down,
		Left,
		Up
	}
}
