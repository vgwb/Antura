using Antura.UI;
using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;

namespace Antura.Discover
{
    public class TaskDisplay : MonoBehaviour
    {
        #region Serialized

        [Header("References")]
        [DeEmptyAlert]
        [SerializeField] TaskCounter counter;
        [DeEmptyAlert]
        [SerializeField] TextRender tfDescription;
        
        #endregion
        
        public bool IsOpen { get; private set; }

        string taskFullDescription; // Not truncated after the first \r
        bool initialized;
        Tween showTween;

        #region Unity + INIT

        void Init()
        {
            if (initialized) return;

            initialized = true;

            bool wasActive = this.gameObject.activeInHierarchy;
            this.gameObject.SetActive(true);

            showTween = ((RectTransform)this.transform).DOAnchorPosY(180, 0.35f).From().SetAutoKill(false).Pause()
                .SetEase(Ease.OutQuart)
                .OnRewind(() => this.gameObject.SetActive(false));
            
            counter.gameObject.SetActive(true);
            counter.Hide();
            
            if (!wasActive) this.gameObject.SetActive(false);
        }

        void Awake()
        {
            Init();
            if (!IsOpen) this.gameObject.SetActive(false);
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Pass 0-or-less to targetItems to disable the counter
        /// </summary>
        public void Show(string taskDescription, int targetItemsToCollect)
        {
            if (IsOpen) return;

            Init();
            IsOpen = true;

            Debug.Log($"Show task (to collect: {targetItemsToCollect}) : \"{taskDescription}\"");
            taskFullDescription = taskDescription;
            tfDescription.text = TruncateDescription(taskFullDescription);
            counter.Setup(0, targetItemsToCollect);
            showTween.PlayForward();
            this.gameObject.SetActive(true);
        }

        public void Hide()
        {
            if (!IsOpen) return;

            Init();
            IsOpen = false;
            
            showTween.PlayBackwards();
        }

        public void SetTargetItems(int value)
        {
            counter.SetTargetItemsTo(value);
        }
        
        public void SetTotItemsCollected(int value)
        {
            counter.SetTotItemsCollectedTo(value);
        }

        public void OnClick()
        {
            // This method can be used to handle click events on the ObjectiveDisplay.
            // For example, it could open a detailed view of the objectives or show a tooltip.
            Debug.Log("ObjectiveDisplay OnClick called");
        }
        
        #endregion

        #region Methods

        string TruncateDescription(string text)
        {
            string[] txts = text.Split('\n');
            return txts[0].Trim();
        }

        #endregion
    }
}
