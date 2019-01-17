using DG.Tweening;
using UnityEngine;

namespace Antura.Map
{
    /// <summary>
    /// A dot on the map. Just visuals. 
    /// </summary>
    public class Dot : MonoBehaviour
    {
        [Header("References")]
        public Material blackDot;
        public Material highlightedDot;

        public Material playDot;
        public Material lockDot;

        public Renderer baseRenderer;
        public Renderer iconRenderer;

        public ParticleSystem selectionPS;

        public void SetAsPlay()
        {
            // DEPRECATED: play button won't show the big Play icon 
            //iconRenderer.material = playDot;
            //iconRenderer.gameObject.SetActive(true);

            // Play button is way bigger
            //transform.localScale = Vector3.one * 25;

            // PS plays
            selectionPS.gameObject.SetActive(true);

            // Start the tutorial too
            var mapScene = MapScene.I as MapScene;
            mapScene.StartTutorialClickOn(transform);
        }


        public void SetAsLock()
        {
            iconRenderer.material = lockDot;
            iconRenderer.gameObject.SetActive(true);
            selectionPS.gameObject.SetActive(false);
        }

        public void SetAsNothing()
        {
            iconRenderer.gameObject.SetActive(false);
            selectionPS.gameObject.SetActive(false);

            var mapScene = MapScene.I as MapScene;
            mapScene.StopTutorialClick();
        }

        public void Highlight(bool choice)
        {
            transform.localScale = choice ? Vector3.one * 15 : Vector3.one * 8;
            baseRenderer.material = choice ? highlightedDot : blackDot;
        }

        #region Appear / Disappear

        public bool Appeared { get; private set; }

        public void Disappear()
        {
            Appeared = false;
            transform.localScale = Vector3.one * 0.5f;
        }

        public void Appear(float delay, float duration)
        {
            if (Appeared) { return; }
            Appeared = true;
            transform.DOScale(Vector3.one * 1.5f, duration)
                .SetEase(Ease.OutElastic, 4f)
                .SetDelay(delay);
        }

        public void FlushAppear()
        {
            if (Appeared) { return; }
            Appeared = true;
            transform.localScale = Vector3.one * 1.5f;
        }

        #endregion
    }
}