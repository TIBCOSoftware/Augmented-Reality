using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TIBCO.LABS.EFTL;
using TIBCO.UX; // to be removed when view will be decoupled
using UnityEngine.UI;

// Your View of a Model
public sealed class BikeViewController: MonoBehaviour
{
    public BikeModel bikeModel;

    const int IDLE_LIMIT = 180; // Update is invoked 60 times per seconds.

    public HeartBeatController heartBeatController;
    public SpeedPanelController speedController;
    public Text powerText;
    public Text cadenceText;

  


    private string msg;


    private string cadence;

    private long wattsPerHour;



    // Subscribe to Model
    void OnEnable()
    {
        bikeModel.OnModelUpdate += OnModelUpdate;
    }

    void OnDisable()
    {
        bikeModel.OnModelUpdate -= OnModelUpdate;
    }

    void Update()
    {
        
        
            cadenceText.text = this.cadence;
            powerText.text = this.wattsPerHour.ToString("0");
        


    }
    
    public  void OnModelUpdate(BikeModel model)
    {
        this.cadence = model.cadence;
        this.wattsPerHour = model.wattsPerHour;
        speedController.SetSpeedInKmh(model.speed);
        heartBeatController.SetHeartRate(model.heartRate);

        // test if idle state ?
        /* 
            heartBeatController.SetHeartRate(0);
            speedController.SetSpeedInKmh(0);
            cadenceText.text = "...";
            powerText.text = "...";
         */
    }

}

