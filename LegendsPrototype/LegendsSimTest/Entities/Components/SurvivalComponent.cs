using LegendsSimTest.Entities.Intents;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class SurvivalComponent : Component {
		private SurvivalIntent survivalIntent;
		public override void onInitialize(GameContext context) {
			base.onInitialize(context);

			var p = entity as Person;
			if(p != null) {
				survivalIntent = new SurvivalIntent();
				survivalIntent.onComplete += onSurvivalCheckComplete;
				p.desires.Add(survivalIntent);
			}
		}

		private void onSurvivalCheckComplete() {
			var p = entity as Person;
			p.desires.Remove(survivalIntent);
			survivalIntent.onComplete = null;

			survivalIntent = new SurvivalIntent();
			p.desires.Add(survivalIntent);
			survivalIntent.onComplete += onSurvivalCheckComplete;
		}
	}
}
