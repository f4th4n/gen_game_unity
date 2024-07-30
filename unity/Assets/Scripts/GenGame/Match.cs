
namespace GenGame
{
  public class Match
  {
    public string MatchId;

    public Match(string matchId)
    {
      MatchId = matchId;
    }

    public override string ToString()
    {
      return $"match id: {MatchId}";
    }
  }
}