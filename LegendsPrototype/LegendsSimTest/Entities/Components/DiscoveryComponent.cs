using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Entities.Items;
using LegendsSimTest.Knowledge;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using SFMLEngine.Entities.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class DiscoveryComponent : Component {
		private InventoryComponent inventory;
		private PositionComponent position;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);

			inventory = entity.components.Get<InventoryComponent>();
			position = entity.components.Get<PositionComponent>();

			var p = entity as Person;
			if(p != null) {
				p.addTaskCallback<SearchInventoryIntent.SearchInventoryByTypeTask>(cbSearchInventoryByTypeTask);
				p.addTaskCallback<SearchInventoryIntent.SearchInventoryByTagTask>(cbSearchInventoryByTagTask);
				p.addTaskCallback<SearchFieldIntent.SearchFieldByTypeTask>(cbSearchFieldByTypeTask);
				p.addTaskCallback<SearchFieldIntent.SearchFieldByTagTask>(cbSearchFieldByTagTask);
			}
		}

		private void cbSearchFieldByTagTask(SearchFieldIntent.SearchFieldByTagTask obj) {
			var searchTypes = obj.searchTags.Select(i => i.GetType());

			var entities = entity.getScene().getEntityList()
				.Where(i => i as IDescriptor != null && (i as IDescriptor).getTags().Select(ii => ii.GetType()).Intersect(searchTypes).Any())
				.Where(i => i as ItemBase == null || (i as ItemBase).container == null);

			entities = entities.OrderBy(i => {
				var c = i.components.Get<PositionComponent>();
				if (c == null) return double.MaxValue;

				return ((position.x - c.x) * (position.x - c.x)) + ((position.y - c.y) * (position.y - c.y));
			});

			obj.complete(new SearchFieldIntent.SearchFieldResult() {
				results = entities.ToList(),
			});
		}

		private void cbSearchFieldByTypeTask(SearchFieldIntent.SearchFieldByTypeTask obj) {
			obj.complete(new SearchFieldIntent.SearchFieldResult() {
				results = entity.getScene().getEntityList().Where(i => obj.searchTypes.Contains(i.GetType())).ToList(),
			});
		}

		private void cbSearchInventoryByTypeTask(SearchInventoryIntent.SearchInventoryByTypeTask obj) {
			obj.complete(new SearchInventoryIntent.SearchInventoryResult() {
				items = inventory.getItems().Where(i => obj.searchTypes.Contains(i.GetType())).ToList(),
			});
		}

		private void cbSearchInventoryByTagTask(SearchInventoryIntent.SearchInventoryByTagTask obj) {
			var searchTypes = obj.searchTags.Select(i => i.GetType());

			var items = inventory.getItems()
				.Where(i => i as IDescriptor != null && (i as IDescriptor).getTags().Select(ii => ii.GetType()).Intersect(searchTypes).Any());

			obj.complete(new SearchInventoryIntent.SearchInventoryResult() {
				items = items.ToList(),
			});
		}
	}
}
