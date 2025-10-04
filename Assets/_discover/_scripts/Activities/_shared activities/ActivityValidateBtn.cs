using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

namespace Antura.Discover.Activities
{
    public class ActivityValidateBtn : MonoBehaviour
    {
        public Button button;
        public Image img;
        private Tween pulseTween;

        void Awake()
        {
            SetState(false);
        }

        void OnDestroy()
        {
            pulseTween?.Kill();
        }

        public void SetState(bool interactable)
        {
            button.interactable = interactable;
            img.color = interactable ? Color.white : new Color(0.5f, 0.5f, 0.5f);

            if (interactable)
            {
                if (pulseTween == null)
                {
                    pulseTween = img.transform.DOPunchScale(Vector3.one * 0.3f, 0.5f, 1, 0.5f).SetLoops(-1, LoopType.Restart);
                }
            }
            else
            {
                pulseTween?.Kill();
                img.transform.localScale = Vector3.one;
            }
        }
    }
}
