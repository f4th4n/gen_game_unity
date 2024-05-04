using WebSocketSharp;
using Newtonsoft.Json;

namespace GenGame
{
    public class Client
    {
        public String Endpoint;
        public WebSocket Ws;
        public String SessionToken;
        private int MsgRefCounter = 0;
        private int JoinRefCounter = 0;

        private HashSet<String> joinedTopics = [];
        private IDictionary<string, int> TopicRef = new Dictionary<string, int>();

        public Client(String host, int port)
        {
            Endpoint = $"ws://{host}:{port}/game/websocket?vsn=2.0.0";
            Ws = Connect();

            Ws.OnMessage += (sender, e) =>
            {
                // Logging
                Log("GenServer res log: " + e.Data);
            };
        }

        public async Task AuthenticateDeviceAsync(String deviceId)
        {
            await JoinTopic("public");
            SessionToken = await CreateSession(deviceId);
        }

        public async Task<Game> CreateMatch()
        {
            // TODO add arg query
            await JoinTopic("gg", new { token = SessionToken });
            var res = await Request("gg", "create_match", new { });
            var matchId = (String)res.Payload.response;
            Game game = new Game(matchId, this);
            return game;
        }

        private async Task<String> CreateSession(String deviceId)
        {
            var res = await Request("public", "create_session", new { username = deviceId });
            return res.Payload.response.token;
        }

        public WebSocket Connect()
        {
            var ws = new WebSocket(Endpoint);
            ws.Connect();

            return ws;
        }

        public async Task<object> Ping()
        {
            await JoinTopic("public");
            var res = await Request("public", "ping", new { });
            return res.Payload.response;
        }

        public async Task JoinTopic(String topicName, object payload = null)
        {
            if (joinedTopics.Contains(topicName))
            {
                return;
            }

            payload ??= new { };

            var resJoinTopic = await Request(topicName, "phx_join", payload, true);
            if (resJoinTopic.TopicName != topicName && resJoinTopic.Payload.status != "ok")
            {
                throw new Exception("cannot join topic");
            }

            joinedTopics.Add(topicName);
        }

        public async Task<PhxChannelResponse> Request(String topicName, String eventName, object payload, Boolean isTypeJoin = false, Boolean waitResponse = true)
        {
            if (isTypeJoin)
            {
                JoinRefCounter++;
                TopicRef[topicName] = JoinRefCounter;
            }

            MsgRefCounter++;

            var tcs = new TaskCompletionSource<PhxChannelResponse>();

            EventHandler<MessageEventArgs> handler = null;

            handler = (sender, e) =>
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<object>>(e.Data);
                var res = new PhxChannelResponse(deserializedData);

                if (res.JoinRef == TopicRef[topicName] && res.MessageRef == MsgRefCounter)
                {
                    Ws.OnMessage -= handler;
                    tcs.SetResult(res);
                }
            };

            if (waitResponse)
            {
                Ws.OnMessage += handler;
            }

            var msg = new PhxChannelRequest(
                JoinRefCounter,
                MsgRefCounter,
                topicName,
                eventName,
                payload);

            Ws.Send(msg.Encode());

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