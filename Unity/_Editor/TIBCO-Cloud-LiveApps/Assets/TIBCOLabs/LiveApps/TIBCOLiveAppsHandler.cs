using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;


// created from Unity3dAzure.WebSockets 

// This handler takes the web socket data bytes and raises a received data event.
// This allows us to parse the bytes data in one place and then raise an event to feed game object recievers or a controller to target multiple game objects.
namespace TIBCO.LABS.LIVEAPPS
{
    public class TIBCOLiveAppsHandler : MonoBehaviour, ICaseHandler
    {

        public TIBCOLiveAppsConnector connector;
       
        
        
        public virtual void ConnectionReady()
        {
            Debug.Log("LiveApps connection is ready ");
        }
        public void GetAllCases(string applicationName, string stateName, string searchString, bool getArtifacts = false)
        {
            connector.GetAllCases(applicationName, stateName, searchString, getArtifacts);
        }




        #region Unity lifecycle

        // Web Socket data handler
        void OnEnable()
        {
           
            connector.OnReady += ConnectionReady;
        }

        void OnDisable()
        {
          
            connector.OnReady -= ConnectionReady;
        }

        #endregion
    }
}
