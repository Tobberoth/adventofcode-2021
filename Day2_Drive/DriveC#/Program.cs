int CalculateDistance()
{
  var depth = 0;
  var horizontal = 0;
  foreach (var line in File.ReadAllLines("input.txt"))
  {
    var direction = line.Split(' ')[0];
    int magnitude = int.Parse(line.Split(' ')[1]);
    switch (direction)
    {
      case "up":
        depth -= magnitude;
        break;
      case "down":
        depth += magnitude;
        break;
      case "forward":
        horizontal += magnitude;
        break;
    }
  }
  return depth * horizontal;
}

int CalculateDistanceWithAim()
{
  var aim = 0;
  var depth = 0;
  var horizontal = 0;
  foreach (var line in File.ReadAllLines("input.txt"))
  {
    var direction = line.Split(' ')[0];
    int magnitude = int.Parse(line.Split(' ')[1]);
    switch (direction)
    {
      case "up":
        aim -= magnitude;
        break;
      case "down":
        aim += magnitude;
        break;
      case "forward":
        horizontal += magnitude;
        depth += aim * magnitude;
        break;
    }
  }
  return depth * horizontal;
}

Console.WriteLine(CalculateDistance());
Console.WriteLine(CalculateDistanceWithAim());