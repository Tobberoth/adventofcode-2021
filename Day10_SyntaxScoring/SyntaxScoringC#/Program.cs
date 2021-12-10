var lines = System.IO.File.ReadAllLines("input.txt");
var result = 0;
foreach (var line in lines)
  result += CheckLine(line);
Console.WriteLine($"Result: {result}");

int CheckLine(string line)
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
      {
        Console.WriteLine($"CORRUPTED: {line}");
        return GetPoints(character);
      }
    }
  }
  return 0;
}

bool IsOpen(char input)
{
  switch (input)
  {
    case '(':
    case '[':
    case '{':
    case '<':
      return true;
    default:
      return false;
  }
}

bool IsMatch(char character, char prev)
{
  switch (character)
  {
    case ')':
      return prev == '(';
    case ']':
      return prev == '[';
    case '}':
      return prev == '{';
    case '>':
      return prev == '<';
    default:
      return false;
  }
}

int GetPoints(char character)
{
  switch (character)
  {
    case ')':
      return 3;
    case ']':
      return 57;
    case '}':
      return 1197;
    case '>':
      return 25137;
    default:
      return 0;
  }
}