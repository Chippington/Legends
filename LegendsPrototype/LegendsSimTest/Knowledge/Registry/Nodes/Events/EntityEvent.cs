using SFMLEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry.Nodes.Events {
	public class EntityEvent : EventNode {
		public IEntity entity { get; protected set; }
		public EntityEvent(IEntity ent) : base() {
			this.entity = ent;
		}
	}
}
