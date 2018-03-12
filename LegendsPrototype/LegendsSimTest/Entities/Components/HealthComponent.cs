using SFMLEngine;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class HealthComponent : Component {
		private int health = 1;
		private int maxHealth = 1;

		public EntityEvent onHealthReachesZero;
		public EntityEvent onHealthReachesMax;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
		}

		public void addHealth(int diff) {
			if (health == 0)
				return;

			if (diff > 0 && health == maxHealth)
				return;

			this.health += diff;
			if(health < 0) {
				health = 0;
				onHealthReachesZero?.Invoke(new EntityEventArgs() {
					component = this,
					entity = entity,
				});
			}

			if(health > maxHealth) {
				health = maxHealth;
				onHealthReachesMax?.Invoke(new EntityEventArgs() {
					component = this,
					entity = entity,
				});
			}
		}

		public void setMaxHealth(int max) {
			this.maxHealth = max;
		}

		public void setHealth(int health) {
			this.health = health;
		}

		public int getMaxHealth() {
			return maxHealth;
		}

		public int getHealth() {
			return health;
		}
	}
}
