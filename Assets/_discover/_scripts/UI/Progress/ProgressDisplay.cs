using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    /// <summary>
    /// Displays the progress bar with the stars
    /// </summary>
    public class ProgressDisplay : MonoBehaviour
    {
        #region Serialized

        [Header("References")]

        [DeEmptyAlert]
        [SerializeField]
        RectTransform progressBar;

        [DeEmptyAlert]
        [SerializeField]
        Image progressFill;

        [DeEmptyAlert]
        [SerializeField]
        ProgressStar[] stars;

        #endregion

        public bool IsOpen { get; private set; }
        public int CurrScore { get; private set; }

        bool initialized;
        int maxScore;
        Tween showTween;

        #region Unity + INIT

        void Init()
        {
            if (initialized)
                return;

            initialized = true;

            bool wasActive = this.gameObject.activeInHierarchy;
            this.gameObject.SetActive(true);

            showTween = ((RectTransform)this.transform).DOAnchorPosX(135, 0.5f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutBack)
                .OnRewind(() => this.gameObject.SetActive(false));

            if (!wasActive)
                this.gameObject.SetActive(false);
        }

        void Awake()
        {
            Init();
            if (!IsOpen)
                this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Setup(int maxScore)
        {
            Show(maxScore, maxScore / 3, maxScore * 2 / 3, maxScore - 2);
        }

        /// <summary>
        /// Call this method to show and setup the progress bar
        /// </summary>
        /// <param name="pMaxScore"></param>
        /// <param name="starsScore"></param>
        private void Show(int pMaxScore, params int[] starsScore)
        {
            if (starsScore.Length < stars.Length)
            {
                Debug.LogError($"ProgressDisplay.Show : starsScore parameter contains less elements ({starsScore.Length}) than existing stars ({stars.Length})");
                return;
            }

            Init();
            IsOpen = true;

            CurrScore = 0;
            maxScore = pMaxScore;
            LayoutRebuilder.ForceRebuildLayoutImmediate(progressBar);

            float barH = progressBar.rect.height;
            if (barH <= 0f)
            {
                barH = progressBar.sizeDelta.y;
            }

            if (Mathf.Approximately(barH, 0f))
            {
                Debug.LogWarning("ProgressDisplay.Show: progress bar height is zero; stars cannot be positioned correctly.");
            }

            for (int i = 0; i < stars.Length; i++)
            {
                ProgressStar star = stars[i];
                RectTransform starT = (RectTransform)star.transform;
                star.Setup(starsScore[i]);
                Vector3 pos = new Vector2(starT.anchoredPosition.x, progressBar.anchoredPosition.y + (barH / maxScore) * star.MinRequiredScore);
                starT.anchoredPosition = pos;
            }

            Refresh();

            showTween.Restart();
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (!IsOpen)
                return;

            Init();
            IsOpen = false;

            showTween.PlayBackwards();
        }

        public void SetCurrentScore(int value)
        {
            if (CurrScore == value)
                return;

            CurrScore = value;
            Refresh();
        }

        #endregion

        #region Methods

        void Refresh()
        {
            progressFill.fillAmount = maxScore > 0 ? (float)CurrScore / maxScore : 0f;
            for (int i = 0; i < stars.Length; i++)
            {
                if (stars[i].MinRequiredScore <= CurrScore)
                    stars[i].Achieve(true);
                else
                    stars[i].Achieve(false);
            }
        }

        #endregion

        #region Test

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestShow_A()
        {
            Show(20, 3, 6, 9);
        }
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestShow_B()
        {
            Show(10, 3, 6, 9);
        }
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestShow_C()
        {
            Show(12, 4, 8, 12);
        }

        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void TestIncrementScoreBy1()
        {
            SetCurrentScore(CurrScore + 1);
        }
        [DeMethodButton(mode = DeButtonMode.PlayModeOnly)]
        void DecrementScoreBy1()
        {
            if (CurrScore > 0)
                SetCurrentScore(CurrScore - 1);
        }

        #endregion
    }
}
