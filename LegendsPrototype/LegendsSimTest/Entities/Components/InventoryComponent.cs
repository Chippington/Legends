using LegendsSimTest.Entities.Items;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class InventoryComponent : Component {
		private List<ItemBase> items;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			items = new List<ItemBase>();
		}

		public void addItem(ItemBase item) {
			items.Add(item);
		}

		public void removeItem(ItemBase item) {
			items.Remove(item);
		}

		public List<ItemBase> getItems() {
			return items;
		}

		public List<T> getItems<T>() where T : ItemBase {
			List<T> ret = new List<T>();
			for(int i = 0; i < items.Count; i++) {
				if (items[i].GetType() == typeof(T))
					ret.Add((T)items[i]);
			}

			return ret;
		}
	}
}
