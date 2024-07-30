using System.Diagnostics;
using GenGame;

public class Program
{
  static void Main(string[] args)
  {
    _ = StartProgram();
    Thread.Sleep(2000);
  }

  private static async Task StartProgram()
  {
    var match = await CreateMatch();
    await JoinMatch(match);
  }

  private static async Task<Match> CreateMatch()
  {
    var genGame = new Client("localhost", 4000);
    genGame.Connect();

    await genGame.AuthenticateDevice("dev-123");
    var resPing = await genGame.Ping();

    Debug.Assert(resPing == "pong");

    Match match = await genGame.CreateMatch();

    genGame.OnChangeState((dynamic payload) =>
    {
      Console.WriteLine($"    there is update state with payload: {payload["chat"]}");
    });

    await genGame.SetState(new { chat = "hi, from player 1" });

    return match;
  }

  private static async Task JoinMatch(Match match)
  {
    var genGame = new Client("localhost", 4000);
    genGame.Connect();

    await genGame.AuthenticateDevice("dev-456");
    await genGame.JoinMatch(match.MatchId);
    await genGame.SetState(new { chat = "hi, from player 2" });
  }
}