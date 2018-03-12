using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LegendsSimTest.Entities.Components;
using LegendsSimTest.Entities.Items;
using SFMLEngine;

namespace LegendsSimTest.Entities {

	public class Person : LivingEntity {
		#region TASKS

		public class Intent {
			public int priority;

			public virtual Queue<Task> plan(Person person) {
				return null;
			}
		}

		public class SurvivalIntent : Intent {
			public override Queue<Task> plan(Person person) {
				ConsumptionIntent intent = new ConsumptionIntent();
				return intent.plan(person);
			}
		}

		public class ConsumptionIntent : Intent {
			ForageForEdibleTask forageTask;
			ConsumeEdibleTask consumeTask;
			public ConsumptionIntent() {
				forageTask = new ForageForEdibleTask();
				consumeTask = new ConsumeEdibleTask();
			}

			public override Queue<Task> plan(Person person) {
				Queue<Task> ret = new Queue<Task>();
				var consumables = person.inventory.getItems<ConsumableItem>();
				if (consumables.Count == 0) {
					ret.Enqueue(forageTask);
				}

				ret.Enqueue(consumeTask);
				return ret;
			}
		}

		public class Task {
			private Stopwatch sw;


			public void start() {
				sw = new Stopwatch();
				sw.Start();
			}

			public TimeSpan time() {
				if (sw == null)
					return default(TimeSpan);

				return sw.Elapsed;
			}
		}

		public class ForageForEdibleTask : Task {

		}

		public class ConsumeEdibleTask : Task {

		}

		#endregion

		public Dictionary<Type, Delegate> funcMap;
		public InventoryComponent inventory;
		public List<Intent> desires;

		public Intent currentIntent;
		public Task currentTask {
			get {
				if (currentIntent == null)
					return null;

				return currentIntent.plan(this).Peek();
			}
		}

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			desires = new List<Intent>();
			funcMap = new Dictionary<Type, Delegate>();
			inventory = components.Add<InventoryComponent>();
			//inventory.addItem(new ConsumableItem());

			addTaskCallback<ConsumeEdibleTask>(cbConsumeEdible);
			addTaskCallback<ForageForEdibleTask>(cbForageEdible);

			ConsumptionIntent intent = new ConsumptionIntent();
			desires.Add(intent);
		}

		private void cbConsumeEdible(Task obj) {
			throw new NotImplementedException();
		}

		private void cbForageEdible(Task obj) {
			throw new NotImplementedException();
		}

		public void addTaskCallback<T>(Action<Task> func) where T : Task {
			if (funcMap.ContainsKey(typeof(T)) == false)
				funcMap.Add(typeof(T), func);
		}

		public void tryInvoke(Task task) {
			if (funcMap.ContainsKey(task.GetType()))
				funcMap[task.GetType()].DynamicInvoke(task);
		}

		public override void onUpdate(GameContext context) {
			base.onUpdate(context);
			var ordered = desires.OrderBy(i => i.priority);
			var priority = ordered.FirstOrDefault();
			if(priority != currentIntent) {
				currentIntent = priority;
				currentTask.start();
			}

			if (currentTask != null)
				tryInvoke(currentTask); 
		}
	}
}
