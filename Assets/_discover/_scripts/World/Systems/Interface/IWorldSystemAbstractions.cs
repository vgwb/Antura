namespace Antura.Discover
{
    /// Runtime subsystem marker (Weather, Time, LivingLetters, etc.)
    public interface IWorldSystem { }

    public interface IWorldAPI
    {
        T Get<T>() where T : class, IWorldSystem;
    }
}
