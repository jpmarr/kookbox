var Track = (function () {
    function Track(info) {
        this._info = info;
        if (info.album) {
            this._album = new Album(info.album);
        }
        this._artist = new Artist(info.artist);
    }
    Object.defineProperty(Track.prototype, "id", {
        get: function () {
            return this._info.id;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Track.prototype, "title", {
        get: function () {
            return this._info.title;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Track.prototype, "duration", {
        get: function () {
            return this._info.duration;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Track.prototype, "album", {
        get: function () {
            return this._album;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Track.prototype, "artist", {
        get: function () {
            return this._artist;
        },
        enumerable: true,
        configurable: true
    });
    return Track;
}());
var Album = (function () {
    function Album(info) {
        this._info = info;
    }
    Object.defineProperty(Album.prototype, "id", {
        get: function () {
            return this._info.id;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Album.prototype, "name", {
        get: function () {
            return this._info.name;
        },
        enumerable: true,
        configurable: true
    });
    return Album;
}());
var Artist = (function () {
    function Artist(info) {
        this._info = info;
    }
    Object.defineProperty(Artist.prototype, "id", {
        get: function () {
            return this._info.id;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Artist.prototype, "name", {
        get: function () {
            return this._info.name;
        },
        enumerable: true,
        configurable: true
    });
    return Artist;
}());
var QueuedTrack = (function () {
    function QueuedTrack(info) {
        this._info = info;
        this._track = new Track(info.track);
    }
    Object.defineProperty(QueuedTrack.prototype, "track", {
        get: function () {
            return this._track;
        },
        enumerable: true,
        configurable: true
    });
    return QueuedTrack;
}());
var Room = (function () {
    function Room(client, info) {
        this._client = client;
        this._info = info;
        if (info.currentTrack) {
            this._currentTrack = new QueuedTrack(info.currentTrack);
        }
    }
    Object.defineProperty(Room.prototype, "id", {
        get: function () {
            return this._info.id;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Room.prototype, "name", {
        get: function () {
            return this._info.name;
        },
        enumerable: true,
        configurable: true
    });
    Object.defineProperty(Room.prototype, "currentTrack", {
        get: function () {
            return this._currentTrack;
        },
        enumerable: true,
        configurable: true
    });
    return Room;
}());
var ActiveRoom = (function () {
    function ActiveRoom(room) {
        this._room = room;
    }
    Object.defineProperty(ActiveRoom.prototype, "room", {
        get: function () {
            return this._room;
        },
        enumerable: true,
        configurable: true
    });
    return ActiveRoom;
}());
var KookboxClient = (function () {
    function KookboxClient(address) {
        var _this = this;
        this.address = address;
        this.onConnectionResponse = function (message) {
            var activeRoom = message.payload.activeRoom;
            if (activeRoom) {
                var room = new Room(_this, activeRoom);
                _this._rooms = [room];
                _this._activeRoom = new ActiveRoom(room);
            }
            console.log("connection response!");
        };
        this.onTrackStarted = function (message) {
            console.log("track started");
        };
        this.onSocketOpen = function () {
            console.log("socket opened");
        };
        this.onSocketMessage = function (ev) {
            console.log(ev.data);
            var message = JSON.parse(ev.data);
            _this._router[message.messageType](message);
        };
        this.onSocketClosed = function () {
            console.log("socket closed");
        };
        this.onSocketError = function () {
            console.log("socket error");
        };
        this._router = {
            1: this.onConnectionResponse,
            2: this.onTrackStarted
        };
    }
    KookboxClient.prototype.connect = function () {
        this._socket = new WebSocket(this.address);
        this._socket.onopen = this.onSocketOpen;
        this._socket.onmessage = this.onSocketMessage;
        this._socket.onclose = this.onSocketClosed;
        this._socket.onerror = this.onSocketError;
    };
    return KookboxClient;
}());
//# sourceMappingURL=KookboxClient.js.map