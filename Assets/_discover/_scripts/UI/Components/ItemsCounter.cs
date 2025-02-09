using Antura.Audio;
using Antura.Core;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    /// <summary>
    /// Shows the number of bones obtained.
    /// Used in the GameResultUI.
    /// </summary>
    public class ItemsCounter : MonoBehaviour
    {

        [Header("References")]
        public TextMeshProUGUI TfCount;
        public RectTransform BoneImg;

        int totItems
        {
            get { return fooTotBones; }
            set
            {
                fooTotBones = value;
                TfCount.text = value.ToString() + "/" + maxItems;
            }
        }

        int maxItems;
        int fooTotBones;
        bool setupDone;
        Tween showTween, increaseTween;

        #region Unity

        void Start()
        {
            Setup();

        }

        void Setup()
        {
            if (setupDone)
            { return; }

            setupDone = true;

            SetValue(0);
            showTween = this.transform.DOScale(0.001f, 0.35f).From().SetEase(Ease.OutBack).SetAutoKill(false).Pause()
                .OnRewind(() => this.gameObject.SetActive(false));
            showTween.Complete();
            increaseTween = BoneImg.transform.DOPunchScale(Vector3.one * 0.15f, 0.35f).SetAutoKill(false).Pause();
        }

        void OnDestroy()
        {
            showTween.Kill();
            increaseTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(bool _setValueAuto = true)
        {
            Setup();
            this.gameObject.SetActive(true);
            showTween.PlayForward();
        }

        public void Hide()
        {
            Setup();
            if (increaseTween != null)
            { increaseTween.Complete(); }
            showTween.Rewind();
        }

        public void SetMax(int _bones)
        {
            maxItems = _bones;
        }
        public void SetValue(int _bones)
        {
            totItems = _bones;
        }

        public void DecreaseBy(int _by)
        {
            totItems -= _by;
        }

        public void IncreaseByOne(bool _animate = true)
        {
            increaseTween.Restart();
            AudioManager.I.PlaySound(Sfx.Blip);
            totItems++;
        }

        //        public void AnimateIncreaseToCurrent(int _by)
        //        {
        //            increaseTween = DOVirtual.Float(totBones, totBones + _by, 0.35f, x => {
        //                totBones = Mathf.RoundToInt(x);
        //            }).SetEase(Ease.Linear).OnKill(()=> increaseTween = null);
        //        }

        #endregion
    }
}
