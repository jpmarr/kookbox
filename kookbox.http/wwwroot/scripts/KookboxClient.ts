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

class KookboxClient {

    private socket: WebSocket;
    private router: IMessageRouter;

    constructor(private readonly address: string) {

        this.router = {
            1: this.onConnectionResponse,
            2: this.onTrackStarted
        };
    }

    connect() {

        this.socket = new WebSocket(this.address);
        this.socket.onopen = this.onSocketOpen;
        this.socket.onmessage = this.onSocketMessage;
        this.socket.onclose = this.onSocketClosed;
        this.socket.onerror = this.onSocketError;
    }

    private onConnectionResponse = (message: IMessage) => {

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
        this.router[message.messageType](message);
    }

    private onSocketClosed = () => {

        console.log("socket closed");
    }

    private onSocketError = () => {

        console.log("socket error"); 

    }
}