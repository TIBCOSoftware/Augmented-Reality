
AFRAME.registerComponent('tibco-eftl', {
  schema: {
    URL: {type: 'string'},
    key: {type: 'string'},
    matcher: {type: 'string',default: ''}
  },
  init: function () {
    console.log(` Login with ${this.data.key}`);
    eFTL.connect(this.data.URL, {
      password: this.data.key,
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
