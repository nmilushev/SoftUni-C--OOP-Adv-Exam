namespace FestivalManager.Tests
{
    using NUnit.Framework;
    using System.Linq;
    using System.Reflection;

    [TestFixture]
    public class SetControllerTests
    {
        [Test]
        public void TestValidConstructor()
        {
            IStage stage = new Stage();
            ISetController setController = new SetController(stage);
            FieldInfo fieldInfo = typeof(SetController)
             .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
             .First(fi => fi.FieldType == typeof(IStage));

            Assert.That(fieldInfo.GetValue(setController), Is.EqualTo(stage));
        }

        [Test]
        public void TestDidNotPerform()
        {
            IStage stage = new Stage();
            stage.AddSet(new Short("shorty"));
            ISetController setController = new SetController(stage);

            Assert.That(() => setController.PerformSets(), Is.EqualTo("1. shorty:\r\n" +
"-- Did not perform"));
        }

        [Test]
        public void TestCannotPerform()
        {
            IStage stage = new Stage();
            stage.AddSet(new Short("shorty"));
            ISetController setController = new SetController(stage);

            Assert.That(() => stage.Sets.First().CanPerform(), Is.EqualTo(false));
        }

        [Test]
        public void TestPerformanceSuccessful()
        {
            IStage stage = new Stage();
            ISet set = new Short("Shorty");

            stage.AddSet(set);

            IPerformer perf = new Performer("HrupHrupAzSumTup", 22);
            perf.AddInstrument(new Guitar());

            stage.AddPerformer(perf);

            ISong song = new Song("hrup", new System.TimeSpan(0, 5, 0));

            stage.AddSong(song);

            set.AddSong(song);

            set.AddPerformer(perf);

            ISetController setController = new SetController(stage);

            setController.PerformSets();

            Assert.That(() => setController.PerformSets(), Is.EqualTo("1. Shorty:\r\n"+
"-- 1. hrup (05:00)\r\n"+
"-- Set Successful"));
        }



        [Test]
        public void TestCanPerformReturnsTrue()
        {
            IStage stage = new Stage();
            ISet set = new Short("Shorty");

            stage.AddSet(set);

            IPerformer perf = new Performer("HrupHrupAzSumTup", 22);
            perf.AddInstrument(new Guitar());

            stage.AddPerformer(perf);

            ISong song = new Song("hrup", new System.TimeSpan(0, 5, 0));

            stage.AddSong(song);

            set.AddSong(song);

            set.AddPerformer(perf);

            ISetController setController = new SetController(stage);

            setController.PerformSets();

            Assert.That(() => stage.Sets.First().CanPerform(), Is.EqualTo(true));
        }

        [Test]
        public void TestInstrumentWearsDownAfterPerformance()
        {
            IStage stage = new Stage();
            ISet set = new Short("Shorty");

            stage.AddSet(set);

            IPerformer perf = new Performer("HrupHrupAzSumTup", 22);
            perf.AddInstrument(new Guitar());

            stage.AddPerformer(perf);

            ISong song = new Song("hrup", new System.TimeSpan(0, 5, 0));

            stage.AddSong(song);

            set.AddSong(song);

            set.AddPerformer(perf);

            ISetController setController = new SetController(stage);

            setController.PerformSets();

            Assert.That(perf.Instruments.First().Wear, Is.EqualTo(40));
        }
    }
}