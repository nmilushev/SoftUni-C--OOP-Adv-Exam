using System;
using System.Linq;

namespace FestivalManager.Core
{
    using System.Collections.Generic;
    using System.Reflection;
	using Controllers.Contracts;
    using IO.Contracts;

	public class Engine : IEngine
	{
	    private IReader reader;
	    private IWriter writer;

        private readonly List<ISet> sets = new List<ISet>();
        private readonly List<ISong> songs = new List<ISong>();
        private readonly List<IPerformer> performers = new List<IPerformer>();

        private IFestivalController festivalCоntroller;
        private ISetController setCоntroller;

        public Engine(IReader reader, IWriter writer, IFestivalController festivalCоntroller, ISetController setCоntroller)
        {
            this.reader = reader;
            this.writer = writer;
            this.festivalCоntroller = festivalCоntroller;
            this.setCоntroller = setCоntroller;
        }

		public void Run()
		{
			while (true)
			{
				var input = this.reader.ReadLine();

				if (input == "END")
					break;

				try
				{
					var result = this.ProcessCommand(input);
					this.writer.WriteLine(result);
				}
				catch (Exception ex)
				{
					this.writer.WriteLine("ERROR: " + ex.Message);
				}
			}

			var end = this.festivalCоntroller.ProduceReport();

			this.writer.WriteLine("Results:");
			this.writer.WriteLine(end);
		}

        public string ProcessCommand(string input)
        {
            var inputArgs = input.Split(" ".ToCharArray().First());

            var command = inputArgs.First();
            var parameters = inputArgs.Skip(1).ToArray();

            if (command == "LetsRock")
            {
                return this.setCоntroller.PerformSets();
            }

            var festivalcontrolfunction = this.festivalCоntroller.GetType()
                .GetMethods()
                .FirstOrDefault(x => x.Name == command);

            string a;

            try
            {
                a = (string)festivalcontrolfunction.Invoke(this.festivalCоntroller, new object[] { parameters });
            }
            catch (TargetInvocationException up)
            {
                throw up.InnerException;
            }

            return a;
        }
    }
}