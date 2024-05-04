using Newtonsoft.Json;

namespace GenGame
{
    public class Game
    {
        private String matchId;
        private Client client;

        public delegate void OnRelayHandler(dynamic something);
        public event OnRelayHandler OnRelay;

        public Game(String argMatchId, Client argClient)
        {
            matchId = argMatchId;
            client = argClient;

            client.Ws.OnMessage += (sender, e) =>
            {
                // Logging
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<object>>(e.Data);
                var res = new PhxChannelResponse(deserializedData);
                if (res.EventName == "relay" && OnRelay != null)
                {
                    OnRelay(res.Payload);
                }
            };
        }

        public async Task<String> Relay(object payload)
        {
            var topic = Topic();
            await client.JoinTopic(topic, new { token = client.SessionToken });
            var res = await client.Request(topic, "set_state", payload, false, false);
            Console.WriteLine("res.Payload");
            Console.WriteLine(res.Payload);
            return "test";
        }

        private String Topic()
        {
            return $"game:{matchId}";
        }
    }
}