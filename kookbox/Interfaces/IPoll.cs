﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.Interfaces
{
    public interface IPoll
    {
        IEnumerable<IMusicListener> VotesFor { get; }
        IEnumerable<IMusicListener> VotesAgainst { get; }
    }
}
