using Antura.Database;
using Antura.LivingLetters;
using Antura.Minigames;
using Kore.Coroutines;
using System.Collections;

namespace Antura.Assessment
{
    public class AssessmentAudioManager
    {
        public void PlayAssessmentMusic()
        {
            audioManager.PlayMusic(Music.Theme7);
        }

        public void PlayKOSound()
        {
            audioManager.PlaySound(Sfx.KO);
        }

        public void PlayStampSound()
        {
            audioManager.PlaySound(Sfx.StampOK);
        }

        public void PlayUIPopup()
        {
            audioManager.PlaySound(Sfx.UIPopup);
        }

        /// <summary>
        /// Antura is Angry sound (when wrong answers are given)
        /// </summary>
        public void AnturaAngrySound()
        {
            audioManager.PlaySound(Sfx.DogBarking);
        }

        public void PlayQuestionBlip()
        {
            audioManager.PlaySound(Sfx.Blip);
        }

        public void PlayPlaceSlot()
        {
            audioManager.PlaySound(Sfx.StarFlower);
        }

        public void PlayRemoveSlot()
        {
            audioManager.PlaySound(Sfx.BalloonPop);
        }

        public void PlayPoofSound()
        {
            audioManager.PlaySound(Sfx.Poof);
        }

        public IYieldable PlayAssessmentCompleteSound()
        {
            return Speak(Localization.Random(
                                        LocalizationDataId.Assessment_Complete_1,
                                        LocalizationDataId.Assessment_Complete_2,
                                        LocalizationDataId.Assessment_Complete_3)
                        );
        }

        public IYieldable PlayAnswerWrong()
        {
            return Speak(Localization.Random(
                                        LocalizationDataId.Assessment_Wrong_1,
                                        LocalizationDataId.Assessment_Wrong_2,
                                        LocalizationDataId.Assessment_Wrong_3)
                        );
        }

        public IYieldable PlayGameDescription()
        {
            return Speak(gameDescription);
        }

        /// <summary>
        /// Play audio and show subtitles for a dialogue. You can "yield return it"
        /// </summary>
        /// <param name="ID">Dialogue ID</param>
        /// <returns>Yield instruction to wait it ends</returns>
        public IYieldable Dialogue(LocalizationDataId ID, bool showWalkieTalkie)
        {
            return new WaitCoroutine(DialogueCoroutine(ID, showWalkieTalkie, true));
        }

        /// <summary>
        /// Play audio for a dialogue. You can "yield return it"
        /// </summary>
        /// <param name="ID">Dialogue ID</param>
        /// <returns>Yield instruction to wait it ends</returns>
        public IYieldable Speak(LocalizationDataId ID)
        {
            return new WaitCoroutine(DialogueCoroutine(ID, false, false));
        }

        private IAudioManager audioManager;
        private ISubtitlesWidget widget;
        private LocalizationDataId gameDescription;
        private PriorityTikets ticket = new PriorityTikets();

        public AssessmentAudioManager(IAudioManager audioManager,
                                    ISubtitlesWidget widget,
                                    LocalizationDataId gameDescription)
        {
            this.audioManager = audioManager;
            this.widget = widget;
            this.gameDescription = gameDescription;
        }

        /// <summary>
        /// Play LL sound files
        /// </summary>
        /// <param name="data">Sound to play</param>
        public void PlayLetterData(ILivingLetterData data)
        {
            AssessmentConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                data, true,
                soundType: AssessmentConfiguration.Instance.GetVocabularySoundType()
            );
        }

        /// <summary>
        /// Play LL sound files, coroutine
        /// </summary>
        /// <param name="data">Sound to play</param>
        public IEnumerator PlayLetterDataCoroutine(ILivingLetterData data)
        {
            var audioSource = AssessmentConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(
                data, true,
                soundType: AssessmentConfiguration.Instance.GetVocabularySoundType()
            );

            while (audioSource.IsPlaying)
            {
                yield return null;
            }
        }

        /// <summary>
        /// Now dialougues are just ignored if there's already some audio playing.
        /// </summary>
        /// <param name="ID"> Statement to play/display</param>
        private IEnumerator DialogueCoroutine(LocalizationDataId ID, bool showWalkieTalkie, bool showSubtitles)
        {
            yield return Wait.For(0.2f);

            var audioTicket = ticket.LockHighPriority();
            bool playing = false;

            if (ticket.IsHighPriorityTicketValid(audioTicket))
            {
                // Can Play Audio
                playing = true;

                if (showSubtitles)
                {
                    widget.DisplaySentence(ID, 2.2f, showWalkieTalkie);
                }

                if (showWalkieTalkie && showSubtitles)
                { // give time for walkietalkie sound
                    yield return Wait.For(0.2f);
                }

                audioManager.PlayDialogue(ID, () => { playing = false; });

                while (playing)
                {
                    yield return null;
                }

                if (showSubtitles)
                {
                    widget.Clear();
                }

                yield return Wait.For(0.2f);
            }

            ticket.UnlockHighPriorityTicket(audioTicket);
        }
    }
}
