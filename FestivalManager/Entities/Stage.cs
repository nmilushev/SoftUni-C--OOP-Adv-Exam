
using System.Collections.Generic;
using System.Linq;

public class Stage : IStage
{
    private readonly List<ISet> sets;
    private readonly List<ISong> songs;
    private readonly List<IPerformer> performers;

    public Stage()
    {
        //better use DI here
        this.sets = new List<ISet>();
        this.songs = new List<ISong>();
        this.performers = new List<IPerformer>();
    }

    public IReadOnlyCollection<ISet> Sets => this.sets;

    public IReadOnlyCollection<ISong> Songs => this.songs;

    public IReadOnlyCollection<IPerformer> Performers => this.performers;

    public void AddPerformer(IPerformer performer)
    {
        this.performers.Add(performer);
    }

    public void AddSet(ISet set)
    {
        this.sets.Add(set);
    }

    public void AddSong(ISong song)
    {
        this.songs.Add(song);
    }

    public IPerformer GetPerformer(string name)
    {
        return this.performers.FirstOrDefault(p => p.Name == name);
    }

    public ISet GetSet(string name)
    {
        return this.sets.FirstOrDefault(s => s.Name == name);
    }

    public ISong GetSong(string name)
    {
        return this.songs.FirstOrDefault(s => s.Name == name);
    }

    public bool HasPerformer(string name)
    {
        IPerformer performer = this.performers.FirstOrDefault(p => p.Name == name);
        if (performer == null)
        {
            return false;
        }
        return true;
    }

    public bool HasSet(string name)
    {
        ISet set = this.sets.FirstOrDefault(s => s.Name == name);
        if (set == null)
        {
            return false;
        }
        return true;
    }

    public bool HasSong(string name)
    {
        ISong song = this.songs.FirstOrDefault(s => s.Name == name);
        if (song == null)
        {
            return false;
        }
        return true;
    }
}