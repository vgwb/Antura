using Antura.Audio;
using Antura.Core;
using Antura.Extensions;
using Antura.Helpers;
using Antura.UI;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Rewards
{
    /// <summary>
    /// Controls the panel that shows information on the results after a play session ends.
    /// </summary>
    public class EndsessionResultPanel : MonoBehaviour
    {
        [Header("Settings")]
        public LayerMask RewardsGosLayer;

        public float Godrays360Duration = 15f;

        [Header("References")]
        public EndsessionMinigames Minigames;

        public EndsessionBar Bar;
        public CanvasGroup GodraysCanvas;
        public RectTransform Godray0, Godray1;
        public GameObject[] RewardsGos;
        public Camera[] RewardsCams;

        [Header("Audio")]
        public Sfx SfxMinigamePopup = Sfx.UIPopup;

        public Sfx SfxIncreaseBar = Sfx.UIPopup;
        public Sfx SfxGainStar = Sfx.Win;
        public Sfx SfxShowContinue = Sfx.UIPauseIn;

        public static EndsessionResultPanel I { get; private set; }
        private bool setupDone;
        private List<RectTransform> releasedMinigamesStars;
        private Tween showTween, godraysTween;
        private Sequence minigamesStarsToBarTween;

        #region Unity + Setup

        void Setup()
        {
            if (setupDone)
            { return; }

            setupDone = true;
            I = this;

            showTween = DOTween.Sequence().SetAutoKill(false).Pause()
                .Append(this.GetComponent<Image>().DOFade(0, 0.35f).From().SetEase(Ease.Linear))
                .Join(GodraysCanvas.DOFade(0, 0.35f).From().SetEase(Ease.Linear))
                .OnRewind(() =>
                {
                    this.gameObject.SetActive(false);
                    godraysTween.Pause();
                });
            godraysTween = DOTween.Sequence().SetAutoKill(false).Pause().SetLoops(-1, LoopType.Restart)
                .Append(Godray0.DORotate(new Vector3(0, 0, 360), Godrays360Duration, RotateMode.FastBeyond360).SetRelative()
                    .SetEase(Ease.Linear))
                .Join(Godray1.DORotate(new Vector3(0, 0, -360), Godrays360Duration, RotateMode.FastBeyond360).SetRelative()
                    .SetEase(Ease.Linear));

            this.gameObject.SetActive(false);
        }

        void Awake()
        {
            Setup();
        }

        void OnDestroy()
        {
            if (I == this)
            { I = null; }
            this.StopAllCoroutines();
            showTween.Kill();
            godraysTween.Kill();
            minigamesStarsToBarTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(List<EndsessionResultData> _sessionData, int _alreadyUnlockedRewards, bool _immediate)
        {
            ContinueScreen.Close(true);
            Hide(true);
            Setup();

            this.StopAllCoroutines();
            if (_immediate)
            {
                showTween.Complete();
            }
            else
            {
                showTween.Restart();
            }
            godraysTween.Restart();
            this.gameObject.SetActive(true);
            this.StartCoroutine(CO_Show(_sessionData, _alreadyUnlockedRewards));
        }

        public void Hide(bool _immediate)
        {
            if (!setupDone)
                return;

            ContinueScreen.Close(true);
            this.StopAllCoroutines();
            if (_immediate)
            {
                showTween.Rewind();
            }
            else
            {
                showTween.PlayBackwards();
            }
            Bar.Hide();
            Minigames.Hide();
            minigamesStarsToBarTween.Kill();
            if (releasedMinigamesStars != null)
            {
                foreach (RectTransform rt in releasedMinigamesStars)
                {
                    Destroy(rt.gameObject);
                }
                releasedMinigamesStars = null;
            }
        }

        #endregion

        #region Methods

        IEnumerator CO_Show(List<EndsessionResultData> _sessionData, int _alreadyUnlockedRewards)
        {
            yield return null;

            SetRewardsGos();

            // Show minigames
            Bar.Hide();
            Minigames.Show(_sessionData);
            yield return new WaitForSeconds(1);

            // Show bar
            if (_alreadyUnlockedRewards > 2)
            {
                _alreadyUnlockedRewards = 2;
            }
            while (_alreadyUnlockedRewards > -1)
            {
                Bar.Achievements[_alreadyUnlockedRewards].AchieveReward(true, true);
                _alreadyUnlockedRewards--;
            }
            Bar.Show(_sessionData.Count * 3);
            //GameResultUI.I.BonesCounter.Show();
            while (!Bar.ShowTween.IsComplete())
            {
                yield return null;
            }

            // Start filling bar and/or show Continue button
            releasedMinigamesStars = Minigames.CloneStarsToMainPanel();
            if (releasedMinigamesStars.Count > 0)
            {
                minigamesStarsToBarTween = DOTween.Sequence();
                Vector2 to = Bar.GetComponent<RectTransform>().anchoredPosition;
                for (int i = 0; i < releasedMinigamesStars.Count; ++i)
                {
                    RectTransform mgStar = releasedMinigamesStars[i];
                    minigamesStarsToBarTween.Insert(i * 0.2f, mgStar.DOAnchorPos(to, 0.3f).OnComplete(() => Bar.IncreaseBy(1)))
                        .Join(mgStar.GetComponent<Image>().DOFade(0, 0.2f).SetDelay(0.1f).SetEase(Ease.InQuad))
                        .Join(mgStar.DORotate(new Vector3(0, 0, 180), 0.3f));
                }
                yield return new WaitForSeconds(minigamesStarsToBarTween.Duration());
            }
            AudioManager.I.PlaySound(SfxShowContinue);
            ContinueScreen.Show(Continue, ContinueScreenMode.Button, true);
        }

        public void Continue()
        {
            AppManager.I.NavigationManager.GoToNextScene();
        }

        void SetRewardsGos()
        {
            for (int i = 0; i < RewardsGos.Length; ++i)
            {
                GameObject go = RewardsGos[i];
                if (go.transform.childCount == 0)
                { continue; }
                go.SetLayerRecursive(GenericHelper.LayerMaskToIndex(RewardsGosLayer));
                CameraHelper.FitRewardToUICamera(go.transform.GetChild(0), RewardsCams[i], true);
            }
        }

        #endregion
    }
}
