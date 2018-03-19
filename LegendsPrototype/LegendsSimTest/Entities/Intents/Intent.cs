﻿using SFMLEngine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Intents {
	public delegate void IntentEvent();

	public class Intent : ObjectBase {
		public interface ITask {
			IntentEvent onComplete { get; set; }
			int priority { get; set; }

			Result getResult();
		}

		public class Result : ObjectBase { }
		public class Task<R> : ObjectBase, ITask where R : Result {
			public IntentEvent onComplete { get; set; }
			public R result { get; protected set; }
			public int priority { get; set; }

			public virtual void complete(R result) {
				this.result = result;
				onComplete?.Invoke();

				log("Result: " + result.ToString());
			}

			public Result getResult() {
				return this.result;
			}
		}

		public IntentEvent onComplete;
		public int priority;

		public virtual ITask getTask() {
			return null;
		}

		public virtual void onIntentActivated() { }
		public virtual void onIntentDeactivated() { }
		public virtual void reset() { }
		public virtual void complete() {
			onComplete?.Invoke();
		}
	}
}