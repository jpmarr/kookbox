using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces.Internal
{
    internal interface IMusicRoomController : IMusicRoom
    {
        Task OpenAsync();
        Task CloseAsync();
    }
}
