using UnityEngine;
using DG.Tweening;

namespace Antura.Minigames.Maze
{
    public class MazeShowPrefab : MonoBehaviour
    {
        public int letterIndex = 0;

        void Start()
        {
            var targetPos = transform.position;
            transform.position += Vector3.right * 40;

            transform.DOMove(targetPos, 1.0f).OnComplete(() =>
            {
                //MazeConfiguration.Instance.Context.GetAudioManager().PlayVocabularyData(letterId);
                MazeGame.instance.showCharacterMovingIn();
            });
        }

        public void moveOut(bool win = false)
        {
            transform.DOMove(new Vector3(-50, 0, -1), 2).OnComplete(() =>
            {
                Destroy(gameObject);
            });
        }
    }
}
