var BITLENGTH = 12;

int getPowerConsumption(List<int> binaries)
{
  int gamma = 0, epsilon = 0;
  for (var i = BITLENGTH; i > 0; i--)
  {
    int on = 0, off = 0;
    foreach (var bin in binaries)
    {
      _ = ((bin >> i-1) & 1) == 1
        ? on++
        : off++;
    }
    if (on > off)
      gamma += 1 << i-1;
    else
      epsilon += 1 << i-1; 
  }
  return gamma * epsilon;
}

int filter(List<int> binaries, bool getCommon)
{
  List<int> result = new(binaries);
  for (var i = BITLENGTH; i > 0; i--)
  {
    List<int> onNums = new();
    List<int> offNums = new();
    foreach (var bin in result)
    {
      if (((bin >> i-1) & 1) == 1)
        onNums.Add(bin);
      else
        offNums.Add(bin);
    }
    if (onNums.Count >= offNums.Count)
      result = getCommon ? onNums : offNums;
    else
      result = getCommon ? offNums : onNums;
    if (result.Count == 1)
      return result[0];
  }
  return 0;
}

int getLifeSupportRating(List<int> binaries)
{
  return filter(binaries, true) * filter(binaries, false);
}

var diagnostics = System.IO.File.ReadAllLines("input.txt");
var binaries = diagnostics.Select(i => Convert.ToInt32(i, 2)).ToList();
Console.WriteLine($"PowerConsumptionRating: {getPowerConsumption(binaries)}");
Console.WriteLine($"LifeSupportRating: {getLifeSupportRating(binaries)}");