using LegendsSimTest.Knowledge;
using LegendsSimTest.Knowledge.Tags;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Items {
	public class ConsumableItem : ItemBase, IDescriptor {
		public IEnumerable<ITag> getTags() {
			return new List<ITag>() {
				new Consumable(),
			};
		}
	}
}
