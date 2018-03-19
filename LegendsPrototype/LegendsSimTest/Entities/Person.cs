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
		protected SurvivalIntent survival;
		public Dictionary<Type, Delegate> funcMap;
		public InventoryComponent inventory;
		public Position position;
		public List<Intent> desires;

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

			desires = new List<Intent>();
			funcMap = new Dictionary<Type, Delegate>();
			inventory = components.Add<InventoryComponent>();
			position = components.Add<Position>();
			inventory.addItem(new ConsumableItem());

			addTaskCallback<SearchInventoryIntent.SearchInventoryTask>(cbSearchInventoryTask);
			addTaskCallback<CheckStatusIntent.CheckStatusTask>(cbCheckStatusTask);
			addTaskCallback<ConsumeIntent.ConsumeTask>(cbConsumeTask);
			addTaskCallback<MoveIntent.MoveTask>(cbMoveTask);

			survival = new SurvivalIntent();
			survival.onComplete += onSurvivalCheckComplete;

			desires.Add(survival);
		}

		private void onSurvivalCheckComplete() {
			desires.Remove(survival);
			survival.onComplete = null;

			survival = new SurvivalIntent();
			desires.Add(survival);
			survival.onComplete += onSurvivalCheckComplete;
		}

		private void cbSearchInventoryTask(SearchInventoryIntent.SearchInventoryTask obj) {
			obj.complete(new SearchInventoryIntent.SearchInventoryResult() {
				items = inventory.getItems().Where(i => obj.searchTypes.Contains(i.GetType())).ToList(),
			});
		}

		private void cbCheckStatusTask(CheckStatusIntent.CheckStatusTask obj) {
			obj.complete(new CheckStatusIntent.CheckStatusResult() {
				health = health.getHealth(),
			});
		}

		private void cbConsumeTask(ConsumeIntent.ConsumeTask obj) {
			health.addHealth(10);
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
			var ordered = desires.OrderBy(i => i.priority);
			var priority = ordered.FirstOrDefault();
			if(priority != currentIntent) {
				currentIntent = priority;
			}

			if (currentIntent != null && currentTask != null) {
				if(lastTask != currentTask) {
					lastTask = currentTask;
					log("Starting Task: " + currentTask.ToString());
				}

				tryInvoke(currentTask);
			}
		}
	}
}
