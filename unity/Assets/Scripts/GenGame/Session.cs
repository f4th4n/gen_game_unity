namespace GenGame
{
  public class Session
  {
    static public async Task<String> AuthenticateDevice(Connection connection, String deviceId)
    {
      await Connection.JoinTopic(connection, "public");
      var res = await Connection.Request(connection, "public", "create_session", new { username = deviceId });
      var token = res.Payload.response.token;
      connection.SessionToken = token;
      return token;
    }
  }
}