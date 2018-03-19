using LegendsSimTest.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class SearchInventoryIntent : Intent {
		public class SearchInventoryTask : Task<SearchInventoryResult> {
			public List<Type> searchTypes { get; protected set; }
			public SearchInventoryTask(List<Type> searchTypes) {
				this.searchTypes = searchTypes;
			}

			public override string ToString() {
				string types = "";
				for(int i = 0; i < searchTypes.Count; i++) {
					var type = searchTypes[i].Name;
					types += type + ", ";
				}

				types = types.Trim().Trim(',');
				return string.Format("Search Inventory Task (SearchTypes: [{0}])", types);
			}
		}

		public class SearchInventoryResult : Result {
			public List<ItemBase> items;

			public override string ToString() {
				string types = "";
				for (int i = 0; i < items.Count; i++) {
					var type = items[i].GetType().Name;
					types += type + ", ";
				}

				types = types.Trim().Trim(',');
				return string.Format("Search Inventory Result (Count: {1}, Item Types: [{0}])", types, items.Count);
			}
		}

		protected SearchInventoryTask task;
		protected SearchInventoryResult result;

		public SearchInventoryIntent(List<Type> searchTypes) {
			task = new SearchInventoryTask(searchTypes);
			task.onComplete += () => complete();
		}

		public override ITask getTask() {
			return task;
		}

		public override void complete() {
			this.result = task.result;
			base.complete();
		}

		public SearchInventoryResult getResult() {
			return result;
		}
	}
}
