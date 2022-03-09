using UnityEngine;
using System.Collections.Generic;
using Antura.LivingLetters;

namespace Antura.Minigames.Tobogan
{
    public class LettersTower : MonoBehaviour
    {
        public bool HasLettersInBacklog { get { return backlogTube.Count > 0; } }
        public bool HasStackedLetters { get { return stackedLetters.Count > 0; } }
        public bool IsLetterFalling { get { return fallingLetter != null; } }

        // The tower is a stack of letters, that could be crash
        private List<LivingLetterRagdoll> stackedLetters = new List<LivingLetterRagdoll>();
        Dictionary<LivingLetterRagdoll, System.Action> releasedCallbacks = new Dictionary<LivingLetterRagdoll, System.Action>();

        public const float LETTER_HEIGHT = 4.8f;
        public float TowerFullHeight { get { return stackedLetters.Count * LETTER_HEIGHT; } }
        public GameObject letterPrefab;
        public GameObject shadow;

        // Used to manage the backlog in the tube and the falling letter
        public Queue<LivingLetterRagdoll> backlogTube = new Queue<LivingLetterRagdoll>();
        LivingLetterRagdoll fallingLetter;
        float tremblingTimer = 0;

        public bool testAddLetter = false;

        // Used to simulate tower bounciness
        public bool doBounce = false;
        public float Elasticity = 100.0f;
        float currentHeight;
        float yVelocity = 0.0f;
        float lastCompressionValue = 1;

        Material shadowMaterial;
        float maxShadowAlpha;

        // Used to simulate tower swing
        float maxSwingAmountFactor = 80.0f;
        float minSwingAmountFactor = 2.0f;

        float swingAmountFactor = 10.0f;

        public float swingSpeed = 0.15f;
        float currentSwing = 0.0f;
        float crashingSwing = 0.0f;
        float swingPercentage = 0; // a swing starts from center, go left, go right and comes back to center

        // Used to calculate the right moment in which a letter should be dropped
        public Transform fallingSpawnPosition;
        public TremblingTube tube;
        float fallingSpawnHeight;
        // I use here time, instead of integrating over timesteps to be sure that the fall takes that time
        float fallingTime = 0.0f;
        float remainingFallingTime = 0.0f;
        float letterInitialFallSpeed = -10.0f;
        float spawnTimer = 0;

        // Crash behaviour
        public event System.Action onCrashed;
        public bool isCrashingRequested = false;
        float crashIntervalBetweenLettersTimer = 0;
        float crashDirection = 1;
        float crashingSpeed = 0;

        /// <summary>
        /// Crash the tower!
        /// </summary>
        public void RequestCrash()
        {
            isCrashingRequested = true;
            backlogTube.Clear();
        }

        /// <summary>
        /// Add a new letter to falling queue, it will be released when is the right time
        /// </summary>
        public void AddLetter(System.Action onLetterReleased)
        {
            var newLetter = GameObject.Instantiate(letterPrefab);
            newLetter.transform.SetParent(transform, false);

            newLetter.SetActive(false);

            var letterComponent = newLetter.GetComponent<LivingLetterRagdoll>();
            backlogTube.Enqueue(letterComponent);

            if (onLetterReleased != null)
                releasedCallbacks.Add(letterComponent, onLetterReleased);
        }

        void Start()
        {
            currentHeight = TowerFullHeight;

            shadowMaterial = shadow.GetComponentInChildren<Renderer>().material;
            var oldCol = shadowMaterial.color;
            maxShadowAlpha = oldCol.a;
            oldCol.a = 0;
            shadowMaterial.color = oldCol;
            /*
            // Test
            for (int i = 0; i < 15; ++i)
                AddLetter(null);
                */
        }

