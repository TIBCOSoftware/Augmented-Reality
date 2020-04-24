using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TIBCO.LABS.EFTL;

public class InteractionEventPublisher : DataHandler
{

    #region IDataHandler methods
    

    public override void CloudMessagingReady()
    {
        Debug.Log("TCM is ready ");
        JsonObject message = new JsonObject();

        message["demo_tcm"] = "Unity demo starting";


        Publish(message);
    }
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnBecameVisible()
    {
        Debug.Log("Object is Visible");
        JsonObject message = new JsonObject();

        message["app_id"] = "demo"; 
        message["source"] = "object";
        message["source_id"] = gameObject.name;
        message["event"] = "BecameVisible";
        message["timestamp"] = System.DateTime.Now;

        Publish(message);
    }
    void OnBecameInvisible()
    {
        Debug.Log("Object is Invisible");
        JsonObject message = new JsonObject();

        message["app_id"] = "demo";
        message["source"] = "object";
        message["source_id"] = gameObject.name;
        message["event"] = "BecameInvisible";
        message["timestamp"] = System.DateTime.Now;

        Publish(message);
    }
}
