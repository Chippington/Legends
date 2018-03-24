using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge {
	public class WorldTime {
		private static bool first = true;
		private static DateTime origin;
		public static DateTime Now {
			get {
				if (first) {
					origin = DateTime.Now;
					first = false;
				}

				return new DateTime((DateTime.Now - origin).Ticks * timeMultiplier);
			}
		}

		public static int timeMultiplier { get; private set; } = 1000;
	}
}
