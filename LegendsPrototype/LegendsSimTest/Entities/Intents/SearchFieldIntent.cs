using LegendsSimTest.Entities.Items;
using LegendsSimTest.Knowledge;
using LegendsSimTest.Knowledge.Tags;
using SFMLEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class SearchFieldIntent : Intent {
		public class SearchFieldByTypeTask : Task<SearchFieldResult> {
			public List<Type> searchTypes { get; protected set; }
			public SearchFieldByTypeTask(List<Type> searchTypes) {
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

		public class SearchFieldByTagTask : Task<SearchFieldResult> {
			public List<ITag> searchTags { get; protected set; }
			public SearchFieldByTagTask(List<ITag> searchTags) {
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

		public class SearchFieldResult : Result {
			public List<IEntity> results;

			public override string ToString() {
				string types = "";
				for (int i = 0; i < results.Count; i++) {
					var type = results[i].GetType().Name;
					types += type + ", ";
				}

				types = types.Trim().Trim(',');
				return string.Format("Search Inventory Result (Count: {1}, Item Types: [{0}])", types, results.Count);
			}
		}

		protected Task<SearchFieldResult> task;
		protected SearchFieldResult result;

		public SearchFieldIntent(List<Type> searchTypes) {
			task = new SearchFieldByTypeTask(searchTypes);
			task.onComplete += () => complete();
		}

		public SearchFieldIntent(List<ITag> searchTags) {
			task = new SearchFieldByTagTask(searchTags);
			task.onComplete += () => complete();
		}

		public override ITask getTask() {
			return task;
		}

		public override void complete() {
			this.result = task.result;
			base.complete();
		}

		public SearchFieldResult getResult() {
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
