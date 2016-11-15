using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicRoom
    {
        IMusicQueue Queue { get; }
        IEnumerable<IMusicListener> Listeners { get; }
    }
}
