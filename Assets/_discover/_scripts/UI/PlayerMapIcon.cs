using Antura.Minigames.DiscoverCountry.Interaction;
using DG.Tweening;
using UnityEngine;

namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerMapIcon : MonoBehaviour
    {
        float defY;
        Tween showTween;
        
        #region Unity

        void Start()
        {
            defY = this.transform.position.y;
            
            showTween = this.GetComponentInChildren<SpriteRenderer>().DOFade(1, 0.45f).From(0).SetAutoKill(false).Pause()
                .SetDelay(0.3f)
                .SetEase(Ease.Linear);
            
            Hide(true);
        }

        void OnDestroy()
        {
            showTween.Kill();
        }

        void Update()
        {
            Vector3 pos = InteractionManager.I.player.transform.position;
            pos.y = defY;
            this.transform.position = pos;
        }

        #endregion
        
        #region Public Methods

        public void Show()
        {
            showTween.timeScale = 1f;
            showTween.Restart();
            this.gameObject.SetActive(true);
        }

        public void Hide(bool immediate = false)
        {
            if (immediate)
            {
                showTween.Rewind();
                this.gameObject.SetActive(false);
            }
            else
            {
                showTween.timeScale = 2.5f;
                showTween.PlayBackwards();
            }
        }

        #endregion
    }
}