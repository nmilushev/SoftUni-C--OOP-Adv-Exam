namespace FestivalManager.Entities.Factories
{
	using Contracts;

	public class PerformerFactory : IPerformerFactory
	{
		public IPerformer CreatePerformer(string name, int age)
		{
			var performer = new Performer(name, age);

			return performer;
		}
	}
}