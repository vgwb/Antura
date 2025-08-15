using System;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Antura.Discover.AI
{
    public enum NodeStatus
    {
        Success,
        Failure,
        Running
    }

    public abstract class BTNode
    {
        public string Name { get; protected set; }
        public NodeStatus Status { get; protected set; } = NodeStatus.Failure;

        protected BTNode(string name = "Node")
        {
            Name = name;
        }

        public abstract NodeStatus Execute(AIContext context);

        public virtual void Reset()
        {
            Status = NodeStatus.Failure;
        }

        public virtual void OnDrawGizmos(Vector3 position, float radius = 0.5f)
        {
            // Optional: Visual debugging in Scene view
        }
    }

    // Composite Nodes (have children)
    public abstract class CompositeNode : BTNode
    {
        protected List<BTNode> children = new List<BTNode>();
        protected int currentChild = 0;

        public List<BTNode> Children
        {
            get { return children; }
        }

        protected CompositeNode(string name) : base(name) { }

        public void AddChild(BTNode child)
        {
            children.Add(child);
        }

        public void AddChildren(params BTNode[] childNodes)
        {
            children.AddRange(childNodes);
        }

        public override void Reset()
        {
            base.Reset();
            currentChild = 0;
            foreach (var child in children)
            {
                child.Reset();
            }
        }
    }

    // Selector: runs children until one succeeds (OR logic)
    public class Selector : CompositeNode
    {
        public Selector(string name) : base(name) { }

        public override NodeStatus Execute(AIContext context)
        {
            for (int i = currentChild; i < children.Count; i++)
            {
                var status = children[i].Execute(context);

                if (status == NodeStatus.Success)
                {
                    Reset();
                    return NodeStatus.Success;
                }
                else if (status == NodeStatus.Running)
                {
                    currentChild = i;
                    return NodeStatus.Running;
                }
                // Continue to next child on Failure
            }

            Reset();
            return NodeStatus.Failure;
        }
    }

    // Sequence: runs children until one fails (AND logic)
    public class Sequence : CompositeNode
    {
        public Sequence(string name) : base(name) { }

        public override NodeStatus Execute(AIContext context)
        {
            for (int i = currentChild; i < children.Count; i++)
            {
                var status = children[i].Execute(context);

                if (status == NodeStatus.Failure)
                {
                    Reset();
                    return NodeStatus.Failure;
                }
                else if (status == NodeStatus.Running)
                {
                    currentChild = i;
                    return NodeStatus.Running;
                }
                // Continue to next child on Success
            }

            Reset();
            return NodeStatus.Success;
        }
    }

    // Decorator Nodes (modify single child behavior)
    public abstract class Decorator : BTNode
    {
        protected BTNode child;

        protected Decorator(string name, BTNode child) : base(name)
        {
            this.child = child;
        }

        public override void Reset()
        {
            base.Reset();
            child?.Reset();
        }
    }

    // Inverter: flips success/failure
    public class Inverter : Decorator
    {
        public Inverter(string name, BTNode child) : base(name, child) { }

        public override NodeStatus Execute(AIContext context)
        {
            if (child == null)
                return NodeStatus.Failure;

            var status = child.Execute(context);

            switch (status)
            {
                case NodeStatus.Success:
                    return NodeStatus.Failure;
                case NodeStatus.Failure:
                    return NodeStatus.Success;
                default:
                    return status; // Running stays Running
            }
        }
    }

    // Repeater: repeats child N times or until failure
    public class Repeater : Decorator
    {
        private readonly int maxTimes;
        private int currentCount = 0;

        public Repeater(string name, BTNode child, int times = -1) : base(name, child)
        {
            maxTimes = times; // -1 = infinite
        }

        public override NodeStatus Execute(AIContext context)
        {
            if (child == null)
                return NodeStatus.Failure;

            while (maxTimes == -1 || currentCount < maxTimes)
            {
                var status = child.Execute(context);

                if (status == NodeStatus.Running)
                    return NodeStatus.Running;
                if (status == NodeStatus.Failure)
                {
                    Reset();
                    return NodeStatus.Failure;
                }

                currentCount++;
                child.Reset(); // Reset for next iteration
            }

            Reset();
            return NodeStatus.Success;
        }

        public override void Reset()
        {
            base.Reset();
            currentCount = 0;
        }
    }

    // Parallel: runs all children simultaneously
    public class Parallel : CompositeNode
    {
        public enum Policy
        {
            RequireOne,    // Success when one child succeeds
            RequireAll,    // Success when all children succeed
            RequireMajority // Success when majority succeed
        }

        private readonly Policy successPolicy;
        private readonly Policy failurePolicy;
        private readonly List<NodeStatus> childStatuses = new List<NodeStatus>();

        public Parallel(string name, Policy successPolicy = Policy.RequireOne, Policy failurePolicy = Policy.RequireAll) : base(name)
        {
            this.successPolicy = successPolicy;
            this.failurePolicy = failurePolicy;
        }

        public override NodeStatus Execute(AIContext context)
        {
            // Initialize status list if needed
            while (childStatuses.Count < children.Count)
                childStatuses.Add(NodeStatus.Failure);

            int successCount = 0;
            int failureCount = 0;
            int runningCount = 0;

            // Execute all children
            for (int i = 0; i < children.Count; i++)
            {
                if (childStatuses[i] != NodeStatus.Running)
                    continue; // Skip finished children

                childStatuses[i] = children[i].Execute(context);

                switch (childStatuses[i])
                {
                    case NodeStatus.Success:
                        successCount++;
                        break;
                    case NodeStatus.Failure:
                        failureCount++;
                        break;
                    case NodeStatus.Running:
                        runningCount++;
                        break;
                }
            }

            // Check success condition
            bool shouldSucceed = false;
            switch (successPolicy)
            {
                case Policy.RequireOne:
                    shouldSucceed = successCount > 0;
                    break;
                case Policy.RequireAll:
                    shouldSucceed = successCount == children.Count;
                    break;
                case Policy.RequireMajority:
                    shouldSucceed = successCount > children.Count / 2;
                    break;
            }

            // Check failure condition
            bool shouldFail = false;
            switch (failurePolicy)
            {
                case Policy.RequireOne:
                    shouldFail = failureCount > 0;
                    break;
                case Policy.RequireAll:
                    shouldFail = failureCount == children.Count;
                    break;
                case Policy.RequireMajority:
                    shouldFail = failureCount > children.Count / 2;
                    break;
            }

            if (shouldSucceed)
            {
                Reset();
                return NodeStatus.Success;
            }
            if (shouldFail)
            {
                Reset();
                return NodeStatus.Failure;
            }

            return NodeStatus.Running;
        }

        public override void Reset()
        {
            base.Reset();
            childStatuses.Clear();
        }
    }

    // Random Selector: picks random child to execute
    public class RandomSelector : CompositeNode
    {
        private readonly System.Random random = new System.Random();
        private List<int> shuffledIndices = new List<int>();
        private int currentIndex = 0;

        public RandomSelector(string name) : base(name) { }

        public override NodeStatus Execute(AIContext context)
        {
            // Initialize shuffled list if needed
            if (shuffledIndices.Count != children.Count)
            {
                shuffledIndices.Clear();
                for (int i = 0; i < children.Count; i++)
                    shuffledIndices.Add(i);
                ShuffleList();
                currentIndex = 0;
            }

            for (int i = currentIndex; i < shuffledIndices.Count; i++)
            {
                var childIndex = shuffledIndices[i];
                var status = children[childIndex].Execute(context);

                if (status == NodeStatus.Success)
                {
                    Reset();
                    return NodeStatus.Success;
                }
                else if (status == NodeStatus.Running)
                {
                    currentIndex = i;
                    return NodeStatus.Running;
                }
            }

            Reset();
            return NodeStatus.Failure;
        }

        private void ShuffleList()
        {
            for (int i = shuffledIndices.Count - 1; i > 0; i--)
            {
                int randomIndex = random.Next(i + 1);
                int temp = shuffledIndices[i];
                shuffledIndices[i] = shuffledIndices[randomIndex];
                shuffledIndices[randomIndex] = temp;
            }
        }

        public override void Reset()
        {
            base.Reset();
            ShuffleList();
            currentIndex = 0;
        }
    }

    // Probability Selector: chooses children based on weights
    public class ProbabilitySelector : CompositeNode
    {
        private readonly List<float> weights = new List<float>();
        private readonly System.Random random = new System.Random();

        public ProbabilitySelector(string name) : base(name) { }

        public void AddChildWithWeight(BTNode child, float weight)
        {
            AddChild(child);
            weights.Add(weight);
        }

        public override NodeStatus Execute(AIContext context)
        {
            if (children.Count == 0)
                return NodeStatus.Failure;

            var selectedChild = SelectWeightedChild();
            return selectedChild?.Execute(context) ?? NodeStatus.Failure;
        }

        private BTNode SelectWeightedChild()
        {
            float totalWeight = 0f;
            for (int i = 0; i < Math.Min(children.Count, weights.Count); i++)
                totalWeight += weights[i];

            float randomValue = (float)random.NextDouble() * totalWeight;
            float currentWeight = 0f;

            for (int i = 0; i < Math.Min(children.Count, weights.Count); i++)
            {
                currentWeight += weights[i];
                if (randomValue <= currentWeight)
                    return children[i];
            }

            return children[children.Count - 1]; // Fallback
        }
    }

    // Cooldown: prevents child from running too frequently
    public class Cooldown : Decorator
    {
        private readonly float cooldownTime;
        private float lastExecutionTime = -1f;

        public Cooldown(string name, BTNode child, float cooldownSeconds) : base(name, child)
        {
            cooldownTime = cooldownSeconds;
        }

        public override NodeStatus Execute(AIContext context)
        {
            float currentTime = Time.time;

            if (lastExecutionTime >= 0 && currentTime - lastExecutionTime < cooldownTime)
                return NodeStatus.Failure;

            var status = child?.Execute(context) ?? NodeStatus.Failure;

            if (status == NodeStatus.Success || status == NodeStatus.Failure)
                lastExecutionTime = currentTime;

            return status;
        }

        public override void Reset()
        {
            base.Reset();
            // Don't reset lastExecutionTime - cooldown persists
        }
    }

    // Until Success: keeps trying child until it succeeds
    public class UntilSuccess : Decorator
    {
        public UntilSuccess(string name, BTNode child) : base(name, child) { }

        public override NodeStatus Execute(AIContext context)
        {
            if (child == null)
                return NodeStatus.Failure;

            var status = child.Execute(context);

            if (status == NodeStatus.Success)
                return NodeStatus.Success;

            // Keep trying on failure or running
            child.Reset();
            return NodeStatus.Running;
        }
    }

    // Until Failure: keeps trying child until it fails
    public class UntilFailure : Decorator
    {
        public UntilFailure(string name, BTNode child) : base(name, child) { }

        public override NodeStatus Execute(AIContext context)
        {
            if (child == null)
                return NodeStatus.Failure;

            var status = child.Execute(context);

            if (status == NodeStatus.Failure)
                return NodeStatus.Success;

            // Keep trying on success or running
            child.Reset();
            return NodeStatus.Running;
        }
    }

    // Condition Node
    public class Condition : BTNode
    {
        private readonly Func<AIContext, bool> conditionFunc;

        public Condition(string name, Func<AIContext, bool> condition) : base(name)
        {
            conditionFunc = condition;
        }

        public override NodeStatus Execute(AIContext context)
        {
            return conditionFunc?.Invoke(context) == true ? NodeStatus.Success : NodeStatus.Failure;
        }
    }

    // BT Action Node (renamed to avoid conflict with System.Action)
    public class BTAction : BTNode
    {
        private readonly Func<AIContext, NodeStatus> actionFunc;

        public BTAction(string name, Func<AIContext, NodeStatus> action) : base(name)
        {
            actionFunc = action;
        }

        public override NodeStatus Execute(AIContext context)
        {
            return actionFunc?.Invoke(context) ?? NodeStatus.Failure;
        }
    }

    // Wait Node (for timed behaviors)
    public class Wait : BTNode
    {
        private readonly float duration;
        private float startTime = -1f;

        public Wait(string name, float duration) : base(name)
        {
            this.duration = duration;
        }

        public override NodeStatus Execute(AIContext context)
        {
            if (startTime < 0)
            {
                startTime = Time.time;
                return NodeStatus.Running;
            }

            if (Time.time - startTime >= duration)
            {
                Reset();
                return NodeStatus.Success;
            }

            return NodeStatus.Running;
        }

        public override void Reset()
        {
            base.Reset();
            startTime = -1f;
        }
    }

    // Advanced Action Node with built-in state management
    public class StatefulAction : BTNode
    {
        private readonly Func<AIContext, NodeStatus> onStart;
        private readonly Func<AIContext, NodeStatus> onUpdate;
        private readonly System.Action<AIContext> onEnd;
        private bool hasStarted = false;

        public StatefulAction(string name,
            Func<AIContext, NodeStatus> onStart = null,
            Func<AIContext, NodeStatus> onUpdate = null,
            System.Action<AIContext> onEnd = null) : base(name)
        {
            this.onStart = onStart;
            this.onUpdate = onUpdate;
            this.onEnd = onEnd;
        }

        public override NodeStatus Execute(AIContext context)
        {
            // First execution - call start
            if (!hasStarted)
            {
                hasStarted = true;
                var startStatus = onStart?.Invoke(context) ?? NodeStatus.Success;
                if (startStatus != NodeStatus.Running)
                {
                    onEnd?.Invoke(context);
                    Reset();
                    return startStatus;
                }
            }

            // Ongoing execution - call update
            var updateStatus = onUpdate?.Invoke(context) ?? NodeStatus.Success;
            if (updateStatus != NodeStatus.Running)
            {
                onEnd?.Invoke(context);
                Reset();
            }

            return updateStatus;
        }

        public override void Reset()
        {
            base.Reset();
            hasStarted = false;
        }
    }

    // Conditional Action: only executes if condition is met
    public class ConditionalAction : BTNode
    {
        private readonly Func<AIContext, bool> condition;
        private readonly Func<AIContext, NodeStatus> action;

        public ConditionalAction(string name, Func<AIContext, bool> condition, Func<AIContext, NodeStatus> action) : base(name)
        {
            this.condition = condition;
            this.action = action;
        }

        public override NodeStatus Execute(AIContext context)
        {
            if (condition?.Invoke(context) == true)
                return action?.Invoke(context) ?? NodeStatus.Failure;

            return NodeStatus.Failure;
        }
    }

    // Enhanced Blackboard with events and persistence
    public class Blackboard
    {
        private readonly Dictionary<string, object> data = new Dictionary<string, object>();
        private readonly Dictionary<string, System.Action<object>> changeListeners = new Dictionary<string, System.Action<object>>();

        public void Set<T>(string key, T value)
        {
            var oldValue = data.ContainsKey(key) ? data[key] : null;
            data[key] = value;

            // Notify listeners of change
            if (changeListeners.ContainsKey(key) && !Equals(oldValue, value))
                changeListeners[key]?.Invoke(value);
        }

        public T Get<T>(string key, T defaultValue = default)
        {
            if (data.TryGetValue(key, out var value) && value is T typedValue)
            {
                return typedValue;
            }
            return defaultValue;
        }

        public bool Has(string key)
        {
            return data.ContainsKey(key);
        }

        public void Clear()
        {
            data.Clear();
            changeListeners.Clear();
        }

        public void Subscribe(string key, System.Action<object> callback)
        {
            changeListeners[key] = callback;
        }

        public void Unsubscribe(string key)
        {
            changeListeners.Remove(key);
        }

        // Increment/Decrement for counters
        public void Increment(string key, int amount = 1)
        {
            var current = Get<int>(key, 0);
            Set(key, current + amount);
        }

        public void Decrement(string key, int amount = 1)
        {
            Increment(key, -amount);
        }

        // Timer utilities
        public void StartTimer(string key, float duration)
        {
            Set(key + "_start", Time.time);
            Set(key + "_duration", duration);
        }

        public bool IsTimerExpired(string key)
        {
            var startTime = Get<float>(key + "_start", 0f);
            var duration = Get<float>(key + "_duration", 0f);
            return Time.time >= startTime + duration;
        }

        public float GetTimerProgress(string key)
        {
            var startTime = Get<float>(key + "_start", 0f);
            var duration = Get<float>(key + "_duration", 1f);
            return Mathf.Clamp01((Time.time - startTime) / duration);
        }
    }

    // AI Context - contains all data needed for AI decisions
    public class AIContext
    {
        public MonoBehaviour Entity { get; set; }
        public Blackboard Blackboard { get; set; }
        public float DeltaTime { get; set; }
        public Transform Transform { get; set; }

        // Antura-specific context data
        public List<GameObject> NearbyEntities { get; set; } = new List<GameObject>();
        public List<GameObject> NearbyFood { get; set; } = new List<GameObject>();
        public List<GameObject> NearbyThreats { get; set; } = new List<GameObject>();
        public List<GameObject> NearbySameSpecies { get; set; } = new List<GameObject>();
        public List<GameObject> NearbyDisturbances { get; set; } = new List<GameObject>();

        public AIContext(MonoBehaviour entity)
        {
            Entity = entity;
            Transform = entity.transform;
            Blackboard = new Blackboard();
        }
    }

    // Main Behavior Tree class
    [System.Serializable]
    public class BehaviorTree
    {
        [SerializeField] private string treeName;
        private BTNode rootNode;
        private AIContext context;
        private bool isRunning = false;

        public string TreeName { get { return treeName; } }
        public bool IsRunning { get { return isRunning; } }
        public Blackboard Blackboard { get { return context?.Blackboard; } }
        public BTNode RootNode { get { return rootNode; } }

        public BehaviorTree(string name, BTNode root, MonoBehaviour entity)
        {
            treeName = name;
            rootNode = root;
            context = new AIContext(entity);
        }

        public NodeStatus Tick(float deltaTime = 0f)
        {
            if (rootNode == null || context == null)
                return NodeStatus.Failure;

            context.DeltaTime = deltaTime;
            isRunning = true;

            var status = rootNode.Execute(context);

            if (status != NodeStatus.Running)
            {
                rootNode.Reset();
                isRunning = false;
            }

            return status;
        }

        public void Reset()
        {
            rootNode?.Reset();
            isRunning = false;
        }

        public void UpdateContext(AIContext newContext)
        {
            if (newContext != null)
            {
                context = newContext;
            }
        }
    }

    // Base AI Entity Component for Unity
    public abstract class AIEntity : MonoBehaviour
    {
        [Header("AI Settings")]
        [SerializeField] protected float detectionRadius = 5f;
        [SerializeField] protected LayerMask detectionLayers = -1;
        [SerializeField] protected bool enableGizmos = true;

        protected BehaviorTree behaviorTree;
        protected AIContext aiContext;

        public BehaviorTree BehaviorTree { get { return behaviorTree; } }
        public AIContext Context { get { return aiContext; } }

        protected virtual void Start()
        {
            aiContext = new AIContext(this);
            behaviorTree = CreateBehaviorTree();
        }

        protected virtual void Update()
        {
            if (behaviorTree != null)
            {
                UpdateAIContext();
                behaviorTree.Tick(Time.deltaTime);
            }
        }

        protected abstract BehaviorTree CreateBehaviorTree();

        protected virtual void UpdateAIContext()
        {
            if (aiContext == null)
                return;

            // Clear previous frame data
            aiContext.NearbyEntities.Clear();
            aiContext.NearbyFood.Clear();
            aiContext.NearbyThreats.Clear();
            aiContext.NearbySameSpecies.Clear();
            aiContext.NearbyDisturbances.Clear();

            // Detect nearby objects
            var colliders = Physics.OverlapSphere(transform.position, detectionRadius, detectionLayers);

            foreach (var col in colliders)
            {
                if (col.gameObject == gameObject)
                    continue;

                aiContext.NearbyEntities.Add(col.gameObject);
                CategorizeEntity(col.gameObject);
            }
        }

        protected virtual void CategorizeEntity(GameObject entity)
        {
            // Override in derived classes to categorize entities
            // Example: check tags, components, etc.

            if (entity.CompareTag("Food"))
                aiContext.NearbyFood.Add(entity);
            else if (entity.CompareTag("Threat"))
                aiContext.NearbyThreats.Add(entity);
            else if (entity.GetType() == GetType()) // Same species
                aiContext.NearbySameSpecies.Add(entity);
        }

        protected virtual void OnDrawGizmos()
        {
            if (!enableGizmos)
                return;

            // Draw detection radius
            Gizmos.color = Color.yellow;
            Gizmos.DrawSphere(transform.position, detectionRadius);

            // Let behavior tree draw debug info
            behaviorTree?.RootNode?.OnDrawGizmos(transform.position);
        }
    }
}
