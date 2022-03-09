namespace Antura.Minigames
{
    /// <summary>
    /// Provides access to the MinigamesStarsWidget UI element for minigames.
    /// <seealso cref="MinigamesStarsWidget"/>
    /// </summary>
    public interface IStarsWidget
    {
        void Show(int stars);
        void Hide();
    }
}
