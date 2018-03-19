using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using LegendsSimTest.Entities.Components;
using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Entities.Items;
using SFMLEngine;
using SFMLEngine.Entities.Components.Common;
using static LegendsSimTest.Entities.Intents.Intent;

namespace LegendsSimTest.Entities {

	public class Person : LivingEntity {
		protected SurvivalIntent survivalIntent;
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
			base.onInitialize(context);
			health.setMaxHealth(100);
			health.setHealth(10);

			inventory = components.Add<InventoryComponent>();
			hunger = components.Add<HungerComponent>();

			desires = new List<Intent>();
			funcMap = new Dictionary<Type, Delegate>();
			inventory.addItem(new ConsumableItem());

			addTaskCallback<SearchInventoryIntent.SearchInventoryTask>(cbSearchInventoryTask);
			addTaskCallback<CheckStatusIntent.CheckStatusTask>(cbCheckStatusTask);
			addTaskCallback<ConsumeIntent.ConsumeTask>(cbConsumeTask);
			addTaskCallback<MoveIntent.MoveTask>(cbMoveTask);
			addTaskCallback<IdleIntent.IdleTask>(cbIdleTask);

			survivalIntent = new SurvivalIntent();
			survivalIntent.onComplete += onSurvivalCheckComplete;

			idleIntent = new IdleIntent();
			idleIntent.priority = 3.0d;

			desires.Add(survivalIntent);
			desires.Add(idleIntent);
		}

		private void cbIdleTask(IdleIntent.IdleTask obj) { }

		private void onSurvivalCheckComplete() {
			desires.Remove(survivalIntent);
			survivalIntent.onComplete = null;

			survivalIntent = new SurvivalIntent();
			desires.Add(survivalIntent);
			survivalIntent.onComplete += onSurvivalCheckComplete;
		}

		private void cbSearchInventoryTask(SearchInventoryIntent.SearchInventoryTask obj) {
			obj.complete(new SearchInventoryIntent.SearchInventoryResult() {
				items = inventory.getItems().Where(i => obj.searchTypes.Contains(i.GetType())).ToList(),
			});
		}

		private void cbCheckStatusTask(CheckStatusIntent.CheckStatusTask obj) {
			obj.complete(new CheckStatusIntent.CheckStatusResult() {
				health = health.getHealth(),
				hunger = hunger.getHunger(),
			});
		}

		private void cbConsumeTask(ConsumeIntent.ConsumeTask obj) {
			if (taskTime.Elapsed.TotalSeconds < 0.3d)
				return;

			hunger.subtractHunger(30);
			obj.complete(new ConsumeIntent.ConsumeResult());
		}

		private void cbMoveTask(MoveIntent.MoveTask obj) {
			position.x = obj.x;
			position.y = obj.y;
			obj.complete(new MoveIntent.MoveResult(true));
		}

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
	}
}
