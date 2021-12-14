var WIDTH = 10;
var HEIGHT = 10;
Step1(ReadInput("input.txt"));
Step2(ReadInput("input.txt"));

void Step1(Dictionary<(int, int), Octopus> octopodes)
{
  var count = 0;
  for (var round = 0; round < 100; round++)
    count += Step(octopodes);
  Console.WriteLine($"{count} flashes");
}

void Step2(Dictionary<(int, int), Octopus> octopodes)
{
  var roundCount = 0;
  while (true)
  {
    roundCount++;
    var flashes = Step(octopodes);
    if (flashes == 100)
    {
      Console.WriteLine($"100 flashes on round {roundCount}");
      break;
    }
  }
}

Dictionary<(int, int), Octopus> ReadInput(string filename)
{
  var octopodes = new Dictionary<(int,int), Octopus>();
  var lines = System.IO.File.ReadAllLines(filename);
  for (var y = 0; y < HEIGHT; y++)
    for (var x = 0; x < WIDTH; x++)
      octopodes.Add((x, y), new Octopus(int.Parse(lines[y][x].ToString()), x, y));
  return octopodes;
}

int Step(Dictionary<(int,int), Octopus> octopodes)
{
  var count = 0;
  foreach (var octopus in octopodes.Values)
  {
    octopus.HasFlashed = false;
    octopus.EnergyLevel++;
  }

  var aboutToFlash = octopodes.Values.Where(o => o.EnergyLevel > 9);
  while (aboutToFlash.Count() != 0)
  {
    foreach (var octopus in aboutToFlash)
    {
      count++;
      octopus.HasFlashed = true;
      octopus.EnergyLevel = 0;
      var allAdjacent = GetAdjacent(octopus, octopodes);
      foreach (var adj in allAdjacent)
        if (!adj.HasFlashed)
          adj.EnergyLevel++;
    }
    aboutToFlash = octopodes.Values.Where(o => o.EnergyLevel > 9 && !o.HasFlashed);
  }
  return count;
}

List<Octopus> GetAdjacent(Octopus octo, Dictionary<(int,int), Octopus> octopodes)
{
  List<Octopus> ret = new();
  if (octo.X < WIDTH-1)
    ret.Add(octopodes[(octo.X + 1, octo.Y)]);
  if (octo.X > 0)
    ret.Add(octopodes[(octo.X - 1, octo.Y)]);
  if (octo.Y < HEIGHT-1)
  {
    ret.Add(octopodes[(octo.X, octo.Y + 1)]);
    if (octo.X < WIDTH-1)
      ret.Add(octopodes[(octo.X + 1, octo.Y + 1)]);
    if (octo.X > 0)
      ret.Add(octopodes[(octo.X - 1, octo.Y + 1)]);
  }
  if (octo.Y > 0)
  {
    ret.Add(octopodes[(octo.X, octo.Y - 1)]);
    if (octo.X < WIDTH-1)
      ret.Add(octopodes[(octo.X + 1, octo.Y - 1)]);
    if (octo.X > 0)
      ret.Add(octopodes[(octo.X - 1, octo.Y - 1)]);
  }
  return ret;
}

public class Octopus
{
  public int X { get; init; }
  public int Y { get; init; }
  public int EnergyLevel { get; set; }
  public bool HasFlashed { get; set; } = false;
  public Octopus(int energyLevel, int x, int y) =>
    (EnergyLevel, X, Y) = (energyLevel, x, y);
}