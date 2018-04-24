namespace FestivalManager.Entities.Factories
{
	using System;
	using System.Linq;
	using System.Reflection;
	using Contracts;

	public class InstrumentFactory : IInstrumentFactory
	{
		public IInstrument CreateInstrument(string type)
		{
            Type typeType = Assembly.GetCallingAssembly()
               .GetTypes().FirstOrDefault(t => t.Name == type);

            return (IInstrument)Activator.CreateInstance(typeType);
        }
	}
}