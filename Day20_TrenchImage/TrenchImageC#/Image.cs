public class Image
{
  public int Width { get; init; }
  public int Height { get; init; }
  public bool InfiniteValue { get; set; }
  private bool[,] ImageData { get; init; }
  public Image(string filename)
  {
    var lines = File.ReadAllLines(filename).Skip(2).ToList();
    Height = lines.Count;
    Width = lines[0].Length;
    ImageData = new bool[Width, Height];
    for (var y = 0; y < Height; y++)
      for (var x = 0; x < Width; x++)
        ImageData[x, y] = lines[y][x] == '#' ? true : false;
  }
  public Image(int width, int height)
  {
    Width = width;
    Height = height;
    ImageData = new bool[Width, Height];
  }
  public bool GetPixel(int x, int y)
  {
    if (x < 0 || y < 0) return InfiniteValue;
    if (x >= Width || y >= Height) return InfiniteValue;
    return ImageData[x, y];
  }
  public void SetPixel(int x, int y, bool value)
  {
    if (x < 0 || y < 0 || x >= Width || y >= Height)
    {}
    else
      //throw new InvalidOperationException("Trying to set pixel outside of image");
      ImageData[x, y] = value;
  }
  public int GetLitPixels()
  {
    var count = 0;
    foreach (var pixel in ImageData)
      if (pixel) count++;
    return count;
  }
  public void Print()
  {
    for (var y = 0; y < Height; y++)
    {
      for (var x = 0; x < Width; x++)
        Console.Write(ImageData[x,y] ? "#" : ".");
      Console.Write("\r\n"); 
    }
  }
}