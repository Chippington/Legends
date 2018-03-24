using LegendsSimTest.Knowledge.Registry;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class MemoryComponent : Component {
		public class Reference<T> where T : Node {
			public T node { get; protected set; }

			public Reference(T node) {
				this.node = node;
			}
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
		}
	}
}
