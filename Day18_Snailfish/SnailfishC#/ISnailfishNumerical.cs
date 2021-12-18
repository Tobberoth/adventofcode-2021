public interface ISnailfishNumerical
{
  public int GetMagnitude();

  public ISnailfishNumerical DeepCopy();

  public RegularNumber GetLeftRegular();

  public RegularNumber GetRightRegular();
}