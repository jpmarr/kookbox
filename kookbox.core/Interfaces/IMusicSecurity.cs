﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace kookbox.core.Interfaces
{
    public interface IMusicSecurity
    {
        void CheckListenerHasPermission<T>(IMusicListener listener, Permission requiredPermission, Option<T> target);
    }

    public enum Permission
    {
        Connect,
        Request,
        StartListenerPoll,
        StartTrackPoll,
        ControlPlayback,
        ControlVolume,
        AdministerRoom,
        BanListener,
        BanTrack,
        CreateRoom   
    }
}
