using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class MusicSources : IMusicSources
    {
        public IEnumerable<IMusicSource> AllSources { get; }

        public Task<IObservable<IMusicSearchResults>> SearchAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }
    }
}
