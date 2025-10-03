using Antura.Discover;
using UnityEngine;

public class BasicRigidBodyPush : MonoBehaviour
{
	public LayerMask pushLayers;
	public bool canPush;
	[Range(0.5f, 5f)] public float strength = 1.1f;

	private DiscoverCharacterMotorAdapter motorAdapter;

	private void Awake()
	{
		motorAdapter = GetComponent<DiscoverCharacterMotorAdapter>();
		if (motorAdapter != null)
		{
			motorAdapter.MovementHit += HandleMotorMovementHit;
		}
	}

	private void OnDestroy()
	{
		if (motorAdapter != null)
		{
			motorAdapter.MovementHit -= HandleMotorMovementHit;
		}
	}

	private void HandleMotorMovementHit(Collider hitCollider, Vector3 hitNormal, Vector3 hitPoint, Vector3 motorVelocity)
	{
		if (!canPush)
		{
			return;
		}

		Vector3 moveDirection = motorVelocity.sqrMagnitude > 0.001f ? motorVelocity.normalized : -hitNormal;
		PushRigidBody(hitCollider, moveDirection);
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		if (!canPush)
		{
			return;
		}

		PushRigidBody(hit.collider, hit.moveDirection);
	}

	private void PushRigidBody(Collider collider, Vector3 moveDirection)
	{
		// make sure we hit a non kinematic rigidbody
		Rigidbody body = collider.attachedRigidbody;
		if (body == null || body.isKinematic)
		{
			return;
		}

		// make sure we only push desired layer(s)
		var bodyLayerMask = 1 << body.gameObject.layer;
		if ((bodyLayerMask & pushLayers.value) == 0)
		{
			return;
		}

		// We dont want to push objects below us
		if (moveDirection.y < -0.3f)
		{
			return;
		}

		// Calculate push direction from move direction, horizontal motion only
		Vector3 pushDir = new Vector3(moveDirection.x, 0.0f, moveDirection.z);
		if (pushDir.sqrMagnitude < 0.0001f)
		{
			return;
		}
		pushDir.Normalize();

		// Apply the push and take strength into account
		body.AddForce(pushDir * strength, ForceMode.Impulse);
	}
}
