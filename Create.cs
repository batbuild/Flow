// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;

namespace Flow
{
	/// <summary>
	/// Bootstrapper for the flow library using default implementations
	/// </summary>
	public static class Create
	{
		/// <summary>
		/// Create a new Kernel and Factory
		/// </summary>
		/// <returns>
		/// The kernel.
		/// </returns>
		public static IKernel NewKernel()
		{
			return NewFactory().Kernel;
		}

		/// <summary>
		/// Create a new Kernel and Factory
		/// </summary>
		/// <returns>
		/// The factory.
		/// </returns>
		public static IFactory NewFactory()
		{
			var kernel = new Kernel();
			var factory = new Factory();

			kernel.Factory = factory;
			factory.Kernel = kernel;

			kernel.Root = new Node { Kernel = kernel };

			return factory;
		}
	}
}

// Christian: This was added to support Unity 3.x I believe. It produces a warning but I don't want to remove it yet.

#pragma warning disable 1685

namespace System.Runtime.CompilerServices
{
    public class ExtensionAttribute : Attribute { }
}
