using WebSocketSharp;
using Newtonsoft.Json;

namespace GenGame
{
  public class Connection
  {
    public WebSocket Ws;
    public string SessionToken;
    private int MsgRefCounter = 0;
    private int JoinRefCounter = 0;

    private HashSet<String> joinedTopics = [];
    private IDictionary<string, int> TopicRef = new Dictionary<string, int>();
    public delegate void OnChangeStateHandler(dynamic something);
    public event OnChangeStateHandler OnChangeState;

    public Connection(string host, int port)
    {
      Ws = new WebSocket($"ws://{host}:{port}/game/websocket?vsn=2.0.0");
    }

    static public void Connect(Connection connection)
    {
      connection.Ws.Connect();
    }

    static public async Task<string> Ping(Connection connection)
    {
      await JoinTopic(connection, "public");
      var res = await Connection.Request(connection, "public", "ping", new { });
      return res.Payload.response;
    }

    static public async Task JoinTopic(Connection connection, String topicName, object payload = null)
    {
      if (connection.joinedTopics.Contains(topicName))
      {
        return;
      }

      payload ??= new { };

      var resJoinTopic = await Connection.Request(connection, topicName, "phx_join", payload, true);
      if (resJoinTopic.TopicName != topicName && resJoinTopic.Payload.status != "ok")
      {
        throw new Exception("cannot join topic");
      }

      connection.joinedTopics.Add(topicName);
    }

    static public async Task<PhxChannelResponse> Request(Connection connection, String topicName, String eventName, object payload, Boolean isTypeJoin = false, Boolean waitResponse = true)
    {
      if (isTypeJoin)
      {
        connection.JoinRefCounter++;
        connection.TopicRef[topicName] = connection.JoinRefCounter;
      }

      connection.MsgRefCounter++;

      var tcs = new TaskCompletionSource<PhxChannelResponse>();

      EventHandler<MessageEventArgs> handler = null;

      handler = (sender, e) =>
      {
        var deserializedData = JsonConvert.DeserializeObject<IEnumerable<object>>(e.Data);
        var res = new PhxChannelResponse(deserializedData);

        if (res.JoinRef == connection.TopicRef[topicName] && res.MessageRef == connection.MsgRefCounter)
        {
          connection.Ws.OnMessage -= handler;
          tcs.SetResult(res);
        }
      };

      if (waitResponse)
      {
        connection.Ws.OnMessage += handler;
      }

      var msg = new PhxChannelRequest(
          connection.JoinRefCounter,
          connection.MsgRefCounter,
          topicName,
          eventName,
          payload);

      connection.Ws.Send(msg.Encode());

      return await tcs.Task;
    }

    private void Log(String log)
    {
      Console.ForegroundColor = ConsoleColor.Gray;
      Console.WriteLine(log);
      Console.ResetColor();
    }
  }
}