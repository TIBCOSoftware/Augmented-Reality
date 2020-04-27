# Hololens 2 Sample TIBCO Cloud Messaging integration
## Purpose
[TIBCO Cloud™ Messaging](https://www.tibco.com/products/tibco-messaging)  is the foundation of the TIBCO® Connected Intelligence platform, capitalizing on data in real time wherever it is, and making it available to applications that drive action based on analytical insights.

This Project is using our Unity3D packages in a simple demo app.

`CloudMessagingObserverScene` contains a simple object (from MRTK samples) and a Connection to TIBCO Cloud Messaging service under the `MainController` object.

The App will publish messages when some interaction happen on the object. At the moment the app is detection the following events
- BecameVisible and BecameInvisble
- HasFocus and LostFocus
- [PointerName]Down
- [PointerName]Clicked

PointerName is *PokePointer* or *DefaultControllerPointer*.

You can see the messages  in the TIBCO Cloud Messaging "Demo application" :
from https://messaging.cloud.tibco.com/ one the Help panel (question mark in the page header) and select *Run the Demo* link.

From the same "Demo Application" you can send messages to change the color of the object.
Just enter
- set color red
- set color blue
- set color green
It is a sample application, so we have implemented a very simple, text based set of commands. The sample can be extended to implement solutions using elaborated JSON messages.


A panel in the Scene is also displaying received messages.


## Install

- Open the project in Unity 2019.2.17f1
- Import MRTK 2.3.0 foundation
- Import TIBCOFTL Package (from this repository under Packages folder)
- Set your Credentials

You can test in Unity Editor.

Deploy on Hololens and enjoy.

**Set credentials**

For security reasons,  credential are stored as property keys in a resource file named *TIBCO-credentials.txt* which is ignored from git so credentials are not checked in.

The directory *TIBCOLabs > Utils > Resources*  contains a sample resource file. You can copy it with a the correct name or just create *TIBCO-credentials.txt* resource and have the following lines :
* eftl.socketurl = Web Socket URI
* eftl.authkey =   your authentication key

**Prerequisite**

Get a TIBCO Cloud Account with TIBCO Cloud Messaging subscription.

Obtain the channel URL and one Authentication key.

## Details

Check the script `InteractionEventPublisher` attached to the object.

## Next Steps

Receiving and sending messages is a very powerful capability to implement the business logic of your application outside the Unity code. In an other sample we will show how to use a rule engine (based on complex event processing capabilities offered by TIBCO Cloud Events) to detect some interaction patterns and udpate the scene.

Messaging can also be used for inter application communication scenario.

We will also add events to the InteractionEventPublisher. (IsTouched, GetCloser, ...).
