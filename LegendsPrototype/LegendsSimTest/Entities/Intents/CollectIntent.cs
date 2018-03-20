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

		public class CollectResult : Result {
			public ItemBase item;
			public CollectResult(ItemBase item) {
				this.item = item;
			}
		}

		protected MoveIntent move;
		protected CollectTask task;

		public CollectIntent(ItemBase item) {
			move = new MoveIntent(item.position.x, item.position.y);
			move.onComplete += () => {
				move = null;
			};

			task = new CollectTask(item);
			task.onComplete += () => complete();
		}

		public override ITask getTask() {
			if (move != null) return move.getTask();
			return task;
		}
	}
}
