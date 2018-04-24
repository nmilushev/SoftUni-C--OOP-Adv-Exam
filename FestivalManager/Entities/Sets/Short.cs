
using System;

public class Short : Set
{
    private const int maxDuration = 15;

    public Short(string name)
        : base(name, new TimeSpan(0, maxDuration, 0))
    {
    }
}