using System;

namespace FestivalManager.Entities.Instruments
{
	public abstract class Instrument : IInstrument
	{
		private double wear;
		private const int MaxWear = 100;
        private const int MinWear = 0;

        protected Instrument()
		{
			this.Wear = MaxWear;
		}

		public double Wear
		{
			get
			{
				return this.wear;
			}
			private set
			{
				this.wear = Math.Min(Math.Max(value, MinWear), MaxWear);
			}
		}

		protected abstract int RepairAmount { get; }

        public bool IsBroken => this.Wear <= MinWear;

        public void Repair() => this.Wear += this.RepairAmount;

		public void WearDown() => this.Wear -= this.RepairAmount;

		
		public override string ToString()
		{
			var instrumentStatus = this.IsBroken ? "broken" : $"{this.Wear}%";

			return $"{this.GetType().Name} [{instrumentStatus}]";
		}
	}
}