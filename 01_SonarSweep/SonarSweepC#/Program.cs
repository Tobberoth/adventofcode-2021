Console.WriteLine($"Depth increased {GetBasicIncrease()} times");
Console.WriteLine($"Window depth increased {GetSlidingWindow()} times");

int GetBasicIncrease()
{
  var previousMeasurement = 0;
  var increaseCount = 0;
  foreach (var line in File.ReadAllLines("input.txt"))
  {
    var measurement = int.Parse(line);
    if (measurement > previousMeasurement)
      increaseCount++;
    previousMeasurement = measurement;
  }
  return increaseCount - 1; // Ignore initial increase
}

int GetSlidingWindow()
{
  List<int> window = new();
  var previousTotal = 0;
  var increaseCount = 0;
  foreach (var line in File.ReadAllLines("input.txt"))
  {
    var measurement = int.Parse(line);
    window = window.Append(measurement).ToList();
    if (window.Count > 3)
      window.RemoveAt(0);
    if (window.Count == 3)
    {
      var total = window.Sum();
      if (total > previousTotal)
        increaseCount++;
      previousTotal = total;
    }
  }
  return increaseCount - 1; // Ignore initial increase
}