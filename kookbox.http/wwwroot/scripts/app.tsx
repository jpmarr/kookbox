/// <reference path="../../typings/react/react.d.ts" />
interface IKookboxAppProps {
    test: string;
}

class KookboxApp extends React.Component<IKookboxAppProps, {}> {
    render() {
        return <div>{this.props.test}</div>;
    }
}

let socket = new WebSocket("ws://localhost:5000");
socket.onmessage = msg => {
    //console.log(msg.data);
};