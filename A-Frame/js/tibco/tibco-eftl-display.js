AFRAME.registerComponent('tibco-eftl-display', {
  schema: {
    eftlListenerId: {type: 'string'},
    width: {type: 'string', default: '0.5'},
    height: {type: 'string', default: '0.2'}
  },
  init: function () {
    var eftlListener = document.querySelector(this.data.eftlListenerId);
    this.eftlData = eftlListener.components["tibco-eftl"];
    var slate = document.createElement('a-entity');
    slate.setAttribute('geometry', `primitive: plane;  width: ${this.data.width}; height: ${this.data.height}`);
    slate.setAttribute('material',"color: #444444; transparent: true; opacity: 0.80;");
    // parent origin will be left bottom corner
    slate.object3D.position.x = this.data.width/2;
    slate.object3D.position.y = this.data.height/2;
    this.el.appendChild(slate);
    // add text box
    this.textbox = document.createElement('a-entity');
    this.textbox.setAttribute('text',`anchor: left; baseline: top; wrapCount: 35; \
    transparent: true; \
    opacity: 1.00; \
    width: ${0.9 * this.data.width} \
    color: #8888FF;`);
    this.textbox.object3D.position.z = 0.005;
    this.textbox.object3D.position.x = 0.05 * this.data.width;
    this.textbox.object3D.position.y = this.data.height-0.05*this.data.height;
    this.el.appendChild(this.textbox);
    eftlListener.addEventListener("msg", this.newMessage.bind(this));

  },
  newMessage: function(evt)
  {
      var msg = evt.detail;
      this.textbox.setAttribute( "text", "value", msg);
  }
});
