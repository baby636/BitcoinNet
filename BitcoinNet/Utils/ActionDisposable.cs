﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BitcoinNet
{
	internal class ActionDisposable : IDisposable
	{
		Action onEnter, onLeave;
		public ActionDisposable(Action onEnter, Action onLeave)
		{
			this.onEnter = onEnter;
			this.onLeave = onLeave;
			onEnter();
		}

		// IDisposable Members

		public void Dispose()
		{
			onLeave();
		}
	}
}
