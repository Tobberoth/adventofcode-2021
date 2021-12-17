var shooter = new Shooter("input.txt");
var maxInitial = 500;
var highestY = 0;
for (var x = -maxInitial; x < maxInitial; x++)
{
  for (var y = -maxInitial; y < maxInitial; y++)
  {
    var maxY = shooter.Shoot(x, y);
    if (maxY > highestY)
      highestY = maxY;
  }
}
Console.WriteLine($"Highest Y: {highestY}");

public class Shooter
{
  public List<int> XRange { get; set; }
  public List<int> YRange { get; set; }
  public int MaxSteps = 500;

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

  public int Shoot(int x, int y)
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
      if (IsProbeInRange(probe.x, probe.y))
        return maxY;
    }
    return 0;
  }

  private bool IsProbeInRange(int x, int y)
  {
    return (XRange.Contains(x) && YRange.Contains(y));
  }

  public override string ToString()
  {
    return $"Shooter [XRange: [{XRange.First()}..{XRange.Last()}] YRange: [{YRange.First()}..{YRange.Last()}]]";
  }
}