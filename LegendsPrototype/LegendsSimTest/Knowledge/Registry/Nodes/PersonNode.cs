using LegendsSimTest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry.Nodes {
	public class PersonNode : Node {
		private Person _person;
		public Person person {
			get {
				if (_person == null)
					_person = ((PersonNode)prev).person;

				return _person;
			}
		}

		public PersonNode(Person person) : base(null) {
			this._person = person;
		}

		public PersonNode(PersonNode prev) : base(prev) {

		}
	}
}
