﻿using System.Collections.Generic;

/*
* CarrotCake CMS
* http://www.carrotware.com/
*
* Copyright 2011, Samantha Copeland
* Dual licensed under the MIT or GPL Version 3 licenses.
*
* Date: October 2011
*/

namespace Carrotware.CMS.Interface {

	public interface IWidgetParmData {
		Dictionary<string, string> PublicParmValues { get; set; }
	}
}