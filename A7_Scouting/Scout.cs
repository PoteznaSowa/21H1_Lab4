using System;

namespace A7_Scouting {
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
}
