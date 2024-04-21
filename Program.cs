Console.WriteLine("Connected");

// using System.Net.WebSockets;
// using System.Text;

// var genGame = new GenGame.GenGame();

// Console.WriteLine("Connected");

// // join channel
// var message = """
// [
//     "1",
//     "1",
//     "public",
//     "phx_join",
//     {}
// ]
// """;
// var buffer = Encoding.UTF8.GetBytes(message);
// await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);


// var receiveBuffer = new byte[1024];
// var receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
// var receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

// Console.WriteLine($"Received message: {receivedMessage}");

// // create session
// message = """
// [
//     123,
//     null,
//     "public",
//     "create_session",
//     {
//         "username": "wildan"
//     }
// ]
// """;
// buffer = Encoding.UTF8.GetBytes(message);
// await ws.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, CancellationToken.None);

// receiveBuffer = new byte[1024];
// receiveResult = await ws.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);
// receivedMessage = Encoding.UTF8.GetString(receiveBuffer, 0, receiveResult.Count);

// Console.WriteLine($"Received message: {receivedMessage}");
