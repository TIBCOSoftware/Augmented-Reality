using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TIBCO.LABS.EFTL;



// Your Model implementation : update model data with incoming messages from EFTL
// ViewController must subscribe to OnModelUpdate
//
public sealed class BikeModel : DataHandler
{
    public delegate void ModelUpdate(BikeModel sender);
    public  event ModelUpdate OnModelUpdate;

    const int IDLE_LIMIT = 20; // number of mesages received with 0 revolutions


    public int heartRate;
    public long wattsPerHour;
    public string cadence;
    public float speed;
    private long lastSpeedTime = 0L;
    private long lastPower = 0L;
    private long lastSpeedRevolution = 0L;
    private long speedTime = 0L;
   
    
    private long speedRevolution;
    public int idle = IDLE_LIMIT;

    #region IDataHandler methods
    public override void OnData(JsonObject message)
    {
        try
        {



            Debug.Log("BikeMode Update " + message);


            if (message.ContainsKey("dataType"))
            {

                string dataType = message["dataType"].ToString();
                // Debug.Log("dataType " + dataType);
                // Debug.Log("Message received " + dataType);
                switch (dataType)
                {
                    case "HeartRate":
                        SetHeartBeat(message);
                        break;
                    case "Power":
                        SetPower(message);
                        break;
                    case "Speed":
                        SetSpeed(message);
                        break;
                    case "Cadence":
                        SetCadence(message);
                        break;
                    default:
                        Debug.Log("Message received " + dataType);
                        break;
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


    private void SetCadence(JsonObject body)
    {
        // Debug.Log("Cadence deviceType ");
        if (body.ContainsKey("deviceType") && body["deviceType"].ToString() == "11")
        {



            JsonArray data = (JsonArray)body["data"];
            JsonObject val = (JsonObject)data[0];
            this.cadence = val["value"].ToString();
            Debug.Log("Cadence " + this.cadence);
        }

    }
    private void SetPower(JsonObject body)
    {
        JsonArray data = (JsonArray)body["data"];
        JsonObject val = (JsonObject)data[0];
        long power = (long)val["value"]; //Item[0]["value"].ToString();

        wattsPerHour = power - lastPower;

        if (wattsPerHour < 0) { wattsPerHour = 65536L + wattsPerHour; }
        Debug.Log("Power " + power + " diff " + wattsPerHour);
        lastPower = power;

    }
    private void SetHeartBeat(JsonObject body)
    {
        JsonArray data = (JsonArray)body["data"];
        JsonObject val = (JsonObject)data[0];
        heartRate = int.Parse(val["value"].ToString());
        Debug.Log("Heart Rate " + heartRate);



    }
    private void SetSpeed(JsonObject body)
    {
        JsonArray data = (JsonArray)body["data"];
        JsonObject valTime = (JsonObject)data[0];
        JsonObject valRevolution = (JsonObject)data[1];
        this.speedTime = long.Parse(valTime["value"].ToString());
        this.speedRevolution = long.Parse(valRevolution["value"].ToString());
        Debug.Log("Speed Time:" + this.speedTime + " revolution " + this.speedRevolution);
        computeModelData();
        
    }

    private void computeModelData()
    {



        // compute Speed using time and revolution count
       /* if (lastSpeedTime == undefined) { lastSpeedTime = speedTime; }
        if (lastSpeedRevolution == undefined) { lastSpeedRevolution = speedRevolution; }
        */
        long deltaTime = speedTime - lastSpeedTime;
        if (deltaTime < 0) { deltaTime = 65536L + deltaTime; }
        long deltaRevolution = speedRevolution - lastSpeedRevolution;
        if (deltaRevolution < 0) { deltaRevolution = 65536L + deltaRevolution; }

        Debug.Log("Speed DeltaTime:" + deltaTime + " Delta revolution " + deltaRevolution);
        // we compute the speed on just the 2 last points -> we may have to use more distant points in time !
        if (deltaTime != 0)
        {
            idle = 0;

            //  Debug.Log("deltaTime " + deltaTime+" deltaRevolutions :"+ deltaRevolution);
            speed = ((float)deltaRevolution / (float)deltaTime) * 7630.848F;

            lastSpeedRevolution = speedRevolution;
            lastSpeedTime = speedTime;


        }

        else
        {
            // Debug.Log("Idle State , revolution "+ deltaRevolution);
            if (deltaRevolution == 0)
            {

                if (idle == IDLE_LIMIT)
                {
                    idleState();

                }
                if (idle <= IDLE_LIMIT) { idle += 1; }
            }

        }
    }

        private void idleState()
        {
            Debug.Log("Idle State *******************");
            
            wattsPerHour = 0L;
            heartRate = 0;
            speed = 0;


            
        }


    }

