using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IPoll
    {
        IEnumerable<IMusicListener> VotesFor { get; }
        IEnumerable<IMusicListener> VotesAgainst { get; }
    }
}
