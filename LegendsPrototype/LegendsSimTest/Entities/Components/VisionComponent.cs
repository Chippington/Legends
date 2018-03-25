using LegendsSimTest.Entities.Items;
using SFML.Graphics;
using SFMLEngine;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components;
using SFMLEngine.Entities.Components.Common;
using SFMLEngine.Entities.Components.Physics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class VisionComponent : Component, ICollider {
		public List<IEntity> nearby;

		private float radius;
		private BoundingBox bounds;
		private PositionComponent position;
		private MemoryComponent memory;

		public VisionComponent(PositionComponent position) {
			this.position = position;
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			nearby = new List<IEntity>();
			radius = 100f;
			bounds = new BoundingBox(-radius, -radius, radius, radius);
			memory = entity.getComponents().Get<MemoryComponent>();
		}

		public BoundingBox getBoundingBox() {
			return bounds;
		}

		public void setVisionRadius(float r) {
			this.radius = r;
			bounds = new BoundingBox(-radius, -radius, radius, radius);
		}

		public void onEnterCollision(ICollider other) {
			if (other as RigidBody == null) return;

			if (other.getEntity() as Person != null)
				memory?.onPersonEnterVisionRange((Person)other.getEntity());

			var otherEnt = other.getEntity();
			if (otherEnt != null) {
				nearby.Add(otherEnt);
			}
		}

		public void onLeaveCollision(ICollider other) {
			if (other as RigidBody == null) return;

			if (other.getEntity() as Person != null)
				memory?.onPersonLeaveVisionRange((Person)other.getEntity());

			var otherEnt = other.getEntity();
			if (otherEnt != null) {
				nearby.Remove(otherEnt);
			}
		}

		public void onStepCollision(ICollider other) {
		}

		public void setIgnoreCallbacks(bool ignore) { }
		public bool getIgnoreCallbacks() {
			return false;
		}

		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
			bounds.x = position.x;
			bounds.y = position.y;
		}

		CircleShape shape;
		public override void onDraw(GameContext context) {
			base.onDraw(context);
			if (shape == null) {
				shape = new CircleShape(radius);
				shape.FillColor = new Color(0, 0, 0, 0);
				shape.OutlineColor = new Color(0, 125, 255);
				shape.OutlineThickness = 1f;
			}

			shape.Position = new SFML.System.Vector2f(
				bounds.x - radius,
				bounds.y - radius);

			context.window.Draw(shape);
		}
	}
}
