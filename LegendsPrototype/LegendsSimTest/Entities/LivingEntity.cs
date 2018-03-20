using LegendsSimTest.Entities.Components;
using LegendsSimTest.Knowledge;
using SFMLEngine;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities {
	public class LivingEntity : Entity, IDescriptor {
		protected HealthComponent health { get; private set; }
		protected Position position { get; private set; }
		public override void onInitialize(GameContext context) {
			base.onInitialize(context);

			this.position = components.Add<Position>();
			this.health = components.Add<HealthComponent>();
		}

		public bool isAlive() {
			return health.getHealth() > 0;
		}

		public virtual IEnumerable<ITag> getTags() {
			throw new NotImplementedException();
		}
	}
}
