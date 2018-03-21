using LegendsSimTest.Entities.Intents;
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

			var p = (entity as Person).components.Get<IntentComponent>();
			if (p != null) {
				p.addTaskCallback<CollectIntent.ClaimTask>(cbClaimTask);
				p.addTaskCallback<CollectIntent.CollectTask>(cbCollectTask);
			}
		}

		private void cbCollectTask(CollectIntent.CollectTask obj) {
			if (obj.target.container != null || (obj.target.claimedBy != null && obj.target.claimedBy != entity) || obj.target.isDestroyed()) {
				obj.complete(new CollectIntent.CollectResult(null));
				return;
			}

			addItem(obj.target);
			obj.complete(new CollectIntent.CollectResult(obj.target));
		}

		private void cbClaimTask(CollectIntent.ClaimTask obj) {
			ItemBase ret = null;
			if (obj.target.claimedBy == null) {
				obj.target.claim(entity);
				ret = obj.target;
			}

			obj.complete(new CollectIntent.ClaimResult(ret));
		}

		public void addItem(ItemBase item) {
			if(item.container != null) {
				item.container.removeItem(item);
			}

			items.Add(item);
			item.container = this;
		}

		public void removeItem(ItemBase item) {
			items.Remove(item);
			item.container = null;
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
