public class SnailfishNumber : ISnailfishNumerical
{
  public ISnailfishNumerical First { get; set; } = default!;

  public ISnailfishNumerical Second { get; set; } = default!;

  public override string ToString() => $"[{First.ToString()},{Second.ToString()}]";

  public static SnailfishNumber operator +(SnailfishNumber a, SnailfishNumber b)
  {
    SnailfishNumber newSnailfishNumber = new();
    newSnailfishNumber.First = a.DeepCopy();
    newSnailfishNumber.Second = b.DeepCopy();
    
    while (true)
    {
      var explodeMemo = new ExplodeMemo();
      newSnailfishNumber = Explode(newSnailfishNumber, explodeMemo);
      if (!explodeMemo.HasHit)
      {
        var result = Split(newSnailfishNumber);
        if (result == null)
          break;
        newSnailfishNumber = result;
      }
    }

    return newSnailfishNumber;
  }

  public static SnailfishNumber Explode(SnailfishNumber number, ExplodeMemo memo, int count = 0)
  {
    if (count == 4)
    {
      memo.HasHit = true;
      memo.ReplaceWithZero = true;
      var leftValue = number.First as RegularNumber;
      var rightValue = number.Second as RegularNumber;
      if (leftValue == null || rightValue == null)
        throw new InvalidOperationException("Explosive pairs should always be composed of regular numbers");
      memo.ValueToGoLeft = leftValue.Value;
      memo.ValueToGoRight = rightValue.Value;
      return number;
    }

    // Check left
    var leftSnail = number.First as SnailfishNumber;
    if (leftSnail != null) // if null, this is a regular number, ignore
    {
      Explode(leftSnail, memo, count + 1);
      if (memo.HasHit && memo.ReplaceWithZero)
      {
          number.First = new RegularNumber(0);
          memo.ReplaceWithZero = false;
      }
      // Came up from left and has a hit, check for left number in Second
      if (memo.ValueToGoRight != null)
      {
        var reg = number.Second.GetLeftRegular();
        reg.Value += memo.ValueToGoRight.Value;
        memo.ValueToGoRight = null;
      }
    }

    // If didn't find left, check right
    if (!memo.HasHit)
    {
      var rightSnail = number.Second as SnailfishNumber;
      if (rightSnail != null)
      {
        Explode(rightSnail, memo, count + 1);
        if (memo.HasHit && memo.ReplaceWithZero)
        {
          number.Second = new RegularNumber(0);
          memo.ReplaceWithZero = false;
        }
        // Came up from right and has a hit, check for right number in First
        if (memo.ValueToGoLeft != null)
        {
          var reg = number.First.GetRightRegular();
          reg.Value += memo.ValueToGoLeft.Value;
          memo.ValueToGoLeft = null;
        }
      }
    }

    return number;
  }

  private static SnailfishNumber? Split(ISnailfishNumerical target)
  {
    var regNum = target as RegularNumber;
    if (regNum != null && regNum.Value > 9)
      return PerformSplit(regNum);
    var snail = target as SnailfishNumber;
    if (snail != null)
    {
      var newNumber = Split(snail.First);
      if (newNumber != null)
      {
        var temp = new SnailfishNumber();
        temp.First = newNumber;
        temp.Second = snail.Second.DeepCopy();
        return temp;
      }
      var newNumber2 = Split(snail.Second);
      if (newNumber2 != null)
      {
        var temp = new SnailfishNumber();
        temp.First = snail.First.DeepCopy();
        temp.Second = newNumber2;
        return temp;
      }
    }
    return null;
  }

  private static SnailfishNumber PerformSplit(RegularNumber regNum)
  {
    var newLeft = (int)Math.Floor(regNum.Value / 2.0);
    var newRight = (int)Math.Ceiling(regNum.Value / 2.0);
    var tempNumber = new SnailfishNumber();
    tempNumber.First = new RegularNumber(newLeft);
    tempNumber.Second = new RegularNumber(newRight);
    return tempNumber;
  }

  public int GetMagnitude() => (First.GetMagnitude() * 3) + (Second.GetMagnitude() * 2);

  public ISnailfishNumerical DeepCopy()
  {
    var newNumber = new SnailfishNumber();
    newNumber.First = First.DeepCopy();
    newNumber.Second = Second.DeepCopy();
    return newNumber;
  }

  public RegularNumber GetLeftRegular() => First.GetLeftRegular();

  public RegularNumber GetRightRegular() => Second.GetRightRegular();

  public static SnailfishNumber GetFromString(string input)
  {
    var line = input;
    var numberStack = new Stack<SnailfishNumber>();
    foreach (var character in line)
    {
      if (character == '[')
        numberStack.Push(new SnailfishNumber());
      else if (char.IsNumber(character))
      {
        var currentNumber = numberStack.Pop();
        if (currentNumber.First == null)
          currentNumber.First = new RegularNumber(int.Parse(character.ToString()));
        else
          currentNumber.Second = new RegularNumber(int.Parse(character.ToString()));
        numberStack.Push(currentNumber);
      }
      else if (character == ',')
      { /* Do Nothing */ }
      else if (character == ']')
      {
        var finished = numberStack.Pop();
        if (numberStack.Count < 1) // Last one
          return finished;
        var current = numberStack.Pop();
        if (current.First == null)
          current.First = finished;
        else
          current.Second = finished;
        numberStack.Push(current);
      }
    }
    return numberStack.Pop();
  }
}