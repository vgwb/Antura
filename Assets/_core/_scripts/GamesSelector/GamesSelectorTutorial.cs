using System.Collections;
using System.Collections.Generic;
using Antura.Language;
using DG.Tweening;
using UnityEngine;

namespace Antura.GamesSelector
{
    /// <summary>
    /// Tutorial on the GamesSelector.
    /// </summary>
    public class GamesSelectorTutorial : MonoBehaviour
    {
        public SpriteRenderer Finger;
        public TrailRenderer[] trails;

        public bool isPlaying { get; private set; }
        private Tween showTween, moveTween;
        private Sequence trailTimeTween;

        #region Unity

        void Awake()
        {
            Finger.gameObject.SetActive(false);
            //float defFingerZ = Finger.transform.position.z;

            showTween = Finger.DOFade(0, 0.25f).From().SetEase(Ease.Linear).SetAutoKill(false).Pause();
            trailTimeTween = DOTween.Sequence().SetAutoKill(false).Pause();
            foreach (TrailRenderer trenderer in trails)
            {
                TrailRenderer tr = trenderer; // Fixes Unity foreach bug
                trailTimeTween.Insert(0, DOTween.To(() => tr.time, x => tr.time = x, 0.0001f, 0.1f).SetEase(Ease.InQuad));
            }
        }

        void OnDestroy()
        {
            this.StopAllCoroutines();
            showTween.Kill();
            moveTween.Kill();
        }

        #endregion

        #region Public Methods

        public void Play(List<GamesSelectorBubble> _bubbles)
        {
            isPlaying = true;
            moveTween.Kill();
            Finger.gameObject.SetActive(true);

            Vector3[] path = new Vector3[_bubbles.Count + 1];
            //            transform.localPosition = _bubbles[0].transform.localPosition - new Vector3(2, 0, 0);
            //            path[path.Length - 1] = _bubbles[_bubbles.Count - 1].transform.localPosition + new Vector3(2, 0, 0);
            int rtlDirection = LanguageSwitcher.LearningRTL ? 1 : -1;

            transform.localPosition = _bubbles[LanguageSwitcher.LearningRTL ? _bubbles.Count - 1 : 0].transform.localPosition + new Vector3(rtlDirection * 2, 0, 0);

            for (int i = 0; i < _bubbles.Count; ++i)
            {
                int iRTL = LanguageSwitcher.LearningRTL ? _bubbles.Count - i - 1 : i;
                path[i] = _bubbles[iRTL].transform.localPosition;
            }
            path[path.Length - 1] = _bubbles[LanguageSwitcher.LearningRTL ? 0 : _bubbles.Count - 1].transform.localPosition - new Vector3(rtlDirection * 2, 0, 0);

            showTween.Restart();
            moveTween = DOTween.Sequence().SetAutoKill(false)
                .Append(this.transform.DOLocalPath(path, 1.25f, PathType.Linear))
                .OnComplete(() =>
                {
                    showTween.PlayBackwards();
                    trailTimeTween.PlayForward();
                })
                .OnPlay(() => showTween.PlayForward());

            this.StartCoroutine(CO_Play());
        }

        public void Stop()
        {
            isPlaying = false;
            this.StopAllCoroutines();
            moveTween.Kill();
            showTween.PlayBackwards();
            foreach (TrailRenderer tr in trails)
            {
                tr.Clear();
            }
        }

        #endregion

        #region Methods

        IEnumerator CO_Play()
        {
            while (true)
            {
                yield return moveTween.WaitForCompletion();
                yield return new WaitForSeconds(2);
                moveTween.Restart();
                trailTimeTween.Rewind();
                foreach (TrailRenderer tr in trails)
                {
                    tr.Clear();
                }
            }
        }

        #endregion
    }
}
