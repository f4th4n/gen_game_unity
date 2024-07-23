# GenGame Unity

This is C# client library to communicate with [GenGame server](https://github.com/f4th4n/gen_game). It can be used for both generic C# project and Unity.

# Getting Started

First, make sure GenGame server is started. See how to start [here](https://github.com/f4th4n/gen_game#getting-started).

```cs

// connect to GenGame server and then return a client
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

## Test Without Unity

```bash
dotnet build gen_game_unity.csproj
dotnet run gen_game_unity.csproj
```
