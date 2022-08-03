
// place object on top of an object
AFRAME.registerComponent('on-top', {
  schema: {
    target: {
      type: 'string'
    }
  },
  init: function() {
    var el = this.el;
    this.baseObject = document.querySelector(this.data.target).object3D;
    this.baseObjectBox = new THREE.Box3().setFromObject(this.baseObject);


    el.addEventListener('model-loaded', function(e) {
      // compute bounding box
      var obj = this.el.getObject3D('mesh');
      // compute bounding box
      try {
        var bbox = new THREE.Box3().setFromObject(obj);
      //console.log(bbox.min, bbox.max);
        this.el.object3D.position.x = this.baseObject.position.x;
        this.el.object3D.position.y = this.baseObjectBox.max.y - bbox.min.y;
        this.el.object3D.position.z = this.baseObject.position.z;
      } catch (e) {
        this.el.object3D.position.x = this.baseObject.position.x;
        this.el.object3D.position.y = this.baseObjectBox.max.y;
        this.el.object3D.position.z = this.baseObject.position.z;
      }

    }.bind(this));
  }
});
// if raycaster is pointing at this object, press trigger to change color
AFRAME.registerComponent("raycaster-color-change", {
    init: function ()
    {
        this.colors = ["red", "orange", "yellow", "green", "blue", "violet"];

        this.controllerData = document.querySelector("#controller-data").components["controller-listener"];
        this.hoverData      = this.el.components["raycaster-target"];
    },

    tick: function()
    {
        if (this.hoverData.hasFocus && this.controllerData.rightTrigger.pressed )
        {
            let index = Math.floor( this.colors.length * Math.random() );
            let color = this.colors[index];
            this.el.setAttribute("color", color);
        }

        if (!this.hoverData.hasFocus || this.controllerData.rightTrigger.released)
        {
            this.el.setAttribute("color", "#CCCCCC");
        }
    }
});
