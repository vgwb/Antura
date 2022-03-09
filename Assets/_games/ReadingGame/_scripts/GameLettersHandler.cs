using System;
using System.Collections;
using System.Collections.Generic;
using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.ReadingGame
{
    [Serializable]
    public class LetterAnimData
    {
        public string dataID;
        public float timing;
        public float timing2;
        public float timing3;
        public Transform startPosTr;
        public float jumpHeight;
        public float scale;
    }

    public class GameLettersHandler : MonoBehaviour
    {
        public LetterAnimData[] animDatas;

        private List<GameLivingLetter> gameLetters = new List<GameLivingLetter>();
        private GameObject letterObjectViewPrefab;
        private GameObject shadowPrefab;

        public void Initialize(GameObject letterObjectViewPrefab, GameObject shadowPrefab)
        {
            this.letterObjectViewPrefab = letterObjectViewPrefab;
            this.shadowPrefab = shadowPrefab;
        }

        void Update()
        {
            foreach (var g in gameLetters)
                g.Update(Time.deltaTime);
        }


        [Header("Anim Parameters")]
        public float danceSpeedMultiplier = 1f;
        public float waitBeforeStill;
        public float cleanupDestroyDelay;
        public float waitBeforeCrouching;

        public IEnumerator AnimateLettersCO(float periodRatio, bool tutorialMode)
        {
            for (var iLetter = 0; iLetter < animDatas.Length; iLetter++)
            {
                var animData = animDatas[iLetter];
                var llData = new LL_ImageData(animData.dataID);
                var gameLL = new GameLivingLetter(transform.parent, letterObjectViewPrefab, shadowPrefab,
                    llData,
                    animData.startPosTr.position - Vector3.up * animData.jumpHeight, transform.localPosition,
                    animData.startPosTr.position, animData.timing * periodRatio, null, animData.scale,
                    danceSpeedMultiplier);
                gameLetters.Add(gameLL);
                StartCoroutine(WaitThenAnimateCO(animData, gameLL, periodRatio));
            }

            if (!tutorialMode)
            {
                mayDrop = true;
                yield return new WaitForSeconds(waitBeforeStill * periodRatio);
                for (var iLetter = 0; iLetter < animDatas.Length; iLetter++)
                {
                    var animData = animDatas[iLetter];
                    var gameLL = gameLetters[iLetter];
                    gameLL.livingLetter.SetState(LLAnimationStates.LL_still);
                    StartCoroutine(WaitThenDropCO(animData, gameLL, periodRatio));
                }
            }
        }

        private IEnumerator WaitThenAnimateCO(LetterAnimData animData, GameLivingLetter gameLL, float periodRatio)
        {
            yield return new WaitForSeconds((animData.timing + 0.5f) * periodRatio);
            gameLL.livingLetter.DancingSpeed = danceSpeedMultiplier;
            gameLL.livingLetter.SetState(LLAnimationStates.LL_dancing);
            yield return new WaitForSeconds(animData.timing2 * periodRatio);
            gameLL.livingLetter.DoTwirl(null);
            yield return new WaitForSeconds(0f);
            gameLL.livingLetter.SetState(LLAnimationStates.LL_dancing);
        }

        private IEnumerator WaitThenDropCO(LetterAnimData animData, GameLivingLetter gameLL, float periodRatio)
        {
            yield return new WaitForSeconds((waitBeforeCrouching + animData.timing3) * periodRatio);
            if (!mayDrop)
                yield break;
            if (gameLL == null || gameLL.livingLetter == null)
                yield break;
            gameLL.livingLetter.SetState(LLAnimationStates.LL_dancing);
            gameLL.livingLetter.DoDancingLose();
            gameLL.dropped = true;
        }

        private bool mayDrop = false;
        public IEnumerator CleanupCO(bool correct)
        {
            mayDrop = false;
            foreach (var g in gameLetters)
            {
                if (g.dropped)
                    continue;
                g.livingLetter.SetState(LLAnimationStates.LL_dancing);
                if (correct)
                    g.livingLetter.DoDancingWin();
                else
                    g.livingLetter.DoDancingLose();
            }

            yield return new WaitForSeconds(cleanupDestroyDelay);

            foreach (var g in gameLetters)
            {
                g.livingLetter.Poof(4f);
                g.DestroyLetter();
            }
            gameLetters.Clear();
        }

    }
}
