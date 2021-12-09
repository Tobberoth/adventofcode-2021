Run(ReadInput("input.txt"));

void Run(int[,] grid)
{
  var risk = 0;
  List<int> BasinSizes = new();
  for (var y = 0; y < grid.GetLength(1); y++)
  {
    for (var x = 0; x < grid.GetLength(0); x++)
    {
      if (IsPositionLowPoint(grid, x, y))
      {
        BasinSizes.Add(GetBasinSize(grid, x, y));
        risk += grid[x, y] + 1;
      }
    }
  }
  Console.WriteLine($"Risk: {risk}");
  var topBasins = BasinSizes
    .OrderByDescending(x => x)
    .Take(3)
    .Aggregate(1, (acc, x) => acc * x);
  Console.WriteLine($"Biggest Basins: {topBasins}");
}

int GetBasinSize(int[,] grid, int x, int y, HashSet<(int,int)>? Counted = null)
{
  if (Counted == null)
    Counted = new HashSet<(int,int)>();
  var result = 0;
  if (Counted.Contains((x, y)))
    return 0;
  Counted.Add((x, y));
  var thisPoint = grid[x,y];
  if (thisPoint == 9)
    return 0;
  if (x > 0)
    if (thisPoint <= grid[x-1, y])
      result += GetBasinSize(grid, x-1, y, Counted);
  if (x < grid.GetLength(0) - 1)
    if (thisPoint <= grid[x+1, y])
      result += GetBasinSize(grid, x+1, y, Counted);
  if (y > 0)
    if (thisPoint <= grid[x, y-1])
      result += GetBasinSize(grid, x, y-1, Counted);
  if (y < grid.GetLength(1) - 1)
    if (thisPoint <= grid[x, y+1])
      result += GetBasinSize(grid, x, y+1, Counted);
  return result + 1;
}

bool IsPositionLowPoint(int[,] grid, int x, int y)
{
  var point = grid[x, y];
  if ((x > 0 && point >= grid[x-1, y]) ||
    ((x < grid.GetLength(0) - 1) && point >= grid[x+1, y]) ||
    (y > 0 && point >= grid[x, y-1]) ||
    ((y < grid.GetLength(1) - 1) && point >= grid[x, y+1]))
    return false;
  return true;
}

int[,] ReadInput(string filename)
{
  var lines = System.IO.File.ReadAllLines(filename).Where(l => !string.IsNullOrEmpty(l)).ToList();
  var rowCount = lines.Count;
  var colCount = lines[0].Length;
  var result = new int[colCount, rowCount];
  for (var y = 0; y < rowCount; y++)
    for (var x = 0; x < colCount; x++)
      result[x, y] = int.Parse(lines[y][x].ToString());
  return result;
}