﻿using LegendsSimTest.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class SurvivalIntent : Intent {
		protected Intent currentIntent;

		public SurvivalIntent() {
			currentIntent = new CheckStatusIntent();
			currentIntent.onComplete += () => {
				var statusIntent = currentIntent as CheckStatusIntent;
				var status = statusIntent.getResult();

				if(status.health < 50) {
					currentIntent = new SearchInventoryIntent(new List<Type>() { typeof(ConsumableItem) });
					currentIntent.onComplete += () => {
						var searchIntent = currentIntent as SearchInventoryIntent;
						var search = searchIntent.getResult();

						if(search.items.Count > 0) {
							currentIntent = new ConsumeIntent((ConsumableItem)search.items[0]);
							currentIntent.onComplete += () => {
								complete();
								return;
							};
						} else {
							complete();
							return;
						}
					};
				} else {
					complete();
					return;
				}
			};
		}

		public override void complete() {
			base.complete();
			currentIntent = null;
		}

		public override ITask getTask() {
			if (currentIntent == null) return null;
			return currentIntent.getTask();
		}
	}
}