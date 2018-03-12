using LegendsSimTest.Entities.Components;
using SFMLEngine;
using SFMLEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities {
	public class LivingEntity : Entity {
		protected HealthComponent health { get; private set; }

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);

			this.health = components.Add<HealthComponent>();
		}

		public bool isAlive() {
			return health.getHealth() > 0;
		}
	}
}
