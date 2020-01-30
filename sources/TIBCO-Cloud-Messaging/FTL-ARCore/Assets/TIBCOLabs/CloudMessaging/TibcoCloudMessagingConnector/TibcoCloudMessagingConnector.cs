using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

// created from Unity3dAzure.WebSockets 
using Unity3dAzure.WebSockets; //  used for webSockets Events

namespace TIBCO.LABS.EFTL
{
    // ProtocolOpConstants

    // ProtocolConstants
    internal static class ProtocolOpConstants
    {
        // Access Point protocol op codes
        internal const int OP_HEARTBEAT = 0;
        internal const int OP_LOGIN = 1;
        internal const int OP_WELCOME = 2;
        internal const int OP_SUBSCRIBE = 3;
        internal const int OP_SUBSCRIBED = 4;
        internal const int OP_UNSUBSCRIBE = 5;
        internal const int OP_UNSUBSCRIBED = 6;
        internal const int OP_EVENT = 7;
        internal const int OP_MESSAGE = 8;
        internal const int OP_ACK = 9;
        internal const int OP_ERROR = 10;
        internal const int OP_DISCONNECT = 11;
        internal const int OP_GOODBYE = 12;
        internal const int OP_MAP_SET = 20;
        internal const int OP_MAP_GET = 22;
        internal const int OP_MAP_REMOVE = 24;
        internal const int OP_MAP_RESPONSE = 26;
    }
    internal static class ProtocolConstants
    {
        // WebSocket protocol
        internal static readonly String EFTL_WS_PROTOCOL = "v1.eftl.tibco.com";

        // Access Point protocol field name
        internal static readonly String OP_FIELD = "op";
        internal static readonly String USER_FIELD = "user";
        internal static readonly String PASSWORD_FIELD = "password";
        internal static readonly String CLIENT_ID_FIELD = "client_id";
        internal static readonly String CLIENT_TYPE_FIELD = "client_type";
        internal static readonly String CLIENT_VERSION_FIELD = "client_version";
        internal static readonly String HEARTBEAT_FIELD = "heartbeat";
        internal static readonly String TIMEOUT_FIELD = "timeout";
        internal static readonly String MAX_SIZE_FIELD = "max_size";
        internal static readonly String MATCHER_FIELD = "matcher";
        internal static readonly String DURABLE_FIELD = "durable";
        internal static readonly String ACK_FIELD = "ack";
        internal static readonly String ERR_CODE_FIELD = "err";
        internal static readonly String REASON_FIELD = "reason";
        internal static readonly String ID_FIELD = "id";
        internal static readonly String IDS_FIELD = "ids";
        internal static readonly String MSG_FIELD = "msg";
        internal static readonly String TO_FIELD = "to";
        internal static readonly String BODY_FIELD = "body";
        internal static readonly String SEQ_NUM_FIELD = "seq";
        internal static readonly String RESUME_FIELD = "_resume";
        internal static readonly String LOGIN_OPTIONS_FIELD = "login_options";
        internal static readonly String ID_TOKEN_FIELD = "id_token";
        internal static readonly String SERVICE_FIELD = "_service";
        internal static readonly String QOS_FIELD = "_qos";
        internal static readonly String MAP_FIELD = "map";
        internal static readonly String KEY_FIELD = "key";
        internal static readonly String VALUE_FIELD = "value";

        // Data loss service
        internal static readonly String DATALOSS_TYPE = "data_loss";
        internal static readonly String COUNT_FIELD = "count";

        // Presence service
        internal static readonly String IDENTITY_PRESENCE_TYPE = "identity_presence";
        internal static readonly String GROUP_PRESENCE_TYPE = "group_presence";
        internal static readonly String ACTION_FIELD = "_action";
        internal static readonly String IDENTITY_FIELD = "_identity";
        internal static readonly String GROUP_FIELD = "_group";
        internal static readonly String REQUEST_FIELD = "_request";
        internal static readonly String ACTION_JOIN = "join";
        internal static readonly String ACTION_LEAVE = "leave";
        internal static readonly String ACTION_TIMEOUT = "timeout";
        internal static readonly String REQUEST_CURRENT = "current";
    }

    public  class TibcoCloudMessagingConnector : MonoBehaviour
    {
        public delegate void MessageReceived (JsonObject message);
        public  MessageReceived OnEftlMessage;

        [Header("Connection Details")]

        [SerializeField]
        protected string FTLSocketUri = "wss://messaging.cloud.tibco.com/tcm/TIB_SUB_[your-id]/channel";
        [SerializeField]
        protected string AuthKey;

