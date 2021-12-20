var enhancer = new ImageEnhancer("input.txt");
var img = new Image("input.txt");
for (var i = 0; i < 50; i++)
  img = enhancer.Enhance(img);
Console.WriteLine($"Lit pixels: {img.GetLitPixels()}");