# GenGame Unity

This is C# client library to communicate with [GenGame server](https://github.com/f4th4n/gen_game). It can be used for both generic C# project and Unity.

# Getting Started

First, make sure GenGame server is started. See how to start [here](https://github.com/f4th4n/gen_game#getting-started).

```cs

using GenGame;

// connect to GenGame server and then return a single object
// that used to communicate with the server
var genGame = new Client("localhost", 4000);
genGame.Connect();

// create user and authenticate it
await genGame.AuthenticateDevice("dev-123");

// try to ping to the server
var resPing = await genGame.Ping();
Debug.Assert(resPing == "pong");

// create match and return GenGame.Game object
Match match = await genGame.CreateMatch();

// add callback OnChangeState, called when there is state update event
genGame.OnChangeState((dynamic payload) =>
{
    Console.WriteLine($"there is update state with payload: {payload["move_x"]}");
    Console.WriteLine("test success");
});

await genGame.SetState(new { move_x = 110 });
```

## Test Without Unity

```bash
dotnet build gen_game_unity.csproj
dotnet run gen_game_unity.csproj
```
