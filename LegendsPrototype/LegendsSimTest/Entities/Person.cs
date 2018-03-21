using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LegendsSimTest.Entities.Components;
using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Entities.Items;
using LegendsSimTest.Knowledge;
using SFML.Graphics;
using SFMLEngine;
using SFMLEngine.Entities.Components.Common;
using static LegendsSimTest.Entities.Intents.Intent;

namespace LegendsSimTest.Entities {

	public class Person : LivingEntity {
		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			components.Add<PersonComponent>();
			health.setMaxHealth(100);
			health.setHealth(10);
		}

		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
		}

		private CircleShape shape;
		public override void onDraw(GameContext context) {
			base.onDraw(context);
			if(shape == null) {
				shape = new CircleShape(5f);
				shape.FillColor = new Color(0, 125, 125);
			}

			shape.Position = new SFML.System.Vector2f(position.x, position.y);
			context.window.Draw(shape);
		}
	}
}
