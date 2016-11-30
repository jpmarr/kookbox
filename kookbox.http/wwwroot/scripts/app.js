var __extends = (this && this.__extends) || function (d, b) {
    for (var p in b) if (b.hasOwnProperty(p)) d[p] = b[p];
    function __() { this.constructor = d; }
    d.prototype = b === null ? Object.create(b) : (__.prototype = b.prototype, new __());
};
/// <reference path="../../typings/react/react.d.ts" />
var KookboxApp = (function (_super) {
    __extends(KookboxApp, _super);
    function KookboxApp() {
        _super.apply(this, arguments);
    }
    KookboxApp.prototype.render = function () {
        return React.createElement("div", null, this.props.test);
    };
    return KookboxApp;
}(React.Component));
var socket = new WebSocket("ws://localhost:5000");
socket.onopen = function () {
    console.log("socket opened");
    socket.send(JSON.stringify({
        messageType: 1,
        version: 1,
        payload: {}
    }));
};
socket.onmessage = function (msg) {
    console.log(msg.data);
};
socket.onerror = function (evt) {
    console.log("error:" + evt);
};
socket.onclose = function () {
    console.log("socket closed");
};
//# sourceMappingURL=app.js.map