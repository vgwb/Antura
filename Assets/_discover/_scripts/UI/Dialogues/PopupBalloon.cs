using UnityEngine;
using DG.Tweening;

namespace Antura.Discover
{
    public class PopupBalloon : AbstractDialogueBalloon
    {
        [SerializeField] RectTransform root;

        protected override void CreateShowTween()
        {
            if (root == null)
                root = GetComponent<RectTransform>();

            // Scale-in tween from 0.9 to 1 with slight fade
            root.localScale = Vector3.one * 0.9f;
            CanvasGroup cg = GetComponent<CanvasGroup>();
            if (cg == null) cg = gameObject.AddComponent<CanvasGroup>();
            cg.alpha = 0f;

            showTween = DOTween.Sequence()
                .AppendCallback(() => { cg.alpha = 0f; root.localScale = Vector3.one * 0.9f; })
                .Append(cg.DOFade(1f, 0.15f))
                .Join(root.DOScale(1f, 0.2f).SetEase(Ease.OutBack))
                .Pause()
                .SetAutoKill(false);
        }
    }
}
