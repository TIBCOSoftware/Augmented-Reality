
AFRAME.registerComponent("robot-arm", {
  schema: {
    baserotation: {type: 'number', default: 0.0},
    armrotation: {type: 'number',default: 0.0},
    forearmrotation: {type: 'number',default: 0.0}
  },
    init: function ()
    {

      var base = document.createElement('a-gltf-model');
      base.setAttribute('src', `assets/arm/arm-01.glb`);
      this.rotating = document.createElement('a-gltf-model');
      this.rotating.setAttribute('src', `assets/arm/arm-02.glb`);
      //rotating.setAttribute('shadow',"cast: true;receive: false");
      this.rotating.object3D.position.z = 0.0;
      this.rotating.object3D.position.x = 0.0;
      this.rotating.object3D.position.y = 0.35;


      this.connecting = document.createElement('a-gltf-model');
      this.connecting .setAttribute('src', `assets/arm/arm-connecting.gltf`);
      //connecting.setAttribute('shadow',"cast: true;receive: false");

      this.connecting .object3D.position.x = -0.17;
      this.connecting .object3D.position.y = 0.25;
      this.connecting .object3D.position.z = 0.0;

      this.forearm = document.createElement('a-gltf-model');
      this.forearm.setAttribute('src', `assets/arm/arm-04.glb`);
      this.forearm.object3D.position.x = 0.0;
      this.forearm.object3D.position.y = 0.817;
      this.forearm.object3D.position.z = 0.0;

      //this.el.appendChild(forearm);
      this.rotating.appendChild(this.connecting );
      this.connecting .appendChild(this.forearm);
      base.appendChild(this.rotating);
      this.el.appendChild(base);
      this.position(this.data.baserotation, this.data.armrotation, this.data.forearmrotation);
    },
    position: function( baserotation,armrotation, forearmrotation ) {
      const DEG2RAD = Math.PI / 180;
      this.rotating.object3D.rotation.y = baserotation * DEG2RAD;
      this.connecting.object3D.rotation.z = armrotation * DEG2RAD;
      this.forearm.object3D.rotation.z = forearmrotation * DEG2RAD;

    },
    tick: function() {
      this.position(this.data.baserotation, this.data.armrotation, this.data.forearmrotation);
    }
});