        void Update()
        {
            if (backlogTube.Count > 0)
                tremblingTimer = 0.75f;

            if (tremblingTimer > 0f)
            {
                tremblingTimer -= Time.deltaTime;
                tube.Trembling = true;
            }
            else
                tube.Trembling = false;

            fallingSpawnHeight = (fallingSpawnPosition.position - transform.position).y;

            if (fallingLetter == null)
            {
                // Manage backlog
                UpdateBacklog();
            }

            if (fallingLetter != null)
            {
                // Update current falling letter
                UpdateFallingLetter(fallingLetter);
            }

            UpdateTowerMovements();

            var oldCol = shadowMaterial.color;

            float targetAlpha;

            if (stackedLetters.Count > 0)
                targetAlpha = maxShadowAlpha;
            else if (fallingLetter != null)
                targetAlpha = Mathf.Lerp(maxShadowAlpha, 0, Vector3.Distance(fallingLetter.transform.position, transform.position) / 20);
            else
                targetAlpha = 0;

            oldCol.a = Mathf.Lerp(oldCol.a, targetAlpha, Time.deltaTime * 5.0f);
            shadowMaterial.color = oldCol;

            if (testAddLetter)
            {
                testAddLetter = false;
                AddLetter(null);
            }
        }


        void UpdateTowerMovements()
        {
            float normalHeight = TowerFullHeight;

            //// Simulate elastic movement
            float elasticForce = ComputeElasticForce();

            // F = m * a; since all letters are equal, we approximate letter mass to 1 and work on Elasticity to model the graphics feedback
            // F -> 1 * a
            float yAcceleration = elasticForce + Physics.gravity.y;

            if (stackedLetters.Count < 2)
            {
                yVelocity = 0;
                currentHeight = TowerFullHeight;
            }
            else
            {
                // V = a * t
                yVelocity += yAcceleration * Time.deltaTime;

                // Integrates changes between last timestep
                // h += V * t + 0.5 * a * t^2
                currentHeight += yVelocity * Time.deltaTime + 0.5f * yAcceleration * Time.deltaTime * Time.deltaTime;

                // Put a maximum of height so letters cannot be detached
                if (currentHeight > normalHeight + LETTER_HEIGHT * 0.2f)
                {
                    currentHeight = normalHeight + LETTER_HEIGHT * 0.2f;

                    //yVelocity = -yVelocity*0.8f;// elastic bounce
                    yVelocity = Mathf.Min(yVelocity, 0.1f); // just a small jump
                }
                else if (currentHeight < LETTER_HEIGHT)
                    currentHeight = LETTER_HEIGHT;
            }

            //// Simulate a bit of horizontal swinging
            float swingFrequency = swingSpeed;

            swingPercentage += Time.deltaTime * swingFrequency;
            swingPercentage = Mathf.Repeat(swingPercentage, 1);
            float currentSwingNormalized = Mathf.Sin(swingPercentage * 2 * Mathf.PI);

            currentSwing = currentSwingNormalized * swingAmountFactor + crashingSwing;

            swingAmountFactor = Mathf.Lerp(swingAmountFactor, minSwingAmountFactor, 0.1f * Time.deltaTime);
            swingAmountFactor = Mathf.Clamp(swingAmountFactor, minSwingAmountFactor, maxSwingAmountFactor);

            // Update letters positions
            if (currentHeight == 0)
                lastCompressionValue = 1;
            else
                lastCompressionValue = currentHeight / normalHeight;
            for (int i = 0, count = stackedLetters.Count; i < count; ++i)
            {
                float heightSwingFactor = GetHeightSwingFactor(i);

                stackedLetters[i].transform.position = transform.position + Vector3.up * (i * LETTER_HEIGHT * lastCompressionValue) + transform.right * currentSwing * heightSwingFactor;

                if (i > 0)
                {
                    stackedLetters[i].transform.up = (stackedLetters[i].transform.position - stackedLetters[i - 1].transform.position).normalized;
                    stackedLetters[i].transform.right = Vector3.Cross(stackedLetters[i].transform.up, transform.forward);

                }
            }

            if (doBounce)
            {
                doBounce = false;

                Bounce();
            }


            if (isCrashingRequested && !IsLetterFalling)
            {
                crashingSwing += crashDirection * crashingSpeed * Time.deltaTime;
                crashingSpeed += 150 * Time.deltaTime;

                // wait for a good swing
                if (Mathf.Abs(currentSwing) > maxSwingAmountFactor * 1.2f)
                {
                    // Do Actual Crash
                    crashIntervalBetweenLettersTimer -= Time.deltaTime;
                    if (crashIntervalBetweenLettersTimer <= 0)
                    {
                        CrashTop();
                    }
                }
            }
            else
            {
                crashDirection = (swingPercentage < 0.25f || swingPercentage > 0.75f) ? 1 : -1;
                crashingSwing = 0;
                crashingSpeed = 0;
            }
        }

