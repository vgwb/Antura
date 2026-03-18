using Antura.Discover.Interaction;
using UnityEngine;

namespace Antura.Discover
{
    public class InteractableIcon : AbstractMapIcon
    {
        [SerializeField] bool rotate = true;
        [SerializeField, Range(0f, 60f)] float rotationSpeed = 10f;
        [SerializeField] Color activeColor = Color.white;
        [SerializeField] Color doneColor = new(0.45f, 0.9f, 0.45f, 1f);

        MapIconState currentState = MapIconState.On;

        public override bool IsEnabled => currentState != MapIconState.Off;

        void Update()
        {
            if (!rotate || currentState != MapIconState.On)
                return;

            transform.Rotate(Vector3.up, rotationSpeed * Time.deltaTime, Space.Self);
        }

        public override void SetMapIconState(MapIconState state)
        {
            currentState = state == MapIconState.Default ? MapIconState.Off : state;
            if (currentState == MapIconState.Done)
            {
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

        #endregion
    }
}
