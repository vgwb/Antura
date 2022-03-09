using Antura.EditorUtilities;
using DG.Tweening;
using UnityEngine;

namespace Antura.Tutorial
{
    /// <summary>
    /// Can be a single trail, or a collection of multiple trails
    /// </summary>
    public class TutorialUITrailGroup : MonoBehaviour
    {
        internal float Time { get; private set; } // Highest trail time between all trails
        internal TrailRenderer[] Trails;
        bool initialized;
        float[] defStartWidths, defEndWidths;
        int[] defSortingOrder;
        Vector3 lastPos;
        bool isWaitingToDespawn;
        Tween waitingTween;

        #region Unity + Init

        void Init()
        {
            if (initialized)
            { return; }

            initialized = true;

            Trails = this.GetComponentsInChildren<TrailRenderer>(true);
            int count = Trails.Length;
            defStartWidths = new float[count];
            defEndWidths = new float[count];
            defSortingOrder = new int[count];
            for (int i = 0; i < count; ++i)
            {
                TrailRenderer tr = Trails[i];
                if (Time < tr.time)
                { Time = tr.time; }
                defStartWidths[i] = tr.startWidth;
                defEndWidths[i] = tr.endWidth;
                SortingOrder3D sort = tr.GetComponent<SortingOrder3D>();
                if (sort != null)
                { defSortingOrder[i] = sort.SortingOrder; }
                else
                { defSortingOrder[i] = tr.GetComponent<Renderer>().sortingOrder; }
            }
        }

        void Awake()
        {
            Init();
        }

        void OnDestroy()
        {
            waitingTween.Kill();
        }

        void LateUpdate()
        {
            if (isWaitingToDespawn)
            { return; }

            if (lastPos - this.transform.position == Vector3.zero)
            {
                isWaitingToDespawn = true;
                waitingTween = DOVirtual.DelayedCall(Time, Despawn, false);
            }
            else
            {
                lastPos = this.transform.position;
            }
        }

        #endregion

        #region Public Methods

        public void Spawn(Vector3 _position, bool _overlayed)
        {
            Init();
            this.gameObject.SetActive(true);
            this.transform.position = _position;
            this.transform.rotation = Quaternion.identity;
            lastPos = _position - Vector3.one;
            for (int i = 0; i < Trails.Length; ++i)
            {
                TrailRenderer tr = Trails[i];
                tr.startWidth = defStartWidths[i] * TutorialUI.GetCameraBasedScaleMultiplier(_position);
                tr.endWidth = defEndWidths[i] * TutorialUI.GetCameraBasedScaleMultiplier(_position);
                tr.sortingOrder = _overlayed ? defSortingOrder[i] : 0;
                tr.Clear();
            }
        }

        public void Despawn()
        {
            isWaitingToDespawn = false;
            waitingTween.Kill();
            foreach (TrailRenderer tr in Trails)
            { tr.Clear(); }
            this.gameObject.SetActive(false);
        }

        #endregion
    }
}
