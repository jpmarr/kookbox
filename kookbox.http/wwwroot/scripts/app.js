/// <reference path="../../typings/react/react.d.ts" />
/// <reference path="kookboxclient.ts" />
class KookboxApp extends React.Component {
    render() {
        return React.createElement("div", null, this.props.test);
    }
}
let client = new KookboxClient("ws://localhost:5000");
client.connect();
//# sourceMappingURL=app.js.map