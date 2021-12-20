void DrawVector(int[,] grid, Vector vector)
{
  foreach (var (x,y) in GetAllPoints(vector))
    grid[x,y]++;
}

List<int> GetRange(int pos1, int pos2)
{
  var diff = pos1 == pos2 ? 1 : Math.Abs(pos1 - pos2) + 1;
  var start = Math.Min(pos1, pos2);
  var range = Enumerable.Range(start, diff).ToList();
  if (range.Count > 1 && pos1 < pos2)
    range.Reverse();
  return range;
}

IEnumerable<(int x, int y)> GetAllPoints(Vector vector)
{
  var xRange = GetRange(vector.x1, vector.x2);
  var yRange = GetRange(vector.y1, vector.y2);
  while (xRange.Count < yRange.Count) // Handle vertical
    xRange.Add(xRange[0]);
  while (yRange.Count < xRange.Count) // Handle horizontal
    yRange.Add(yRange[0]);
  for (var p = 0; p < xRange.Count; p++)
    yield return (xRange[p], yRange[p]);
}

void Run(IEnumerable<Vector> vectors, bool onlyStraight)
{
  int[,] grid = new int[1000,1000];
  foreach (var vector in vectors)
  {
    if (!onlyStraight)
      DrawVector(grid, vector);
    else if (vector.x1 == vector.x2 || vector.y1 == vector.y2)
      DrawVector(grid, vector);
  }
  Console.WriteLine(GetOverlappingPoints(grid));
}

int GetOverlappingPoints(int[,] grid)
{
  var counter = 0;
  for (var x = 0; x < grid.GetLength(0); x++)
    for (var y = 0; y < grid.GetLength(1); y++)
      if (grid[x,y] > 1) counter++;
  return counter;
}

IEnumerable<Vector> ReadInput(string inputFile)
{
  return System.IO.File.ReadAllLines(inputFile)
    .Select(l => l.Split(" -> "))
    .Select(p => new Vector(
      int.Parse(p[0].Split(',')[0]),
      int.Parse(p[0].Split(',')[1]),
      int.Parse(p[1].Split(',')[0]),
      int.Parse(p[1].Split(',')[1])
    ));
}

var vectors = ReadInput("input.txt");
Run(vectors, true); // 8350
Run(vectors, false); // 19374

record Vector(int x1, int y1, int x2, int y2);