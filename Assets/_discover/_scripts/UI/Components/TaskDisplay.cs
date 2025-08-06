using Antura.Audio;
using Antura.Core;
using Antura.UI;
using DG.Tweening;
using TMPro;
using UnityEngine;

namespace Antura.Discover
{
    /// <summary>
    /// Shows the number of bones obtained.
    /// Used in the GameResultUI.
    /// </summary>
    public class TaskDisplay : MonoBehaviour
    {

        [Header("References")]
        public TextRender TaskText;
        public GameObject CounterGO;
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

            this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
            increaseTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Show(string objectiveText, int _maxItems)
        {
            Setup();

            TaskText.text = objectiveText;
            maxItems = _maxItems;
            totItems = 0;
            if (maxItems <= 0)
            {
                CounterGO.SetActive(false);
            }
            else
            {
                CounterGO.SetActive(true);
            }
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

        public void OnClick()
        {
            Debug.Log("ObjectiveDisplay OnClick called");
            // This method can be used to handle click events on the ObjectiveDisplay.
            // For example, it could open a detailed view of the objectives or show a tooltip.
        }
        #endregion
    }
}
