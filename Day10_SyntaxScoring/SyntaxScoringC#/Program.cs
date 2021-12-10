var lines = System.IO.File.ReadAllLines("input.txt");
PrintCorruptedScore(lines); // 374061
PrintIncompleteScore(lines); // 2116639949

void PrintCorruptedScore(string[] lines)
{
  var result = 0;
  foreach (var line in lines)
  {
    var points = CheckLineCorrupted(line);
    result += points;
  }
  Console.WriteLine($"Corrupted: {result}");
}

void PrintIncompleteScore(string[] lines)
{
  List<long> result = new();
  foreach (var line in lines)
  {
    var incompleteScore = CheckLineIncomplete(line);
    if (incompleteScore > 0)
      result.Add(incompleteScore);
  }
  result.Sort();
  var score = result[(int)Math.Floor(result.Count/2.0)];
  Console.WriteLine($"Incomplete: {score}");
}

int CheckLineCorrupted(string line)
{
  Stack<char> stack = new();
  foreach (var character in line)
  {
    if (IsOpen(character))
      stack.Push(character);
    else
    {
      var prev = stack.Pop();
      if (!IsMatch(character, prev))
        return GetCorruptedScore(character);
    }
  }
  return 0;
}

long CheckLineIncomplete(string line)
{
  Stack<char> stack = new();
  foreach (var character in line)
  {
    if (IsOpen(character))
      stack.Push(character);
    else
    {
      var prev = stack.Pop();
      if (!IsMatch(character, prev))
        return 0;
    }
  }
  long score = 0;
  while (stack.Count > 0)
  {
    var closingChar = stack.Pop();
    score = score * 5 + GetIncompleteScore(closingChar);
  }
  return score;
}

bool IsOpen(char input)
{
  return input switch
  {
    '(' or '[' or '{' or '<' => true,
    _ => false
  };
}

char GetMatch(char character)
{
  return character switch
  {
    '(' => ')',
    ')' => '(',
    '[' => ']',
    ']' => '[',
    '{' => '}',
    '}' => '{',
    '<' => '>',
    '>' => '<',
    _ => throw new ArgumentException("Invalid character")
  };
}

bool IsMatch(char character, char prev)
{
  return prev == GetMatch(character);
}

int GetCorruptedScore(char character)
{
  return character switch
  {
    ')' => 3,
    ']' => 57,
    '}' => 1197,
    '>' => 25137,
    _ => 0
  };
}

int GetIncompleteScore(char character)
{
  return character switch
  {
    '(' => 1,
    '[' => 2,
    '{' => 3,
    '<' => 4,
    _ => throw new ArgumentException("Invalid character for scoring")
  };
}