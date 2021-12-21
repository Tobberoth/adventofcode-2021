Run();

void Run()
{
  var players = GetPlayersFromInput("input.txt").ToList();
  Console.WriteLine(Play(players[0], players[1]));
}

IEnumerable<Player> GetPlayersFromInput(string filename)
{
  foreach (var line in File.ReadAllLines(filename))
  {
    var startingPos = int.Parse(line.Substring(line.Length-1));
    yield return new Player(startingPos, 0);
  }
}

long Play(Player p1, Player p2)
{
  Dictionary<(Player p1, Player p2), long> memo = new();
  return PlayRecursive(p1, p2, memo);
}

long PlayRecursive(Player p1, Player p2, Dictionary<(Player p1, Player p2), long> memo)
{
  if (memo.ContainsKey((p1, p2)))
    return memo[(p1, p2)];

  long res = -1;
  if (p1.Score >= 21)
  {
    memo.Add((p1, p2), 1);
    return 1;
  }
  else if (p2.Score >= 21)
  {
    memo.Add((p1, p2), 0);
    return 0;
  }
  else
  {
    res = 0;
    for (var p11 = 1; p11 <= 3; p11++)
    {
      for (var p12 = 1; p12 <= 3; p12++)
      {
        for (var p13 = 1; p13 <= 3; p13++)
        {
          var p1Dice = (p11+p12+p13);
          var npos1 = GetPosition(p1.Position, p1Dice);
          var np1 = new Player(npos1, p1.Score + npos1);
          if (np1.Score >= 21)
          {
            res += 1;
            continue;
          }
          for (var p21 = 1; p21 <= 3; p21++)
          {
            for (var p22 = 1; p22 <= 3; p22++)
            {
              for (var p23 = 1; p23 <= 3; p23++)
              {
                var p2Dice = (p21+p22+p23);
                var npos2 = GetPosition(p2.Position, p2Dice);
                var np2 = new Player(npos2, p2.Score + npos2);
                res += PlayRecursive(np1, np2, memo);
              }
            }
          }
        }
      }
    }
  }
  memo.Add((p1, p2), res);
  return res;
}

int GetPosition(int pos, int dice)
{
  var end = (pos + dice) % 10;
  if (end == 0) end = 10;
  return end;
}

public record Player(int Position, int Score);