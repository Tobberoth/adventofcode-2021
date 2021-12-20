public class Scanner
{
  public int ID { get; init; }

  public int X { get; set; }
  public int Y { get; set; }
  public int Z { get; set; }

  public List<Beacon> Beacons { get; init; }

  public Dictionary<double, (int b1, int b2)> BeaconDistances { get; init; }

  public Scanner(int id)
  {
    ID = id;
    Beacons = new();
    BeaconDistances = new();
  }

  internal Scanner GetNormalizedScanner(HashSet<BeaconMatch> matches)
  {
    var match1 = matches.First();
    var match2 = matches.Skip(1).First();
    var match1BBeacon = match1.BBeacon;
    var match2BBeacon = match2.BBeacon;
    
    // Normalize Rotation
    Beacon properlyRotatedBBeacon = new Beacon(0,0,0);
    List<(char axis, int count)> RotationMemory = new();
    var zRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, match1BBeacon, match2BBeacon, 'Z');
    var xRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, match1BBeacon, match2BBeacon, 'X');
    var yRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, match1BBeacon, match2BBeacon, 'Y');
    if (zRotationData != null)
    {
      xRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, zRotationData.Value.rotatedMatch1B, zRotationData.Value.rotatedMatch2B, 'X');
      yRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, xRotationData!.Value.rotatedMatch1B, xRotationData.Value.rotatedMatch2B, 'Y');
      RotationMemory.Add(('Z', zRotationData.Value.rotations));
      RotationMemory.Add(('X', xRotationData.Value.rotations));
      RotationMemory.Add(('Y', yRotationData!.Value.rotations));
      properlyRotatedBBeacon = yRotationData.Value.rotatedMatch1B;
    }
    else if (xRotationData != null)
    {
      zRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, xRotationData.Value.rotatedMatch1B, xRotationData.Value.rotatedMatch2B, 'Z');
      yRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, zRotationData!.Value.rotatedMatch1B, zRotationData.Value.rotatedMatch2B, 'Y');
      RotationMemory.Add(('X', xRotationData.Value.rotations));
      RotationMemory.Add(('Z', zRotationData.Value.rotations));
      RotationMemory.Add(('Y', yRotationData!.Value.rotations));
      properlyRotatedBBeacon = yRotationData.Value.rotatedMatch1B;
    }
    else if (yRotationData != null)
    {
      zRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, yRotationData.Value.rotatedMatch1B, yRotationData.Value.rotatedMatch2B, 'Z');
      xRotationData = FindRotationMatch(match1.ABeacon, match2.ABeacon, zRotationData!.Value.rotatedMatch1B, zRotationData.Value.rotatedMatch2B, 'X');
      RotationMemory.Add(('Y', yRotationData.Value.rotations));
      RotationMemory.Add(('Z', zRotationData.Value.rotations));
      RotationMemory.Add(('X', xRotationData!.Value.rotations));
      properlyRotatedBBeacon = xRotationData.Value.rotatedMatch1B;
    }

    // Calculate position
    var trueX = match1.ABeacon.X + properlyRotatedBBeacon.X * -1; 
    var trueY = match1.ABeacon.Y + properlyRotatedBBeacon.Y * -1;
    var trueZ = match1.ABeacon.Z + properlyRotatedBBeacon.Z * -1;

    var normalizedScanner = new Scanner(900 + this.ID);
    normalizedScanner.X = trueX;
    normalizedScanner.Y = trueY;
    normalizedScanner.Z = trueZ;
    foreach (var beacon in this.Beacons)
    {
      var rBeacon = beacon;
      foreach ((char axis, int count) in RotationMemory)
        for (var rot = 0; rot < count; rot++)
          rBeacon = Beacon.Rotate(rBeacon, axis);
      normalizedScanner.Beacons.Add(new Beacon(rBeacon.X + trueX, rBeacon.Y + trueY, rBeacon.Z + trueZ));
    }
    normalizedScanner.CalculateBeaconDistances();
    return normalizedScanner;
  }

  private (Beacon rotatedMatch1B, Beacon rotatedMatch2B, int rotations)? FindRotationMatch(Beacon match1ABeacon, Beacon match2ABeacon, Beacon match1BBeacon, Beacon match2BBeacon, char axis)
  {
    var aXDiff = match1ABeacon.X - match2ABeacon.X;
    var aZDiff = match1ABeacon.Z - match2ABeacon.Z;
    var bXDiff = match1BBeacon.X - match2BBeacon.X;
    var bZDiff = match1BBeacon.Z - match2BBeacon.Z;
    var rotatedMatch1B = match1BBeacon;
    var rotatedMatch2B = match2BBeacon;
    var RotationCount = 0;
    while (RotationCount < 4)
    {
      RotationCount++;
      rotatedMatch1B = Beacon.Rotate(rotatedMatch1B, axis);
      rotatedMatch2B = Beacon.Rotate(rotatedMatch2B, axis);
      bXDiff = rotatedMatch1B.X - rotatedMatch2B.X;
      bZDiff = rotatedMatch1B.Z - rotatedMatch2B.Z;
      switch (axis)
      {
        case 'X':
          if (aZDiff == bZDiff)
            return (rotatedMatch1B, rotatedMatch2B, RotationCount);
          break;
        case 'Y':
          if (aXDiff == bXDiff)
            return (rotatedMatch1B, rotatedMatch2B, RotationCount);
          break;
        case 'Z':
          if (aXDiff == bXDiff)
            return (rotatedMatch1B, rotatedMatch2B, RotationCount);
          break;
      }
    }
    return null;
  }

  public void CalculateBeaconDistances()
  {
    for (var i = 0; i < Beacons.Count; i++)
      for (var j = i+1; j < Beacons.Count; j++)
        if (i != j)
          BeaconDistances.Add(Beacon.GetDistance(Beacons[i], Beacons[j]), (i,j));
  }

  public int GetManhattanDistance(Scanner b)
  {
      var xDiff = X - b.X;
      var yDiff = Y - b.Y;
      var zDiff = Z - b.Z;
      return Math.Abs(xDiff) + Math.Abs(yDiff) + Math.Abs(zDiff);
  }

  public static IEnumerable<((int i, int j) aPair, (int i, int j) bPair, double Distance)> GetSameDistanceBeaconPairs(Scanner a, Scanner b)
  {
    foreach (var ad in a.BeaconDistances)
      foreach (var bd in b.BeaconDistances)
        if (ad.Key == bd.Key) yield return (aPair: ad.Value, bPair: bd.Value, Distance: ad.Key);
  }
}
