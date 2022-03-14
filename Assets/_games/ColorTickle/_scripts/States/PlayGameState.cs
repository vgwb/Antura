using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.ColorTickle
{
    public class PlayGameState : FSM.IState
    {
        #region PRIVATE MEMBERS

        ColorTickleGame game;
        GameObject m_CurrentLetter;

        private int m_MaxLives => game.MaxLives;
        int m_Lives;
        int m_Rounds;
        int m_iRoundsSuccessfull;

        float m_PercentageLetterColored;

        ColorsUIManager m_ColorsUIManager;

        // LL components
        LivingLetterController m_LetterObjectView;
        TMPTextColoring m_TMPTextColoringLetter;
        SurfaceColoring m_SurfaceColoringLetter;
        ColorTickle_LLController m_LLController;
        HitStateLLController m_HitStateLLController;

        float tryAnturaTimer;

        // LL vanishing vars
        bool m_bLLVanishing = false;
        float m_fTimeToDisappear = 3f;
        float m_fDisappearTimeProgress = 0;
        bool tickled = false;
        #endregion

        public PlayGameState(ColorTickleGame game)
        {
            this.game = game;
        }

        public void EnterState()
        {
            m_Rounds = game.rounds;
            m_iRoundsSuccessfull = 0;

            //Init ColorCanvas and PercentageLetterColoredButton
            InitGameUI();

            ResetState();

            game.anturaController.OnStateChanged += AnturaInteractions;

            //Init the first letter
            tickled = false;
            m_CurrentLetter = game.myLetters[m_Rounds - 1];
            m_CurrentLetter.gameObject.SetActive(true);
            InitLetter();

            ResetLaunchTimer(true);
        }

        public void ExitState()
        {
        }

        void ResetLaunchTimer(bool firstTime)
        {
            tryAnturaTimer = Mathf.Lerp(4, 15, 1 - game.Difficulty) + Random.value * 4;

            if (firstTime)
            {
                tryAnturaTimer *= 0.5f;
            }
        }

        public void Update(float delta)
        {
            tryAnturaTimer -= delta;

            if (m_Rounds <= 0 || game.starsAwarded >= 3)
            {
                game.SetCurrentState(game.ResultState);
            }
            else
            {
                bool stillWaitForInput = !(m_HitStateLLController != null && m_HitStateLLController.hitState == eHitState.HIT_LETTEROUTSIDE) && (m_TMPTextColoringLetter != null && m_TMPTextColoringLetter.IsTouching);

                CalcPercentageLetterColored();

                if (m_bLLVanishing) //if the LL is about to vanish
                {
                    m_fDisappearTimeProgress += Time.deltaTime;

                    //if(m_LetterObjectView.GetState()!=LLAnimationStates.LL_dancing)//when the dance is finished ---> DoDancingWin/Lose do not exit from this state
                    if (m_fDisappearTimeProgress >= m_fTimeToDisappear)//after the given time is reached
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

                        m_CurrentLetter.SetActive(false);
                        var controller = m_CurrentLetter.GetComponent<HitStateLLController>();
                        if (controller != null)
                        {
                            controller.OnTouchedOutside -= OnTickled;
                            controller.OnTouchedShape -= TryEnableAntura;
                        }

                        --m_Rounds;
                        if (m_Rounds > 0) //activate next LL
                        {
                            ResetState();
                            m_ColorsUIManager.ChangeButtonsColor();
                            tickled = false;
                            m_CurrentLetter = game.myLetters[m_Rounds - 1];
                            m_CurrentLetter.gameObject.SetActive(true);
                            // Initialize the next letter
                            InitLetter();
                        }
                    }
                }
                else if ((m_PercentageLetterColored >= 100 && !stillWaitForInput) || m_Lives <= 0) //else check for letter completed
                {
                    game.anturaController.ForceAnturaToGoBack();//we completed the letter, antura turn back
                    m_bLLVanishing = true; //LL is about to disappear

                    //disable color components to avoid input in this phase (or ignore input using touch manager?)
                    DisableLetterComponents();

                    ColorTickleConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                        m_LetterObjectView.Data,
                        soundType: ColorTickleConfiguration.Instance.GetVocabularySoundType()
                    );//play letter pronounce again

                    m_SurfaceColoringLetter.Reset();//reset to clean surface of LL (maybe make a function to clean it rather than reinitialize it)

                    //LL does win or lose animation
                    if (m_PercentageLetterColored >= 100)
                    {
                        m_iRoundsSuccessfull += 1;
                        game.CurrentScore = m_iRoundsSuccessfull;

                        m_LetterObjectView.DoHorray();
                        ColorTickleConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Win);

                        game.Context.GetLogManager().OnAnswered(m_LetterObjectView.Data, true);

                        //play win particle
                        game.winParticle.SetActive(true);
                        foreach (var particles in game.winParticle.GetComponentsInChildren<ParticleSystem>(true))
                        {
                            particles.Play();
                        }

                    }
                    else if (m_Lives <= 0)
                    {
                        /*m_LetterObjectView.DoDancingLose(); //this just set trigger for lose on dancing animation
                        m_LetterObjectView.SetState(LLAnimationStates.LL_dancing);*/
                        m_LetterObjectView.DoAngry();
                        game.Context.GetLogManager().OnAnswered(m_LetterObjectView.Data, false);
                        game.Context.GetAudioManager().PlaySound(Sfx.LetterAngry);
                        game.Context.GetAudioManager().PlaySound(Sfx.Lose);
                    }
                }
                else if (tickled)
                {
                    tickled = false;
                    if (m_CurrentLetter != null)
                        m_CurrentLetter.GetComponent<HitStateLLController>().TicklesLetter();
                    LoseLife();
                }
            }
        }


        public void UpdatePhysics(float delta)
        {
        }

        #region PRIVATE FUNCTIONS

        private void ResetState()
        {
            m_Lives = m_MaxLives;
            game.gameUI.SetMaxLives(m_MaxLives);
            game.gameUI.SetLives(m_MaxLives);
            m_PercentageLetterColored = 0;
        }

        private void InitGameUI()
        {
            game.gameUI = game.Context.GetOverlayWidget();
            game.gameUI.Initialize(true, false, true);
            game.gameUI.SetStarsThresholds(1, 3, 5);

            game.colorsCanvas.gameObject.SetActive(true);
            m_ColorsUIManager = game.colorsCanvas.GetComponentInChildren<ColorsUIManager>();
            m_ColorsUIManager.SetBrushColor += SetBrushColor;
        }

        void OnTickled()
        {
            tickled = true;
        }

        private void InitLetter()
        {
            m_LetterObjectView = m_CurrentLetter.GetComponent<LivingLetterController>();

            if (ColorTickleConfiguration.Instance.Variation == ColorTickleVariation.Image)
            {
                m_LetterObjectView.TransformIntoImage();
                m_LetterObjectView.LabelRender.color = Color.white;
            }

            m_TMPTextColoringLetter = m_CurrentLetter.GetComponent<TMPTextColoring>();
            m_SurfaceColoringLetter = m_CurrentLetter.GetComponent<SurfaceColoring>();

            m_LLController = m_CurrentLetter.GetComponent<ColorTickle_LLController>();
            m_LLController.movingToDestination = true;

            m_HitStateLLController = m_CurrentLetter.GetComponent<HitStateLLController>();
            m_HitStateLLController.OnTouchedOutside += OnTickled;

            m_LLController.OnDestinationReached += delegate ()
            {
                game.Context.GetAudioManager().PlayVocabularyData(
                    m_LetterObjectView.Data,
                    soundType: ColorTickleConfiguration.Instance.GetVocabularySoundType()
                );
            };//play audio on destination


            m_HitStateLLController.OnTouchedShape += TryEnableAntura;
            //game.anturaController.targetToLook = m_CurrentLetter.transform; //make antura look at the LL on rotations

            SetBrushColor(m_ColorsUIManager.defaultColor);

            ResetLaunchTimer(true);
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
            //--TODO maybe this can be better if the LL controller handles all the LL states rather than just the hits
            if (game.anturaController.anturaState != AnturaContollerState.BARKING) //if the life loss wasn't caused inside Antura disruption
            {
                game.anturaController.ForceAnturaToGoBack();//we tickled the letter, antura turn back
            }
            else //if it was we need also to overwrite the LL tickling animation
            {
                m_LetterObjectView.SetState(LLAnimationStates.LL_walking); //keep running in fear instead of tickling
            }
            //--

            m_Lives--;

            game.gameUI.SetLives(m_Lives);

        }


        private void CalcPercentageLetterColored()
        {
            float percentageRequiredToWin = m_TMPTextColoringLetter.percentageRequiredToWin;
            m_PercentageLetterColored = ((m_TMPTextColoringLetter.GetRachedCoverage() * 100.0f) / percentageRequiredToWin) * 100.0f;
            if (m_PercentageLetterColored > 100.0f)
            {
                m_PercentageLetterColored = 100.0f;
            }
        }

        private void TryEnableAntura()
        {
            if (tryAnturaTimer < 0)
            {
                if (game.Difficulty >= 0.6f)
                {
                    game.anturaController.LaunchAnturaDisruption();
                }

                ResetLaunchTimer(false);
            }
        }

        private void AnturaReachedLetter()
        {
            m_LetterObjectView.SetState(LLAnimationStates.LL_walking);
            m_LetterObjectView.HasFear = true;
            m_LetterObjectView.SetWalkingSpeed(1);
            game.Context.GetAudioManager().PlaySound(Sfx.LetterFear);
            //m_LetterObjectView.Crouching = true;
        }

        private void AnturaGoingAway()
        {
            m_LetterObjectView.SetState(LLAnimationStates.LL_still);
            m_LetterObjectView.HasFear = false;
            m_LetterObjectView.SetWalkingSpeed(0);
            //m_LetterObjectView.Crouching = false;
        }

        /// <summary>
        /// This is called by Antura controller with the change state event to apply any
        /// needed interactions.
        /// </summary>
        /// <param name="eState">Current state for Antura</param>
        private void AnturaInteractions(AnturaContollerState eState)
        {
            if (eState == AnturaContollerState.BARKING) //Antura scared the LL
            {
                AnturaReachedLetter();
            }
            else if (eState == AnturaContollerState.COMINGBACK) //Antura is returning to his place
            {
                if (m_LetterObjectView.GetState() != LLAnimationStates.LL_tickling) //if the LL is tickling antura didn't reach it (fix)
                {
                    AnturaGoingAway();
                }

            }
        }

        #endregion
    }

}
