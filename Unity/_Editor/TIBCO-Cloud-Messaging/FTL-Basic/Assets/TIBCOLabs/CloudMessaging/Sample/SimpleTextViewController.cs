using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

using TIBCO.LABS.EFTL;



// Sample implementation of a Datahandler which is updating a TextMesh when a json message is received on TIBCO Cloud Messaging connector
// expecting a message like {"dataType":"Power","data":"130"}
// test using curl -i -u :[your-key] https://messaging.cloud.tibco.com/tcm/TIB_SUB_[your-id]/channel/v1/publish -d '{"dataType":"Power","data":"130"}
public class SimpleTextViewController : DataHandler
{
    public TextMesh textMesh;
    private string text;
    private bool updateRequired = false;

    #region IDataHandler methods
    public override void OnData(JsonObject message)
    {
        try
        {
            Debug.Log("Received FTL message " + message);

            if (message.ContainsKey("dataType"))
            {

                string dataType = message["dataType"].ToString();
                // Debug.Log("dataType " + dataType);
                // Debug.Log("Message received " + dataType);

                SendTextToDemoTcm("Unity received "+dataType);

                switch (dataType)
                {
                    
                    case "Power":
                        this.text = message["data"].ToString();
                        this.updateRequired = true;
                        break;

                    default:
                        Debug.Log("Unsupported dataType " + dataType);
                        break;

                }
                // publish ModelUpdate provide refreence to this model object

            } else
            {
                Debug.Log("Message ignored ");
            }

        }
        catch (Exception e)
        {
            Debug.Log("Error in handleMessage " + e.Message);
        }
    }
    private  void SendTextToDemoTcm(string msg)
    {
        
        JsonObject message = new JsonObject();

        message["demo_tcm"] = msg;


        this.Publish(message);
    }

    public override void CloudMessagingReady()
    {
        Debug.Log("TCM is ready ");
        SendTextToDemoTcm("Unity demo starting");

    }
    #endregion
    void Start()
    {
        textMesh.text = "waiting ...";
        
    }
    void Update()
    {
        if (!updateRequired)
        {
            return;
        }
        // update TextMesh component on this gameObject
        textMesh.text = text;
        updateRequired = false;
    }
}

