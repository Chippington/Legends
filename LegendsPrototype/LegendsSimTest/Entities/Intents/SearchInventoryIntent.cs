using LegendsSimTest.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendsSimTest.Knowledge;
using LegendsSimTest.Knowledge.Tags;

namespace LegendsSimTest.Entities.Intents {
	public class SearchInventoryIntent : Intent {
		public class SearchInventoryByTypeTask : Task<SearchInventoryResult> {
			public List<Type> searchTypes { get; protected set; }
			public SearchInventoryByTypeTask(List<Type> searchTypes) {
				this.searchTypes = searchTypes;
			}

			public override string ToString() {
				string types = "";
				for (int i = 0; i < searchTypes.Count; i++) {
					var type = searchTypes[i].Name;
					types += type + ", ";
				}

				types = types.Trim().Trim(',');
				return string.Format("Search Inventory Task (SearchTypes: [{0}])", types);
			}
		}

		public class SearchInventoryByTagTask : Task<SearchInventoryResult> {
			public List<ITag> searchTags { get; protected set; }
			public SearchInventoryByTagTask(List<ITag> searchTags) {
				this.searchTags = searchTags;
			}

			public override string ToString() {
				string types = "";
				for (int i = 0; i < searchTags.Count; i++) {
					var type = searchTags[i].name;
					types += type + ", ";
				}

				types = types.Trim().Trim(',');
				return string.Format("Search Inventory Task (SearchTags: [{0}])", types);
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

		protected Task<SearchInventoryResult> task;
		protected SearchInventoryResult result;

		public SearchInventoryIntent(List<Type> searchTypes) {
			task = new SearchInventoryByTypeTask(searchTypes);
			task.onComplete += () => complete();
		}

		public SearchInventoryIntent(List<ITag> searchTags) {
			task = new SearchInventoryByTagTask(searchTags);
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

		public override IEnumerable<ITag> getTags() {
			var tags = new List<ITag>();
			tags.Add(new Investigating());
			tags.Add(new Searching());
			tags.AddRange(base.getTags());
			return tags;
		}
	}
}
