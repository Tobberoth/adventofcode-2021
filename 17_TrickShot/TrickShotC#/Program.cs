var shooter = new Shooter("input.txt");
var maxInitial = 200;
var highestY = 0;
var countHits = 0;
for (var x = -maxInitial; x < maxInitial; x++)
{
  for (var y = -maxInitial; y < maxInitial; y++)
  {
    (bool hit, int maxY) = shooter.Shoot(x, y);
    if (maxY > highestY)
      highestY = maxY;
    if (hit)
      countHits++;
  }
}
Console.WriteLine($"Highest Y: {highestY}");
Console.WriteLine($"Hit {countHits} times");

public class Shooter
{
  public List<int> XRange { get; set; }
  public List<int> YRange { get; set; }
  public int MaxSteps = 400;

  public Shooter(string filename)
  {
    var ranges = File.ReadAllLines(filename)[0]
      .Remove(0, 13)
      .Split(", ");
    var xRangeStartEnd = ranges[0].Remove(0, 2)
      .Split("..")
      .Select(x => int.Parse(x))
      .ToList();
    var yRangeStartEnd = ranges[1].Remove(0, 2)
      .Split("..")
      .Select(y => int.Parse(y))
      .ToList();
    XRange = Enumerable.Range(
      xRangeStartEnd[0],
      xRangeStartEnd[1] + 1 - xRangeStartEnd[0]).ToList();
    YRange = Enumerable.Range(
      yRangeStartEnd[0],
      yRangeStartEnd[1] + 1 - yRangeStartEnd[0]).ToList();
  }

  public (bool hit, int maxY) Shoot(int x, int y)
  {
    var probe = (x: 0, y: 0);
    var maxY = 0;
    for (var step = 0; step < MaxSteps; step++)
    {
      probe.x += x;
      probe.y += y;
      if (probe.y > maxY)
        maxY = probe.y;
      if (x > 0) x--;
      else if (x < 0) x++;
      y--;
      if (XRange.Contains(probe.x) && YRange.Contains(probe.y))
        return (true, maxY);
    }
    return (false, 0);
  }
}