using System.Collections;
using DG.Tweening;
using UnityEngine;

namespace Antura.Tutorial
{
    public class TutorialUIFinger : TutorialUIProp
    {
        public Sprite ClickSprite;

        Sprite defSprite;

        #region Unity

        protected override void Awake()
        {
            base.Awake();

            defSprite = Img.sprite;
        }

        void Update()
        {
            this.transform.LookAt(transform.position + TutorialUI.I.CamT.rotation * Vector3.forward, TutorialUI.I.CamT.up);
        }

        #endregion

        #region Public Methods

        public void Click(Transform _parent, Vector3 _position)
        {
            Reset();
            Show(_parent, _position);
            this.StartCoroutine(CO_Click());
        }

        public void ClickRepeat(Transform _parent, Vector3 _position, float _duration, float _clicksPerSecond)
        {
            Reset();
            Show(_parent, _position);
            this.StartCoroutine(CO_ClickRepeat(_duration, _clicksPerSecond));
        }

        #endregion

        #region Methods

        IEnumerator CO_Click()
        {
            if (!ShowTween.IsComplete())
            {
                ShowTween.timeScale = 2f;
                ShowTween.PlayForward();
                yield return ShowTween.WaitForCompletion();
            }

            ShowTween.timeScale = 1;
            Img.sprite = ClickSprite;
            TutorialUI.I.Pools.SpawnClicker(this.transform.parent, this.transform.position, true);
            yield return new WaitForSeconds(0.1f);

            Img.sprite = defSprite;
            yield return new WaitForSeconds(0.2f);

            Hide();
        }

        IEnumerator CO_ClickRepeat(float _duration, float _clicksPerSecond)
        {
            if (!ShowTween.IsComplete())
            {
                ShowTween.timeScale = 2f;
                ShowTween.PlayForward();
                yield return ShowTween.WaitForCompletion();
            }

            ShowTween.timeScale = 1;
            float startTime = Time.time;
            float clickTime = 0;
            float clickWait = 1 / _clicksPerSecond;
            while (Time.time - startTime < _duration)
            {
                if (Time.time - clickTime >= clickWait)
                {
                    clickTime = Time.time;
                    Img.sprite = ClickSprite;
                    TutorialUI.I.Pools.SpawnClicker(this.transform.parent, this.transform.position, true);
                    yield return new WaitForSeconds(0.1f);
                    Img.sprite = defSprite;
                }
                else
                {
                    yield return null;
                }
            }

            Hide();
        }

        #endregion
    }
}
