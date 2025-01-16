using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Antura.Minigames.DiscoverCountry
{
    public class QuestInfoPanelSlideshow : MonoBehaviour
    {
        [SerializeField] Image img;
        [SerializeField] Button btPrev, btNext;

        int imgIndex;
        List<Sprite> sprites;
        CanvasGroup btPrevCG, btNextCG;
        Tween showImgTween;

        void Awake()
        {
            btPrevCG = btPrev.gameObject.AddComponent<CanvasGroup>();
            btNextCG = btNext.gameObject.AddComponent<CanvasGroup>();
            
            btPrev.onClick.AddListener(() => {
                if (imgIndex == 0) return;
                imgIndex--;
                ShowImage(sprites[imgIndex]);
                RefreshButtons();
            });
            btNext.onClick.AddListener(() => {
                if (imgIndex < sprites.Count - 1) return;
                imgIndex++;
                ShowImage(sprites[imgIndex]);
                RefreshButtons();
            });

            showImgTween = img.DOFade(1, 0.5f).From(0).SetAutoKill(false).Pause()
                .SetEase(Ease.Linear);
        }

        void OnDestroy()
        {
            showImgTween.Kill();
        }
        
        public void SetImages(List<Sprite> imgSprites)
        {
            imgIndex = 0;
            sprites = imgSprites;
            bool isEnabled = sprites != null && sprites.Count > 0;
            this.gameObject.SetActive(isEnabled);
            if (isEnabled)
            {
                ShowImage(sprites[0]);
                RefreshButtons();
            }
        }

        void ShowImage(Sprite sprite)
        {
            img.sprite = sprite;
            showImgTween.Restart();
        }

        void RefreshButtons()
        {
            btPrev.interactable = sprites != null && sprites.Count > 1 && imgIndex > 0;
            btPrevCG.alpha = btPrev.interactable ? 1 : 0.6f;
            btNext.interactable = sprites != null && sprites.Count > 1 && imgIndex < sprites.Count - 1;
            btNextCG.alpha = btNext.interactable ? 1 : 0.6f;
        }
    }
}