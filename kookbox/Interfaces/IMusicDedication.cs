using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicDedication
    {
        IMusicListener DedicatedTo { get; }
        Option<string> Message { get; }
    }
}
