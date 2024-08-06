using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace GenGame
{
    public class Client
    {
        public Connection Connection;
        public Match Match;

        public Client(string host, int port)
        {
            Connection = new Connection(host, port);
            Match = new Match("");
        }

        public void OnChangeState(Action<dynamic> onChangeState)
        {
            Connection.Ws.OnMessage += (sender, e) =>
            {
                var deserializedData = JsonConvert.DeserializeObject<IEnumerable<object>>(e.Data);
                var res = new PhxChannelResponse(deserializedData);

                if (res.EventName == "relay" && onChangeState != null)
                {
                    onChangeState(res.Payload);
                }
            };
        }

        public void Connect()
        {
            Connection.Connect(Connection);
        }

        public async Task<string> AuthenticateDevice(string deviceId)
        {
            return await Session.AuthenticateDevice(Connection, deviceId);
        }

        public async Task<string> Ping()
        {
            return await Connection.Ping(Connection);
        }

        public async Task<Match> CreateMatch()
        {
            return await Game.CreateMatch(Connection, Match);
        }

        public async Task JoinMatch(string matchId)
        {
            await Game.JoinMatch(Connection, Match, matchId);
        }

        public async Task<string> GetLastMatchId()
        {
            return await Game.GetLastMatchId(Connection);
        }

        public async Task SetState(dynamic Payload)
        {
            await Game.SetState(Connection, Match, Payload);
        }
    }
}