using Antura.Discover.Interaction;
using UnityEngine;

namespace Antura.Discover
{
    public class PlayerMapIcon : AbstractMapIcon
    {
        public override bool IsEnabled => true;
        public bool Rotate;

        [SerializeField] Transform iconVisual;
        [SerializeField, Range(10f, 400f)] float minCameraHeight = 35f;
        [SerializeField, Range(10f, 600f)] float maxCameraHeight = 200f;
        [SerializeField, Range(0.1f, 3f)] float minScaleMultiplier = 0.45f;
        [SerializeField, Range(0.1f, 3f)] float maxScaleMultiplier = 1.35f;
        [SerializeField, Range(0f, 20f)] float scaleLerpSpeed = 8f;
        [SerializeField, Range(0f, 60f)] float rotationSpeed = 10f;

        Vector3 defaultVisualScale = Vector3.one;
        float currentScaleMultiplier = 1f;
        bool mapModeActive;
        bool subscribed;

        #region Methods

        protected override Vector3 GetPosition()
        {
            return InteractionManager.I.player.transform.position;
        }

        void OnEnable()
        {
            EnsureVisualReference();
            DiscoverNotifier.Game.OnMapCameraActivated.Subscribe(OnMapCameraActivated);
            subscribed = true;
            OnMapCameraActivated(CameraManager.I != null && CameraManager.I.Mode == CameraMode.Map);
        }

        void OnDisable()
        {
            if (subscribed)
            {
                DiscoverNotifier.Game.OnMapCameraActivated.Unsubscribe(OnMapCameraActivated);
                subscribed = false;
            }
        }

        void Update()
        {
            if (Rotate)
            {
                transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
            }

            if (mapModeActive)
            {
                UpdateScale();
            }
        }

        #endregion

        void EnsureVisualReference()
        {
            if (iconVisual == null)
            {
                var sprite = GetComponentInChildren<SpriteRenderer>();
                if (sprite != null)
                {
                    iconVisual = sprite.transform;
                }
            }

            if (iconVisual == null)
            {
                iconVisual = transform;
            }

            defaultVisualScale = iconVisual.localScale;
            currentScaleMultiplier = 1f;
        }

        void OnMapCameraActivated(bool activated)
        {
            mapModeActive = activated;
            if (!mapModeActive)
            {
                ResetScale();
            }
            else
            {
                UpdateScale(true);
            }
        }

        void UpdateScale(bool instant = false)
        {
            if (iconVisual == null)
                return;

            float cameraHeight = GetActiveCameraHeight();
            if (cameraHeight <= 0f)
                return;

            float targetMultiplier;
            if (maxCameraHeight > minCameraHeight)
            {
                float normalizedHeight = Mathf.InverseLerp(minCameraHeight, maxCameraHeight, cameraHeight);
                targetMultiplier = Mathf.Lerp(minScaleMultiplier, maxScaleMultiplier, normalizedHeight);
            }
            else
            {
                targetMultiplier = maxScaleMultiplier;
            }

            if (instant)
            {
                currentScaleMultiplier = targetMultiplier;
            }
            else
            {
                currentScaleMultiplier = Mathf.Lerp(currentScaleMultiplier, targetMultiplier, Time.deltaTime * (scaleLerpSpeed <= 0f ? 1f : scaleLerpSpeed));
            }

            iconVisual.localScale = defaultVisualScale * currentScaleMultiplier;
        }

        void ResetScale()
        {
            currentScaleMultiplier = 1f;
            if (iconVisual != null)
            {
                iconVisual.localScale = defaultVisualScale;
            }
        }

        float GetActiveCameraHeight()
        {
            if (CameraManager.I != null && CameraManager.I.MainCamTrans != null)
            {
                return CameraManager.I.MainCamTrans.position.y;
            }

            var mainCam = Camera.main;
            return mainCam != null ? mainCam.transform.position.y : 0f;
        }
    }
}
