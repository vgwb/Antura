namespace Antura.Core
{
    /// <summary>
    /// Text flow options. I assume most languages are LeftToRight
    /// and Right To left, We may have to add further options in future
    /// if we need Chinese/Japanese.
    /// </summary>
    public enum TextDirection
    {
        LeftToRight = 0,
        RightToLeft = 1
    }
}
