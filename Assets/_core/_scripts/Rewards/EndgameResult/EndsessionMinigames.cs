using System.Collections.Generic;
using Antura.Audio;
using Antura.Core;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Rewards
{
    /// <summary>
    /// Visual container for minigame instances in the EndsessionResultPanel
    /// </summary>
    public class EndsessionMinigames : MonoBehaviour
    {
        public EndsessionMinigame MinigamePrefab;

        readonly List<EndsessionMinigame> minigames = new List<EndsessionMinigame>();
        Tween showTween;
        Sequence minigamesTween;

        #region Unity

        void Awake()
        {
            MinigamePrefab.gameObject.SetActive(false);

            showTween = this.GetComponent<RectTransform>().DOAnchorPosX(1210, 0.0001f).From().SetAutoKill(false).Pause();
        }

        void OnDestroy()
        {
            showTween.Kill();
            minigamesTween.Kill();
        }

        #endregion

        #region Public Methods

        internal void Reset()
        {
            // TODO
        }

        internal void Show(List<EndsessionResultData> _sessionData)
        {
            int totGames = _sessionData.Count;
            // Fill + Reset/set
            foreach (EndsessionMinigame minigame in minigames)
                minigame.Reset();
            while (minigames.Count < totGames)
            {
                EndsessionMinigame mg = Instantiate(MinigamePrefab);
                mg.GetComponent<RectTransform>().SetParent(MinigamePrefab.transform.parent, false);
                mg.gameObject.SetActive(true);
                minigames.Add(mg);
            }
            for (int i = 0; i < minigames.Count; ++i)
            {
                EndsessionMinigame mg = minigames[i];
                if (i < totGames)
                {
                    EndsessionResultData data = _sessionData[i];
                    mg.gameObject.SetActive(i < totGames);
                    mg.SetIcon(AppManager.I.AssetManager.GetMainIcon(data.MiniGameData));
                    mg.SetStars(data.Stars);
                }
                else
                    mg.gameObject.SetActive(false);
            }
            // Tween
            showTween.PlayForward();
            minigamesTween = DOTween.Sequence();
            for (int i = 0; i < totGames; ++i)
            {
                EndsessionMinigame mg = minigames[i];
                float startPos = i * 0.1f;
                minigamesTween.InsertCallback(startPos, () => AudioManager.I.PlaySound(EndsessionResultPanel.I.SfxMinigamePopup))
                    .Join(mg.Bubble.DOScale(0.0001f, 0.35f).From().SetEase(Ease.OutBack));
                int starsLen = mg.Stars.Length;
                for (int c = 0; c < starsLen; ++c)
                {
                    Image star = mg.Stars[c];
                    minigamesTween.Insert(startPos + 0.2f + (starsLen - c - 1) * 0.1f,
                        star.GetComponent<RectTransform>().DOAnchorPosX(0, 0.45f).From().SetEase(Ease.OutBack));
                    minigamesTween.Join(star.DOFade(0, 0.35f).From().SetEase(Ease.Linear));
                }
            }
        }

        internal void Hide()
        {
            minigamesTween.Kill(true);
            showTween.Rewind();
        }

        internal List<RectTransform> CloneStarsToMainPanel()
        {
            List<RectTransform> starsClones = new List<RectTransform>();
            foreach (EndsessionMinigame mg in minigames)
            {
                if (!mg.gameObject.activeSelf)
                    continue;
                for (int i = 0; i < mg.GainedStars; ++i)
                {
                    RectTransform orStar = mg.Stars[i].GetComponent<RectTransform>();
                    RectTransform star = Instantiate(orStar);
                    star.SetParent(this.transform.parent, false);
                    star.transform.position = orStar.transform.position;
                    starsClones.Add(star);
                    mg.Stars[i].gameObject.SetActive(false);
                }
            }

            return starsClones;
        }

        #endregion
    }
}
