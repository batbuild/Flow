﻿// (C) 2012 Christian Schladetsch. See http://www.schladetsch.net/flow/license.txt for Licensing information.

using System;
using System.Text;

namespace Flow
{
	internal class Generator<TR> : Transient, ITypedGenerator<TR>
	{
		/// <inheritdoc />
		public TR Value { get; protected set; }

		/// <inheritdoc />
		public event GeneratorHandler Suspended;

		/// <inheritdoc />
		public event GeneratorHandler Resumed;

		/// <inheritdoc />
		public event GeneratorHandler Stepped;

		/// <inheritdoc />
		public bool Running { get; private set; }

		/// <inheritdoc />
		public int StepNumber { get; private set; }

		/// <inheritdoc />
		public virtual bool Step()
		{
			++StepNumber;

			if (Stepped != null)
				Stepped(this);

			return true;
		}

		/// <inheritdoc />
		public virtual void Post()
		{
		}

		/// <inheritdoc />
		public void Suspend()
		{
			if (!Running || !Exists)
				return;
			
			Running = false;
			
			if (Suspended != null)
				Suspended(this);
		}

		/// <inheritdoc />
		public void Resume()
		{
			if (Running || !Exists)
				return;
			
			Running = true;

			if (Resumed != null)
				Resumed(this);
		}

		/// <inheritdoc />
		public void SuspendAfter(ITransient transient)
		{
			Resume();

			transient.Deleted += tr => Suspend();
		}

		/// <inheritdoc />
		public bool ResumeAfter(ITransient transient)
		{
			if (transient == null || !transient.Exists) 
			{
				Resume();
				return true;
			}

			Suspend();

			transient.Deleted += tr => Resume();

			return true;
		}

		/// <inheritdoc />
		public bool ResumeAfter(TimeSpan span)
		{
			if (!Exists)
				return false;

			ResumeAfter(Factory.NewTimer(span));

			return true;
		}

		/// <inheritdoc />
		public bool SuspendAfter(TimeSpan span)
		{
			if (!Exists)
				return false;

			SuspendAfter(Factory.NewTimer(span));

			return true;
		}
		
		/// <inheritdoc />
		public IFuture<TR2> GetNext<TR2>(ITypedGenerator<TR2> gen)
		{
			var future = Kernel.Factory.NewFuture<TR2>();
			GeneratorHandler del;
			del = delegate(IGenerator tr) { GenStepped<TR2>(gen, future, del); };
			gen.Stepped += del;
			return future;
		}

		void GenStepped<TR2>(ITypedGenerator<TR2> gen, IFuture<TR2> future, GeneratorHandler del)
		{
			gen.Stepped -= del;
			future.Value = gen.Value;
		}
	}
}