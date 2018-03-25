using LegendsSimTest.Entities.Components;
using LegendsSimTest.Knowledge;
using SFMLEngine;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components.Common;
using SFMLEngine.Entities.Components.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities {
	public class LivingEntity : Entity, IDescriptor {
		public HealthComponent health { get; private set; }
		public PositionComponent position { get; private set; }
		public RigidBody rigidBody { get; private set; }

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);

			this.position = components.Add<PositionComponent>();
			this.health = components.Add<HealthComponent>();
			this.rigidBody = components.Add<RigidBody>();
		}

		public bool isAlive() {
			return health.getHealth() > 0;
		}

		public void kill() {
			health.setHealth(0);
			onDeath();
		}

		public virtual void onDeath() { }

		public virtual IEnumerable<ITag> getTags() {
			List<ITag> tags = new List<ITag>();
			var comps = components.Values.Select(i => i as IDescriptor).Where(i => i != null);
			foreach (var comp in comps)
				tags.AddRange(comp.getTags());

			return tags.GroupBy(i => i.GetType()).Select(i => i.First());
		}
	}
}
