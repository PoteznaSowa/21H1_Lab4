using System;

namespace A7_Scouting {
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
}
