﻿
	public interface IInstrument
	{
		double Wear { get; }

		void Repair();

		void WearDown();

		bool IsBroken { get; }
	}