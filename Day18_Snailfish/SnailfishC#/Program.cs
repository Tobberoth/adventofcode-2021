StepOne();
StepTwo();

void StepOne()
{
  var numbers = File.ReadAllLines("input.txt");
  var num1 = SnailfishNumber.GetFromString(numbers[0]);
  for (var i = 1; i < numbers.Length; i++)
    num1 = num1 + SnailfishNumber.GetFromString(numbers[i]);
  Console.WriteLine(num1);
  Console.WriteLine($"Magnitude: {num1.GetMagnitude()}");
}

void StepTwo()
{
  var numbers = File.ReadAllLines("input.txt")
    .Select(l => SnailfishNumber.GetFromString(l))
    .ToList();
  var maxMagnitude = 0;
  foreach (var num1 in numbers)
  {
    foreach (var num2 in numbers)
    {
      if (!System.Object.ReferenceEquals(num1, num2))
      {
        var res = num1 + num2;
        var mag = res.GetMagnitude();
        if (mag > maxMagnitude)
          maxMagnitude = mag;
      }
    }
  }
  Console.WriteLine($"Max magnitude: {maxMagnitude}");
}