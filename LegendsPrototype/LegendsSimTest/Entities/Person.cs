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
		protected IdleIntent idleIntent;

		public Dictionary<Type, Delegate> funcMap;
		public InventoryComponent inventory;
		public HungerComponent hunger;
		public List<Intent> desires;
		public Stopwatch taskTime;

		public Intent currentIntent;
		public ITask currentTask {
			get {
				if (currentIntent == null)
					return null;

				return currentIntent.getTask();
			}
		}

		public override void onInitialize(GameContext context) {
			funcMap = new Dictionary<Type, Delegate>();
			desires = new List<Intent>();
			base.onInitialize(context);

			health.setMaxHealth(100);
			health.setHealth(10);

			inventory = components.Add<InventoryComponent>();
			hunger = components.Add<HungerComponent>();

			components.Add<DiscoveryComponent>();
			components.Add<SurvivalComponent>();
			components.Add<MovementComponent>();

			addTaskCallback<CheckStatusIntent.CheckStatusTask>(cbCheckStatusTask);
			addTaskCallback<ChopTreeIntent.ChopTreeTask>(cbChopTreeTask);
			addTaskCallback<CollectIntent.CollectTask>(cbCollectTask);
			addTaskCallback<ConsumeIntent.ConsumeTask>(cbConsumeTask);
			addTaskCallback<CollectIntent.ClaimTask>(cbClaimTask);
			addTaskCallback<IdleIntent.IdleTask>(cbIdleTask);

			idleIntent = new IdleIntent();
			idleIntent.priority = 3.0d;

			desires.Add(idleIntent);
		}

		#region CALLBACKS

		/*----------------------------------------------------------------------------------------------*/
		/*----------------------------------------------------------------------------------------------*/
		/*----------------------------------------------------------------------------------------------*/

		private void cbIdleTask(IdleIntent.IdleTask obj) { }

		private void cbClaimTask(CollectIntent.ClaimTask obj) {
			ItemBase ret = null;
			if (obj.target.claimedBy == null) {
				obj.target.claim(this);
				ret = obj.target;
			}

			obj.complete(new CollectIntent.ClaimResult(ret));
		}

		private void cbChopTreeTask(ChopTreeIntent.ChopTreeTask obj) {
			obj.target.kill();
			obj.complete(new ChopTreeIntent.ChopTreeResult());
		}

		private void cbCollectTask(CollectIntent.CollectTask obj) {
			if (obj.target.container != null || (obj.target.claimedBy != null && obj.target.claimedBy != this) || obj.target.isDestroyed()) {
				obj.complete(new CollectIntent.CollectResult(null));
				return;
			}

			inventory.addItem(obj.target);
			obj.complete(new CollectIntent.CollectResult(obj.target));
		}

		private void cbCheckStatusTask(CheckStatusIntent.CheckStatusTask obj) {
			obj.complete(new CheckStatusIntent.CheckStatusResult() {
				health = health.getHealth(),
				hunger = hunger.getHunger(),
			});
		}

		private void cbConsumeTask(ConsumeIntent.ConsumeTask obj) {
			if (taskTime.Elapsed.TotalSeconds < 0.3d) {
				if (obj.consumable.isDestroyed() == false && obj.consumable.container == null)
					obj.complete(new ConsumeIntent.ConsumeResult(ConsumeIntent.ConsumeResult.Reason.ITEMNOTFOUND));

				return;
			}

			hunger.subtractHunger(30);
			obj.consumable.destroy();
			obj.complete(new ConsumeIntent.ConsumeResult());
		}

		/*----------------------------------------------------------------------------------------------*/
		/*----------------------------------------------------------------------------------------------*/
		/*----------------------------------------------------------------------------------------------*/

		#endregion

		public void addTaskCallback<T>(Action<T> func) where T : ITask {
			if (funcMap.ContainsKey(typeof(T)) == false)
				funcMap.Add(typeof(T), func);
		}

		public void tryInvoke(ITask task) {
			if (funcMap.ContainsKey(task.GetType()))
				funcMap[task.GetType()].DynamicInvoke(task);
		}

		private ITask lastTask;
		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
			var ordered = desires.OrderByDescending(i => {
				if (i == currentIntent) return i.priority + (i.priority / 3);
				return i.priority;
			});

			var priority = ordered.FirstOrDefault();
			if(priority != currentIntent) {
				if (currentIntent != null) 
					currentIntent.onIntentDeactivated();

				currentIntent = priority;
				priority.onIntentActivated();
			}

			if (currentIntent != null && currentTask != null) {
				if(lastTask != currentTask) {
					if(taskTime == null) {
						taskTime = new Stopwatch();
					}

					taskTime.Reset();
					taskTime.Start();
					lastTask = currentTask;
					//log("Starting Task: " + currentTask.ToString());
				}

				tryInvoke(currentTask);
			}
		}

		public override IEnumerable<ITag> getTags() {
			List<ITag> tags = new List<ITag>();
			if (currentIntent != null)
				tags.AddRange(currentIntent.getTags());

			var comps = components.Values.Select(i => i as IDescriptor).Where(i => i != null);
			foreach (var comp in comps)
				tags.AddRange(comp.getTags());

			return tags.GroupBy(i => i.GetType()).First();
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
