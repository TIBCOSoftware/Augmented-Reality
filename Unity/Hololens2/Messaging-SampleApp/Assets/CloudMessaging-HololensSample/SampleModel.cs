using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TIBCO.LABS.EFTL;



// Your Model implementation : update model data with incoming messages from EFTL
/// <summary>
/// Simple Model and View+Controller implementation
/// the model get updated by external events
/// the view+controller subscribe to the model update info and change view accordingly.
/// In this simple case the View is a simple Text (in a panel).
/// The model has a simple "info" data
/// </summary>
public sealed class SampleModel : DataHandler
{
    public delegate void ModelUpdate(SampleModel sender);
    public event ModelUpdate OnModelUpdate;

    [HideInInspector]
    public string info;
    [HideInInspector]
    public Color objectColor = Color.red;
    private JsonObject eventInfo;
    
   



    #region IDataHandler methods
    public override void OnData(JsonObject message)
    {
        try
        {



            Debug.Log("SampleMode Update " + message);


            if (message.ContainsKey("demo_tcm") && !message.ContainsKey("tibcolabs_art")) // message from Cloud Demo app 
            {

                this.eventInfo = message;
                // check if the message is a command of the form "set color blue" ...
                // it is a very simple implementation and a real message would be a structured json
                var command = this.eventInfo["demo_tcm"].ToString();

                if (command.StartsWith("set color")) {
                    if (command.Contains("blue"))
                    {
                        objectColor = Color.blue;
                    } else if (command.Contains("red"))
                    {
                        objectColor = Color.red;
                    }
                    else if (command.Contains("green"))
                    {
                        objectColor = Color.green;
                    }

                } else
                {
                    this.info = "received message :\n" + command;
                }
                // publish ModelUpdate provide refreence to this model object

                if (OnModelUpdate != null)
                {
                    OnModelUpdate(this);
                }

            }
            if (message.ContainsKey("tibcolabs_art"))
            {

                this.eventInfo = message;
                this.info = "application : "+this.eventInfo["app_id"]+"\nsource : "+this.eventInfo["source"] +"\nid : "+ this.eventInfo["source_id"]+"\nevent "+this.eventInfo["event"];
                if (message.ContainsKey("value"))
                {
                    this.info += "\nvalue : " + this.eventInfo["value"];
                }

                // publish ModelUpdate provide refreence to this model object

                if (OnModelUpdate != null)
                {
                    OnModelUpdate(this);
                }

            }

        }
        catch (Exception e)
        {
            Debug.Log("Error in handleMessage " + e.StackTrace);
        }
    }
    #endregion


    


}

