﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LegendsSimTest.Knowledge {
	public interface ITag {
		string name { get; set; }
	}

	public interface IActionTag : ITag { }
}
