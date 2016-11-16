using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IBan
    {
        IMusicListener Initiator { get; }
        DateTimeOffset ExpiryTimestamp { get; }
        Option<string> Message { get; }
    }
}
