var KookboxClient = (function () {
    function KookboxClient(address) {
        var _this = this;
        this.address = address;
        this.onConnectionResponse = function (message) {
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
            _this.router[message.messageType](message);
        };
        this.onSocketClosed = function () {
            console.log("socket closed");
        };
        this.onSocketError = function () {
            console.log("socket error");
        };
        this.router = {
            1: this.onConnectionResponse,
            2: this.onTrackStarted
        };
    }
    KookboxClient.prototype.connect = function () {
        this.socket = new WebSocket(this.address);
        this.socket.onopen = this.onSocketOpen;
        this.socket.onmessage = this.onSocketMessage;
        this.socket.onclose = this.onSocketClosed;
        this.socket.onerror = this.onSocketError;
    };
    return KookboxClient;
}());
//# sourceMappingURL=KookboxClient.js.map