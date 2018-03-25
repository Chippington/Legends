using LegendsSimTest.Entities.Intents;
using LegendsSimTest.Knowledge.Registry;
using LegendsSimTest.Knowledge.Registry.Nodes;
using SFMLEngine;
using SFMLEngine.Entities;
using SFMLEngine.Entities.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static LegendsSimTest.Entities.Intents.RememberIntent;

namespace LegendsSimTest.Entities.Components {
	public class MemoryComponent : Component {
		public interface IReference {
			Node node { get; set; }
		}

		public class Reference<T> : IReference where T : Node {
			public T node { get; protected set; }
			Node IReference.node { get => this.node; set => this.node = (T)value; }

			public Reference(T node) {
				this.node = node;
			}

			public override string ToString() {
				return "Reference: " + node.ToString();
			}
		}

		public List<IReference> references;
		private IntentComponent intent;


		public override void onInitialize(GameContext context) {
			base.onInitialize(context);
			references = new List<IReference>();
			intent = components.Get<IntentComponent>();
			if(intent != null) {
				intent.addTaskCallback<CommitToMemoryTask>(cbCommitToMemory);
				intent.addTaskCallback<RetrieveFromMemoryTask>(cbRetrieveFromMemory);
			}
		}

		private void cbRetrieveFromMemory(RetrieveFromMemoryTask obj) {
			obj.complete(new RetrieveFromMemoryResult(obj.reference.node));
		}

		private void cbCommitToMemory(CommitToMemoryTask obj) {
			var pn = obj.entity as Person;
			if(pn != null) {
				var ret = commitPerson(pn);
				obj.complete(new CommitToMemoryResult(ret));
				return;
			}
		}

		public void onPersonEnterVisionRange(Person other) {

		}

		public void onPersonLeaveVisionRange(Person other) {
			if(intent != null) {
				var i = new RememberIntent();
				i.commitToMemory(other);
				i.priority = double.MaxValue;
				intent.addIntent(i);
			}
		}

		public Reference<EntityNode> commitEntity(IEntity entity) {
			var l = Registry.Current.getLatest(entity) as EntityNode;
			if (l == null) {
				l = new EntityNode(entity);
			} else {
				l = new EntityNode(l);
			}

			Registry.Current.registerNode(l);
			var r = new Reference<EntityNode>(l);
			references.Add(r);
			return r;
		}

		public Reference<PersonNode> commitPerson(Person other) {
			var l = Registry.Current.getLatest(other) as PersonNode;
			if (l == null) {
				l = new PersonNode(other);
			} else {
				l = new PersonNode(l);
			}

			Registry.Current.registerNode(l);
			var r = new Reference<PersonNode>(l);
			references.Add(r);
			return r;
		}
	}
}
