using SFMLEngine;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Entities.Components {
	public class Trait {
		protected float _weight;
		public float weight {
			get {
				return _weight;
			}
			set {
				_weight = Math.Max(Math.Min(1f, value), 0f);
			}
		}

		public virtual string getName() {
			return GetType().Name;
		}
	}

	public class Trait<T> : Trait where T : Trait {
		protected T _anti;
		public T anti {
			get {
				if (_anti == null)
					_anti = Activator.CreateInstance<T>();

				_anti.weight = 1f - weight;
				return _anti;
			}

			private set { }
		}
	}

	public class Introvert : Trait<Extrovert> { }
	public class Extrovert : Trait<Introvert> { }



	public class PersonalityComponent : Component {
		protected Dictionary<Type, Trait> traitMap;

		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			traitMap = new Dictionary<Type, Trait>();
		}

		public T get<T>() where T : Trait {
			if (traitMap.ContainsKey(typeof(T)) == false)
				return default(T);

			return (T)traitMap[typeof(T)];
		}

		public T add<T>() where T : Trait {
			var inst = Activator.CreateInstance<T>();
			traitMap.Add(typeof(T), inst);
			return inst;
		}
	}
}
