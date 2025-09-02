using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

namespace Antura.Discover
{
    public class LivingLetterSpawner : MonoBehaviour, ILivingLetterSpawner
    {
        [Header("Prefab & Placement")]
        public EdLivingLetter Prefab;
        public bool UseVolumes = false;
        public List<SpawnVolume> Volumes = new();    // used if UseVolumes = true
        public float SpawnRadius = 12f;              // used if UseVolumes = false
        public float NavMeshMaxSampleDist = 2.0f;

        [Header("Population")]
        public int InitialCount = 24;
        public bool MaintainPopulation = true;
        public int MaintainCount = 24;
        public int HardMaxLive = 64;

        [Header("Ratios")]
        [Range(0f, 1f)] public float LetterRatio { get; set; } = 0.45f;
        [Range(0f, 1f)] public float WordRatio { get; set; } = 0.35f;
        [Range(0f, 1f)] public float CardRatio { get; set; } = 0.20f;

        [Header("Pools (auto-built)")]
        public TopicData Topic;
        public List<string> LetterPool = new();
        public List<string> WordPool = new();
        public List<CardData> CardPool = new();

        [Header("Wander Defaults")]
        public float DefaultWanderRadius = 10f;

        // runtime
        private readonly List<GameObject> _live = new();
        private float _maintainTimer;

        void OnValidate()
        {
            NormalizeRatios();
        }

        void Start()
        {
            if (!Prefab)
            {
                Debug.LogWarning($"[{nameof(LivingLetterSpawner)}] Missing Prefab.", this);
                enabled = false;
                return;
            }

            // If no volumes provided but UseVolumes is on, auto-pick children
            if (UseVolumes && Volumes.Count == 0)
                Volumes.AddRange(GetComponentsInChildren<SpawnVolume>(true));

            // Build pools if Topic is set
            if (Topic)
                BuildPoolsFromTopic(Topic);

            // Initial spawn
            int count = Mathf.Clamp(InitialCount, 0, HardMaxLive);
            for (int i = 0; i < count; i++)
                SpawnOne();
        }

        void Update()
        {
            if (!MaintainPopulation)
                return;
            _maintainTimer += Time.deltaTime;
            if (_maintainTimer < 0.5f)
                return; // check twice per second
            _maintainTimer = 0f;

            int target = Mathf.Clamp(MaintainCount, 0, HardMaxLive);
            while (_live.Count < target)
            {
                if (!SpawnOne())
                    break;
            }

            // Cull nulls (destroyed externally)
            for (int i = _live.Count - 1; i >= 0; i--)
                if (_live[i] == null)
                    _live.RemoveAt(i);
        }

        // === ILivingLetterSpawner API ===

        public void BuildPoolsFromTopic(TopicData topic, string[] extraWords = null, CardData[] extraCards = null)
        {
            Topic = topic;

            // Cards
            CardPool.Clear();
            if (topic)
            {
                var cards = topic.GetAllCards();
                foreach (var c in cards)
                    if (c && !CardPool.Contains(c))
                        CardPool.Add(c);
            }
            if (extraCards != null)
                foreach (var c in extraCards)
                    if (c && !CardPool.Contains(c))
                        CardPool.Add(c);

            // Words (derive from card labels if you don’t have a dedicated Word dataset)
            WordPool.Clear();
            foreach (var c in CardPool)
            {
                string label = GetCardLabel(c);
                if (!string.IsNullOrWhiteSpace(label) && label.Length <= 14)
                    WordPool.Add(label);
            }
            if (extraWords != null)
                foreach (var w in extraWords)
                    if (!string.IsNullOrWhiteSpace(w) && !WordPool.Contains(w))
                        WordPool.Add(w);

            // Letters from words (unique, uppercase)
            var set = new HashSet<char>();
            foreach (var w in WordPool)
                foreach (char ch in w)
                    if (char.IsLetter(ch))
                        set.Add(char.ToUpperInvariant(ch));

            LetterPool.Clear();
            foreach (var ch in set)
                LetterPool.Add(ch.ToString());

            if (LetterPool.Count == 0)
                LetterPool.Add("A"); // safety
            if (WordPool.Count == 0)
                WordPool.Add("Word");
            if (CardPool.Count == 0)
                Debug.LogWarning($"[{nameof(LivingLetterSpawner)}] CardPool is empty after BuildPools.", this);
        }

