using System.Collections.Generic;

namespace A8_FindPrimes {
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