        void CrashTop()
        {
            /*
            // Do Actual Crash

            for (int i = 0, count = stackedLetters.Count; i < count; ++i)
            {
                var randomVelocity = Random.insideUnitSphere * 10.0f;
                randomVelocity.y = Mathf.Abs(randomVelocity.y);

                //randomVelocity.y = Mathf.Min(Mathf.Abs(randomVelocity.y), 5);

                randomVelocity += transform.right * crashDirection * i;

                stackedLetters[i].GetComponent<LivingLetterRagdoll>().SetRagdoll(true, randomVelocity);
            }
            stackedLetters.Clear();
            */
            if (stackedLetters.Count > 0)
            {
                int letterID = (stackedLetters.Count - 1);

                crashIntervalBetweenLettersTimer = 0.1f;
                var randomVelocity = Random.insideUnitSphere * 10.0f;
                randomVelocity.y = Mathf.Abs(randomVelocity.y);

                //randomVelocity.y = Mathf.Min(Mathf.Abs(randomVelocity.y), 5);

                randomVelocity += transform.right * crashDirection * letterID;

                stackedLetters[letterID].GetComponent<LivingLetterRagdoll>().SetRagdoll(true, randomVelocity);
                stackedLetters.RemoveAt(letterID);
                currentHeight -= LETTER_HEIGHT;
            }

            if (stackedLetters.Count == 0 && onCrashed != null)
            {
                isCrashingRequested = false;
                onCrashed();
            }
        }

        float GetHeightSwingFactor(int i)
        {
            float heightSwingFactor = i / 30.0f;
            return heightSwingFactor * heightSwingFactor;
        }

        float ComputeElasticForce()
        {
            // "Normal" height of tower
            float normalHeight = TowerFullHeight;

            // dx
            float deltaHeight = currentHeight - normalHeight;

            // F = -K * dx
            float elasticForce = -Elasticity * deltaHeight;

            return elasticForce;
        }

        void UpdateFallingLetter(LivingLetterRagdoll letter)
        {
            remainingFallingTime -= Time.deltaTime;

            float passedTime = fallingTime - remainingFallingTime;
            letter.transform.position = transform.position + Vector3.up * (
                letterInitialFallSpeed * passedTime +
                fallingSpawnHeight + 0.5f * Physics.gravity.y * passedTime * passedTime
                );

            bool toStack = false;
            // Manage attaching
            if (stackedLetters.Count == 0)
            {
                toStack = (remainingFallingTime <= 0);
            }
            else
            {
                float fullHeight = TowerFullHeight;
                float compressedHeight = lastCompressionValue * fullHeight;

                float currentFallHeight = letter.transform.position.y - transform.position.y - compressedHeight;
                //float totalFallHeight = fallingSpawnHeight - compressedHeight;

                toStack = (currentFallHeight <= 0);
            }

            // Stack it
            if (toStack)
            {
                StackFallingLetter();
            }
        }

