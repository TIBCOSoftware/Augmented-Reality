using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Unity3dAzure.WebSockets
{
    public sealed class UnityWebSocketScriptDemo : UnityWebSocket
    {
        //const int IDLE_LIMIT = 180; // Update is invoked 60 times per seconds.
        const int IDLE_LIMIT = 360; // Update is invoked 30 times per seconds.

        // Unity Inspector fields
        [SerializeField]
        private string webSocketUri = "wss://messaging.cloud.tibco.com/tcm/TIB_SUB_<your sub id>/channel";
        [SerializeField]
        private string authkey = "<your key>";

        public GameObject msgText;
        
        [SerializeField]
        private bool AutoConnect = false;
        private string msg;
        private string power = "0";
        private int idle = UnityWebSocketScriptDemo.IDLE_LIMIT;
        
        #region Web Socket connection

        void Start()
        {
            // Config Websocket
            WebSocketUri = webSocketUri;
            Password = authkey;
            // Headers = headers;

            if (AutoConnect)
            {
                Connect();
            }
        }

        public void ReConnect()
        {
            Connect();
        }

        void Update()
        {
            msgText.transform.GetComponent<TextMesh>().text = power;

            if (idle == 0)
            {
                msgText.transform.GetComponent<TextMesh>().text = "0";
            }
                    
            if (idle == UnityWebSocketScriptDemo.IDLE_LIMIT)
            {
                idleState();
            }

            if (idle <= UnityWebSocketScriptDemo.IDLE_LIMIT) {
                idle += 1;
            }
        }

        private void idleState()
        {
            Debug.Log("FTL Idle State");
            power = "waiting ...";
        }
        void OnDisable()
        {
            Close();
        }
        #endregion

        #region Web Socket handlers
        protected override void handleMessage(JsonObject message)
        {
            try
            {
                JsonObject body;
                body = (JsonObject)message["body"];
                Debug.Log("FTL handleMessage " + body);

                if (body.ContainsKey("dataType"))
                {
                    string dataType = body["dataType"].ToString();
                    Debug.Log("FTL Message received: " + dataType);

                    switch (dataType)
                    {
                        
                        case "Power":
                            SetPower(body);
                            break;
                        
                        default:
                            Debug.Log("FTL Message received " + dataType);
                            break;
                    }
                }
                this.msg = body.ToString();

            } catch (Exception e)
            {
                Debug.Log("FTL Error in handleMessage " + e.Message);
            }

        }
        
        private void SetPower(JsonObject body)
        {
            // *** Sample {"dataType":"Power","data":"130"}
            power = body["data"].ToString(); 
            Debug.Log("Data " + power );   
        }

        protected override void OnWebSocketOpen(object sender, EventArgs e)
        {
            this.msg = "Web socket is open";
            Debug.Log("FTL Open in Script");
            base.OnWebSocketOpen(sender, e);
        }
        protected override void OnWebSocketError(object sender, WebSocketErrorEventArgs e)
        {
            Debug.LogError("FTL Web socket error: " + e.Message);
            this.msg = e.Message;
            DisconnectWebSocket();
        }
        
        protected override void OnWebSocketClose (object sender, WebSocketCloseEventArgs e) {
          Debug.Log ("FTL Web socket closed with reason: " + e.Reason + " !");
          if (!e.WasClean) {
                DisconnectWebSocket ();
            }
            DettachHandlers();

            //Reconnect in case it is no usual Disconnect
            if (e.Reason != " 1005") {
                ReConnect();
            } 
        }

        /*
        protected override void OnWebSocketMessage (object sender, WebSocketMessageEventArgs e) {
          Debug.LogFormat ("Web socket {1} message:\n{0}", e.Data, e.IsBinary ? "binary" : "string");
          // Raise web socket data handler event
          if (OnData != null) {
            OnData (e.RawData, e.Data, e.IsBinary);
          }
        }
        */

        #endregion

    }

}
