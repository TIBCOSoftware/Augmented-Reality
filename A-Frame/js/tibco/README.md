# A-Frame components related to TIBCO CIC Services

## TIBCO Messaging eFTL

Use tag

`<a-entity id="eftl" tibco-eftl="URL:EFTL_URL;key:EFTL_KEY";matcher="some matcher">`

to connect to TIBCO Messaging.

* URL : the name of credentials entry with the TIBCO messaging endpoint url
* key : the name of credentials entry with the TIBCO messaging authorization KEY
* matcher : optional matcher expression

This element will emit a "msg" event on each message received from TIBCO Messaging.

A UI component can use an event listener for "msg" event on this component. It will receive an event containg the message payload in evt.detail as a JSON string.
