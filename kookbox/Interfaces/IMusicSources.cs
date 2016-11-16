using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicSources
    {
        IEnumerable<IMusicSource> Sources { get; }
        // operations that work across multiple sources like Search etc
    }
}
