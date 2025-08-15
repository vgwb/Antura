using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Antura.Discover.AI
{
    public class PigeonAI : AIEntity
    {
        [Header("Pigeon Settings")]
        [SerializeField] private float flightSpeed = 10f;
        [SerializeField] private float walkSpeed = 2f;
        [SerializeField] private float flightHeight = 3f;
        [SerializeField] private float flightDuration = 2f;
        [SerializeField] private string currentState = "idle";

        // Flying state management
        private bool isFlying = false;
        private float flightStartTime;
        private Vector3 flightStartPos;
        private Vector3 flightTargetPos;
        private Vector3 groundPosition;

        public string CurrentState { get { return currentState; } }
        public bool IsFlying { get { return isFlying; } }

        protected override void Start()
        {
            base.Start();
            groundPosition = transform.position;
        }

        protected override void Update()
        {
            base.Update();

            // Handle flying movement
            if (isFlying)
            {
                UpdateFlightMovement();
            }
        }

        protected override BehaviorTree CreateBehaviorTree()
        {
            var root = new Selector("Pigeon Root");

            // Priority 1: Flee from threats
            var fleeFromCat = new Sequence("Flee from Cat");
            fleeFromCat.AddChildren(
                new Condition("Cat Nearby?", ctx => HasNearbyThreat("Cat")),
                new BTAction("Fly Away from Cat", ctx => FlyAway())
            );

            // Priority 2: Flee from disturbances
            var fleeFromDisturbance = new Sequence("Flee from Disturbance");
            fleeFromDisturbance.AddChildren(
                new Condition("Disturbance Nearby?", ctx => HasNearbyDisturbance()),
                new BTAction("Fly Away from Disturbance", ctx => FlyAway())
            );

            // Priority 3: Social behavior
            var socialBehavior = new Sequence("Social Behavior");
            socialBehavior.AddChildren(
                new Condition("Other Pigeon Nearby?", ctx => ctx.NearbySameSpecies.Count > 0),
                new Condition("Not Flying?", ctx => !isFlying), // Don't stare while flying
                new BTAction("Stare at Pigeon", ctx => StareAtPigeon()),
                new Wait("Stare Duration", 2f)
            );

            // Priority 4: Foraging (only when on ground)
            var foraging = new Selector("Foraging");
            var eatFood = new Sequence("Eat Food");
            eatFood.AddChildren(
                new Condition("Not Flying?", ctx => !isFlying),
                new Condition("Food Available?", ctx => ctx.NearbyFood.Count > 0),
                new BTAction("Eat Food", ctx => EatFood())
            );

            var lookForFood = new Sequence("Look for Food");
            lookForFood.AddChildren(
                new Condition("Not Flying?", ctx => !isFlying),
                new BTAction("Look for Food", ctx => LookForFood())
            );

            foraging.AddChildren(
                eatFood,
                lookForFood
            );

            root.AddChildren(fleeFromCat, fleeFromDisturbance, socialBehavior, foraging);

            return new BehaviorTree("Pigeon Behavior", root, this);
        }

        protected override void CategorizeEntity(GameObject entity)
        {
            base.CategorizeEntity(entity);

            // Pigeon-specific categorization
            if (entity.CompareTag("Cat"))
                aiContext.NearbyThreats.Add(entity);
            else if (entity.CompareTag("Player") || entity.CompareTag("Car"))
                aiContext.NearbyDisturbances.Add(entity);
            else if (entity.GetComponent<PigeonAI>() != null)
                aiContext.NearbySameSpecies.Add(entity);
        }

        private bool HasNearbyThreat(string threatType)
        {
            return aiContext.NearbyThreats.Exists(threat => threat.CompareTag(threatType));
        }

        private bool HasNearbyDisturbance()
        {
            return aiContext.NearbyDisturbances.Count > 0;
        }

        private NodeStatus FlyAway()
        {
            if (!isFlying)
            {
                StartFlying();
            }

            // Continue flying until duration is complete
            if (isFlying && Time.time - flightStartTime < flightDuration)
            {
                return NodeStatus.Running; // Keep flying
            }
            else if (isFlying)
            {
                // Finished flying, land
                LandOnGround();
                return NodeStatus.Success;
            }

            return NodeStatus.Success;
        }

        private void StartFlying()
        {
            currentState = "flying";
            isFlying = true;
            flightStartTime = Time.time;
            flightStartPos = transform.position;

            // Calculate flee direction away from threats
            Vector3 fleeDirection = GetFleeDirection();
            if (fleeDirection == Vector3.zero)
                fleeDirection = UnityEngine.Random.insideUnitCircle.normalized;

            // Set target position: away from threat + up in the air
            flightTargetPos = transform.position + new Vector3(fleeDirection.x, 0, fleeDirection.z) * flightSpeed * flightDuration;
            flightTargetPos.y = groundPosition.y + flightHeight;

            Debug.Log(name + ": Taking flight! Flying to " + flightTargetPos);

            // Disable gravity if rigidbody exists
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = false;
                rb.linearVelocity = Vector3.zero;
            }
        }

        private void UpdateFlightMovement()
        {
            if (!isFlying)
                return;

            float flightProgress = (Time.time - flightStartTime) / flightDuration;
            flightProgress = Mathf.Clamp01(flightProgress);

            // Smooth flight arc - goes up then comes down
            float heightCurve = Mathf.Sin(flightProgress * Mathf.PI); // Creates an arc
            Vector3 currentFlightPos = Vector3.Lerp(flightStartPos, flightTargetPos, flightProgress);
            currentFlightPos.y = groundPosition.y + (flightHeight * heightCurve);

            // Move the pigeon
            transform.position = currentFlightPos;

            // Face movement direction
            Vector3 moveDirection = (flightTargetPos - flightStartPos).normalized;
            if (moveDirection != Vector3.zero)
            {
                transform.rotation = Quaternion.LookRotation(moveDirection);
            }
        }

        private void LandOnGround()
        {
            isFlying = false;
            currentState = "landing";

            // Make sure pigeon is on the ground
            Vector3 landingPos = transform.position;
            landingPos.y = groundPosition.y;
            transform.position = landingPos;

            // Re-enable gravity if rigidbody exists
            var rb = GetComponent<Rigidbody>();
            if (rb != null)
            {
                rb.useGravity = true;
            }

            // Update ground position for future flights
            groundPosition = transform.position;

            Debug.Log(name + ": Landed safely!");
        }

        private NodeStatus StareAtPigeon()
        {
            currentState = "staring";
            if (aiContext.NearbySameSpecies.Count > 0)
            {
                var target = aiContext.NearbySameSpecies[0];
                transform.LookAt(target.transform.position);
                Debug.Log(name + ": Staring at " + target.name);
            }
            return NodeStatus.Success;
        }

        private NodeStatus EatFood()
        {
            currentState = "eating";
            if (aiContext.NearbyFood.Count > 0)
            {
                var food = aiContext.NearbyFood[0];

                // Move towards food
                Vector3 directionToFood = (food.transform.position - transform.position).normalized;
                transform.position += directionToFood * walkSpeed * Time.deltaTime;

                // Look at food
                transform.LookAt(food.transform.position);

                // Debug.Log(name + ": Eating " + food.name);

                // If close enough, "consume" the food
                if (Vector3.Distance(transform.position, food.transform.position) < 0.5f)
                {
                    // Optionally destroy or hide the food
                    // Destroy(food);
                    return NodeStatus.Success;
                }

                return NodeStatus.Running; // Still moving toward food
            }
            return NodeStatus.Failure;
        }

        private NodeStatus LookForFood()
        {
            currentState = "searching";
            //Debug.Log(name + ": Looking for food");

            // Random walk or systematic search
            Vector3 randomDirection = UnityEngine.Random.insideUnitCircle.normalized;
            Vector3 moveDirection = new Vector3(randomDirection.x, 0, randomDirection.y);

            transform.position += moveDirection * walkSpeed * Time.deltaTime;
            transform.rotation = Quaternion.LookRotation(moveDirection);

            return NodeStatus.Success;
        }

        private Vector3 GetFleeDirection()
        {
            if (aiContext.NearbyThreats.Count == 0 && aiContext.NearbyDisturbances.Count == 0)
                return Vector3.zero;

            Vector3 totalThreatDirection = Vector3.zero;

            // Calculate direction away from all threats
            foreach (var threat in aiContext.NearbyThreats)
            {
                Vector3 directionFromThreat = (transform.position - threat.transform.position).normalized;
                totalThreatDirection += directionFromThreat;
            }

            foreach (var disturbance in aiContext.NearbyDisturbances)
            {
                Vector3 directionFromDisturbance = (transform.position - disturbance.transform.position).normalized;
                totalThreatDirection += directionFromDisturbance;
            }

            return totalThreatDirection.normalized;
        }

        protected override void OnDrawGizmos()
        {
            base.OnDrawGizmos();

            if (isFlying)
            {
                // Draw flight path
                Gizmos.color = Color.blue;
                Gizmos.DrawLine(flightStartPos, flightTargetPos);

                // Draw target position
                Gizmos.color = Color.red;
                Gizmos.DrawWireSphere(flightTargetPos, 0.3f);
            }

            // Draw ground position
            Gizmos.color = Color.green;
            Gizmos.DrawWireCube(groundPosition, Vector3.one * 0.2f);
        }
    }
}
