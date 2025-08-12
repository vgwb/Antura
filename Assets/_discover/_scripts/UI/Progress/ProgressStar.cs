using System.Drawing;
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
        [SerializeField] UnityEngine.UI.Image star;

        [DeEmptyAlert]
        [SerializeField] GameObject line;

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
                    .Join(star.DOColor(UnityEngine.Color.white, 0.1f).From(new UnityEngine.Color(0, 0, 0, 0.2f)).SetEase(Ease.Linear))
                    .Join(star.transform.DOPunchScale(Vector3.one * 1.2f, 0.5f))
                    .Insert(0.25f, star.transform.DOLocalRotate(new Vector3(0, 0, 360), 0.5f, RotateMode.FastBeyond360).From(true));
            }

            MinRequiredScore = minRequiredScore;
            Achieve(false);
        }

        public void Achieve(bool achieve)
        {
            if (Achieved == achieve)
                return;

            Achieved = achieve;
            if (Achieved)
            {
                line.SetActive(true);
                achievedTween.Restart();
            }
            else
            {
                line.SetActive(false);
                achievedTween.Rewind();
            }
        }

        #endregion
    }
}
