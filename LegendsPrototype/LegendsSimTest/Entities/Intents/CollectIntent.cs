using LegendsSimTest.Entities.Items;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class CollectIntent : Intent {
		public class CollectTask : Task<CollectResult> {
			public ItemBase target;
			public CollectTask(ItemBase target) {
				this.target = target;
			}
		}

		public class ClaimTask : Task<ClaimResult> {
			public ItemBase target;
			public ClaimTask(ItemBase target) {
				this.target = target;
			}
		}

		public class ClaimResult : Result {
			public ItemBase target;
			public ClaimResult(ItemBase target) {
				this.target = target;
			}
		}

		public class CollectResult : Result {
			public ItemBase item;
			public CollectResult(ItemBase item) {
				this.item = item;
			}

			public override string ToString() {
				return string.Format("Collect Result (Success: {0})", item == null ? "False" : "True");
			}
		}

		protected ClaimTask claim;
		protected MoveIntent move;
		protected CollectTask task;

		public CollectIntent(ItemBase item) {
			claim = new ClaimTask(item);
			claim.onComplete += () => {
				if (claim.result == null) complete();
				if (claim.result.target == null) complete();

				claim = null;
			};

			move = new MoveIntent(item.position.x, item.position.y);
			move.onComplete += () => {
				move = null;
			};

			task = new CollectTask(item);
			task.onComplete += () => complete();
		}

		public override ITask getTask() {
			if (claim != null) return claim;
			if (move != null) return move.getTask();
			return task;
		}
	}
}
