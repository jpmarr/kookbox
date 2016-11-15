using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicListener
    {
        string Name { get; }
        IMusicRoom Room { get; }
    }
}
