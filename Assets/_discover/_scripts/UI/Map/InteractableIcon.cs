using Antura.Discover.Interaction;
using UnityEngine;

namespace Antura.Discover
{
    public class InteractableIcon : AbstractMapIcon
    {
        [SerializeField] Color activeColor = Color.white;
        [SerializeField] Color doneColor = new(0.45f, 0.9f, 0.45f, 1f);

        MapIconState currentState = MapIconState.On;
        Quaternion restingRotation;
        Quaternion restingVisualRotation;
        bool hasRestingRotations;

        public override bool IsEnabled => currentState != MapIconState.Off;

        void LateUpdate()
        {
            if (currentState != MapIconState.Done)
                return;

            ApplyDoneRotation();
        }

        public override void SetMapIconState(MapIconState state)
        {
            currentState = state == MapIconState.Default ? MapIconState.Off : state;
            if (currentState == MapIconState.Done)
            {
                ApplyDoneRotation();
                SetIconColor(doneColor);
            }
            else
            {
                SetIconColor(activeColor);
            }
        }

        #region Methods

        protected override Vector3 GetPosition()
        {
            return Vector3.zero;
        }

        void ApplyDoneRotation()
        {
            CacheRestingRotations();
            transform.localRotation = restingRotation;

            if (iconRenderer != null)
            {
                iconRenderer.transform.localRotation = restingVisualRotation;
            }
        }

        void CacheRestingRotations()
        {
            if (hasRestingRotations)
                return;

            restingRotation = transform.localRotation;
            restingVisualRotation = iconRenderer != null ? iconRenderer.transform.localRotation : Quaternion.identity;
            hasRestingRotations = true;
        }

        #endregion
    }
}
