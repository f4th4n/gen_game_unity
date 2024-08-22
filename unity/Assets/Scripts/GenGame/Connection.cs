using WebSocketSharp;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using Microsoft.CSharp;


namespace GenGame
{
  public class Connection
  {
    public WebSocket Ws;
    public string SessionToken;
    private int MsgRefCounter = 0;
    private int JoinRefCounter = 0;

    private HashSet<String> joinedTopics = new HashSet<String> { };
    private IDictionary<string, int> TopicRef = new Dictionary<string, int>();
    public delegate void OnChangeStateHandler(dynamic something);
    public event OnChangeStateHandler OnChangeState;

    public Connection(string host, int port)
    {
      Ws = new WebSocket($"ws://{host}:{port}/game/websocket?vsn=2.0.0");
    }

    static public async Task<int> Connect(Connection connection)
    {
      var tcs = new TaskCompletionSource<int>();
      connection.Ws.OnOpen += (sender, e) =>
      {
        tcs.SetResult(1);
      };

      connection.Ws.Connect();

      return await tcs.Task;
    }

    static public async Task<string> Ping(Connection connection)
    {
      await JoinChannel(connection, "public");
      var res = await Request(connection, "public", "ping", new { });
      return res.Payload.response;
    }

    static public async Task JoinChannel(Connection connection, string topicName, object payload = null)
    {
      if (connection.joinedTopics.Contains(topicName))
      {
        return;
      }

      payload ??= new { };

      var resJoinChannel = await Request(connection, topicName, "phx_join", payload, true);
      if (resJoinChannel.TopicName != topicName && resJoinChannel.Payload.status != "ok")
      {
        throw new Exception("cannot join topic");
      }

      connection.joinedTopics.Add(topicName);
    }

    static public async Task<PhxChannelResponse> Request(Connection connection, string topicName, string eventName, object payload, Boolean isJoiningChannel = false, Boolean waitResponse = true)
    {
      if (isJoiningChannel)
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

    private void Log(string log)
    {
      Console.ForegroundColor = ConsoleColor.Gray;
      Console.WriteLine(log);
      Console.ResetColor();
    }
  }
}