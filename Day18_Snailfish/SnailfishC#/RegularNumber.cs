public class RegularNumber : ISnailfishNumerical
{
  public int Value { get; set; }
  public RegularNumber(int value) => Value = value;

  public override string ToString() => $"{Value}";

  public int GetMagnitude() => Value;

  public ISnailfishNumerical DeepCopy() => new RegularNumber(Value);

  public RegularNumber GetLeftRegular() => this;

  public RegularNumber GetRightRegular() => this;
}