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
Match match = await genGame.CreateMatch();  // <---- save this match.MatchId somewhere so you can let other player join this game

// add callback OnChangeState, called when there is state update event
genGame.OnChangeState((dynamic payload) =>
{
    Console.WriteLine($"there is update state with payload: {payload["chat"]}");
    Console.WriteLine("test success");
});

await genGame.SetState(new { chat = "hi, from player 1" });
```

Let the 2nd player join the match:

```cs
var genGame = new Client("localhost", 4000);
genGame.Connect();

await genGame.AuthenticateDevice("dev-456");

// join a game
await genGame.JoinMatch("some-match-id");

await genGame.SetState({ chat: "hi, from player 2" })

```

## Test Without Unity

```bash
dotnet build
dotnet run
```
