﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge.Tags {
	public class Tag : ITag {
		private string _name;
		public string name {
			get {
				if (_name == null)
					_name = GetType().Name;

				return _name;
			}

			set { }
		}
	}

	public class Consumable : Tag { }
	public class Starving : Tag { }
	public class Bloated : Tag { }
	public class Healthy : Tag { }
	public class Hungry : Tag { }
	public class Sickly : Tag { }
	public class Alive : Tag { }
	public class Dead : Tag { }
	public class Full : Tag { }
	public class Idle : Tag { }

	public class Investigating : Tag, IActionTag { }
	public class Searching : Tag, IActionTag { }
	public class Eating : Tag, IActionTag { }
	public class Moving : Tag, IActionTag { }
}
