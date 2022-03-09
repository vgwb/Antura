using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class BallShadowController : MonoBehaviour
    {
        public static BallShadowController instance;

        void Awake()
        {
            instance = this;
        }

        void Update()
        {
            Vector3 ballPosition = BallController.instance.transform.position;
            Vector3 groundPosition = GroundController.instance.transform.position;

            Vector3 shadowPosition = ballPosition;
            shadowPosition.y = ballPosition.y > groundPosition.y ? groundPosition.y + 0.01f : ballPosition.y;
            transform.position = shadowPosition;

            float ballElevation = ballPosition.y - groundPosition.y;

            float scale = 6f + ballElevation * 0.5f;
            transform.localScale = new Vector3(scale, scale, 12.72f);
        }
    }
}
