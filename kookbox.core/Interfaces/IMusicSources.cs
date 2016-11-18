using System.Collections.Generic;

namespace kookbox.core.Interfaces
{
    public interface IMusicSources
    {
        IEnumerable<IMusicSource> Sources { get; }
        // operations that work across multiple sources like Search etc
    }
}
