using Antura.Dog;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    public class IdleAnturaState : AnturaState
    {
        float actionTimer;
        float changeStateTimer;

        public IdleAnturaState(ReadingGameAntura antura)
            : base(antura)
        {
            this.antura = antura;
        }

        public override void EnterState()
        {
            if (antura.AllowSitting)
                antura.animator.State = AnturaAnimationStates.sitting;
            else
                antura.animator.State = AnturaAnimationStates.idle;

            actionTimer = Random.Range(3, 4);
            changeStateTimer = Random.Range(4, 5);
        }

        public override void ExitState()
        {
            antura.animator.IsExcited = false;
            antura.animator.IsAngry = false;
            antura.animator.IsSad = false;
        }

        public override void Update(float delta)
        {
            if (antura.Mood == ReadingGameAntura.AnturaMood.HAPPY)
            {
                antura.animator.IsExcited = true;
                antura.animator.IsAngry = false;
                antura.animator.IsSad = false;
            }
            else if (antura.Mood == ReadingGameAntura.AnturaMood.ANGRY)
            {
                antura.animator.IsExcited = false;
                antura.animator.IsAngry = true;
                antura.animator.IsSad = false;
            }
            else
            {
                antura.animator.State = AnturaAnimationStates.idle;
                antura.animator.IsExcited = false;
                antura.animator.IsAngry = false;
                antura.animator.IsSad = true;
            }

            actionTimer -= delta;
            changeStateTimer -= delta;

            if (changeStateTimer <= 0 && antura.Mood != ReadingGameAntura.AnturaMood.SAD)
            {
                antura.SetCurrentState(antura.WalkingState);
            }
            else if (actionTimer <= 0)
            {
                actionTimer = Random.Range(2.5f, 4.0f);

                if (Random.value < 0.5f && antura.Mood != ReadingGameAntura.AnturaMood.SAD &&
                    antura.AllowSitting)
                    antura.animator.State = AnturaAnimationStates.sitting;
                else
                {
                    antura.animator.State = AnturaAnimationStates.idle;

                    if (antura.Mood == ReadingGameAntura.AnturaMood.HAPPY)
                    {
                        if (Random.value < 0.25f)
                        {
                            antura.animator.DoSniff();
                        }
                    }
                    else if (antura.Mood == ReadingGameAntura.AnturaMood.ANGRY)
                    {
                        antura.animator.DoShout(() => { ReadingGameConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.DogBarking); });
                    }
                }
            }
        }

        public override void UpdatePhysics(float delta)
        {

        }
    }
}
