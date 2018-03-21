using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class AgeComponent : Component {
		public long birthTime { get; protected set; }
		public TimeSpan age { get {
				return new TimeSpan(Environment.TickCount - birthTime);
			}
		}

		private float curTime;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			birthTime = Environment.TickCount;
		}

		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
			curTime = context.time.seconds;
		}

		public TimeSpan getAge() {
			return this.age;
		}
	}
}
