using System;
using System.Timers;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

using TIBCO.EFTL;

public class Subscriber 
{

    public bool loaded = false;
    public Subscriber(String url, String key, IEFTLReceiver controller, Text info)
    {
        // Connection properties.
        


        Hashtable props = new Hashtable();
        props.Add(EFTL.PROPERTY_PASSWORD, key);

        // Durable subscriptions require a unique client identifier.
        //
        props.Add(EFTL.PROPERTY_CLIENT_ID, "unity"+System.Guid.NewGuid());

        // Start a connection to TIBCO Cloud Messaging.
        //
        // ConnectionListener.OnConnect() is invoked when
        // successfully connected.
        //
        // ConnectionListener.OnDisconnect() is invoked when
        // the connection fails or becomes disconnected.
        //
        info.text += "\nEFTL.Connect";
        try { 
        EFTL.Connect(url, props, new ConnectionListener(controller, info));
        }
        catch (Exception e)
        {
            info.text = "Error : "+e.ToString();

        }

    }

    public class ConnectionListener : IConnectionListener
    {
        private IEFTLReceiver unityObject;
        private Text info;
        public  ConnectionListener(IEFTLReceiver controller,  Text info)
        {
            this.unityObject = controller;
            this.info = info;
            this.info.text += "\nEFTL Connection Listener created";
        }
        public void OnConnect(IConnection connection)
        {
            this.info.text = "Connected to TIBCO Cloud Messaging";
            this.unityObject.OnConnect();
            //Debug.Log("Connected to TIBCO Cloud Messaging");
            try
            {
                // just channel "unity"
                //connection.Subscribe("{}", "unity", new SubscriptionListener(connection, this.unityObject, this.info));

                // all channels
                connection.Subscribe("{}", "", new SubscriptionListener(connection, this.unityObject, this.info));
            }
            catch (Exception e)
            {
                this.info.text = "Error : " + e.ToString();
            }
        }

        public void OnDisconnect(IConnection connection, int code, String reason)
        {
            this.info.text="Disconnected from TIBCO Cloud Messaging: " + reason;
        }

        public void OnError(IConnection connection, int code, String reason)
        {
            this.info.text="Connection error: " + reason;
        }

        public void OnReconnect(IConnection connection)
        {
            this.info.text="Reconnected to eFTL server";
        }
    }

    public class SubscriptionListener : ISubscriptionListener
    {
        IConnection connection;
        private IEFTLReceiver unityObject;
        private Text info;
        string subscriptionId;
        public SubscriptionListener(IConnection connection, IEFTLReceiver controller, Text info)
        {
            this.connection = connection;
            this.unityObject = controller;
            this.info = info;
            this.info.text += "Listener created ";
        }

        public void OnError(String subscriptionId, int code, String reason)
        {
            this.info.text = "Subscription error: " + reason;

            // Disconnect from TIBCO Cloud Messaging.
            connection.Disconnect();
        }

        public void OnMessages(IMessage[] messages)
        {
            this.info.text = "messages received";
            foreach (IMessage msg in messages)
            {
                Debug.Log("Received message:\n" + msg);
                // ExecuteEvents.Execute<ICustomMessageTarget>(StatusObject, null, (x, y) => x.Message1());
                //
                this.unityObject.receiveEFTL(messages);
                Debug.Log("Status message updated:\n" + msg);
            }
        }

        public void OnSubscribe(String subscriptionId)
        {
            this.subscriptionId = subscriptionId;
            this.info.text = "Subscription done";
            // Unsubscribe and disconnect after a time in seconds.
            //Timer timer = new Timer(30000);
            //timer.Elapsed += cleanupTask;
            //timer.Start();
        }

        public void cleanupTask(object source, ElapsedEventArgs e)
        {
            // Remove the durable subscription.
            connection.Unsubscribe(this.subscriptionId);

            // Disconnect from TIBCO Cloud Messaging.
            connection.Disconnect();
        }        
    }
}
