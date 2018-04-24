using System;
using System.Linq;
using System.Reflection;

namespace FestivalManager.Entities.Factories
{
	using Contracts;

	public class SetFactory : ISetFactory
	{
		public ISet CreateSet(string name, string type)
		{

            Type typeType = Assembly.GetCallingAssembly()
                .GetTypes().FirstOrDefault(t => t.Name == type);

            return (ISet)Activator.CreateInstance(typeType, new object[] { name });
		}
	}
}