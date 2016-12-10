using System;
using System.Collections;
using System.Collections.Generic;
using kookbox.core.Interfaces;

namespace kookbox.core
{
    public class MusicSources : IMusicSources
    {
        private readonly List<IMusicSource> sources = new List<IMusicSource>();

        public IEnumerable<IMusicSource> AllSources => sources;

        public void RegisterMusicSource(IMusicSource source)
        {
            if (source == null)
                throw new ArgumentNullException(nameof(source));

            sources.Add(source);
        }

        public IObservable<ISearchResults> SearchAsync(string searchCriteria)
        {
            throw new NotImplementedException();
        }

        public IEnumerator<IMusicSource> GetEnumerator()
        {
            return sources.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }
    }
}
