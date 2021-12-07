var crabPositions = System.IO.File.ReadAllLines("input.txt")[0]
  .Split(",")
  .Select(x => int.Parse(x))
  .ToList();

var resultData = (optimalSteps: Int32.MaxValue, targetPos: 0);
for (var targetPos = 0; targetPos < crabPositions.Count; targetPos++)
{
  var steps = 0;
  foreach (var crabPos in crabPositions)
  {
    steps += Math.Abs(crabPos - targetPos);
  }
  if (resultData.optimalSteps > steps)
  {
    resultData.optimalSteps = steps;
    resultData.targetPos = targetPos;
  }
}

Console.WriteLine($"Optimal target {resultData.targetPos} with {resultData.optimalSteps} fuel");