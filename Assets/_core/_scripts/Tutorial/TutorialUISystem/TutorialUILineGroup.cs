using System.Collections.Generic;
using Antura.EditorUtilities;
using UnityEngine;

namespace Antura.Tutorial
{
    public class TutorialUILineGroup : MonoBehaviour
    {
        public float MinPointsDistance = 0.1f;

        // Serialized because fucking Unity doesn't have a GetWidths method for LineRenderers
        public LineRenderer[] Lines;

        public float[] DefStartWidths, DefEndWidths;

        readonly List<Vector3> wps = new List<Vector3>();
        Vector3[] wpsArray;
        bool initialized;
        int[] defSortingOrder;

        #region Unity + Init

        void Init()
        {
            if (initialized)
            { return; }

            initialized = true;

            int count = Lines.Length;
            defSortingOrder = new int[count];
            for (int i = 0; i < count; ++i)
            {
                LineRenderer lr = Lines[i];
                SortingOrder3D sort = lr.GetComponent<SortingOrder3D>();
                if (sort != null)
                { defSortingOrder[i] = sort.SortingOrder; }
                else
                { defSortingOrder[i] = lr.GetComponent<Renderer>().sortingOrder; }
            }
        }

        void Awake()
        {
            Init();
        }

        #endregion

        #region Public Methods

        public void Spawn(Vector3 _position, bool _overlayed)
        {
            Init();
            wps.Add(_position);
            this.gameObject.SetActive(true);
            this.transform.position = _position;
            this.transform.rotation = Quaternion.identity;
            for (int i = 0; i < Lines.Length; ++i)
            {
                LineRenderer lr = Lines[i];
                lr.startWidth = TutorialUI.I.Cam.fieldOfView * DefStartWidths[i] / 45f;
                lr.endWidth = TutorialUI.I.Cam.fieldOfView * DefEndWidths[i] / 45f;
                lr.sortingOrder = _overlayed ? defSortingOrder[i] : 0;
            }
        }

        public void Despawn()
        {
            this.gameObject.SetActive(false);
            wps.Clear();
            foreach (LineRenderer lr in Lines)
            {
                lr.positionCount = 0;
                lr.SetPositions(new Vector3[0]);
            }
        }

        public void AddPosition(Vector3 _position)
        {
            if (wps.Count > 0 && Vector3.Distance(_position, wps[wps.Count - 1]) < MinPointsDistance)
            { return; }

            wps.Add(_position);
            wpsArray = wps.ToArray();
            int count = wps.Count;
            foreach (LineRenderer lr in Lines)
            {
                lr.positionCount = count;
                lr.SetPositions(wpsArray);
            }
        }

        #endregion
    }
}
