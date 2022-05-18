using System;
using Antura.Audio;
using Antura.Core;
using Antura.Database;
using Antura.Language;
using Antura.UI;
using DG.DeExtensions;
using DG.Tweening;
using UnityEngine;

namespace Antura.GamesSelector
{
    /// <summary>
    /// A single bubble in the GamesSelector, representing a mini-game.
    /// </summary>
    public class GamesSelectorBubble : MonoBehaviour
    {
        public GameObject Main;
        public GameObject Cover; // Has collider
        public SpriteRenderer Ico;
        public UIMinigameVariationBadge Badge;
        public ParticleSystem PouffParticleSys;

        public bool IsOpen { get; private set; }
        private bool hasBadge;
        private Tween shakeTween, openTween, showBadgeTween;

        #region Unity

        void OnDestroy()
        {
            shakeTween.Kill(true);
            openTween.Kill(true);
            showBadgeTween.Kill(true);
        }

        #endregion

        #region Public Methods

        public void Setup(MiniGameData miniGameData, float _x)
        {
            Sprite icon = AppManager.I.AssetManager.GetMainIcon(miniGameData);
            Open(false);
            Ico.sprite = icon;
            hasBadge = miniGameData.HasBadge;
            Badge.Assign(miniGameData);
            transform.localPosition = new Vector3(_x, 0, 0);
            shakeTween = DOTween.Sequence().SetLoops(-1, LoopType.Yoyo)
                .Append(Cover.transform.DOShakeScale(4, 0.035f, 6, 90f, false))
                .Join(Cover.transform.DOShakeRotation(7, 7, 3, 90f, false));
        }

        public void Open(bool _doOpen = true)
        {
            IsOpen = _doOpen;
            Cover.SetActive(!_doOpen);
            Main.SetActive(_doOpen);
            Badge.gameObject.SetActive(_doOpen && hasBadge);

            if (_doOpen)
            {
                PouffParticleSys.gameObject.SetActive(true);
                PouffParticleSys.time = 0;
                PouffParticleSys.Play();
                shakeTween.Kill(true);
                openTween = Main.transform.DOPunchRotation(new Vector3(0, 0, 45), 0.75f);
                if (hasBadge)
                {
                    showBadgeTween = Badge.transform.DOLocalMoveY(0, 0.45f).From().SetDelay(0.35f).SetEase(Ease.OutBack);
                }
                AudioManager.I.PlaySound(Sfx.Poof);
            }
            else
            {
                PouffParticleSys.Stop();
                PouffParticleSys.Clear();
                PouffParticleSys.gameObject.SetActive(false);
            }
        }

        #endregion
    }
}
