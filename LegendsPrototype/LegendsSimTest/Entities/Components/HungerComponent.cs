using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Knowledge;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class HungerComponent : Component {
		private IntentComponent intent;
		private double consumed;
		private double hunger {
			get {
				if (startTime == null) return 0d;
				return (WorldTime.Now - startTime).TotalHours - consumed;
			}
		}

		private DateTime startTime;
		private Intent survivalIntent;

		public HungerComponent(IntentComponent intent) {
			this.intent = intent;
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			startTime = WorldTime.Now;

			if (intent != null) {
				intent.addTaskCallback<ConsumeIntent.ConsumeTask>(this.cbConsumeTask);

				survivalIntent = new SurvivalIntent();
				survivalIntent.onComplete += onSurvivalCheckComplete;
				intent.addIntent(survivalIntent);
			}
		}

		private void cbConsumeTask(ConsumeIntent.ConsumeTask obj) {
			if ((WorldTime.Now - obj.startTime).TotalMinutes < 5) {
				if (obj.consumable.isDestroyed() == false && obj.consumable.container == null)
					obj.complete(new ConsumeIntent.ConsumeResult(ConsumeIntent.ConsumeResult.Reason.ITEMNOTFOUND));

				return;
			}

			subtractHunger(6d);
			obj.consumable.destroy();
			obj.complete(new ConsumeIntent.ConsumeResult());
		}

		private void onSurvivalCheckComplete() {
			var p = (entity as Person).components.Get<IntentComponent>();
			p.removeIntent(survivalIntent);
			survivalIntent.onComplete = null;

			survivalIntent = new SurvivalIntent();
			p.addIntent(survivalIntent);
			survivalIntent.onComplete += onSurvivalCheckComplete;
		}

		public double getHunger() {
			return hunger;
		}

		public void subtractHunger(double val) {
			consumed += val;
		}
	}
}
