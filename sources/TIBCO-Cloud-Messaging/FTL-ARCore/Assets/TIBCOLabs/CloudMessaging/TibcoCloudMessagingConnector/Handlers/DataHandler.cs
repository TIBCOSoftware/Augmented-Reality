using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


// created from Unity3dAzure.WebSockets 

// This handler takes the web socket data bytes and raises a received data event.
// This allows us to parse the bytes data in one place and then raise an event to feed game object recievers or a controller to target multiple game objects.
namespace TIBCO.LABS.EFTL {
  public  class DataHandler : MonoBehaviour, IDataHandler {

        public TibcoCloudMessagingConnector cloudMessagingConnector;
        // Override OnData method in your own subclass to pass any custom event args
        public  virtual void OnData(JsonObject message)
        {
      Debug.Log("handleMessage " + message);
    }
    

    

    #region Unity lifecycle

    // Web Socket data handler
    void OnEnable () {
            cloudMessagingConnector.OnEftlMessage += OnData;
    }

    void OnDisable () {
            cloudMessagingConnector.OnEftlMessage -= OnData;
    }

    #endregion
  }
}
