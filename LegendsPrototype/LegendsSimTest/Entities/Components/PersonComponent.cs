using SFMLEngine;
using SFMLEngine.Entities.Components;
using SFMLEngine.Entities.Components.Common;
using SFMLEngine.Entities.Components.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class PersonComponent : Component {
		public AgeComponent age;
		public IntentComponent intent;
		public HealthComponent health;
		public HungerComponent hunger;
		public MovementComponent movement;
		public PositionComponent position;
		public DiscoveryComponent discovery;
		public InventoryComponent inventory;
		public VisionComponent vision;
		public MemoryComponent memory;
		public RigidBody rigidBody;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			position = entity.components.Add<PositionComponent>();
			rigidBody = entity.components.Add<RigidBody>();
			intent = entity.components.Add<IntentComponent>();
			memory = entity.components.Add<MemoryComponent>();
			vision = entity.components.Add<VisionComponent>();
			discovery = entity.components.Add<DiscoveryComponent>();
			inventory = entity.components.Add<InventoryComponent>();
			movement = entity.components.Add<MovementComponent>();
			health = entity.components.Add<HealthComponent>();
			hunger = entity.components.Add<HungerComponent>();
			age = entity.components.Add<AgeComponent>();
		}
	}
}
