/// <reference path="../../typings/react/react.d.ts" />
/// <reference path="kookboxclient.ts" />

interface IKookboxAppProps {
    test: string;
}

class KookboxApp extends React.Component<IKookboxAppProps, {}> {
    render() {
        return <div>{this.props.test}</div>;
    }
}

let client = new KookboxClient("ws://localhost:5000");
client.connect();  

