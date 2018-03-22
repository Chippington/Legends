using LegendsSimTest.Entities.Intents;
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
				if (timer == null) return 0d;
				return timer.Elapsed.TotalSeconds - consumed;
			}
		}

		private Stopwatch timer;
		private Intent survivalIntent;

		public HungerComponent(IntentComponent intent) {
			this.intent = intent;
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			timer = new Stopwatch();
			timer.Start();

			if (intent != null) {
				intent.addTaskCallback<ConsumeIntent.ConsumeTask>(this.cbConsumeTask);

				survivalIntent = new SurvivalIntent();
				survivalIntent.onComplete += onSurvivalCheckComplete;
				intent.addIntent(survivalIntent);
			}
		}

		private void cbConsumeTask(ConsumeIntent.ConsumeTask obj) {
			if (obj.time.Elapsed.TotalSeconds < 0.3d) {
				if (obj.consumable.isDestroyed() == false && obj.consumable.container == null)
					obj.complete(new ConsumeIntent.ConsumeResult(ConsumeIntent.ConsumeResult.Reason.ITEMNOTFOUND));

				return;
			}

			subtractHunger(30);
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
