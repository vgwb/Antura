using UnityEngine;
using UnityEngine.UI;

namespace Antura.Rewards
{
    /// <summary>
    /// Represents a minigame in the End Session Results panel
    /// </summary>
    public class EndsessionMinigame : MonoBehaviour
    {
        [Tooltip("Alpha will be ignored")]
        public Color StarOffColor = Color.red;

        [Header("References")]
        public Transform Bubble;

        public Image Ico;
        public Image[] Stars;

        public int GainedStars { get; private set; }
        Color starDefColor;

        #region Unity

        void Awake()
        {
            StarOffColor.a = 1;
            starDefColor = Stars[0].color;
        }

        #endregion

        #region Public Methods

        internal void Reset()
        {
            // TODO
        }

        internal void SetIcon(Sprite _sprite)
        {
            Ico.sprite = _sprite;
        }

        internal void SetStars(int _numStars)
        {
            GainedStars = _numStars;
            for (int i = 0; i < Stars.Length; ++i)
            {
                Image star = Stars[i];
                star.gameObject.SetActive(true);
                if (i < _numStars)
                {
                    star.color = starDefColor;
                    star.transform.localScale = Vector3.one;
                }
                else
                {
                    star.color = StarOffColor;
                    star.transform.localScale = Vector3.one * 0.6f;
                }
            }
        }

        #endregion
    }
}
