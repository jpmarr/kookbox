﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IMusicListener
    {
        string Name { get; }
        bool IsConnected { get; }
        Option<IMusicRoom> ActiveRoom { get; }
        Option<IBan> Ban { get; }
        IEnumerable<IMusicListenerRole> ServerRoles { get; }
    }
}
