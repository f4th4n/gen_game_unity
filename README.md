# GenGame Unity

This is C# client library to communicate with GenServer. It can be used for both generic C# project and Unity.

# Connect to GenGame

```cs
var deviceId = "dev-123";
var sessionToken = await client.AuthenticateDeviceAsync(deviceId);
```

# Getting Started

```cs

// create GenGame client
var client = new Client("localhost", 4000);

// create user and authenticate it
await client.AuthenticateDeviceAsync("dev-123");

// create match and return GenGame.Game object
Game game = await client.CreateMatch();

// add callback OnRelay, called when there is on-relay event
game.OnRelay += new Game.OnRelayHandler((dynamic payload) =>
{
    Console.WriteLine($"there is relay: {payload["move_x"]}");
});

// dispatch event relay to all player in the same match with some payload
await game.Relay(new { move_x = 110 });
```
