using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Knowledge;
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
		protected float vx, vy;
		protected float tx, ty;
		protected float moveSpeed;
		protected bool moving;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			moveSpeed = 5f * WorldTime.timeMultiplier;
			position = entity.components.Get<PositionComponent>();

			var p = (entity as Person).components.Get<IntentComponent>();
			if (p != null) {
				p.addTaskCallback<MoveIntent.MoveToPointTask>(cbMoveToPointTask);
				p.addTaskCallback<MoveIntent.MoveToEntityTask>(cbMoveToEntityTask);
			}
		}

		private void cbMoveToPointTask(MoveIntent.MoveToPointTask obj) {
			moving = true;
			tx = obj.x;
			ty = obj.y;

			float dx = (position.x - tx) * (position.x - tx);
			float dy = (position.y - ty) * (position.y - ty);
			float dist = (float)Math.Sqrt(dx + dy);

			if (dist <= 1f) {
				obj.complete(new MoveIntent.MoveResult(true));
				moving = false;
			}
		}

		private void cbMoveToEntityTask(MoveIntent.MoveToEntityTask obj) {
			moving = true;
			var pos = obj.target.components.Get<PositionComponent>();
			if (pos == null) obj.complete(new MoveIntent.MoveResult(MoveIntent.MoveResult.Reason.NOPATHFOUND));

			tx = pos.x;
			ty = pos.y;

			float dx = (position.x - tx) * (position.x - tx);
			float dy = (position.y - ty) * (position.y - ty);
			float dist = (float)Math.Sqrt(dx + dy);

			if (dist <= 1f) {
				obj.complete(new MoveIntent.MoveResult(true));
				moving = false;
			}
		}

		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
			if (!moving) return;

			float dx = (position.x - tx) * (position.x - tx);
			float dy = (position.y - ty) * (position.y - ty);
			float dist = (float)Math.Sqrt(dx + dy);

			vx = ((tx - position.x) / dist) * moveSpeed;
			vy = ((ty - position.y) / dist) * moveSpeed;

			vx *= (float)context.time.delta;
			vy *= (float)context.time.delta;

			float moveDist = (float)Math.Sqrt((vx * vx) + (vy * vy));
			if(moveDist > dist) {
				position.x = tx;
				position.y = ty;
				return;
			} 

			position.x += (float)(vx * context.time.delta) * moveSpeed;
			position.y += (float)(vy * context.time.delta) * moveSpeed;
		}
	}
}