        protected string Client_id;
        protected List<UnityKeyValue> Headers;

        protected IWebSocket _ws;

        protected bool isAttached = false;

        #region MonoBehavior methods
        void OnEnable()
        {
            Connect(); 
        }
        void OnDisable()
        {
            Close();
        }

        #endregion

        #region Web Socket methods

        public virtual void Connect()
        {
            // Client_id and Password will be used in OnWebSocketOpen to do the authentication with TCM FTL
            Client_id = "unity-" + System.Guid.NewGuid();
            ConnectWebSocket();
        }

        public virtual void Close()
        {
            DisconnectWebSocket();
        }

        #endregion

        #region Web Socket handlers

        protected virtual void OnWebSocketOpen(object sender, EventArgs e)
        {
            string msg = "{\"op\":1,\"client_type\":\"c#\",\"client_verion\":\"3.4\",\"password\":\"";
            msg += AuthKey + "\",\"client_id\":\"" + "unity" + Client_id + "\"}";
            Debug.Log("FTL socket is open: " + msg);
            SendText(msg);

            /*
             JsonObject message = new JsonObject();

                message[ProtocolConstants.OP_FIELD]              = ProtocolOpConstants.OP_LOGIN;
                message[ProtocolConstants.CLIENT_TYPE_FIELD]     = "c#";
                message[ProtocolConstants.CLIENT_VERSION_FIELD]  = Version.EFTL_VERSION_STRING_SHORT;

                if (user != null)
                    message[ProtocolConstants.USER_FIELD] = user;
                if (password != null)
                    message[ProtocolConstants.PASSWORD_FIELD] = password;

                if (clientId != null && reconnectId != null) 
                {
                    message[ProtocolConstants.CLIENT_ID_FIELD] = clientId;
                    message[ProtocolConstants.ID_TOKEN_FIELD] = reconnectId;
                } 
                else if (identifier != null) 
                {
                    message[ProtocolConstants.CLIENT_ID_FIELD] = identifier;
                }

                JsonObject loginOptions = new JsonObject();

                // add resume when auto-reconnecting
                if (Interlocked.Read(ref reconnecting) == 1)
                    loginOptions[ProtocolConstants.RESUME_FIELD] = "true";

                try 
                {
                    // add user properties
                    foreach (String key in props.Keys) 
                    {
                        if (key.Equals(EFTL.PROPERTY_USERNAME) ||
                            key.Equals(EFTL.PROPERTY_PASSWORD) ||
                            key.Equals(EFTL.PROPERTY_CLIENT_ID))
                            continue;

                        String val = (String) props[key];

                        if (String.Compare("true", val) == 0)
                            loginOptions[key] = "true";
                        else
                            loginOptions[key] = val;
                    }
                } 
                catch(Exception) 
                {
                    //Console.WriteLine(e.StackTrace);
                }

                message[ProtocolConstants.LOGIN_OPTIONS_FIELD] = loginOptions;

                try
                {
                    webSocket.send(message.ToString());
                } 
                catch(Exception) 
                {

                }
             */

        }

        protected virtual void OnWebSocketClose(object sender, WebSocketCloseEventArgs e)
        {
            Debug.Log("FTL socket closed with reason: " + e.Reason );
            DettachHandlers();

            // reconnect in case of unwanted Disconnect Errors
            if (e.Code == 1006) // eFTLConnection Class Reference - Connection error (1006). Programs may attempt to reconnect.
            {
               Debug.Log("FTL socket reconnect!");
               Connect();
            }

            // handle standard Unity3D Runtime Stop
            if (e.Code == 1005) // Unity 3D App/Player stopped.
            {
                Debug.Log("FTL socket fully closed, because App stopped.");
            }
        }

