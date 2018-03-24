using LegendsSimTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry.Nodes.Events {
	public class LifetimeEvent : EntityEvent {
		public DateTime time { get; protected set; }
		public LivingEntity livingEntity { get; protected set; }

		public LifetimeEvent(LivingEntity entity) : base(entity) {
			this.livingEntity = entity;
		}

		public override void onRegister() {
			base.onRegister();
			time = WorldTime.Now;
		}
	}
}
