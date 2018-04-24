namespace FestivalManager.Entities.Factories
{
	using System;
	using Contracts;

	public class SongFactory : ISongFactory
	{
		public ISong CreateSong(string name, TimeSpan duration)
		{
			var song = new Song(name, duration);
			return song;
		}
	}
}