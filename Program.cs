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

    var client = new Client("localhost", 4000);

    await client.AuthenticateDeviceAsync("dev-123");
    // await client.Ping();

    Game game = await client.CreateMatch();

    game.OnRelay += new Game.OnRelayHandler((dynamic payload) =>
    {
      Console.WriteLine($"there is relay: {payload["move_x"]}");
    });

    await game.Relay(new { move_x = 110 });
  }
}