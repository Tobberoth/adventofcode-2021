Run();

void Run()
{
  var players = GetPlayersFromInput("input.txt").ToList();
  var dice = new DeterministicDice();
  var foundWinner = false;
  while (!foundWinner)
  {
    foreach (var player in players)
    {
      player.Move(dice.Throw());
      if (player.Score >= 1000)
      {
        foundWinner = true;
        break;
      }
    }
  }
  var losingPlayerScore = players.Single(p => p.Score < 1000).Score;
  Console.WriteLine(losingPlayerScore * dice.ThrownTimes);
}

IEnumerable<Player> GetPlayersFromInput(string filename)
{
  foreach (var line in File.ReadAllLines(filename))
  {
    var id = int.Parse(line.Remove(0, 7)[0].ToString());
    var startingPos = int.Parse(line.Substring(line.Length-1));
    yield return new Player(id, startingPos);
  }
}

public interface IDice
{
    public int Throw();
}

public class DeterministicDice : IDice
{
  public int ThrownTimes { get; private set; } = 0;
  private int NextResult { get; set; } = 1;
  private int MaxResult { get; set; } = 100;
  public int Throw()
  {
    var result = NextResult++;
    if (NextResult > 100) NextResult = 1;
    result += NextResult++;
    if (NextResult > 100) NextResult = 1;
    result += NextResult++;
    if (NextResult > 100) NextResult = 1;
    ThrownTimes += 3;
    return result;
  }
}

public class Player
{
  public int ID { get; init; }
  public int Position { get; private set; }
  public int Score { get; private set; }
  public Player (int id, int startingPosition) =>
    (ID, Position) = (id, startingPosition);
  public void Move(int steps)
  {
    Position += steps;
    Position = Position % 10;
    if (Position == 0) Position = 10;
    Score += Position;
  }
}