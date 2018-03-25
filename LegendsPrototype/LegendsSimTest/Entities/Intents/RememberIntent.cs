using LegendsSimTest.Knowledge.Registry;
using SFMLEngine.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LegendsSimTest.Entities.Components.MemoryComponent;

namespace LegendsSimTest.Entities.Intents {
	public class RememberIntent : Intent {
		public class CommitToMemoryResult : Result {
			public IReference reference { get; protected set; }
			public CommitToMemoryResult(IReference reference) {
				this.reference = reference;
			}
		}

		public class RetrieveFromMemoryResult : Result {
			public Node referenceData { get; protected set; }
			public RetrieveFromMemoryResult(Node data) {
				this.referenceData = data;
			}
		}

		public class CommitToMemoryTask : Task<CommitToMemoryResult> {
			public IEntity entity { get; protected set; }
			public CommitToMemoryTask(IEntity entity) {
				this.entity = entity;
			}
		}

		public class RetrieveFromMemoryTask : Task<RetrieveFromMemoryResult> {
			public IReference reference { get; protected set; }
			public RetrieveFromMemoryTask(IReference reference) {
				this.reference = reference;
			}
		}

		private ITask task;
		private Result result;

		public void commitToMemory(IEntity ent) {
			task = new CommitToMemoryTask(ent);
			task.onComplete += () => {
				this.result = task.getResult();
				complete();
			};
		}

		public void retrieveFromMemory(IReference reference) {
			task = new RetrieveFromMemoryTask(reference);
			task.onComplete += () => {
				this.result = task.getResult();
				complete();
			};
		}

		public override ITask getTask() {
			return task;
		}

		public Result getResult() {
			return result;
		}
	}
}
