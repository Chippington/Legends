using SFML.System;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry.Nodes {
	public class EntityNode : Node {
		public virtual IEntity entity { get; private set; }
		public virtual Vector2f position { get; protected set; }

		public EntityNode(IEntity entity) : base(null) {
			this.entity = entity;
			var pos = entity.components.Get<PositionComponent>();
			if (pos != null) position = new Vector2f(pos.x, pos.y);
		}

		public EntityNode(EntityNode previous) : base(previous) {
			this.entity = previous.entity;
		}
	}
}
