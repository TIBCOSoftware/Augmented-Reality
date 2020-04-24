# Hololens / TIBCO Cloud Messaging integration
## Purpose
[TIBCO Cloud™ Messaging](https://www.tibco.com/products/tibco-messaging)  is the foundation of the TIBCO® Connected Intelligence platform, capitalizing on data in real time wherever it is, and making it available to applications that drive action based on analytical insights.

This Unity3D packages provides components to
* Connect to a TIBCO Cloud Messaging channel
* Subscribe to receive real-time messages published on channel using matchers
* publish messages


Using those components you can build AR experiences displaying real-time data coming from IoT sensors, enterprise applications, and more ...
## Packages
You can use the generated package available in the *_Packages* folder.
## Prerequisite
Get a TIBCO Cloud Account with TIBCO Cloud Messaging subscription.

Obtain the channel URL and one Authentication key.

## Experimenting

**Test Scene**

Unity, open: *TIBCOLabs > CloudMessaging > Sample > CloudMessaging-TestScene*

**Set credentials**

For security reasons,  credential are stored as property keys in a resource file named *TIBCO-credentials.txt* which is ignored from git so credentials are not checked in.

The Scene contains a MainController Object. In the MainController, you can update the prefix used to read the properties. The default value is 'eftl'.

The directory *TIBCOLabs > Utils > Resources*  contains a sample resource file. You can copy it with a the correct name or just create *TIBCO-credentials.txt* resource and have the following lines :
* eftl.socketurl = Web Socket URI
* eftl.authkey =   your authentication key


**Play**

The Scene is using a SimpleTextViewController : it implements DataHandler and updates a TextMesh when a json message is received on TIBCO Cloud Messaging connector

It is expecting a message like

 {"dataType":"Power","data":"130"}

* Play the Scene in Unity Editor

* Send a message using curl :
> curl -i -u :[your-key] https://messaging.cloud.tibco.com/tcm/TIB_SUB_[your-id]/channel/v1/publish -d "{\\"dataType\\":\\"Power\\",\\"data\\":\\"130\\"}"


Note that the sample is using a matcher to receive messages containing the field *dataType*. The matcher is

> {"dataType" : true}


SimpleTextViewController is also publishing messages.

The published messages have a *demo_tcm* field and can be received by the TIBCO Cloud Messaging "Demo application" :
from https://messaging.cloud.tibco.com/ one the Help panel (question mark in the page header) and select *Run the Demo* link.

The messages published by our unity application will appear there.

## Use in your application
Simply write a class extending the HandleData class for your application.

Also have a look at the sample applications for Hololens or Mobile published in this repository.
