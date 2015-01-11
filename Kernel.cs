// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	internal class Kernel : Generator<bool>, IKernel
	{
		/// <inheritdoc />
		public INode Root { get; set; }

		/// <inheritdoc />
		public new IFactory Factory { get; internal set; }

		/// <inheritdoc />
		public ITimeFrame Time { get { return _time; } }

		internal Kernel(ITimeFrame timeFrame)
		{
			_time = timeFrame;
		}

		/// <inheritdoc />
		public override void Step()
		{
			StepTime();

			if (IsNullOrEmpty(Root))
				return;

			Root.Step();
			Root.Post();
		}

		void StepTime()
		{
			_time.Step();
		}

		private readonly ITimeFrame _time;
	}
}