using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Microsoft.MixedReality.Toolkit.Input;
using TIBCO.LABS.EFTL;
#if WINDOWS_UWP
using Windows.Security.ExchangeActiveSyncProvisioning;
#endif



public class InteractionEventPublisher : DataHandler, IMixedRealityFocusHandler, IMixedRealityPointerHandler
{
    private bool IsVisible;
    #region IDataHandler methods
    private string DeviceId;


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
#if WINDOWS_UWP
        var info = new EasClientDeviceInformation();
        // Debug.Log("Device :"+info.FriendlyName);
        this.DeviceId = info.Id.ToString();
#else
        this.DeviceId = "debug";
#endif

    }

    // Update is called once per frame
    void Update()
    {

        Vector3 screenPoint = Camera.main.WorldToViewportPoint(transform.position);
        bool onScreen = screenPoint.z > 0 && screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.y > 0 && screenPoint.y < 1;
        if (onScreen && !IsVisible)
        {
            OnBecameVisible();

        }
        if (!onScreen && IsVisible)
        {
            OnBecameInvisible();
        }
        IsVisible = onScreen;
    }

    void OnBecameVisible()
    {
        Debug.Log("Object is Visible");
        PublishObjectStatus("BecameVisible");
    }
    void OnBecameInvisible()
    {
        Debug.Log("Object is Invisible");
        PublishObjectStatus("BecameInvisible");

    }
    void PublishObjectStatus(string eventName)

    {


        Debug.Log(eventName);
        JsonObject message = new JsonObject();
        message["_dest"] = "unity_out";
        message["tibcolabs_art"] = "v1";
        message["device_id"] = DeviceId;
        message["demo_tcm"] = $"From '{Application.productName}' on device '{DeviceId}' : object {gameObject.name} {eventName} {System.DateTime.Now}";
        message["app_id"] = Application.productName;
        message["source"] = "object";
        message["source_id"] = gameObject.name;
        message["event"] = eventName;
        message["timestamp"] = System.DateTime.Now.ToString();

        Publish(message);
    }
    #region IMixedRealityFocusHandler methods


    void IMixedRealityFocusHandler.OnFocusEnter(FocusEventData eventData)
    {
        Debug.Log("Has Focus");
        PublishObjectStatus("HasFocus");
    }

    void IMixedRealityFocusHandler.OnFocusExit(FocusEventData eventData)
    {
        Debug.Log("Lost Focus");
        PublishObjectStatus("LostFocus");
    }
    #endregion

    #region IMixedRealityPointerHandler methods
    // used if object has a Touchable with Event-to-Receive = Pointer
    void IMixedRealityPointerHandler.OnPointerDown(MixedRealityPointerEventData eventData)
    {
        //Debug.Log("OnPointerDown " + eventData.Pointer.PointerName);//eventData.MixedRealityInputAction.Description);
        PublishObjectStatus(eventData.Pointer.PointerName+"Down");
    }
    void IMixedRealityPointerHandler.OnPointerUp(MixedRealityPointerEventData eventData)
    {
        //Debug.Log("OnPointerUp" + eventData.MixedRealityInputAction.Description);//eventData.MixedRealityInputAction.Description);
    }

    void IMixedRealityPointerHandler.OnPointerClicked(MixedRealityPointerEventData eventData)
    {
        Debug.Log("OnPointerClicked " + eventData.MixedRealityInputAction.Description);
        PublishObjectStatus(eventData.Pointer.PointerName + "Clicked");
    }


    void IMixedRealityPointerHandler.OnPointerDragged(MixedRealityPointerEventData eventData)
    {
        // Debug.Log("OnPointerDragged");
    }


    #endregion
}
