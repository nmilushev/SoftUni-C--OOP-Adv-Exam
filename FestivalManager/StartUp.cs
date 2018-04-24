namespace FestivalManager
{
    using Core;
    using Core.Controllers;
    using Core.Controllers.Contracts;
    using FestivalManager.Core.IO;
    using FestivalManager.Core.IO.Contracts;
    using FestivalManager.Entities.Factories;
    using FestivalManager.Entities.Factories.Contracts;

    public static class StartUp
    {
        public static void Main(string[] args)
        {
            Stage stage = new Stage();

            ISetController setController = new SetController(stage);

            ISetFactory setFactory = new SetFactory();
            IInstrumentFactory instrumentFactory = new InstrumentFactory();
            IPerformerFactory performerFactory = new PerformerFactory();
            ISongFactory songFactory = new SongFactory();

            IFestivalController festivalController = new FestivalController(stage, setController, setFactory, instrumentFactory, performerFactory, songFactory);

            IReader reader = new ConsoleReader();
            IWriter writer = new ConsoleWriter();
            var engine = new Engine(reader, writer, festivalController, setController);

            engine.Run();
        }
    }
}