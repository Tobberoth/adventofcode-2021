Run(ReadInput("input.txt"), 80); // 377263
Run(ReadInput("input.txt"), 256); // 1695929023803

void Run(List<LanternfishSwarm> fishes, int cycles)
{
  for (var cycle = 0; cycle < cycles; cycle++)
  {
    foreach (var fish in fishes.ToList())
    {
      var newFish = fish.Update();
      if (newFish != null) fishes.Add(newFish);
    }
    fishes = CombineSwarms(fishes);
  }
  Console.WriteLine($"After {cycles} cycles, {fishes.Sum(s => s.Amount)} exist");
}

List<LanternfishSwarm> ReadInput(string filePath)
{
  var fishes = System.IO.File.ReadAllLines(filePath)[0]
    .Split(',')
    .Select(i => new LanternfishSwarm(int.Parse(i), 1))
    .ToList();
  return CombineSwarms(fishes);
}

List<LanternfishSwarm> CombineSwarms(List<LanternfishSwarm> swarms)
{
  List<LanternfishSwarm> result = new();
  var ageGroups = swarms.GroupBy(s => s.Age);
  foreach (var ageGroup in ageGroups)
    result.Add(new LanternfishSwarm(ageGroup.Key, ageGroup.ToList().Sum(s => s.Amount)));
  return result;
}

public class LanternfishSwarm
{
  public long Amount { get; set; }
  public int Age { get; private set; }
  public LanternfishSwarm(long amount) => (Age, Amount) = (8, amount);
  public LanternfishSwarm(int age, long amount) => (Age, Amount) = (age, amount);
  public LanternfishSwarm? Update()
  {
    Age--;
    if (Age < 0)
    {
      Age = 6;
      return new LanternfishSwarm(Amount);
    }
    return null;
  }
}