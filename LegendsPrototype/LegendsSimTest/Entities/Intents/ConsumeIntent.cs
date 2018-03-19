using LegendsSimTest.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class ConsumeIntent : Intent {
		public class ConsumeTask : ConsumeIntent.Task<ConsumeResult> {
			public ConsumableItem consumable { get; protected set; }
			public ConsumeTask(ConsumableItem consumable) {
				this.consumable = consumable;
			}
		}

		public class ConsumeResult : Intent.Result {
			public enum Reason {
				ITEMNOTFOUND,
			}

			public bool success;
			public Reason reason;

			public ConsumeResult() { success = true; }
			public ConsumeResult(bool success) { this.success = success; }
			public ConsumeResult(Reason failureReason) { this.reason = failureReason; }
		}

		protected ConsumeTask task;
		protected ConsumeResult result;

		public ConsumeIntent(ConsumableItem consumable) {
			task = new ConsumeTask(consumable);
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

		public ConsumeResult getResult() {
			return result;
		}
	}
}
