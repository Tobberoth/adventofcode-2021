public record Beacon(int X, int Y, int Z)
{
  public static double GetDistance(Beacon a, Beacon b)
  {
    return Math.Sqrt(
      Math.Pow((b.X - a.X), 2) +
      Math.Pow((b.Y - a.Y), 2) +
      Math.Pow((b.Z - a.Z), 2));
  }

  public static Beacon Rotate(Beacon beacon, char axis)
  {
    switch (axis)
    {
      case 'Z':
        return new Beacon(
          (int)Math.Round((beacon.X * Math.Cos(Math.PI / 2)) - (beacon.Y * Math.Sin(Math.PI / 2))),
          (int)Math.Round((beacon.X * Math.Sin(Math.PI / 2)) + (beacon.Y * Math.Cos(Math.PI / 2))),
          beacon.Z
        );
      case 'X':
        return new Beacon(
          beacon.X,
          (int)Math.Round((beacon.Y * Math.Cos(Math.PI / 2)) - (beacon.Z * Math.Sin(Math.PI / 2))),
          (int)Math.Round((beacon.Y * Math.Sin(Math.PI / 2)) + (beacon.Z * Math.Cos(Math.PI / 2)))
        );
      case 'Y':
        return new Beacon(
          (int)Math.Round((beacon.X * Math.Cos(Math.PI / 2)) + (beacon.Z * Math.Sin(Math.PI / 2))),
          beacon.Y,
          (int)Math.Round(-(beacon.X * Math.Sin(Math.PI / 2)) - (beacon.Z * Math.Cos(Math.PI / 2)))
        );
    }
    return beacon;
  }
}
