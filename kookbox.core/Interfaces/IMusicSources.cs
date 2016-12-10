using System;
using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicSources : IEnumerable<IMusicSource>
    {
        void RegisterMusicSource(IMusicSource source);

        // operations that work across multiple sources like Search etc
        IObservable<ISearchResults> SearchAsync(string searchCriteria);
    }
}
