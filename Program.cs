using System.Diagnostics;
using GenGame;

public class Program
{
  static async Task Main(string[] args)
  {
    Start();
    Thread.Sleep(2000);
  }

  private static async void Start()
  {
    CreateMatch();
    Console.WriteLine("✅ all test passed");
  }

  private static async void CreateMatch()
  {
    var genGame = new Client("localhost", 4000);
    genGame.Connect();

    await genGame.AuthenticateDevice("dev-123");
    var resPing = await genGame.Ping();

    Debug.Assert(resPing == "pong");

    Match match = await genGame.CreateMatch();

    genGame.OnChangeState((dynamic payload) =>
    {
      Console.WriteLine($"there is update state with payload: {payload["move_x"]}");
      Console.WriteLine("test success");
    });

    await genGame.SetState(new { move_x = 110 });
  }
}