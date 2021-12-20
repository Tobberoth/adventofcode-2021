using System.Text;

public class ImageEnhancer
{
  private string EnhancementAlgorithm { get; init; }
  public ImageEnhancer(string filename)
  {
    EnhancementAlgorithm = File.ReadAllLines(filename)[0];
  }

  public Image Enhance(Image image)
  {
    // Create new image using enhancer
    var outputImage = new Image(image.Width+2, image.Height+2);
    // For each pixel (begin -1 end +1 to expand)
    for (var y = -1; y < image.Height+1; y++)
    {
      for (var x = -1; x < image.Width+1; x++)
      {
        // Check 3x3 grid
        var filter = new StringBuilder();
        filter.Append(BoolToSymbol(image.GetPixel(x-1, y-1)));
        filter.Append(BoolToSymbol(image.GetPixel(x, y-1)));
        filter.Append(BoolToSymbol(image.GetPixel(x+1, y-1)));
        filter.Append(BoolToSymbol(image.GetPixel(x-1, y)));
        filter.Append(BoolToSymbol(image.GetPixel(x, y)));
        filter.Append(BoolToSymbol(image.GetPixel(x+1, y)));
        filter.Append(BoolToSymbol(image.GetPixel(x-1, y+1)));
        filter.Append(BoolToSymbol(image.GetPixel(x, y+1)));
        filter.Append(BoolToSymbol(image.GetPixel(x+1, y+1)));
        outputImage.SetPixel(x+1, y+1, GetEnhancement(filter.ToString()));
      }
    }
    if (GetEnhancement("000000000"))
      outputImage.InfiniteValue = !image.InfiniteValue;
    return outputImage;
  }

  private bool GetEnhancement(String filter)
  {
    var index = Convert.ToInt32(filter, 2);
    return EnhancementAlgorithm[index] == '#' ? true : false;
  }

  private string BoolToSymbol(bool input)
  {
    return input ? "1" : "0";
  }
}
