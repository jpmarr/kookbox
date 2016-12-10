using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IPoll
    {
        IEnumerable<IUser> VotesFor { get; }
        IEnumerable<IUser> VotesAgainst { get; }
    }
}
