// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Diagnostics;

namespace Flow
{
	/// <summary>
	/// TODO: delta-capping, pausing, introduction of zulu/sim time differences
	/// </summary>
	internal class TimeFrame : ITimeFrame
	{
		private Stopwatch _stopwatch;

		public TimeFrame()
		{
			_stopwatch = new Stopwatch();
			Now = TimeSpan.Zero;
			Last = TimeSpan.Zero;
			Delta = TimeSpan.Zero;
		}

		/// <inheritdoc />
		public System.TimeSpan Last { get; internal set; }

		/// <inheritdoc />
		public System.TimeSpan Now { get; internal set; }

		/// <inheritdoc />
		public System.TimeSpan Delta { get; internal set; }

		public void Step()
		{
			if (_stopwatch.IsRunning == false)
			{
				_stopwatch.Reset();
				_stopwatch.Start();
			}

			Last = Now;
			Delta = _stopwatch.Elapsed - Last;
			Now = _stopwatch.Elapsed;
		}
	}
}