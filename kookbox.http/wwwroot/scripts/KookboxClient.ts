interface IMessage {
    messageType: number;
    version: number;
    correlationId?: number;
    payload: any;
}

interface IMessageHandler {
    (message: IMessage): void;
}

interface IMessageRouter {
    [index: number]: IMessageHandler;
}

interface IConnectionResponse {
    activeRoom: IRoomInfo;
}

interface IArtistInfo {

    id: string;
    name: string;    
}

interface IAlbumInfo {

    id: string;
    name: string;
}

interface ITrackInfo {

    id: string;
    title: string;
    duration: any;
    album: IAlbumInfo | null;
    artist: IArtistInfo;
}

interface IUserInfo {

    id: string;
    name: string; 
}

interface IQueuedTrackInfo {

    track: ITrackInfo;
    queuedTimestamp: any;
    requester: IUserInfo | null;
}

interface IRoomInfo {

    id: string;
    name: string;
    creator: IUserInfo;
    currentTrack: IQueuedTrackInfo | null;
    playbackState: any;
    currentTrackPosition: any;
    upcomginQueueLength: number;
    listenerCount: number;   
}

class Track {

    private readonly _info: ITrackInfo;
    private readonly _album: Album | null;
    private readonly _artist: Artist;

    constructor(info: ITrackInfo) {
        this._info = info;

        if (info.album) {
            this._album = new Album(info.album);
        }
        this._artist = new Artist(info.artist);
    }   

    get id(): string {
        return this._info.id;
    }

    get title(): string {
        return this._info.title;
    }

    get duration(): any {
        return this._info.duration;
    }

    get album(): Album | null {
        return this._album;
    }

    get artist(): Artist {
        return this._artist;
    }
}

class Album {
    private readonly _info: IAlbumInfo;

    constructor(info: IAlbumInfo) {
        this._info = info;
    }

    get id(): string {
        return this._info.id;
    }

    get name(): string {
        return this._info.name;
    }
}

class Artist {
    private readonly _info: IArtistInfo;

    constructor(info: IArtistInfo) {
        this._info = info;
    }

    get id(): string {
        return this._info.id;
    }

    get name(): string {
        return this._info.name;
    }
    
}

class QueuedTrack {

    private readonly _info: IQueuedTrackInfo;
    private readonly _track: Track;

    constructor(info: IQueuedTrackInfo) {
        this._info = info;
        this._track = new Track(info.track);
    }

    get track(): Track {
        return this._track;
    }
}

class Room {

    private readonly _client: KookboxClient;
    private readonly _info: IRoomInfo;
    private _currentTrack: QueuedTrack | null;

    constructor(client: KookboxClient, info: IRoomInfo) {
        this._client = client;
        this._info = info;

        if (info.currentTrack) {
            this._currentTrack = new QueuedTrack(info.currentTrack);
        }
    }

    get id(): string {
        return this._info.id;
    }

    get name(): string {
        return this._info.name;
    }

    get currentTrack(): QueuedTrack | null {
        return this._currentTrack;
    }
}

class ActiveRoom {

    private readonly _room: Room;

    constructor(room: Room) {
        this._room = room;
    }

    get room(): Room {
        return this._room;
    }
}

class KookboxClient {

    private _socket: WebSocket;
    private _router: IMessageRouter;
    private _rooms: Room[];
    private _activeRoom: ActiveRoom;

    constructor(private readonly address: string) {

        this._router = {
            1: this.onConnectionResponse,
            2: this.onTrackStarted
        };
    }

    connect() {

        this._socket = new WebSocket(this.address);
        this._socket.onopen = this.onSocketOpen;
        this._socket.onmessage = this.onSocketMessage;
        this._socket.onclose = this.onSocketClosed;
        this._socket.onerror = this.onSocketError;
    }

    private onConnectionResponse = (message: IMessage) => {

        const activeRoom = (<IConnectionResponse>message.payload).activeRoom;
        if (activeRoom) {
            const room = new Room(this, activeRoom);
            this._rooms = [room];
            this._activeRoom = new ActiveRoom(room);
        }
        console.log("connection response!");
    }

    private onTrackStarted = (message: IMessage) => {

        console.log("track started"); 
    }

    private onSocketOpen = () => {

        console.log("socket opened");
    }

    private onSocketMessage = (ev: MessageEvent) => {

        console.log(ev.data);

        const message = JSON.parse(ev.data) as IMessage;
        this._router[message.messageType](message);
    }

    private onSocketClosed = () => {

        console.log("socket closed");
    }

    private onSocketError = () => {

        console.log("socket error"); 

    }
}