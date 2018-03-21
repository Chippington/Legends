using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Knowledge;
using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LegendsSimTest.Entities.Intents.Intent;

namespace LegendsSimTest.Entities.Components {
	public class IntentComponent : Component, IDescriptor {
		protected Dictionary<Type, Delegate> funcMap;
		protected IdleIntent idleIntent;
		protected List<Intent> intents;
		protected Intent currentIntent;
		protected Stopwatch taskTime;

		public InventoryComponent inventory;
		public HungerComponent hunger;
		public HealthComponent health;

		private ITask lastTask;
		public ITask currentTask {
			get {
				if (currentIntent == null)
					return null;

				return currentIntent.getTask();
			}
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			funcMap = new Dictionary<Type, Delegate>();
			intents = new List<Intent>();

			inventory = entity.components.Add<InventoryComponent>();
			hunger = entity.components.Add<HungerComponent>();
			health = entity.components.Add<HealthComponent>();

			addTaskCallback<CheckStatusIntent.CheckStatusTask>(cbCheckStatusTask);
			addTaskCallback<ChopTreeIntent.ChopTreeTask>(cbChopTreeTask);
			addTaskCallback<IdleIntent.IdleTask>(cbIdleTask);

			idleIntent = new IdleIntent();
			idleIntent.priority = 3.0d;
			intents.Add(idleIntent);
		}

		public void addTaskCallback<T>(Action<T> func) where T : ITask {
			if (funcMap.ContainsKey(typeof(T)) == false)
				funcMap.Add(typeof(T), func);
		}

		public void tryInvoke(ITask task) {
			if (funcMap.ContainsKey(task.GetType()))
				funcMap[task.GetType()].DynamicInvoke(task);
		}

		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
			var ordered = intents.OrderByDescending(i => {
				if (i == currentIntent) return i.priority + (i.priority / 3);
				return i.priority;
			});

			var priority = ordered.FirstOrDefault();
			if (priority != currentIntent) {
				if (currentIntent != null)
					currentIntent.onIntentDeactivated();

				currentIntent = priority;
				priority.onIntentActivated();
			}

			if (currentIntent != null && currentTask != null) {
				if (lastTask != currentTask) {
					if (taskTime == null) {
						taskTime = new Stopwatch();
					}

					taskTime.Reset();
					taskTime.Start();
					lastTask = currentTask;
					//log("Starting Task: " + currentTask.ToString());
				}

				currentTask.time = taskTime;
				tryInvoke(currentTask);
			}
		}

		private void cbIdleTask(IdleIntent.IdleTask obj) { }

		private void cbChopTreeTask(ChopTreeIntent.ChopTreeTask obj) {
			obj.target.kill();
			obj.complete(new ChopTreeIntent.ChopTreeResult());
		}

		private void cbCheckStatusTask(CheckStatusIntent.CheckStatusTask obj) {
			obj.complete(new CheckStatusIntent.CheckStatusResult() {
				health = health.getHealth(),
				hunger = hunger.getHunger(),
			});
		}

		public IEnumerable<ITag> getTags() {
			List<ITag> tags = new List<ITag>();
			if (currentIntent != null)
				tags.AddRange(currentIntent.getTags());

			return tags;
		}

		public void addIntent(Intent intent) {
			intents.Add(intent);
		}

		public void removeIntent(Intent intent) {
			intents.Remove(intent);
		}

		public List<Intent> getIntents() {
			return intents;
		}
	}
}
