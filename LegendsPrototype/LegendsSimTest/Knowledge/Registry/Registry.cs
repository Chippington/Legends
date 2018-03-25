using LegendsSimTest.Entities;
using LegendsSimTest.Knowledge.Registry.Nodes;
using SFMLEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry {
	public class Registry {
		public static Registry Current = new Registry();

		private Dictionary<Type, List<Node>> nodeMap;
		private Dictionary<IEntity, EntityNode> entityNodeMap;

		public Registry() {
			nodeMap = new Dictionary<Type, List<Node>>();
			entityNodeMap = new Dictionary<IEntity, EntityNode>();
		}

		public void registerNode(Node node) {
			defaultAdd(node);

			var ent = node as EntityNode;
			if(ent != null) {
				entityNodeMap[ent.entity] = ent;
			}
		}

		public EntityNode getLatest(IEntity e) {
			if (entityNodeMap.ContainsKey(e) == false)
				return null;

			return entityNodeMap[e];
		}

		private void defaultAdd(Node node) {
			if (nodeMap.ContainsKey(node.GetType()) == false)
				nodeMap.Add(node.GetType(), new List<Node>());

			nodeMap[node.GetType()].Add(node);
		}
	}
}
