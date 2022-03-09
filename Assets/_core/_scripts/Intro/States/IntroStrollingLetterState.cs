namespace Antura.Intro
{
    public abstract class IntroStrollingLetterState : FSM.IState
    {
        protected IntroStrollingLetter letter;

        public IntroStrollingLetterState(IntroStrollingLetter letter)
        {
            this.letter = letter;
        }

        public abstract void EnterState();
        public abstract void ExitState();
        public abstract void Update(float delta);
        public abstract void UpdatePhysics(float delta);
    }
}
