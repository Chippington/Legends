using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class IdleIntent : Intent {
		public class IdleTask : Intent.Task<IdleResult> { }
		public class IdleResult : Intent.Result { }

		protected IdleTask task;

		public IdleIntent() {
			task = new IdleTask();
		}

		public override ITask getTask() {
			return task;
		}
	}
}
