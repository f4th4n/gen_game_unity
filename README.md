# GenGame Unity

This is C# client library to communicate with [GenGame server](https://github.com/f4th4n/gen_game). It can be used for both generic C# project and Unity.

# Getting Started

1. Make sure GenGame server is started. See how to start [here](https://github.com/f4th4n/gen_game#getting-started)
2. Download dll from [release](https://github.com/f4th4n/gen_game_unity/releases) page
3. Unarchive it, then put it all dll files to your unity project
4. Then you can start using GenGame, see example below:

```cs

using GenGame;
using System.Diagnostics;

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
string matchId = genGame.GetLastMatchId();
await genGame.JoinMatch(matchId);

await genGame.SetState({ chat: "hi, from player 2" })

```

## Build Yourself

If you want to build yourself, see [this doc](/dev.md) for more detail.
