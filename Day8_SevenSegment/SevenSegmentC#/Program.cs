Run1(ReadInput("input.txt"));

List<Analysis> ReadInput(string filename)
{
  return System.IO.File.ReadAllLines(filename)
    .Select(s => new Analysis(s.Split(" | ")[0].Split(' '), s.Split(" | ")[1].Split(' ')))
    .ToList();
}

void Run1(List<Analysis> data)
{
  var counter = 0;
  foreach (var analysis in data)
  {
    foreach (var outValue in analysis.OutputValues)
    {
      if (outValue.Length == 2 || outValue.Length == 3 || outValue.Length == 4 || outValue.Length == 7)
        counter++;
    }
  }
  Console.WriteLine($"{counter}");
}

record Analysis(string[] InputValues, string[] OutputValues);
