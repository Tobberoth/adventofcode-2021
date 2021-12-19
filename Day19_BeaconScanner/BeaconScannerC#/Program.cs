using System.Text.RegularExpressions;

var scanners = ReadScannersFromFile("input.txt");
var normalizedScanners = NormalizeScanners(scanners);
Console.WriteLine($"Unique beacons: " + GetUniqueBeacons(normalizedScanners));
Console.WriteLine("Max manhattan distance: " + GetMaxManhattanDistance(normalizedScanners));

List<Scanner> NormalizeScanners(List<Scanner> scanners)
{
  var normalizedScanners = new List<Scanner>();
  var notNormalizedScanners = scanners.ToList();
  normalizedScanners.Add(notNormalizedScanners[0]);
  notNormalizedScanners.Remove(notNormalizedScanners[0]);
  while (notNormalizedScanners.Count > 0)
  {
    foreach (var normalizedScanner in normalizedScanners.ToList())
    {
      for (var i = 0; i < notNormalizedScanners.ToList().Count; i++)
      {
        var matches = GetMatches(normalizedScanner, notNormalizedScanners[i]);
        if (matches.Count > 11)
        {
          var norm = notNormalizedScanners[i].GetNormalizedScanner(matches);
          normalizedScanners.Add(norm);
          notNormalizedScanners.Remove(notNormalizedScanners[i]);
        }
      }
    }
  }
  return normalizedScanners;
}

int GetUniqueBeacons(List<Scanner> normalizedScanners)
{
  return normalizedScanners.SelectMany(s => s.Beacons).ToHashSet().Count;
}

int GetMaxManhattanDistance(List<Scanner> normalizedScanners)
{
  return normalizedScanners
    .Select(scanner =>
      normalizedScanners.Select(scanner2 => scanner.GetManhattanDistance(scanner2)).Max())
    .Max();
}

HashSet<BeaconMatch> GetMatches(Scanner scanner1, Scanner scanner2)
{
  var data = Scanner.GetSameDistanceBeaconPairs(scanner1, scanner2).ToList();
  HashSet<BeaconMatch> ret = new();
  var iGroups = data.GroupBy(d => d.aPair.i);
  foreach (var iGroup in iGroups)
  {
    if (iGroup.Count() > 4)
    {
      var maxIGroup = iGroup.Select(x => x.bPair.i).GroupBy(i => i).MaxBy(ig => ig.Count());
      var maxJGroup = iGroup.Select(x => x.bPair.j).GroupBy(j => j).MaxBy(jg => jg.Count());
      var Corresponder = new[] { maxIGroup, maxJGroup }.MaxBy(g => g!.Count())!.Key;
      ret.Add(new BeaconMatch(scanner1.ID, scanner2.ID, scanner1.Beacons[iGroup.Key], scanner2.Beacons[Corresponder]));
    }
  }
  var jGroups = data.GroupBy(d => d.aPair.j);
  foreach (var jGroup in jGroups)
  {
    if (jGroup.Count() > 4)
    {
      var maxIGroup = jGroup.Select(x => x.bPair.i).GroupBy(i => i).MaxBy(ig => ig.Count());
      var maxJGroup = jGroup.Select(x => x.bPair.j).GroupBy(j => j).MaxBy(jg => jg.Count());
      var Corresponder = new[] { maxIGroup, maxJGroup }.MaxBy(g => g!.Count())!.Key;
      ret.Add(new BeaconMatch(scanner1.ID, scanner2.ID, scanner1.Beacons[jGroup.Key], scanner2.Beacons[Corresponder]));
    }
  }
  return ret;
}

List<Scanner> ReadScannersFromFile(string filename)
{
  List<Scanner> ret = new();
  Scanner? currentScanner = null;
  foreach (var line in File.ReadAllLines(filename))
  {
    if (line.Contains("scanner"))
    {
      if (currentScanner != null)
      {
        currentScanner.CalculateBeaconDistances();
        ret.Add(currentScanner);
      }
      var id = int.Parse(Regex.Match(line, "\\d+").Value);
      currentScanner = new Scanner(id);
    }
    if (line.Contains(","))
    {
      var positions = line.Split(',').Select(p => int.Parse(p)).ToList();
      currentScanner!.Beacons.Add(new Beacon(positions[0], positions[1], positions[2]));
    }
  }
  currentScanner!.CalculateBeaconDistances();
  ret.Add(currentScanner!);
  return ret;
}

public record BeaconMatch(int AScannerID, int BScannerID, Beacon ABeacon, Beacon BBeacon);