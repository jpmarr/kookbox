using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicSources
    {
        IEnumerable<IMusicSource> AllSources { get; }
        // operations that work across multiple sources like Search etc
        Task<IObservable<IMusicSearchResults>> SearchAsync(string searchCriteria);
    }
}
