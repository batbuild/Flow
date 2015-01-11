// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	/// Stores information about a time step.
	/// </summary>
	public interface ITimeFrame
	{
		/// <summary>
		/// Gets the elapsed time at the last update since this time frame started.
		/// </summary>
		/// <value>
		/// The elapsed time that the last update was executed at.
		/// </value>
		TimeSpan Last { get; }

		/// <summary>
		/// Gets the elapsed time since this time frame started. This does not change as the update is being performed.
		/// </summary>
		/// <value>
		/// The elapsed time since this time frame started. This does not change as the update is being performed.
		/// </value>
		TimeSpan Now { get; }

		/// <summary>
		/// Gets the delta time to use for this update. Note that (Now - Last) may not always be equal to Delta
		/// </summary>
		/// <value>
		/// The delta time to use for this update.
		/// </value>
		TimeSpan Delta { get; }

		void Step();
	}
}