using SFMLEngine;
using SFMLEngine.Entities.Components;
using SFMLEngine.Entities.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class PersonComponent : Component {
		protected AgeComponent age;
		protected IntentComponent intent;
		protected HealthComponent health;
		protected HungerComponent hunger;
		protected MovementComponent movement;
		protected PositionComponent position;
		protected DiscoveryComponent discovery;
		protected InventoryComponent inventory;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			intent = entity.components.Add<IntentComponent>();
			discovery = entity.components.Add<DiscoveryComponent>();
			inventory = entity.components.Add<InventoryComponent>();
			position = entity.components.Add<PositionComponent>();
			movement = entity.components.Add<MovementComponent>();
			health = entity.components.Add<HealthComponent>();
			hunger = entity.components.Add<HungerComponent>();
			age = entity.components.Add<AgeComponent>();
		}
	}
}
