namespace Antura.FSM
{
    /// <summary>
    /// Represents a game state in the StateManager
    /// </summary>
    public interface IState
    {
        void EnterState();
        void ExitState();

        void Update(float delta);
        void UpdatePhysics(float delta);
    }
}
