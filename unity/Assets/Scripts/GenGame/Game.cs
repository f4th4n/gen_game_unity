namespace GenGame
{
    public class Game
    {
        private String matchId;
        private Client client;

        public async Task<String> Relay(Connection connection, object payload)
        {
            var topic = Topic();
            await Connection.JoinChannel(connection, topic, new { token = connection.SessionToken });
            await Connection.Request(connection, topic, "set_state", payload, false, false);
            return "ok";
        }

        static public async Task<Match> CreateMatch(Connection connection, Match match)
        {
            // TODO add arg query
            await Connection.JoinChannel(connection, "gen_game", new { token = connection.SessionToken });
            var res = await Connection.Request(connection, "gen_game", "create_match", new { });
            if (res.Payload.status == "error")
            {
                throw new Exception((string)res.Payload.response);
            }

            var matchId = (string)res.Payload.response.match_id;
            await JoinMatch(connection, match, matchId);

            return match;
        }

        static public async Task JoinMatch(Connection connection, Match match, string matchId)
        {
            var topic = Topic(matchId);
            await Connection.JoinChannel(connection, topic, new { token = connection.SessionToken });

            match.MatchId = matchId;
        }

        static public async Task SetState(Connection connection, Match match, dynamic payload)
        {
            var topic = Topic(match.MatchId);
            await Connection.JoinChannel(connection, topic, new { token = connection.SessionToken });
            // TODO line below should await-ed
            Connection.Request(connection, topic, "set_state", payload, false, false);
        }

        public string Topic()
        {
            return $"game:{matchId}";
        }

        static public string Topic(string matchId)
        {
            return $"game:{matchId}";
        }
    }
}