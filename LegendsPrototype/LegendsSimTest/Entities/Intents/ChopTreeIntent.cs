using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public class ChopTreeIntent : Intent {
		public class ChopTreeTask : Task<ChopTreeResult> {
			public Tree target;
			public ChopTreeTask(Tree target) {
				this.target = target;
			}
		}

		public class ChopTreeResult : Result { }

		protected MoveIntent move;
		protected ChopTreeTask task;

		public ChopTreeIntent(Tree tree) {
			move = new MoveIntent(tree.position.x, tree.position.y);
			move.onComplete += () => {
				move = null;
			};

			task = new ChopTreeTask(tree);
			task.onComplete += () => complete();
		}

		public override ITask getTask() {
			if (move != null) return move.getTask();
			return task;
		}
	}
}
