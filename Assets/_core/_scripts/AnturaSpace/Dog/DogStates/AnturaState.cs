namespace Antura.AnturaSpace
{
    public class AnturaState : FSM.IState
    {
        protected AnturaSpaceScene controller;

        public AnturaState(AnturaSpaceScene controller)
        {
            this.controller = controller;
        }

        public virtual void EnterState()
        {
        }

        public virtual void ExitState()
        {
        }

        public virtual void Update(float delta)
        {
        }

        public virtual void UpdatePhysics(float delta)
        {
        }

        public virtual void OnTouched()
        {
        }
    }
}
