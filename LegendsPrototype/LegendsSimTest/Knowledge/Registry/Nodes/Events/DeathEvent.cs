using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendsSimTest.Entities;

namespace LegendsSimTest.Knowledge.Registry.Nodes.Events {
	public class DeathEvent : LifetimeEvent {
		public DeathEvent(LivingEntity entity) : base(entity) {
		}
	}
}
