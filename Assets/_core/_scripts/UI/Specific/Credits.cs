using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.UI
{
    /// <summary>
    /// Controls the Credits panel.
    /// </summary>
    public class Credits : MonoBehaviour
    {
        Tween showTween;

        void Start()
        {
            showTween = this.GetComponent<Image>().DOFade(0, 0.36f).From().SetEase(Ease.Linear).SetAutoKill(false)
                .OnRewind(() => this.gameObject.SetActive(false));
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        public void Open()
        {
            gameObject.SetActive(true);
            if (showTween != null)
            { showTween.PlayForward(); }
        }

        public void Close()
        {
            showTween.PlayBackwards();
        }
    }
}
