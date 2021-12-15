var filename = "input.txt";
Run(ReadInput(filename)); // 562
var HugeZone = GenerateHugeZone(ReadInput(filename));
Run(HugeZone); // 2874

void Run(Spot[,] RiskZones)
{
  var MAXWIDTH = RiskZones.GetLength(0)-1;
  var MAXHEIGHT = RiskZones.GetLength(1)-1;
  for (var i = 0; i < 5; i++) // Gets more precise each run, probably needs more runs for more complex paths
  {
    for (var y = 0; y < MAXHEIGHT+1; y++)
      for (var x = 0; x < MAXWIDTH+1; x++)
        RiskZones[x,y].FastestRoute = CalculateFastestRoute(RiskZones[x, y], RiskZones, MAXWIDTH, MAXHEIGHT);
  }
  var lastSpot = RiskZones[MAXWIDTH,MAXHEIGHT];
  Console.WriteLine(lastSpot.FastestRoute.Risk);
}

Spot[,] GenerateHugeZone(Spot[,] spots)
{
  Spot[,] LargeZone = new Spot[spots.GetLength(0)*5,spots.GetLength(1)*5];
  for(var y = 0; y < spots.GetLength(1); y++)
  {
    for(var x = 0; x < spots.GetLength(0); x++)
    {
      for (var by = 0; by < 5; by++)
      {
        for (var bx = 0; bx < 5; bx++)
        {
          var exactX = (bx*spots.GetLength(0))+x;
          var exactY = (by*spots.GetLength(1))+y;
          var risk = spots[x,y].Risk + bx + by;
          if (risk > 9)
            risk = risk % 9;
          if (risk == 0) risk = 1;
          LargeZone[exactX, exactY] = new Spot(exactX, exactY, risk);
        }
      }
    }
  }
  return LargeZone;
}

Route CalculateFastestRoute(Spot spot, Spot[,] riskZones, int MAXWIDTH, int MAXHEIGHT)
{
  if (spot.X == 0 && spot.Y == 0)
  {
    return new Route {
      FullRoute = new List<(int x, int y)>() { (0,0) },
      Risk = 0
    };
  }

  // Check left, top, right, bottom
  List<Route> routes = new();
  if (spot.X > 0)
  {
    var leftRoute = riskZones[spot.X-1, spot.Y].FastestRoute;
    if (leftRoute != null)
      routes.Add(leftRoute);
  }
  if (spot.Y > 0)
  {
    var topRoute = riskZones[spot.X, spot.Y-1].FastestRoute;
    if (topRoute != null)
      routes.Add(topRoute);
  }
  if (spot.X < MAXWIDTH)
  {
    var rightRoute = riskZones[spot.X+1, spot.Y].FastestRoute;
    if (rightRoute != null)
      routes.Add(rightRoute);
  }
  if (spot.Y < MAXHEIGHT)
  {
    var bottomRoute = riskZones[spot.X, spot.Y+1].FastestRoute;
    if (bottomRoute != null)
      routes.Add(bottomRoute);
  }
  var minRoute = routes.MinBy(r => r.Risk);
  return new Route {
    FullRoute = minRoute.FullRoute.Concat(new[] { (spot.X, spot.Y) }).ToList(),
    Risk = minRoute.Risk + spot.Risk
  };
}

Spot[,] ReadInput(string filename)
{
  var lines = System.IO.File.ReadAllLines(filename);
  var maxX = lines[0].Length;
  Spot[,] ret = new Spot[maxX, lines.Length];
  for (var y = 0; y < lines.Length; y++)
    for (var x = 0; x < lines[y].Length; x++)
    {
      ret[x, y] = new Spot(x, y, int.Parse(lines[y][x].ToString()));
    }
  return ret;
}

public class Spot
{
  public int X { get; init; }
  public int Y { get; init; }
  public int Risk { get; init; }
  public Route? FastestRoute { get; set; }
  public Spot(int x, int y, int risk)
  {
    (X, Y, Risk) = (x, y , risk);
    FastestRoute = null;
  }
}

public class Route
{
  public List<(int x, int y)> FullRoute { get; set; }
  public int Risk { get; set; }
  public Route()
  {
    FullRoute = new();
  }
}