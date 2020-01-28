# TIBCO Cloud™ Nimbus® - Unity3D Package

## Overview
This package allows you to connect a Unity3D Application to [TIBCO Cloud™ Nimbus®](https://www.tibco.com/products/tibco-cloud-nimbus) 

This Sample shows a Nimbus Instruction Guide Activity, defined in the Web-based Maps Designer in the TIBCO Cloud.
The Panel can be placed anywhere on the ground, moved around or fixed in the position. It can be adjusted in scale, rotation, and position-height using an Options Menu that can be opened using a 2-finger touch. 

![alt-text](img/nimbus-maps-AR.gif "Image")
<br><sup>Unity3D Scene Sample</sup>

Within this Sample, the Navigation between the different Activities is done using the arrow button at the left and right of the shown Activity.

### Trail Account
register for your own TIBCO Cloud™ Nimbus® trail account on this [Form](https://www.tibco.com/products/tibco-cloud-nimbus/sign-up) to get your personal Subscription Account.

### Nimbus Maps
the Instruction Guide Activity Map of the in the sample above dynamically rendered in AR.

![alt-text](img/nimbus-map.png "Image")
<br><sup>TIBCO Cloud™ Nimbus® Map</sup>

In Nimbus Maps Designer the first Activity looks like follows

![alt-text](img/nimbus-activity.png "Image")
<br><sup>TIBCO Cloud™ Nimbus® Map Activity Details</sup>

Currently, this Sample Implementation supports the following Attributes of this Activity:

- Diagram Title
- Diagram Version
- Diagram Owner
- Activity Title
- Activity Text/Instructions
- Activity Comment/Hint
- Activity Resource/Owner

> Remark: to support Activity Attachments / Links, etc. would be great to add near future.

### Nimbus API
This TIBCO Labs Exploration focused as a first step to support the standard Nimbus JSON that is provided by the private API.
We exploring Ideas of supporting on the fly Map changes as an option, too.

### Nimbus JSON
For Testing open Browser Developer Tools, and go to the Network Tab. Afterward, open your created Nimbus Map 

![alt-text](img/nimbus-json.png "Image")
<br><sup>TIBCO Cloud™ Nimbus® JSON from Chrome Dev Tools</sup>

Search for a Request like the following and copy the JSON response.

```bash
Request URL: https://nimbus.cloud.tibco.com/TIB_SUB_<your Subscription ID>/CtrlWebIsapi.dll/api/process-page/0:<your Nimbus Map ID>
Request Method: GET
```

Store the JSON as 'NimbusMap.json' in your Unity 'Assets/Resources' Folder.

## Target Devices
Tested with AR supporting Android Smartphones and Tablets, [ARCore supported devices](https://developers.google.com/ar/discover/supported-devices).
Glad to hear that it works with ARKit on iOS as well, feel free to raise a GitHub Issue to confirm.

## Content
ready to use [Package](https://github.com/TIBCOSoftware/Augmented-Reality/tree/master/packages/TIBCO-Nimbus/Nimbus-Basic) |
Implementation [Source](https://github.com/TIBCOSoftware/Augmented-Reality/tree/master/sources/TIBCO-Nimbus/Nimbus-Basic/Assets)
