using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using Antura.LivingLetters;
using Random = UnityEngine.Random;
using DG.Tweening;

namespace Antura.Minigames.MixedLetters
{
    public class SeparateLettersSpawnerController : MonoBehaviour
    {
        public static SeparateLettersSpawnerController instance;

        // The delay to start dropping the letters, in seconds:
        private const float LOSE_ANIMATION_DROP_DELAY = 0.75f;

        // The time offset between each letter drop, in seconds:
        private const float LOSE_ANIMATION_DROP_OFFSET = 0.1f;

        // The delay to start vanishing the letters, in seconds:
        private const float LOSE_ANIMATION_POOF_DELAY = 1f;

        // The time offset between each letter vanish, in seconds:
        private const float LOSE_ANIMATION_POOF_OFFSET = 0.1f;

        // The delay to announce the end of the animation, in seconds:
        private const float LOSE_ANIMATION_END_DELAY = 1.5f;

        private const float LOSE_ANIMATION_TWEEN_DURATION = 1f;

        // The delay to start vanishing the letters (for the win animation), in seconds:
        private const float WIN_ANIMATION_POOF_DELAY = 1f;

        // The time offset between each letter vanish, in seconds:
        private const float WIN_ANIMATION_POOF_OFFSET = 0.1f;

        // The delay for the big LL (with the whole word) to appear, in seconds:
        private const float WIN_ANIMATION_BIG_LL_DELAY = 0.5f;

        // The delay for the big LL to start twirling:
        private const float WIN_ANIMATION_BIG_LL_TWIRL_DELAY = 0.5f;

        // The duration of the big LL's twirling:
        private const float WIN_ANIMATION_BIG_LL_TWIRL_DURATION = 2f;

        // The delay to announce the end of the animation, in seconds:
        private const float WIN_ANIMATION_END_DELAY = 0f;

        public SeparateLetterController[] separateLetterControllers;

        public AudioSource audioSource;

        private IEnumerator spawnLettersCoroutine;

        void Awake()
        {
            instance = this;
        }

        public void SetPosition(Vector3 position)
        {
            transform.position = position;
        }

        public void SpawnLetters(List<ILivingLetterData> lettersToSpawn, Action spawnOverCallback)
        {
            spawnLettersCoroutine = SpawnLettersCoroutine(lettersToSpawn, spawnOverCallback);
            StartCoroutine(spawnLettersCoroutine);
        }

        private IEnumerator SpawnLettersCoroutine(List<ILivingLetterData> lettersToSpawn, Action spawnOverCallback)
        {
            PlayCartoonFightSfx();

            yield return new WaitForSeconds(1);

            List<int> indices = new List<int>();
            for (int i = 0; i < lettersToSpawn.Count; i++)
            {
                indices.Add(i);
            }

            bool throwLetterToTheRight = Random.Range(1, 40) % 2 == 0;

            bool spawnLettersInOrder = MixedLettersGame.instance.Difficulty <= MiniGameController.VERY_EASY;
            int numDegreesOfRotation = GetNumDegreesOfRotation();

            for (int i = 0; i < lettersToSpawn.Count; i++)
            {
                int indexToSpawn = spawnLettersInOrder ? i : indices[Random.Range(0, indices.Count)];
                indices.Remove(indexToSpawn);

                LL_LetterData letterToSpawn = (LL_LetterData)lettersToSpawn[indexToSpawn];

                SeparateLetterController separateLetterController = separateLetterControllers[i];
                separateLetterController.Enable();
                separateLetterController.SetPosition(transform.position, false);
                separateLetterController.SetLetter(letterToSpawn);
                separateLetterController.SetRotation(new Vector3(0, 0, Random.Range(0, numDegreesOfRotation + 1) * -90));
                separateLetterController.SetIsKinematic(false);
                separateLetterController.SetCorrectDropZone(MixedLettersGame.instance.dropZoneControllers[indexToSpawn]);
                MixedLettersGame.instance.dropZoneControllers[indexToSpawn].correctLetter = separateLetterController;
                separateLetterController.SetIsSubjectOfTutorial(MixedLettersGame.instance.roundNumber == 0 && indexToSpawn == 0 && MixedLettersGame.instance.TutorialEnabled);
                separateLetterController.AddForce(new Vector3(throwLetterToTheRight ? Random.Range(2f, 6f) : Random.Range(-6f, -2f), Constants.GRAVITY.y * -0.45f), ForceMode.VelocityChange);

                throwLetterToTheRight = !throwLetterToTheRight;

                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.ThrowObj);
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterToSpawn, soundType: MixedLettersConfiguration.Instance.GetVocabularySoundType());

                yield return new WaitForSeconds(0.75f);
            }

            yield return new WaitForSeconds(1);
            audioSource.Stop();

