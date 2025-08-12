using DG.DeInspektor.Attributes;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Discover
{
    public class ProgressStar : MonoBehaviour
    {
        #region Serialized

        [DeEmptyAlert]
        [SerializeField] Image star;

        #endregion
        
        public bool Achieved { get; private set; }
        public int MinRequiredScore { get; private set; }

        Tween achievedTween;

        #region Unity

        void OnDestroy()
        {
            achievedTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Setup(int minRequiredScore)
        {
            if (achievedTween == null)
            {
                achievedTween = DOTween.Sequence().SetAutoKill(false).Pause()
                    .Join(star.DOColor(Color.white, 0.1f).From(Color.black).SetEase(Ease.Linear))
                    .Join(star.transform.DOPunchScale(Vector3.one * 1.2f, 0.5f))
                    .Insert(0.25f, star.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).From(true));
            }
            
            MinRequiredScore = minRequiredScore;
            Achieve(false);
        }

        public void Achieve(bool achieve)
        {
            if (Achieved == achieve) return;

            Achieved = achieve;
            if (Achieved) achievedTween.Restart();
            else achievedTween.Rewind();
        }

        #endregion
    }
}