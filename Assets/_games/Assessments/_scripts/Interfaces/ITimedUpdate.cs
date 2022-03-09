namespace Antura.Assessment
{
    /// <summary>
    /// Update with delta time also classes that are not MonoBehaviours
    /// </summary>
    public interface ITimedUpdate
    {
        //returns true when updating is over.
        void Update(float deltaTime);
    }
}
