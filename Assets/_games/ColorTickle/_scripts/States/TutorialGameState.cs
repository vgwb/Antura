using Antura.Database;
using Antura.LivingLetters;
using Antura.Minigames.Balloons;
using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class TutorialGameState : FSM.IState
    {
        private ColorTickleGame game;
        private GameObject m_TutorialLetter;

        private float m_PercentageLetterColored;

        // LL components
        private LivingLetterController m_LetterObjectView;
        private TMPTextColoring m_TMPTextColoringLetter;
        private SurfaceColoring m_SurfaceColoringLetter;
        private ColorTickle_LLController m_LLController;
        private HitStateLLController m_HitStateLLController;

        // LL vanishing vars
        private bool m_bLLVanishing = false;
        private float m_fTimeToDisappear = 3f;
        private float m_fDisappearTimeProgress = 0;

        public TutorialGameState(ColorTickleGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            m_PercentageLetterColored = 0.0f;

            //Init the tutorial letter
            m_TutorialLetter = game.tutorialLetter;
            m_TutorialLetter.gameObject.SetActive(true);
            InitTutorialLetter();
        }

        public void ExitState()
        {
        }

        public void Update(float delta)
        {
            bool stillWaitForInput = !(m_HitStateLLController != null && m_HitStateLLController.hitState == eHitState.HIT_LETTEROUTSIDE) && (m_TMPTextColoringLetter != null && m_TMPTextColoringLetter.IsTouching);

            CalcPercentageLetterColored();

            if (m_bLLVanishing)
            {
                m_fDisappearTimeProgress += Time.deltaTime;

                if (m_fDisappearTimeProgress >= m_fTimeToDisappear)
                {
                    m_LetterObjectView.Poof(); //LL vanishes
                    game.Context.GetAudioManager().PlaySound(Sfx.Poof);

                    //stop win particle
                    foreach (var particles in game.winParticle.GetComponentsInChildren<ParticleSystem>(true))
                    {
                        particles.Stop();
                    }
                    game.winParticle.SetActive(false);

                    m_bLLVanishing = false;
                    m_fDisappearTimeProgress = 0;

                    //just for possible reusing of the LL renable components
                    EnableLetterComponents();

                    m_TutorialLetter.SetActive(false);

                    game.SetCurrentState(game.PlayState);
                }
            }
            else if (m_PercentageLetterColored >= 100 && !stillWaitForInput)
            {
                game.tutorialUIManager.StartTutorial = false;

                game.anturaController.ForceAnturaToGoBack();//we completed the letter, antura turn back

                m_bLLVanishing = true; //LL is about to disappear

                //disable color components to avoid input in this phase (or ignore input using touch manager?)
                DisableLetterComponents();

                game.Context.GetAudioManager().PlayVocabularyData(
                    m_LetterObjectView.Data,
                    soundType: ColorTickleConfiguration.Instance.GetVocabularySoundType()
                );//play letter pronounce again

                m_SurfaceColoringLetter.Reset();//reset to clean surface of LL (maybe make a function to clean it rather than reinitialize it)


                m_LetterObjectView.DoHorray();
                game.Context.GetAudioManager().PlaySound(Sfx.Win);

                //play win particle
                game.winParticle.SetActive(true);
                foreach (var particles in game.winParticle.GetComponentsInChildren<ParticleSystem>(true))
                {
                    particles.Play();
                }

            }
        }

        public void UpdatePhysics(float delta)
        {
        }

        #region PRIVATE FUNCTIONS
        private void InitTutorialLetter()
        {
            m_LetterObjectView = m_TutorialLetter.GetComponent<LivingLetterController>();

            m_TMPTextColoringLetter = m_TutorialLetter.GetComponent<TMPTextColoring>();
            m_SurfaceColoringLetter = m_TutorialLetter.GetComponent<SurfaceColoring>();

            m_LLController = m_TutorialLetter.GetComponent<ColorTickle_LLController>();
            m_LLController.movingToDestination = true;

            m_HitStateLLController = m_TutorialLetter.GetComponent<HitStateLLController>();
            m_HitStateLLController.OnTouchedOutside += LoseLife;
            m_HitStateLLController.EnableTutorial += EnableTutorialAnimation;

            SetBrushColor(new Color(255, 0, 0, 255));

            DisableLetterComponents();

            //m_LLController.OnDestinationReached += EnableTutorialAnimation;

            m_LLController.OnDestinationReached += delegate ()
            {
                //play intro dialogue
                game.PlayIntro(() =>
                {
                    //play tutorial dialogue on intro finish

                    //for now this is broken, COLORTICKLE_TUTO is repeated like a loop
                    // HACK stop audio and replay music
                    //game.Context.GetAudioManager().PlayDialogue(Db.LocalizationDataId.ColorTickle_Tuto, delegate() { game.Context.GetAudioManager().StopMusic(); game.Context.GetAudioManager().PlayMusic(game.backgroundMusic); });

                    game.PlayTutorial(null);

                    EnableLetterComponents();
                    EnableTutorialAnimation();
                });

            };

        }

        private void EnableLetterComponents()
        {
            m_TMPTextColoringLetter.enabled = true;
            m_SurfaceColoringLetter.enabled = true;
            m_HitStateLLController.enabled = true;
        }

        private void DisableLetterComponents()
        {
            m_TMPTextColoringLetter.enabled = false;
            m_SurfaceColoringLetter.enabled = false;
            m_HitStateLLController.enabled = false;
        }

        private void SetBrushColor(Color color)
        {
            m_TMPTextColoringLetter.brush.SetBrushColor(color); //give the exact color to the letter

            Color brushColor = color;
            brushColor.r += (1 - color.r) * 0.5f;
            brushColor.g += (1 - color.g) * 0.5f;
            brushColor.b += (1 - color.b) * 0.5f;
            m_SurfaceColoringLetter.brush.SetBrushColor(brushColor); //give the desaturated color to the body
        }

        private void LoseLife()
        {
            if (m_HitStateLLController != null)
            {
                m_HitStateLLController.TicklesLetter();
            }
            game.tutorialUIManager.StartTutorial = false;
        }

        private void CalcPercentageLetterColored()
        {
            float percentageRequiredToWin = m_TMPTextColoringLetter.percentageRequiredToWin;
            m_PercentageLetterColored = Mathf.Min(100, ((m_TMPTextColoringLetter.GetRachedCoverage() * 100.0f) / percentageRequiredToWin) * 100.0f);
        }

        private void AnturaReachedLetter()
        {
            m_LetterObjectView.SetState(LLAnimationStates.LL_still);
            m_LetterObjectView.HasFear = true;
            m_LetterObjectView.Crouching = true;
        }

        private void AnturaGoingAway()
        {
            m_LetterObjectView.HasFear = false;
            m_LetterObjectView.Crouching = false;
        }

        private void AnturaInteractions(AnturaContollerState eState)
        {
            if (eState == AnturaContollerState.BARKING)
            {
                //Antura scared the LL
                AnturaReachedLetter();
            }
            else if (eState == AnturaContollerState.COMINGBACK)
            {
                //Antura is returning to his place
                AnturaGoingAway();
            }
        }

        private void EnableTutorialAnimation()
        {
            game.tutorialUIManager.StartTutorial = true;
        }
        #endregion
    }
}