        void StackFallingLetter()
        {
            if (fallingLetter == null)
                return;

            fallingLetter.GetComponentInChildren<LivingLetterController>().Falling = false;
            fallingLetter.GetComponentInChildren<LivingLetterController>().SetState(LLAnimationStates.LL_idle);

            for (int i = 0; i < stackedLetters.Count; ++i)
                stackedLetters[i].GetComponentInChildren<LivingLetterController>().SetState(LLAnimationStates.LL_still);

            currentHeight += LETTER_HEIGHT;
            stackedLetters.Add(fallingLetter);
            var currentFallingLetter = fallingLetter;
            fallingLetter = null;
            Bounce();

            System.Action callback;
            if (releasedCallbacks.TryGetValue(currentFallingLetter, out callback))
            {
                releasedCallbacks.Remove(currentFallingLetter);
                callback();
            }
        }

        void Bounce()
        {
            if (stackedLetters.Count > 2)
                yVelocity += -20f;
            else
                yVelocity += -1f * stackedLetters.Count;

            swingAmountFactor += 20.0f;
        }

        void SpawnLetter(LivingLetterRagdoll letter)
        {
            remainingFallingTime = fallingTime;
            letter.transform.position = transform.position + fallingSpawnHeight * Vector3.up;
            letter.gameObject.SetActive(true);
            fallingLetter = letter;
            letter.GetComponentInChildren<LivingLetterController>().Falling = true;
            spawnTimer = 0;

            ToboganConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.UIPopup);
        }

        void UpdateBacklog()
        {
            if (backlogTube.Count == 0 || isCrashingRequested)
                return;

            // A letter was already scheduled to spawn
            if (spawnTimer > 0)
            {
                spawnTimer -= Time.deltaTime;

                if (spawnTimer <= 0)
                {
                    // Spawn!
                    var currentLetter = backlogTube.Dequeue();
                    SpawnLetter(currentLetter);
                }
            }
            else
            {
                // Must schedule another letter to spawn in the right moment,
                // that is, in order that, when it hits the tower, the swing position is at 0.
                // we can ignore the bounce movement since it's quite small to be approximated to 0

                // Some maths: {....please, do not say that "engineering skills are not needed to make games" no more}

                // The following is true:
                // tFall + spawnTimer = tSwingToCenter + K*swingPeriod*0.5, K in N, K >= 0
                // ---> spawnTimer = tSwingToCenter - tFall + K*swingPeriod*0.5
                // ---> we'll select, in the end, the minimum K in order that spawnTimer >= 0

                // tFall = (- initialVelocity +/- sqrtf(initialVelocity^2 - 2 * g * fallHeight)) / g;

                // sin(theta) = 0 -> theta = 0 + K * PI
                // we'll select, in the end, K in order that tSwingToCenter >= 0 and minimum

                float approximatedFallHeight = fallingSpawnHeight - TowerFullHeight;
                float sqrtDelta = Mathf.Sqrt(letterInitialFallSpeed * letterInitialFallSpeed - 2 * Physics.gravity.y * approximatedFallHeight);

                fallingTime = (-letterInitialFallSpeed - sqrtDelta) / Physics.gravity.y;

                if (stackedLetters.Count < 3 || (GetHeightSwingFactor(stackedLetters.Count) * swingAmountFactor < 1.0f))
                {
                    spawnTimer = 0;
                }
                else
                {
                    float swingFrequency = swingSpeed;

                    // it
                    float tSwingToCenter;
                    if (swingFrequency == 0)
                        tSwingToCenter = 0;
                    else
                        tSwingToCenter = (Mathf.CeilToInt(swingPercentage / 0.5f) * 0.5f - swingPercentage) / swingFrequency;


                    float swingPeriod = (swingFrequency == 0 ? 0 : 1 / swingFrequency) * 0.5f;

                    spawnTimer = tSwingToCenter - fallingTime + swingPeriod * Mathf.Max(0, Mathf.CeilToInt((fallingTime - tSwingToCenter) / swingPeriod));
                }

                if (spawnTimer < 0.05f)
                {
                    // Spawn!
                    var currentLetter = backlogTube.Dequeue();
                    SpawnLetter(currentLetter);
                }
            }
        }
    }
}
