/// <reference path="../../typings/react/react.d.ts" />
interface IKookboxAppProps {
    test: string;
}

class KookboxApp extends React.Component<IKookboxAppProps, {}> {
    render() {
        return <div>{this.props.test}</div>;
    }
}

var socket = new WebSocket("ws://localhost:5000");
socket.onopen = () => {
    console.log("socket opened");
    socket.send(JSON.stringify({
        messageType: 1,
        version: 1,
        payload: {
        }
    }));
};
socket.onmessage = msg => {
    console.log(msg.data);
};
socket.onerror = evt => {
    console.log(`error:${evt}`);
};
socket.onclose = () => {
    console.log("socket closed");
};