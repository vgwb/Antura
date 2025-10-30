namespace Antura.Discover
{
    public interface IWorldSystem { }

    public interface IWorldAPI
    {
        T Get<T>() where T : class, IWorldSystem;
    }
}
