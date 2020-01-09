# TIBCO Cloud Messaging Unity3D Package

## Overview
This package allows you to connect a Unity Application to TIBCO Cloud Messaging using EFTL websocket protocol.
It's based on UnityWebSocket and is working in Unity Editor and on Hololens (UWP).

Once connected, the application can subscribe and receive realtime messages and update the scene accordingly.

A simple sample code demonstrates how to display data in a TextMesh.

### Import into Unity3D
Just created a new Unity Project and import the custom Package.

![alt-text](img/UnityAssets-FTL.png "Image")
<br><sup>Unity3D Library Assets</sup>
### Use the sample scene
Open TIBCOLabs > CloudMessaging > Sample > CloudMessaging-TestScene.

The Scene contains a MainController Object.
In the MainController, update the Web Socket URI and Authkey of the TIBCOCloudMessaginConnector with you TIBCO Cloud Subscription Details.


![alt-text](img/WebSocketConfig.png "Image")
<br><sup>TIBCO Cloud Messaging Configuration</sup>

## sending Messages to Unity3D
The sample is expecting this type of message :

``` json
{"dataType":"Power","data":"130"}
```
### using cURL
TIBCO Cloud Messaging comes with a REST API, as well. So any REST Tool can be used for testing, too. Just replace [your-key] and [your-id] in the following command:

``` bash
curl -i -u :[your-key] https://messaging.cloud.tibco.com/tcm/TIB_SUB_[your-id]/channel/v1/publish -d '{"dataType":"Power","data":"130"}'
```

### using Flogo
Here a sample using TIBCO Cloud Integration Flogo

![alt-text](img/FlogoTCMSender.png "Image")
<br><sup>TIBCO Cloud Integration Configuration</sup>





> the current Implementation subscribes to all instance messaging destinations.

### How it works
1. The MainController GameObject contains the TIBCO Cloud Messaging Connector and SimpleTextViewController
2. The Controller extends DataHandler, it requires a connection (Connector Object) and receives messages.
3. The Controller has a reference to the Text to update when it receives a message.

## Content
ready to use [Package](https://github.com/TIBCOSoftware/Augmented-Reality/tree/master/packages/TIBCO-Cloud-Messaging/FTL-Basic) |
Implementation [Source](https://github.com/TIBCOSoftware/Augmented-Reality/tree/master/sources/TIBCO-Cloud-Messaging/FTL-Basic/Assets)
