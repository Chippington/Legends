using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Registry {
	public class Node {
		public Node next { get; protected set; }

		public Node prev { get; protected set; }

		public Node origin {
			get {
				var n = this;
				while (n.prev != null) n = prev;
				return n;
			}
		}

		public Node latest {
			get {
				var n = this;
				while (n.next != null) n = next;
				return n;
			}
		}

		private int _rev = -1;
		public int rev {
			get {
				if (prev == null)
					return 0;

				if (_rev == -1)
					_rev = prev.rev + 1;

				return _rev;
			}
		}

		public Node(Node previous) {
			this.prev = previous;
			previous.next = this;
		}

		public virtual void onRegister() {

		}
	}
}
