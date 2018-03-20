using LegendsSimTest.Entities.Intents;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using SFMLEngine.Entities.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class MovementComponent : Component {
		public PositionComponent position;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			position = entity.components.Get<PositionComponent>();

			var p = entity as Person;
			if(p != null) {
				p.addTaskCallback<MoveIntent.MoveToPointTask>(cbMoveToPointTask);
				p.addTaskCallback<MoveIntent.MoveToEntityTask>(cbMoveToEntityTask);
			}
		}

		private void cbMoveToPointTask(MoveIntent.MoveToPointTask obj) {
			position.x = obj.x;
			position.y = obj.y;
			obj.complete(new MoveIntent.MoveResult(true));
		}

		private void cbMoveToEntityTask(MoveIntent.MoveToEntityTask obj) {
			var pos = obj.target.components.Get<PositionComponent>();
			if (pos == null) obj.complete(new MoveIntent.MoveResult(MoveIntent.MoveResult.Reason.NOPATHFOUND));

			position.x = pos.x;
			position.y = pos.y;
			obj.complete(new MoveIntent.MoveResult(true));
		}
	}
}
