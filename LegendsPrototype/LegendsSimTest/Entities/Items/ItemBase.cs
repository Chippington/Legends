using LegendsSimTest.Entities.Components;
using SFML.Graphics;
using SFMLEngine;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Items {
	public class ItemBase : Entity {
		public PositionComponent position;
		public IEntity claimedBy;

		protected InventoryComponent _container;
		public InventoryComponent container {
			get {
				return _container;
			}

			set {
				if (value == null) position = components.Get<PositionComponent>();
				else position = value.getEntity().components.Get<PositionComponent>();

				_container = value;
			}
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			position = components.Add<PositionComponent>();
		}

		RectangleShape shape;
		public override void onDraw(GameContext context) {
			base.onDraw(context);
			if (container != null) return;

			if (shape == null) {
				shape = new RectangleShape(new SFML.System.Vector2f(3f, 3f));
				shape.FillColor = new Color(125, 125, 0);
			}

			shape.Position = new SFML.System.Vector2f(position.x, position.y);
			context.window.Draw(shape);
		}

		public override void onDestroy() {
			if (_container != null)
				_container.removeItem(this);

			base.onDestroy();
		}

		public void claim(IEntity claimer) {
			claimedBy = claimer;
		}

		public void free() {
			claimedBy = null;
		}
	}
}
