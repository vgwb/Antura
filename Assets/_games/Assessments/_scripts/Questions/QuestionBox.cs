using Antura.Utilities;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;

namespace Antura.Assessment
{
    public class QuestionBox : MonoBehaviour
    {
        public NineSlicedSprite sprite;

        private Tween tween = null;

        public void WrapBoxAroundWords(IEnumerable<StillLetterBox> letters)
        {
            float ymin = 1000;
            float ymax = -1000;
            float xmin = 1000;
            float xmax = -1000;

            foreach (var letter in letters)
            {
                float xmin_local = letter.transform.localPosition.x - letter.GetHalfWidth();
                float xmax_local = letter.transform.localPosition.x + letter.GetHalfWidth();
                float ymin_local = letter.transform.localPosition.y - letter.GetHalfHeight();
                float ymax_local = letter.transform.localPosition.y + letter.GetHalfHeight();

                if (xmin_local < xmin)
                {
                    xmin = xmin_local;
                }

                if (xmax_local > xmax)
                {
                    xmax = xmax_local;
                }

                if (ymin_local < ymin)
                {
                    ymin = ymin_local;
                }

                if (ymax_local > ymax)
                {
                    ymax = ymax_local;
                }
            }

            sprite.Width = xmax - xmin + 0.8f;
            sprite.Height = ymax - ymin + 0.8f;
            transform.localPosition = new Vector3((xmax + xmin) / 2.0f, (ymax + ymin) / 2.0f, 6.0f);
        }

        public void HideInstant()
        {
            KillTween();
            sprite.transform.localScale = Vector3.zero;
        }

        public void Show()
        {
            KillTween();
            tween = sprite.transform.DOScale(Vector3.one, 0.2f);
        }

        public void Hide()
        {
            KillTween();
            tween = sprite.transform.DOScale(Vector3.zero, 0.2f);
        }

        void OnDestroy()
        {
            KillTween();
        }

        private void KillTween()
        {
            if (tween != null)
            {
                tween.Kill(true);
            }
            tween = null;
        }
    }
}