            spawnOverCallback.Invoke();
        }

        private int GetNumDegreesOfRotation()
        {
            var diff = MixedLettersGame.instance.Difficulty;
            if (diff < 0.2f)
            {
                return 1;
            }
            else if (diff < 0.4f)
            {
                return 1;
            }
            else if (diff < 0.6f)
            {
                return 2;
            }
            else if (diff < 0.8f)
            {
                return 3;
            }
            else
            {
                return 3;
            }
        }

        private void PlayCartoonFightSfx()
        {
            audioSource.pitch = Random.Range(0.8f, 1.2f);
            audioSource.Play();
        }

        public void ShowLoseAnimation(Action OnAnimationEnded)
        {
            StartCoroutine(LoseAnimationCoroutine(OnAnimationEnded));
        }

        private IEnumerator LoseAnimationCoroutine(Action OnAnimationEnded)
        {
            int numLettersActiveInRound = MixedLettersGame.instance.PromptLettersInOrder.Count;

            List<int> correctOrderOfLetters = new List<int>(numLettersActiveInRound);
            List<int> unassignedLetters = new List<int>();

            for (int i = 0; i < numLettersActiveInRound; i++)
            {
                unassignedLetters.Add(i);
            }

            for (int i = 0; i < numLettersActiveInRound; i++)
            {
                var requiredLetter = MixedLettersGame.instance.dropZoneControllers[i].correctLetter.GetLetter();

                for (int j = 0; j < unassignedLetters.Count; j++)
                {
                    if (separateLetterControllers[unassignedLetters[j]].GetLetter().Id == requiredLetter.Id)
                    {
                        correctOrderOfLetters.Add(unassignedLetters[j]);
                        unassignedLetters.RemoveAt(j);
                    }
                }
            }

            for (int i = 0; i < numLettersActiveInRound; i++)
            {
                MixedLettersGame.instance.dropZoneControllers[i].HideRotationButton();
                MixedLettersGame.instance.dropZoneControllers[i].DisableCollider();
                separateLetterControllers[correctOrderOfLetters[i]].SetIsKinematic(true);
                separateLetterControllers[correctOrderOfLetters[i]].transform.DOMove(MixedLettersGame.instance.dropZoneControllers[i].transform.position, LOSE_ANIMATION_TWEEN_DURATION);
                separateLetterControllers[correctOrderOfLetters[i]].transform.DORotate(Vector3.zero, LOSE_ANIMATION_TWEEN_DURATION);
            }

            yield return new WaitForSeconds(LOSE_ANIMATION_TWEEN_DURATION + LOSE_ANIMATION_DROP_DELAY);

            for (int i = 0; i < numLettersActiveInRound; i++)
            {
                separateLetterControllers[correctOrderOfLetters[i]].SetIsKinematic(false);
                yield return new WaitForSeconds(LOSE_ANIMATION_DROP_OFFSET);
            }

            yield return new WaitForSeconds(LOSE_ANIMATION_POOF_DELAY);

            for (int i = 0; i < numLettersActiveInRound; i++)
            {
                separateLetterControllers[correctOrderOfLetters[i]].Vanish();
                yield return new WaitForSeconds(LOSE_ANIMATION_POOF_OFFSET);
            }

            yield return new WaitForSeconds(LOSE_ANIMATION_END_DELAY);

            OnAnimationEnded();
        }

        public void ShowWinAnimation(Action OnVictimLLIsShowingBack, Action OnAnimationEnded)
        {
            StartCoroutine(WinAnimationCoroutine(OnVictimLLIsShowingBack, OnAnimationEnded));
        }

        private IEnumerator WinAnimationCoroutine(Action OnVictimLLIsShowingBack, Action OnAnimationEnded)
        {
            yield return new WaitForSeconds(WIN_ANIMATION_POOF_DELAY);

            for (int i = 0; i < MixedLettersGame.instance.dropZoneControllers.Length; i++)
            {
                if (!MixedLettersGame.instance.dropZoneControllers[i].gameObject.activeInHierarchy)
                {
                    continue;
                }

                if (i != 0)
                {
                    yield return new WaitForSeconds(WIN_ANIMATION_POOF_OFFSET);
                }

                MixedLettersGame.instance.dropZoneControllers[i].droppedLetter.Vanish();
            }

            MixedLettersGame.instance.HideDropZones();

            yield return new WaitForSeconds(WIN_ANIMATION_BIG_LL_DELAY);

            VictimLLController.instance.Enable();
            VictimLLController.instance.Reset();
            VictimLLController.instance.DoHooray();
            VictimLLController.instance.ShowVictoryRays();

            if (MixedLettersConfiguration.Instance.Variation == MixedLettersVariation.BuildWord)
            {
                MixedLettersConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(VictimLLController.instance.letterObjectView.Data, soundType: MixedLettersConfiguration.Instance.GetVocabularySoundType());
                VictimLLController.instance.letterObjectView.TransformIntoImage();
            }

            yield return new WaitForSeconds(WIN_ANIMATION_BIG_LL_TWIRL_DELAY);

            VictimLLController.instance.letterObjectView.DoTwirl(OnVictimLLIsShowingBack);

            yield return new WaitForSeconds(WIN_ANIMATION_BIG_LL_TWIRL_DURATION);
            MixedLettersGame.instance.HideDropZones();
            DisableLetters();

            yield return new WaitForSeconds(WIN_ANIMATION_END_DELAY);

            //OnAnimationEnded();
        }

        public void SetLettersDraggable()
        {
            foreach (SeparateLetterController separateLetterController in separateLetterControllers)
            {
                separateLetterController.SetDraggable();
            }
        }

        public void SetLettersNonInteractive()
        {
            foreach (SeparateLetterController separateLetterController in separateLetterControllers)
            {
                separateLetterController.SetNonInteractive();
            }
        }

        public void ResetLetters()
        {
            foreach (SeparateLetterController separateLetterController in separateLetterControllers)
            {
                separateLetterController.Reset();
            }
        }

        public void DisableLetters()
        {
            foreach (SeparateLetterController separateLetterController in separateLetterControllers)
            {
                separateLetterController.Disable();
            }
        }
    }
}

