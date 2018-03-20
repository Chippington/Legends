using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendsSimTest.Knowledge;
using LegendsSimTest.Knowledge.Tags;

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

		public override IEnumerable<ITag> getTags() {
			var tags = new List<ITag>();
			tags.Add(new Idle());
			tags.AddRange(base.getTags());
			return tags;
		}
	}
}
