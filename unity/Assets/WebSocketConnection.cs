//namespace GenGame {
using System;
using System.Net.WebSockets;

class WebSocketConnection
{
    public async void Create()
    {
        using var ws = new ClientWebSocket();

        var uri = new Uri("ws://localhost:4000/game/websocket?vsn=2.0.0");
        await ws.ConnectAsync(uri, CancellationToken.None);
    }
}
//}
