using Antura.UI;
using DG.Tweening;
using UnityEngine;

namespace Antura.AnturaSpace.UI
{
    public class AnturaSpaceNewIcon : MonoBehaviour
    {
        Tween _tween;

        void Init()
        {
            if (_tween != null)
            {
                return;
            }

            _tween = transform.DORotate(new Vector3(0, 0, 360), 2, RotateMode.FastBeyond360).SetLoops(-1, LoopType.Restart)
                .SetEase(Ease.Linear)
                .SetAutoKill(false).Pause();
        }

        void OnEnable()
        {
            Init();
            _tween.Restart();
        }

        void OnDisable()
        {
            _tween.Rewind();
        }

        void OnDestroy()
        {
            _tween.Kill();
        }
    }
}
