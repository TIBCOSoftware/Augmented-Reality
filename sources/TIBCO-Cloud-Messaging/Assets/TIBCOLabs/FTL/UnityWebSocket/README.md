# Web Sockets for Unity

For Unity developers looking to use Web Sockets in their Unity game / app.

## External dependencies

**First download the required dependencies and extract the contents into your Unity project "Assets" folder.**

* [WebSocket-Sharp* forked for supporting custom headers](https://github.com/deadlyfingers/websocket-sharp)

## Features

- **IWebSocket** interface for targeting the various platforms Unity supports. 
  - **WebSocketMono** utilizes [WebSocket-Sharp*](https://github.com/deadlyfingers/websocket-sharp) and should work on all mono platforms including the Unity Editor on Mac and PC.
  - **WebSocketUWP** utilizes [MessageWebSocket](https://docs.microsoft.com/en-us/uwp/api/windows.networking.sockets.messagewebsocket) for Windows 10 (UWP) apps.

## Interface methods
API | Description
--- | -----------
ConfigureWebSocket(url) | Configures web socket with url and optional headers
ConnectAsync() | Connect to web socket
CloseAsync() | Close web socket connection
SendAsync(data) | Send binary `byte[]` or UTF-8 text `string` with optional callback
IsOpen() | Check if web socket status is open
Url() | Return the URL being used by the web socket

## Interface event delegates
	OnError(object sender, WebSocketErrorEventArgs e);
	OnOpen(object sender, EventArgs e);
	OnMessage(object sender, WebSocketMessageEventArgs e);
	OnClose(object sender, WebSocketCloseEventArgs e);

## Usage

[UnityWebSocketDemo project repo](https://github.com/Unity3dAzure/UnityWebSocketDemo) contains sample scenes showing how to hook all this up in the Unity Editor.

Questions or tweet [@deadlyfingers](https://twitter.com/deadlyfingers)
