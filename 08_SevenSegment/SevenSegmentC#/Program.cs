Run1(ReadInput("input.txt"));
Run2(ReadInput("input.txt"));

List<Analysis> ReadInput(string filename)
{
  return System.IO.File.ReadAllLines(filename)
    .Select(s => {
      var inpValues = s.Split(" | ")[0].Split(' ');
      var outValues = s.Split(" | ")[1].Split(' ');
      return new Analysis(
        inpValues.Select(s => s.ToHashSet()).ToList(),
        outValues.Select(s => s.ToHashSet()).ToList()
      );
    })
    .ToList();
}

void Run1(List<Analysis> data)
{
  var counter = 0;
  foreach (var analysis in data)
  {
    foreach (var outValue in analysis.OutputValues)
    {
      if (outValue.Count == 2 || outValue.Count == 3 || outValue.Count == 4 || outValue.Count == 7)
        counter++;
    }
  }
  Console.WriteLine($"{counter}");
}

void Run2(List<Analysis> data)
{
  var index = 0;
  var totalValue = 0;
  foreach (var analysis in data)
  {
    Dictionary<int, HashSet<char>> valToCharData = new();
    Dictionary<HashSet<char>, int> charToValData = new();
    var allValues = analysis.InputValues.Concat(analysis.OutputValues);
    foreach (var value in allValues)
    {
      switch (value.Count)
      {
        case 2:
          charToValData[value] = 1;
          valToCharData[1] = value;
          break;
        case 3:
          charToValData[value] = 7;
          valToCharData[7] = value;
          break;
        case 4:
          charToValData[value] = 4;
          valToCharData[4] = value;
          break;
        case 7:
          charToValData[value] = 8;
          valToCharData[8] = value;
          break;
      }
    }

    var topSegment = valToCharData[7].First(c => !valToCharData[1].Contains(c));
    var almostNine = valToCharData[4].Concat(new [] { topSegment }).ToHashSet();
    var nine = allValues.FirstOrDefault(s => s.IsSupersetOf(almostNine) && s.Count == almostNine.Count+1);
    if (nine != null)
    {
      charToValData[nine] = 9;
      valToCharData[9] = nine;
    }
    var zero = allValues.FirstOrDefault(s => s.IsSupersetOf(valToCharData[1]) && s.Count == 6 && !s.SetEquals(valToCharData[9]));
    if (zero != null)
    {
      valToCharData[0] = zero;
      charToValData[zero] = 0;
    }
    var six = allValues.FirstOrDefault(s => s.Count == 6 && !s.SetEquals(valToCharData[9]) && !s.SetEquals(valToCharData[0]));
    if (six != null)
    {
      valToCharData[6] = six;
      charToValData[six] = 6;
    }
    var three = allValues.FirstOrDefault(s => s.Count == 5 && s.IsSupersetOf(valToCharData[1]));
    if (three != null)
    {
      valToCharData[3] = three;
      charToValData[three] = 3;
    }
    var five = allValues.FirstOrDefault(s => s.Count == 5 && s.IsSubsetOf(valToCharData[6]));
    if (five != null)
    {
      valToCharData[5] = five;
      charToValData[five] = 5;
    }
    var two = allValues.FirstOrDefault(s => s.Count == 5 && !valToCharData.Values.Contains(s));
    if (two != null)
    {
      valToCharData[2] = two;
      charToValData[two] = 2;
    }

    var finalOutputValue = 0;
    var key = charToValData.Keys.First(k => k.SetEquals(analysis.OutputValues[0]));
    finalOutputValue += charToValData[key] * 1000;
    key = charToValData.Keys.First(k => k.SetEquals(analysis.OutputValues[1]));
    finalOutputValue += charToValData[key] * 100;
    key = charToValData.Keys.First(k => k.SetEquals(analysis.OutputValues[2]));
    finalOutputValue += charToValData[key] * 10;
    key = charToValData.Keys.First(k => k.SetEquals(analysis.OutputValues[3]));
    finalOutputValue += charToValData[key] * 1;
    Console.WriteLine($"{finalOutputValue}");
    totalValue += finalOutputValue;
    index++;
  }
  Console.WriteLine($"Final Total: {totalValue}");
}

record Analysis(List<HashSet<char>> InputValues, List<HashSet<char>> OutputValues);
