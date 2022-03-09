using UnityEngine;
using DG.Tweening;

namespace Antura.Minigames.TakeMeHome
{

    public enum tubeColor
    {
        green, blue, pink, purple
    }

    public class TakeMeHomeTube : MonoBehaviour
    {

        Tweener moveTweener;

        Vector3 originalPosition;
        public tubeColor color;
        public bool upperTube;
        public Transform enterance;
        public GameObject aspiration;
        public GameObject winParticles;
        public GameObject cubeInfo;
        TakeMeHomeTremblingTube trembling;

        // Use this for initialization
        void Start()
        {
            originalPosition = transform.position;

            aspiration.SetActive(false);
            winParticles.SetActive(false);
            trembling = GetComponent<TakeMeHomeTremblingTube>();

        }

        public void showWinParticles()
        {
            //winParticles.SetActive(true);
        }
        public void hideWinParticles()
        {
            //winParticles.SetActive(false);
        }
        public void activate(TakeMeHomeLL ll)
        {
            aspiration.SetActive(true);
            //shake();
            trembling.Trembling = true;
            ll.lastCollidedTube = this;
            //coll.size += Vector3.one * 0.9f;
        }

        public void deactivate(TakeMeHomeLL ll)
        {
            aspiration.SetActive(false);
            //moveTweener = transform.DOMove(originalPosition, 0.5f);
            trembling.Trembling = false;
            ll.lastCollidedTube = null;
            //coll.size = collStartSize;
        }

        // Update is called once per frame
        void Update()
        {

        }


        public void shake()
        {
            if (moveTweener != null)
            {
                moveTweener.Kill();
            }

            TakeMeHomeConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.Hit);
            if (upperTube)
                moveTweener = transform.DOLocalMoveY(originalPosition.y - 0.25f, 0.35f);//transform.DOShakePosition(0.5f, 0.2f, 1).OnComplete(delegate () { transform.position = originalPosition; });
            else
                moveTweener = transform.DOLocalMoveY(originalPosition.y + 0.25f, 0.35f);
        }


    }
}
