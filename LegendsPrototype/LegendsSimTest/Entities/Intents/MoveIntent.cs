using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendsSimTest.Knowledge;
using LegendsSimTest.Knowledge.Tags;

namespace LegendsSimTest.Entities.Intents {
	public class MoveIntent : Intent {
		public class MoveTask : MoveIntent.Task<MoveResult> {
			public float x { get; protected set; }
			public float y { get; protected set; }
			public MoveTask(float x, float y) {
				this.x = x;
				this.y = y;
			}

			public override string ToString() {
				return string.Format("Move Task (Target: [{0},{1}])", x, y);
			}
		}

		public class MoveResult : Intent.Result {
			public enum Reason {
				NOPATHFOUND,
			}

			public MoveResult() { success = true; }
			public MoveResult(bool success) { this.success = success; }
			public MoveResult(Reason failureReason) { this.reason = failureReason; }

			public bool success;
			public Reason reason;

			public override string ToString() {
				return string.Format("Move Result (Success: {0})", success);
			}
		}

		protected MoveTask task;
		protected MoveResult result;

		public MoveIntent(float x, float y) {
			task = new MoveTask(x, y);
			task.onComplete += () => complete();
		}

		public override void complete() {
			result = task.result;
			result.success = true;

			base.complete();
		}

		public override ITask getTask() {
			return task;
		}

		public MoveResult getResult() {
			return result;
		}

		public override IEnumerable<ITag> getTags() {
			var tags = new List<ITag>();
			tags.Add(new Moving());
			tags.AddRange(base.getTags());
			return tags;
		}
	}
}
