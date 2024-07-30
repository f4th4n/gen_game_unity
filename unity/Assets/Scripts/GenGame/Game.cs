namespace GenGame
{
    public class Game
    {
        private String matchId;
        private Client client;

        public async Task<String> Relay(Connection connection, object payload)
        {
            var topic = Topic();
            await Connection.JoinTopic(connection, topic, new { token = connection.SessionToken });
            await Connection.Request(connection, topic, "set_state", payload, false, false);
            return "ok";
        }

        static public async Task<Match> CreateMatch(Connection connection, Match match)
        {
            // TODO add arg query
            await Connection.JoinTopic(connection, "gen_game", new { token = connection.SessionToken });
            var res = await Connection.Request(connection, "gen_game", "create_match", new { });
            if (res.Payload.status == "error")
            {
                throw new Exception((string)res.Payload.response);
            }

            match.MatchId = (string)res.Payload.response.match_id;
            return match;
        }

        static public async Task SetState(Connection connection, Match match, dynamic payload)
        {
            var topic = Topic(match.MatchId);
            await Connection.JoinTopic(connection, topic, new { token = connection.SessionToken });
            await Connection.Request(connection, topic, "set_state", payload);
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