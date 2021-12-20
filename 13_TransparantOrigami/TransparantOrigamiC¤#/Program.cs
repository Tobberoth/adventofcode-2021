var paper = GetTransparentPaper("input.txt");
var folds = GetFolds("input.txt");
foreach (var fold in folds)
{
  if (fold.direction == 'y')
    paper = FoldUp(paper, fold.line);
  else
    paper = FoldLeft(paper, fold.line);
}
Print(paper);
Console.WriteLine($"Count: {CountDots(paper)}");
Console.WriteLine();

bool[,] GetTransparentPaper(string filename)
{
  List<(int X, int Y)> points = new();
  foreach (var line in System.IO.File.ReadAllLines(filename))
  {
    if (string.IsNullOrEmpty(line))
      break;
    points.Add((int.Parse(line.Split(",")[0]), int.Parse(line.Split(",")[1])));
  }
  int width = points.MaxBy(p => p.X).X+1;
  int height = points.MaxBy(p => p.Y).Y+1;
  var startPaper = new bool[width, height];
  foreach (var point in points)
  {
    startPaper[point.X, point.Y] = true;
  }
  return startPaper;
}

(char direction, int line)[] GetFolds(string filename)
{
  var foldStrings = System.IO.File.ReadAllLines(filename)
    .Where(l => l.StartsWith("fold"))
    .ToList();
  (char direction, int line)[] res = new (char direction, int line)[foldStrings.Count];
  var count = 0;
  foreach (var line in foldStrings)
  {
    var lineData = line.Remove(0, 11).Split("=");
    res[count] = (direction: lineData[0][0], line: int.Parse(lineData[1]));
    count++;
  }
  return res;
}

bool[,] FoldUp(bool[,] paper, int Y)
{
  var paperHeight = paper.GetLength(1);
  var newPaper = new bool[paper.GetLength(0), Y];
  for (var y = 0; y < Y; y++)
  {
    for (var x = 0; x < paper.GetLength(0); x++)
    {
      newPaper[x, y] = paper[x, y];
    }
  }
  for (var y = paperHeight-1; y > Y; y--)
  {
    for (var x = 0; x < paper.GetLength(0); x++)
    {
      if (!newPaper[x, Y-y+Y])
        newPaper[x, Y-y+Y] = paper[x, y];
    }
  }
  return newPaper;
}

bool[,] FoldLeft(bool[,] paper, int X)
{
  var paperWidth = paper.GetLength(0);
  var paperHeight = paper.GetLength(1);
  var newPaper = new bool[X, paperHeight];
  for (var y = 0; y < paperHeight; y++)
  {
    for (var x = 0; x < X; x++)
    {
      newPaper[x, y] = paper[x, y];
    }
  }

  for (var y = 0; y < paperHeight; y++)
  {
    for (var x = paperWidth-1; x > X; x--)
    {
      if (!newPaper[X-x+X, y])
        newPaper[X-x+X, y] = paper[x, y];
    }
  }

  return newPaper;
}

void Print(bool[,] paper)
{
  for (var y = 0; y < paper.GetLength(1); y++)
  {
    for (var x = 0; x < paper.GetLength(0); x++)
    {
      Console.Write(paper[x,y] ? "#" : ".");
    }
    Console.Write("\r\n");
  }
}

int CountDots(bool[,] paper)
{
  var count = 0;
  for (var y = 0; y < paper.GetLength(1); y++)
  {
    for (var x = 0; x < paper.GetLength(0); x++)
    {
      count += paper[x,y] ? 1 : 0;
    }
  }
  return count;
}