using LegendsSimTest.Knowledge.Registry.Nodes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry {
	public class Registry {
		public static Registry Current = new Registry();

		private Dictionary<Type, List<Node>> nodeMap;

		public Registry() {
			nodeMap = new Dictionary<Type, List<Node>>();
		}

		public void registerNode(Node node) {
			if (nodeMap.ContainsKey(node.GetType()) == false)
				nodeMap.Add(node.GetType(), new List<Node>());

			nodeMap[node.GetType()].Add(node);
		}
	}
}
