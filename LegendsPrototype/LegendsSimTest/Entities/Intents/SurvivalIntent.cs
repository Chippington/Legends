using LegendsSimTest.Entities.Items;
using LegendsSimTest.Knowledge;
using LegendsSimTest.Knowledge.Tags;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class SurvivalIntent : Intent {
		protected Intent _currentIntent;
		protected Intent currentIntent {
			get { return _currentIntent; }
			set {
				if(value != null && value.GetType() == typeof(SearchFieldIntent)) {

				}

				_currentIntent = value;
			}
		}
		protected Stopwatch timer;

		public SurvivalIntent() {
			timer = new Stopwatch();
			timer.Start();

			currentIntent = new CheckStatusIntent();
			currentIntent.onComplete += () => {
				
			};

			currentIntent = new CheckStatusIntent();
			currentIntent.onComplete += onCheckStatusComplete;
		}

		private void onCheckStatusComplete() {
			var statusIntent = currentIntent as CheckStatusIntent;
			var status = statusIntent.getResult();

			if (status.hunger > 0) {
				currentIntent = new SearchInventoryIntent(new List<ITag>() { new Consumable() });
				currentIntent.onComplete += onSearchInventoryComplete;
			} else {
				complete();
				return;
			}
		}

		private void onSearchInventoryComplete() {
			var searchInventoryIntent = currentIntent as SearchInventoryIntent;
			var searchInventoryResult = searchInventoryIntent.getResult();

			if (searchInventoryResult.items.Count > 0) {
				currentIntent = new ConsumeIntent((ConsumableItem)searchInventoryResult.items[0]);
				currentIntent.onComplete += () => {
					complete();
					return;
				};
			} else {
				currentIntent = new SearchFieldIntent(new List<ITag>() { new Consumable() });
				currentIntent.onComplete += onSearchFieldComplete;
			}
		}

		private void onSearchFieldComplete() {
			var searchFieldIntent = currentIntent as SearchFieldIntent;
			var searchFieldResult = searchFieldIntent.getResult();
			var filteredResults = searchFieldResult.results.Where(i => i as ConsumableItem != null).Select(i => i as ConsumableItem);

			if (filteredResults.Any()) {
				currentIntent = new CollectIntent(filteredResults.First());
				currentIntent.onComplete += onCollectComplete;
			} else {
				complete();
				return;
			}
		}

		private void onCollectComplete() {
			var result = currentIntent.getTask();
			var collectedItem = (currentIntent.getTask().getResult() as CollectIntent.CollectResult).item;
			if (collectedItem == null) {
				currentIntent = new SearchFieldIntent(new List<ITag>() { new Consumable() });
				currentIntent.onComplete += onSearchFieldComplete;
			} else {
				currentIntent = new ConsumeIntent(collectedItem as ConsumableItem);
				currentIntent.onComplete += () => complete();
			}
		}

		public override double priority { get => timer.Elapsed.TotalSeconds; set => base.priority = value; }

		public override void complete() {
			base.complete();
			currentIntent = null;
		}

		public override ITask getTask() {
			if (currentIntent == null) return null;
			return currentIntent.getTask();
		}

		public override void onIntentActivated() {
			base.onIntentActivated();
			timer.Stop();
		}

		public override void onIntentDeactivated() {
			base.onIntentDeactivated();
			timer.Start();
		}

		public override IEnumerable<ITag> getTags() {
			var tags = new List<ITag>();
			tags.Add(new Investigating());
			tags.AddRange(base.getTags());
			return tags;
		}
	}
}
