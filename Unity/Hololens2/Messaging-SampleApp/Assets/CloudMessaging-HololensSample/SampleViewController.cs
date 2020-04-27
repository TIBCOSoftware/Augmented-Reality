using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.Events;
using TIBCO.LABS.EFTL;
using TIBCO.UX; // to be removed when view will be decoupled
using UnityEngine.UI;
using TMPro;

// Your View of a Model

/// <summary>
/// Simple Model and View+Controller implementation
/// the model get updated by external events
/// the view+controller subscribe to the model update info and change view accordingly.
/// In this simple case the View is a simple Text (in a panel).
/// The model has a simple "info" data
/// </summary>
public sealed class SampleViewController : MonoBehaviour
{
    public SampleModel model;



    public TextMeshPro  eventInfoText;
    public GameObject targetObject;

    private string message;
    private UnityEngine.Color color;
 


    // Subscribe to Model
    void OnEnable()
    {
        model.OnModelUpdate += OnModelUpdate;
    }

    void OnDisable()
    {
        model.OnModelUpdate -= OnModelUpdate;
    }

    void Update()
    {
        // set view. data may have changed outside the Unity Update cycle. 
        eventInfoText.text = this.message;
        if (targetObject != null)
        {
            targetObject.GetComponent<Renderer>().material.color = color;
        }
 
    }

    public void OnModelUpdate(SampleModel model)
    {
        this.message = model.info;
        this.color = model.objectColor;
        
         
    }

}

