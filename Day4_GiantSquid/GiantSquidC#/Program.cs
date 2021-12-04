(IEnumerable<int> seq, List<Board> boards) ReadInput(string file)
{
  var lines = System.IO.File.ReadAllLines(file).ToList();
  var lineRow = 0;
  var randomSequence = lines[lineRow++].Split(',').Select(i => int.Parse(i));
  List<Board> boards = new();
  for (lineRow++; lineRow < lines.Count; lineRow++)
  {
    if (string.IsNullOrEmpty(lines[lineRow]))
    {
      lineRow++;
      continue;
    }

    var boardData = lines
      .Take((lineRow)..(lineRow+5))
      .Select(l => l.Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(n => int.Parse(n)))
      .ToList();
    boards.Add(new Board(boardData));
    lineRow += 5;
  }
  return (randomSequence, boards);
}

void RunGame1(IEnumerable<int> randomSequence, List<Board> boards)
{
  foreach (var num in randomSequence)
  {
    foreach (var board in boards)
    {
      if (board.Check(num))
      {
        var unmarkedSum = board.GetUnmarkedSum();
        Console.WriteLine($"Winning score: {unmarkedSum * num}");
        return;
      }
    }
  }
}

void RunGame2(IEnumerable<int> randomSequence, List<Board> boards)
{
  List<Board> filteredBoards = new(boards);
  foreach (var num in randomSequence)
  {
    foreach (var board in filteredBoards.ToList())
    {
      if (board.Check(num))
      {
        if (filteredBoards.Count != 1)
          filteredBoards.Remove(board);
        else
        {
          var unmarkedSum = board.GetUnmarkedSum();
          Console.WriteLine($"Winning score: {unmarkedSum * num}");
          return;
        }
      }
    }
  }
}

(var randomSequence, var boards) = ReadInput("input.txt");
RunGame1(randomSequence, boards);
RunGame2(randomSequence, boards);

public class Board 
{
  (int val, bool check)[,] Grid = new (int val, bool check)[5,5];

  public Board(List<IEnumerable<int>> lines)
  {
    for (int y = 0; y < lines.Count(); y++)
    {
      int x = 0;
      foreach (var n in lines[y])
      {
        Grid[x++, y] = (n, false);
      }
    }
  }

  public int GetUnmarkedSum()
  {
    var result = 0;
    for (int y = 0; y < 5; y++)
    {
      for (int x = 0; x < 5; x++)
      {
        if (!Grid[x,y].check)
          result += Grid[x,y].val;
      }
    }
    return result;
  }

  public bool Check(int val)
  {
    for (int y = 0; y < 5; y++)
    {
      for (int x = 0; x < 5; x++)
      {
        if (Grid[x,y].val == val)
        {
          Grid[x,y].check = true;
          return IsBingo(x, y);
        }
      }
    }
    return false;
  }

  private bool IsBingo(int x, int y)
  {
    return
      (Grid[0,y].check && Grid[1,y].check && Grid[2,y].check && Grid[3,y].check && Grid[4,y].check) ||
      (Grid[x,0].check && Grid[x,1].check && Grid[x,2].check && Grid[x,3].check && Grid[x,4].check);
  }
}
