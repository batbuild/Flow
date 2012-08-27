// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

namespace Flow
{
	/// <summary>
	/// A coroutine is a continuation that can resume after a yield.
	/// </summary>
	public interface ICoroutine<TR> : ITypedGenerator<TR>
	{
	}
}