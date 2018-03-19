using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class HungerComponent : Component {
		private double consumed;
		private double hunger {
			get {
				if (timer == null) return 0d;
				return timer.Elapsed.TotalSeconds - consumed;
			}
		}
		private Stopwatch timer;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			timer = new Stopwatch();
			timer.Start();
		}

		public double getHunger() {
			return hunger;
		}

		public void subtractHunger(double val) {
			consumed += val;
		}
	}
}
