using LegendsSimTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry.Nodes {
	public class PersonNode : EntityNode {
		public List<ITag> tags { get; private set; }
		public Person person { get; private set; }

		public PersonNode(Person person) : base(person) {
			this.person = person;
			this.tags = person.getTags().ToList();
		}

		public PersonNode(PersonNode prev) : base(prev) {
			this.person = prev.person;
			this.tags = prev.tags;
		}

		public override string ToString() {
			return string.Format("N[{0}]", person.ToString());
		}
	}
}
