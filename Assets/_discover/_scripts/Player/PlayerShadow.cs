using UnityEngine;
namespace Antura.Minigames.DiscoverCountry
{
    public class PlayerShadow : MonoBehaviour
    {
        public Transform character; // Assign your character's Transform in the Inspector
        public LayerMask groundLayer; // Set this in the Inspector to the layer(s) used for the ground
        public float maxHeightAboveGround = 10.0f; // Maximum height at which shadow should scale

        private Vector3 initialScale; // To keep track of the initial scale of the shadow

        void Start()
        {
            initialScale = transform.localScale; // Store the initial scale of the shadow
        }

        void Update()
        {
            RaycastHit hit;
            // Perform a raycast straight down from the character's position
            if (Physics.Raycast(character.position, Vector3.down, out hit, Mathf.Infinity, groundLayer))
            {
                // Set the shadow's position to the hit point on the ground
                transform.position = new Vector3(character.position.x, hit.point.y + 0.01f, character.position.z);

                // Calculate the height above ground and scale the shadow accordingly
                float heightAboveGround = character.position.y - hit.point.y;
                float scaleMultiplier = Mathf.Clamp01(1.0f - (heightAboveGround / maxHeightAboveGround));
                transform.localScale = initialScale * scaleMultiplier;
            }
        }
    }
}
