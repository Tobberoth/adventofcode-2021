Run("input.txt", 10);
Run("input.txt", 40);

void Run(string inputFile, int runs)
{
  var initial = System.IO.File.ReadAllLines(inputFile)[0];
  var rules = GetRulesFromInput(inputFile);
  var couples = GenerateCouplesFromInput(initial, rules);
  for (var i = 0; i < runs; i++)
    couples = Step(couples, rules);
  var charCount = GetCharCount(couples, initial);
  var max = charCount.MaxBy(d => d.a);
  var min = charCount.MinBy(d => d.a);
  Console.WriteLine(max.a - min.a);
}

IEnumerable<(char c, long a)> GetCharCount(Dictionary<string, long> couples, string initial)
{
  var data = couples.GroupBy(c => c.Key[1])
    .Select(g => (c: g.Key, a: g.Sum(c => c.Value)));
  var firstLetter = data.First(d => d.c == initial[0]);
  firstLetter.a++;
  return data;
}

Dictionary<string, string> GetRulesFromInput(string inputFile)
{
  return System.IO.File.ReadAllLines(inputFile)
    .Skip(2)
    .Select(l => (rule: l.Split(" -> ")[0], insert: l.Split(" -> ")[1]))
    .ToDictionary(l => l.rule, l => l.insert);
}

Dictionary<string, long> GenerateCouplesFromInput(string initial, Dictionary<string,string> rules)
{
  Dictionary<string,long> couples = new();
  var allChars = initial.Concat(rules.Select(r => r.Value[0])).Distinct();
  foreach (var char1 in allChars)
    foreach (var char2 in allChars)
      couples.TryAdd($"{char1}{char2}", 0);
  for (var i = 0; i < initial.Length-1; i++)
    couples[initial.Substring(i, 2)]++;
  return couples;
}

Dictionary<string,long> Step(Dictionary<string,long> couples, Dictionary<string,string> rules)
{
  Dictionary<string,long> newCouples = new Dictionary<string, long>(couples);
  foreach (var rule in rules)
  {
    if (!couples.ContainsKey(rule.Key)) continue;
    var amount = couples[rule.Key];
    newCouples[rule.Key] -= amount;
    var newKey1 = $"{rule.Key[0]}{rule.Value}"; 
    var newKey2 = $"{rule.Value}{rule.Key[1]}"; 
    newCouples[newKey1] += amount;
    newCouples[newKey2] += amount;
  }
  return newCouples;
}