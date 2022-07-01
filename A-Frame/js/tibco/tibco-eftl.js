
AFRAME.registerComponent('tibco-eftl', {
  schema: {
    URL: {type: 'string', default: "EFTL_URL1"},
    key: {type: 'string', default: "EFTL_KEY1"},
    matcher: {type: 'string',default: ''}
  },
  init: function () {

    const URL = credentials[this.data.URL];
    const key = credentials[this.data.key];
    if (URL === undefined) { throw (`${this.data.URL} is not defined in credentials.js`)};
    if (key === undefined) { throw (`${this.data.key} is not defined in credentials.js`)};
    console.log(` Login with \n${this.data.key} = ${key} \n${this.data.URL} = ${URL} `);
    eFTL.connect(URL, {
      password: key,
      clientId: 'art-'+(Math.random()*100000000).toString(),
      onConnect: function(connection) {
        console.log('Connected to TIBCO Cloud Messaging');

        this.connection = connection;

        // sensor data subscription
        this.connection.subscribe({
          matcher: this.data.matcher,
          durable: '',
          onMessage: function(msg) {

            this.msg = msg;
            this.newmsg = true;
            this.el.emit("msg",msg);
            console.log('Received message from EFTL: ' + msg.toString());
          }.bind(this),
          onSubscribe: function(id) {
          },
          onError: function(id, code, reason) {
            console.log('Subscribe error: ' + reason);
          }
        });

        // Disconnect when navigating away.
        //
        window.addEventListener('unload', function() {
          this.connection.disconnect();
        }.bind(this));
      }.bind(this),
      onDisconnect: function(connection, code, reason) {
        console.log('Disconnected: ' + reason);
      },
    });
  }
});
