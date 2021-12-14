var input = "input.txt";
Dictionary<string, Node> caves = GenerateCaves(input);
var routes = GetRoutes(caves["start"], new List<Node>());
Console.WriteLine($"{routes}");

int GetRoutes(Node start, List<Node> previous)
{
  if (start.Cave.IsEnd)
  {
    PrintStack(previous);
    return 1;
  }
  var count = 0;
  foreach (var node in start.Children)
  {
    var newPrevious = new List<Node>(previous);
    newPrevious.Add(start);
    // Do not go back to start
    if (node.Cave.IsStart) continue;
    if (!node.Cave.IsLarge)
    {
      if (node.Cave.Label == "dc")
        Console.WriteLine("");
      // Check if OK to go to this small cave
      var counts = newPrevious.Where(n => !n.Cave.IsLarge).GroupBy(n => n.Cave.Label).Select(g => g.Count());
      // If any small cave has been visited more than once and this one has been visited, can't go
      if (counts.Any(c => c > 1) && newPrevious.Any(n => n.Cave.Label == node.Cave.Label))
        continue;
    }
    count += GetRoutes(node, newPrevious);
  }
  return count;
}

void PrintStack(List<Node> previous)
{
  foreach (var node in previous)
  {
    System.Console.Write(node.Cave.Label + ",");
  }
  System.Console.Write("\r\n");
}

Dictionary<string, Node> GenerateCaves(string input)
{
  var nodes = new Dictionary<string, Node>();
  foreach (var line in System.IO.File.ReadAllLines(input))
  {
    var cavePair = line.Split('-');
    var label = cavePair[0];
    Node? startNode = null;
    if (nodes.ContainsKey(label))
      startNode = nodes[label];
    else
    {
      startNode = new Node(new Cave(label));
      nodes.Add(label, startNode);
    }
    label = cavePair[1];
    Node? endNode = null;
    if (nodes.ContainsKey(label))
      endNode = nodes[label];
    else
    {
      endNode = new Node(new Cave(label));
      nodes.Add(label, endNode);
    }
    startNode.Children.Add(endNode);
    endNode.Children.Add(startNode);
  }
  return nodes;
}

public class Node
{
  public Cave Cave { get; init; }
  public List<Node> Children { get; init; }
  public Node(Cave cave)
  {
    Cave = cave;
    Children = new List<Node>();
  }
}

public class Cave
{
  public string Label { get; init; }
  public bool IsStart => Label == "start";
  public bool IsEnd => Label == "end";
  public bool IsLarge => !IsStart && !IsEnd && Label.All(c => char.IsUpper(c));
  public Cave(string label) => Label = label;
}