var crabPositions = System.IO.File.ReadAllLines("input.txt")[0]
  .Split(",")
  .Select(x => int.Parse(x))
  .ToList();
Run(CalculateOne); // pos 345, fuel 348996
Run(CalculateTwo); // pos 481, fuel 98231647

void Run(Func<int, int, int> calculateFunc)
{
  var resultData = (optimalSteps: Int32.MaxValue, targetPos: 0);
  for (var targetPos = 0; targetPos < crabPositions.Count; targetPos++)
  {
    var steps = 0;
    foreach (var crabPos in crabPositions)
    {
      steps += calculateFunc(crabPos, targetPos);
    }
    if (resultData.optimalSteps > steps)
    {
      resultData.optimalSteps = steps;
      resultData.targetPos = targetPos;
    }
  }

  Console.WriteLine($"Optimal target {resultData.targetPos} with {resultData.optimalSteps} fuel");
}

int CalculateOne(int crabPos, int targetPos)
{
  return Math.Abs(crabPos - targetPos);
}

int CalculateTwo(int crabPos, int targetPos)
{
  var diff = Math.Abs(crabPos - targetPos);
  var result = 0;
  foreach (var num in Enumerable.Range(1, diff))
  {
    result += num;
  }
  return result;
}