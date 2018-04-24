namespace FestivalManager.Core.Controllers
{
    using System;
    using System.Linq;
    using Contracts;
    using FestivalManager.Entities.Factories;
    using FestivalManager.Entities.Factories.Contracts;

    public class FestivalController : IFestivalController
    {
        private const string TimeFormat = "mm\\:ss";

        private readonly IStage stage;
        private ISetFactory setFactory;// = new SetFactory();
        private IInstrumentFactory instrumentFactory;// = new InstrumentFactory();
        private IPerformerFactory performerFactory;// = new PerformerFactory();
        private ISongFactory songFactory;// = new SongFactory();
        private ISetController setController;


        public FestivalController(IStage stage, 
            ISetController setController, 
            ISetFactory setFactory,
            IInstrumentFactory instrumentFactory,
            IPerformerFactory performerFactory,
            ISongFactory songFactory)
        {
            this.setController = setController;
            this.setFactory = setFactory;
            this.instrumentFactory = instrumentFactory;
            this.performerFactory = performerFactory;
            this.songFactory = songFactory;
            this.stage = stage;
            this.setController = setController;//= new SetController(stage);
        }

        public string ProduceReport()
        {
            var result = string.Empty;

            var totalFestivalLength = new TimeSpan(this.stage.Sets.Sum(s => s.ActualDuration.Ticks));
            if (totalFestivalLength.Hours >= 1)
            {
                var minutes = totalFestivalLength.Hours * 60 + totalFestivalLength.Minutes % 60;

                result += ($"Festival length: {minutes}:{totalFestivalLength:ss}") + "\n";
            }
            else
            {
                result += ($"Festival length: {(totalFestivalLength):mm\\:ss}") + "\n";
            }
            foreach (var set in this.stage.Sets)
            {
                if ((set.ActualDuration.Hours >= 1))
                {
                    var minutes = set.ActualDuration.Hours* 60 + totalFestivalLength.Minutes % 60;

                    result += ($"--{set.Name} ({minutes}:{set.ActualDuration:ss}):") + "\n";
                }
                else
                {
                    result += ($"--{set.Name} ({(set.ActualDuration):mm\\:ss}):") + "\n";
                }

                var performersOrderedDescendingByAge = set.Performers.OrderByDescending(p => p.Age);
                foreach (var performer in performersOrderedDescendingByAge)
                {
                    var instruments = string.Join(", ", performer.Instruments
                        .OrderByDescending(i => i.Wear));

                    result += ($"---{performer.Name} ({instruments})") + "\n";
                }

                if (!set.Songs.Any())
                    result += ("--No songs played") + "\n";
                else
                {
                    result += ("--Songs played:") + "\n";
                    foreach (var song in set.Songs)
                    {
                        result += ($"----{song.Name} ({song.Duration.ToString(TimeFormat)})") + "\n";
                    }
                }
            }

            return result.ToString();
        }

        public string RegisterSet(string[] args)
        {
            string name = args[0];
            string type = args[1];

            ISet setToRegister = this.setFactory.CreateSet(name, type);

            this.stage.AddSet(setToRegister);

            return $"Registered {type} set";
        }

        public string SignUpPerformer(string[] args)
        {
            var name = args[0];
            var age = int.Parse(args[1]);

            var instrumenti = args.Skip(2).ToArray();

            var instrumenti2 = instrumenti
                .Select(i => this.instrumentFactory.CreateInstrument(i))
                .ToArray();

            var performer = this.performerFactory.CreatePerformer(name, age);

            foreach (var instrument in instrumenti2)
            {
                performer.AddInstrument(instrument);
            }

            this.stage.AddPerformer(performer);

            return $"Registered performer {performer.Name}";
        }

        public string RegisterSong(string[] args)
        {
            string name = args[0];

            string[] durationsArgs = args[1].Split(':');

            TimeSpan duration =
                new TimeSpan(0, int.Parse(durationsArgs[0]), int.Parse(durationsArgs[1]));

            ISong songToRegister = this.songFactory.CreateSong(name, duration);
            this.stage.AddSong(songToRegister);
            return $"Registered song {songToRegister.Name} ({duration:mm\\:ss})";
        }

        public string AddSongToSet(string[] args)
        {
            var songName = args[0];
            var setName = args[1];

            if (!this.stage.HasSet(setName))
            {
                throw new InvalidOperationException("Invalid set provided");
            }

            if (!this.stage.HasSong(songName))
            {
                throw new InvalidOperationException("Invalid song provided");
            }

            var set = this.stage.GetSet(setName);
            var song = this.stage.GetSong(songName);

            set.AddSong(song);

            return $"Added {song.Name} ({song.Duration:mm\\:ss}) to {set.Name}";
        }

        public string AddPerformerToSet(string[] args)
        {
            string name = args[0];
            string setName = args[1];

            if (!this.stage.HasPerformer(name))
            {
                throw new InvalidOperationException("Invalid performer provided");
            }

            if (!this.stage.HasSet(setName))
            {
                throw new InvalidOperationException("Invalid set provided");
            }

            var set = this.stage.GetSet(setName);
            var performer = this.stage.GetPerformer(name);

            set.AddPerformer(performer);

            return $"Added {performer.Name} to {set.Name}";
        }

        public string RepairInstruments(string[] args)
        {
            var instrumentsToRepair = this.stage.Performers
                .SelectMany(p => p.Instruments)
                .Where(i => i.Wear < 100)
                .ToArray();

            foreach (var instrument in instrumentsToRepair)
            {
                instrument.Repair();
            }

            return $"Repaired {instrumentsToRepair.Length} instruments";
        }

        private string LetsRock()
        {
            return this.setController.PerformSets();
        }
    }
}