        protected virtual void OnWebSocketMessage(object sender, WebSocketMessageEventArgs e)
        {
            //Debug.LogFormat("FTL socket {1} message:\n{0}", e.Data, e.IsBinary ? "binary" : "string");

            object obj = JsonValue.Parse(e.Data);

            if (obj is JsonArray)
            {
                Debug.Log("FTL Message Array received");
            }
            else if (obj is JsonObject)
            {
                JsonObject message = (JsonObject)obj;

                object op;

                if (message.TryGetValue(ProtocolConstants.OP_FIELD, out op))
                {
                    switch (Convert.ToInt32(op))
                    {
                        case ProtocolOpConstants.OP_HEARTBEAT:
                            //handleHeartbeat(message);
                            Debug.Log("** FTL Heartbeat Message");
                            break;
                        case ProtocolOpConstants.OP_WELCOME:
                            handleWelcome(message);
                            break;
                        case ProtocolOpConstants.OP_SUBSCRIBED:
                            //handleSubscribed(message);
                            break;
                        case ProtocolOpConstants.OP_UNSUBSCRIBED:
                            //handleUnsubscribed(message);
                            break;
                        case ProtocolOpConstants.OP_EVENT:
                            handleMessage(message);
                            break;
                        case ProtocolOpConstants.OP_ERROR:
                            //handleError(message);
                            break;
                        case ProtocolOpConstants.OP_GOODBYE:
                            //handleGoodbye(message);
                            break;
                        case ProtocolOpConstants.OP_ACK:
                            // handleAck(message);
                            break;
                        case ProtocolOpConstants.OP_MAP_RESPONSE:
                            // handleMapResponse(message);
                            break;
                    }
                }
            }
            
        }
        protected virtual void handleMessage(JsonObject message)
        {
            //Debug.Log("OP_EVENT");
            JsonObject body = (JsonObject)message["body"];
            // Raise FTL socket data handler event
            if (OnEftlMessage != null)
            {
                OnEftlMessage(body);
            }
        }
        private void handleWelcome(JsonObject message)
        {
            JsonObject submessage = new JsonObject();

            submessage[ProtocolConstants.OP_FIELD] = ProtocolOpConstants.OP_SUBSCRIBE;
            submessage[ProtocolConstants.ID_FIELD] = "s." + System.Guid.NewGuid();

            Debug.Log("FTL Welcome Message: " + submessage.ToString());

            SendText(submessage.ToString());
        }


        protected virtual void OnWebSocketError(object sender, WebSocketErrorEventArgs e)
        {
            Debug.LogError("FTL socket error: " + e.Message);
            DisconnectWebSocket();
        }

        #endregion

        public void SendText(string text, Action<bool> callback = null)
        {
            if (_ws == null || !_ws.IsOpen())
            {
                Debug.LogWarning("FTL socket is not available to send text message. Try connecting?");
                return;
            }
            _ws.SendAsync(text, callback);
        }

        public void SendUTF8Text(string text, Action<bool> callback = null)
        {
            byte[] data = Encoding.UTF8.GetBytes(text);
            SendBytes(data, callback);
        }

        public virtual void SendInputText(InputField inputField)
        {
            SendText(inputField.text);
        }

        public void SendBytes(byte[] data, Action<bool> callback = null)
        {
            if (_ws == null || !_ws.IsOpen())
            {
                Debug.LogWarning("FTL socket is not available to send bytes. Try connecting?");
                return;
            }
            _ws.SendAsync(data, callback);
        }

        protected void ConnectWebSocket()
        {
            if (string.IsNullOrEmpty(FTLSocketUri))
            {
                Debug.LogError("FTL SocketUri must be set");
                return;
            }

            var customHeaders = new List<KeyValuePair<string, string>>();
            if (Headers != null)
            {
                foreach (UnityKeyValue header in Headers)
                {
                    customHeaders.Add(new KeyValuePair<string, string>(header.key, header.value));
                }
            }

#if ENABLE_WINMD_SUPPORT
        Debug.Log ("Using UWP Web Socket");
        _ws = new WebSocketUWP();
#elif UNITY_EDITOR || ENABLE_MONO
        Debug.Log("Using Mono FTL Socket");
        _ws = new WebSocketMono();
#endif

            if (!isAttached)
            {
                _ws.ConfigureWebSocket(FTLSocketUri, customHeaders);
                Debug.Log("Connect FTL Socket: " + _ws.Url());
                AttachHandlers();
                _ws.ConnectAsync();
            }
        }

        protected void DisconnectWebSocket()
        {
            if (_ws != null && isAttached)
            {
                Debug.Log("Disconnect FTL Socket");
                _ws.CloseAsync();
            }
        }

        protected void AttachHandlers()
        {
            if (isAttached)
            {
                return;
            }
            isAttached = true;
            _ws.OnError += OnWebSocketError;
            _ws.OnOpen += OnWebSocketOpen;
            _ws.OnMessage += OnWebSocketMessage;
            _ws.OnClose += OnWebSocketClose;
        }

        protected void DettachHandlers()
        {
            isAttached = false;
            _ws.OnError -= OnWebSocketError;
            _ws.OnOpen -= OnWebSocketOpen;
            _ws.OnMessage -= OnWebSocketMessage;
            _ws.OnClose -= OnWebSocketClose;
        }

    }

}
