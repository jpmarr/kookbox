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
};
socket.onmessage = msg => {
    console.log(msg.data);
    return false;
};
socket.onerror = evt => {
    console.log(`error:${evt}`);
};
socket.onclose = () => {
    console.log("socket closed");
};