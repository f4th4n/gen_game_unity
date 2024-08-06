using System.Threading.Tasks;

namespace GenGame
{
  public class Session
  {
    static public async Task<string> AuthenticateDevice(Connection connection, string deviceId)
    {
      await Connection.JoinChannel(connection, "public");
      var res = await Connection.Request(connection, "public", "create_session", new { username = deviceId });
      var token = res.Payload.response.token;
      connection.SessionToken = token;
      return token;
    }
  }
}