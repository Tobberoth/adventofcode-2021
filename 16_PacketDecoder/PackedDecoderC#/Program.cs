var binaryData = new BinaryData(FileToBinary("input.txt"));
var pack = GetPackage(binaryData);
//int sum = AddVersions(pack);
long result = pack.GetValue();
Console.WriteLine(result);

int AddVersions(Package pack)
{
  var ret = 0;
  ret += pack.Version;
  foreach (var subPack in pack.SubPackages)
    ret += AddVersions(subPack);
  return ret;
}

string FileToBinary(string filename)
{
  var ret = "";
  foreach (var hex in File.ReadAllLines(filename)[0])
  {
    ret += HexToBinary(hex);
  }
  return ret;
}

Package GetPackage(BinaryData binary, bool pad = true)
{
  Package ret = new();
  ret.Version = binary.GetN(3);
  ret.TypeID = binary.GetN(3);
  if (ret.TypeID == 4)
    ret.Literal = binary.GetLiteral(pad);
  else
  {
    ret.IsOperatorStylePackageCount = binary.GetOne();
    ret.SubPackages = GetSubPackages(binary, ret.IsOperatorStylePackageCount);
  }
  return ret;
}

List<Package> GetSubPackages(BinaryData binary, bool DoPackageCount)
{
  List<Package> ret = new();
  if (DoPackageCount)
  {
    var amount = binary.GetN(11);
    for (var i = 0; i < amount; i++)
    {
      ret.Add(GetPackage(binary, false));
    }
  }
  else
  {
    var length = binary.GetN(15);
    var dataStart = binary.Index;
    var dataRead = 0;
    while (dataRead != length)
    {
      ret.Add(GetPackage(binary, false));
      dataRead = binary.Index - dataStart;
    }
  }
  return ret;
}

string HexToBinary(char Hex)
{
  return Hex switch
  {
    '0' => "0000",
    '1' => "0001",
    '2' => "0010",
    '3' => "0011",
    '4' => "0100",
    '5' => "0101",
    '6' => "0110",
    '7' => "0111",
    '8' => "1000",
    '9' => "1001",
    'A' => "1010",
    'B' => "1011",
    'C' => "1100",
    'D' => "1101",
    'E' => "1110",
    'F' => "1111",
    _ => throw new ArgumentException($"{Hex} is not a valid hexadecimal symbol")
  };
}

public class Package
{
  public int Version { get; set; }
  public int TypeID { get; set; }
  public long Literal { get; set; }
  public bool IsOperatorStylePackageCount { get; set; }
  public List<Package> SubPackages { get; set; }
  
  public Package()
  {
    SubPackages = new();
  }

  public long GetValue()
  {
    long ret = 0;
    switch (TypeID)
    {
      case 0: // Sum
        foreach (var subPack in SubPackages)
        {
          ret += subPack.GetValue();
        }
        break;
      case 1: // product
        ret = 1;
        foreach (var subPack in SubPackages)
        {
          ret = ret * subPack.GetValue();
        }
        break;
      case 2: // min
        ret = long.MaxValue;
        foreach (var subPack in SubPackages)
        {
          var temp = subPack.GetValue();
          if (temp < ret)
            ret = temp;
        }
        break;
      case 3: // max
        ret = long.MinValue;
        foreach (var subPack in SubPackages)
        {
          var temp = subPack.GetValue();
          if (temp > ret)
            ret = temp;
        }
        break;
      case 4: // literal
        ret = Literal;
        break;
      case 5: // greater than
        var pack1 = SubPackages[0].GetValue();
        var pack2 = SubPackages[1].GetValue();
        ret = pack1 > pack2 ? 1 : 0;
        break;
      case 6: // less than
        var pack12 = SubPackages[0].GetValue();
        var pack22 = SubPackages[1].GetValue();
        ret = pack12 < pack22 ? 1 : 0;
        break;
      case 7: // equals
        var pack13 = SubPackages[0].GetValue();
        var pack23 = SubPackages[1].GetValue();
        ret = pack13 == pack23 ? 1 : 0;
        break;
    }
    return ret;
  }

  public override string ToString()
  {
    var ret = $"Package [\r\nType: {TypeID},\r\nVersion {Version},\r\n";
    if (Literal != default)
      ret += $"Literal: {Literal}";
    else
    {
      var operatorStyle = IsOperatorStylePackageCount ? "Package Count" : "Package Length";
      ret += $"Operator type: {operatorStyle}, \r\n";
      ret += $"Packages: [\r\n";
      foreach (var package in SubPackages)
      {
        ret += package.ToString();
      }
      ret += "]\r\n";
    }
    ret += "]\r\n";
    return ret;
  }
}

public class BinaryData
{
  private string Data { get; init; }
  public int Index { get; private set; }
  public BinaryData(string data)
  {
    Data = data;
  }

  public long GetLiteral(bool pad = true)
  {
    var internalData = "";
    var segment = "1";
    var segmentCount = 0;
    while (segment[0] == '1')
    {
      segment = Data.Substring(Index, 5);
      Index += 5;
      internalData += segment.Substring(1, 4);
      segmentCount++;
    }
    if (pad)
      Index += 8 - ((segmentCount * 5 + 6) % 8); // Skip extra zeroes
    return Convert.ToInt64(internalData, 2);
  }

  public bool GetOne()
  {
    var ret = Convert.ToInt32(Data.Substring(Index, 1), 2);
    Index += 1;
    return ret == 1;
  }

  public int GetN(int n)
  {
    var ret = Convert.ToInt32(Data.Substring(Index, n), 2);
    Index += n;
    return ret;
  }
}