        public bool SpawnOne()
        {
            if (_live.Count >= HardMaxLive)
                return false;

            // Pick kind
            NormalizeRatios();
            float r = Random.value;
            LivingLetterKind kind =
                (r < LetterRatio) ? LivingLetterKind.Letter :
                (r < LetterRatio + WordRatio) ? LivingLetterKind.Word :
                LivingLetterKind.Card;

            // Pick payload
            string text = "?";
            CardData card = null;

            switch (kind)
            {
                case LivingLetterKind.Letter:
                    text = Pick(LetterPool);
                    break;
                case LivingLetterKind.Word:
                    text = Pick(WordPool);
                    break;
                case LivingLetterKind.Card:
                    card = Pick(CardPool);
                    text = GetCardLabel(card) ?? "Card";
                    break;
            }

            // Pick position on NavMesh
            Vector3 pos = UseVolumes ? RandomFromVolumes() : RandomInsideRadius();
            if (!TrySampleNavMesh(pos, NavMeshMaxSampleDist, out var hit))
            {
                // Try a few fallbacks near the spawner
                for (int i = 0; i < 5 && !TrySampleNavMesh(transform.position, SpawnRadius, out hit); i++)
                { }
                if (hit.position == Vector3.zero)
                    return false;
            }

            // Spawn
            // var inst = Instantiate(Prefab, hit.position, Quaternion.identity, transform);
            // inst.Configure(kind, text, card);

            // // Ensure wanderer
            // var wander = inst.GetComponent<WanderAgent>() ?? inst.gameObject.AddComponent<WanderAgent>();
            // wander.WanderRadius = DefaultWanderRadius;

            //_live.Add(inst.gameObject);
            return true;
        }

        // === Helpers ===

        private void NormalizeRatios()
        {
            float s = LetterRatio + WordRatio + CardRatio;
            if (s <= 0.0001f)
            { LetterRatio = 0.45f; WordRatio = 0.35f; CardRatio = 0.20f; return; }
            LetterRatio /= s;
            WordRatio /= s;
            CardRatio /= s;
        }

        private static T Pick<T>(IList<T> list)
        {
            if (list == null || list.Count == 0)
                return default;
            int i = Random.Range(0, list.Count);
            return list[i];
        }

        private Vector3 RandomInsideRadius()
        {
            var r = Random.insideUnitCircle * SpawnRadius;
            return transform.position + new Vector3(r.x, 0f, r.y);
        }

        private Vector3 RandomFromVolumes()
        {
            if (Volumes == null || Volumes.Count == 0)
                return RandomInsideRadius();

            // choose a volume with room
            SpawnVolume chosen = null;
            int tries = 6;
            while (tries-- > 0)
            {
                var v = Volumes[Random.Range(0, Volumes.Count)];
                if (v && v.Enabled && CountInVolume(v) < v.MaxLive)
                { chosen = v; break; }
            }
            if (!chosen)
                chosen = Volumes[Random.Range(0, Volumes.Count)];
            return chosen ? chosen.RandomPointInside() : RandomInsideRadius();
        }

        private int CountInVolume(SpawnVolume v)
        {
            int c = 0;
            Vector3 p = v.transform.position;
            float r2 = v.Radius * v.Radius;
            for (int i = _live.Count - 1; i >= 0; i--)
            {
                var go = _live[i];
                if (!go)
                { _live.RemoveAt(i); continue; }
                if ((go.transform.position - p).sqrMagnitude <= r2)
                    c++;
            }
            return c;
        }

        private static bool TrySampleNavMesh(Vector3 point, float maxDistance, out NavMeshHit hit)
        {
            return NavMesh.SamplePosition(point, out hit, maxDistance, NavMesh.AllAreas);
        }

        private string GetCardLabel(CardData card)
        {
            if (!card)
                return null;
            // Try common fields via reflection to avoid tight coupling
            string s = TryGetString(card, "LocalizedName")
                    ?? TryGetString(card, "TextEn")
                    ?? TryGetString(card, "Name")
                    ?? card.name;
            return s;
        }

        private string TryGetString(object o, string member)
        {
            if (o == null)
                return null;
            var t = o.GetType();
            const System.Reflection.BindingFlags F =
                System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public | System.Reflection.BindingFlags.NonPublic;

            var f = t.GetField(member, F);
            if (f != null && f.FieldType == typeof(string))
                return (string)f.GetValue(o);

            var p = t.GetProperty(member, F);
            if (p != null && p.PropertyType == typeof(string) && p.CanRead)
                return (string)p.GetValue(o);

            return null;
        }

        // Editor convenience
        [ContextMenu("Spawn 1")]
        private void CM_SpawnOne() => SpawnOne();
    }
}
