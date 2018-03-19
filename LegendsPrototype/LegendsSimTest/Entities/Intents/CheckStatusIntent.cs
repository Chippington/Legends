using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class CheckStatusIntent : Intent {
		public class CheckStatusTask : CheckStatusIntent.Task<CheckStatusResult> {
			public override string ToString() {
				return "Check Status Task";
			}
		}

		public class CheckStatusResult : Intent.Result {
			public int? health;
			public double? hunger;

			public override string ToString() {
				return string.Format("Check Status Result (Health: {0}, Hunger: {1})", health, hunger);
			}
		}

		protected CheckStatusTask task;
		protected CheckStatusResult result;

		public CheckStatusIntent() {
			task = new CheckStatusTask();
			task.onComplete += () => complete();
		}

		public override void complete() {
			result = task.result;
			base.complete();
		}

		public CheckStatusResult getResult() {
			return result;
		}

		public override ITask getTask() {
			return task;
		}
	}
}
