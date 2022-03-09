using DG.Tweening;
using UnityEngine;

namespace Antura.Tutorial
{
    /// <summary>
    /// Tutorial framework
    /// </summary>
    [RequireComponent(typeof(TutorialUIPools))]
    public class TutorialUI : MonoBehaviour
    {
        public enum DrawLineMode
        {
            LineOnly,
            Finger,
            Arrow,
            FingerAndArrow
        }

        public enum MarkSize
        {
            Normal,
            Big,
            Huge
        }

        [Tooltip("In units x second")]
        public float DrawSpeed = 2;

        [Header("References")]
        public TutorialUIFinger Finger;

        public TutorialUIPools Pools;

        internal static TutorialUI I;
        internal Camera Cam;
        internal Transform CamT;
        const string ResourcePath = "Prefabs/UI/TutorialUI";
        const string TweenId = "TutorialUI";
        Transform currMovingTarget;

        #region Unity

        void Awake()
        {
            I = this;
            if (Cam == null)
            {
                Cam = Camera.main;
                CamT = Cam.transform;
                var tutorialMask = 1 << LayerMask.NameToLayer("TutorialUI");
                Cam.cullingMask = I.Cam.cullingMask | tutorialMask;
            }
        }

        void OnDestroy()
        {
            if (I == this)
            { I = null; }
            DOTween.Kill(TweenId);
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Removes and tutorial element on screen
        /// </summary>
        /// <param name="_destroy">If TRUE, also destroys the TutorialUI gameObject</param>
        public static void Clear(bool _destroy)
        {
            if (I == null)
            { return; }

            if (_destroy)
            {
                Destroy(I.gameObject);
            }
            else
            {
                DOTween.Kill(TweenId);
                I.Finger.Hide(true);
                I.Pools.DespawnAll();
            }
        }

        public static void SetCamera(Camera _camera)
        {
            Init();

            var tutorialMask = 1 << LayerMask.NameToLayer("TutorialUI");
            if (I.Cam != null)
            {
                I.Cam.cullingMask = I.Cam.cullingMask & ~tutorialMask;
            }

            I.Cam = _camera;
            I.CamT = I.Cam.transform;
            I.Cam.cullingMask = I.Cam.cullingMask | tutorialMask;
        }

        /// <summary>
        /// Shows a finger click at the given world position
        /// </summary>
        /// <param name="_position">World position</param>
        public static void Click(Vector3 _position)
        {
            Init();
            I.Finger.Click(I.transform, _position);
        }

        /// <summary>
        /// Shows a repeated finger click at the given world position
        /// </summary>
        /// <param name="_position">World position</param>
        /// <param name="_duration">Duration of the repeat</param>
        /// <param name="_clicksPerSecond">Click per second</param>
        public static void ClickRepeat(Vector3 _position, float _duration = 2, float _clicksPerSecond = 5)
        {
            Init();
            I.Finger.ClickRepeat(I.transform, _position, _duration, _clicksPerSecond);
        }

        /// <summary>
        /// Draws a straight line with the given options.
        /// <para>NOTE: you can chain an OnComplete method call to get a callback when the line has finished drawing</para>
        /// </summary>
        /// <param name="_from">Starting world position</param>
        /// <param name="_to">Ending world position</param>
        /// <param name="_mode">Draw mode (line only, finger, arrow, arrow + finger)</param>
        /// <param name="_persistent">If TRUE, the line will stay on screen until you <see cref="Clear"/> the TutorialUI,
        /// otherwise it will disappear automatically</param>
        /// <param name="_overlayed">If TRUE the line will always appear above other world elements,
        /// otherwise it will behave as a regular world object</param>
        public static TutorialUIAnimation DrawLine(Vector3 _from, Vector3 _to, DrawLineMode _mode, bool _persistent = false,
            bool _overlayed = true)
        {
            Init();
            return I.DoDrawLine(new[] { _from, _to }, PathType.Linear, _mode, _persistent, _overlayed);
        }

        /// <summary>
        /// Draws a curved line with the given options.
        /// <para>NOTE: you can chain an OnComplete method call to get a callback when the line has finished drawing</para>
        /// </summary>
        /// <param name="_path">A series of waypoints (world positions) between which the line will pass.
        /// IMPORTANT: the line drawn between the waypoints will use a CatmullRom curve, so you don't need too many waypoint to actually draw a curve</param>
        /// <param name="_mode">Draw mode (line only, finger, arrow, arrow + finger)</param>
        /// <param name="_persistent">If TRUE, the line will stay on screen until you <see cref="Clear"/> the TutorialUI,
        /// otherwise it will disappear automatically</param>
        /// <param name="_overlayed">If TRUE the line will always appear above other world elements,
        /// otherwise it will behave as a regular world object</param>
        public static TutorialUIAnimation DrawLine(Vector3[] _path, DrawLineMode _mode, bool _persistent = false, bool _overlayed = true)
        {
            Init();
            return I.DoDrawLine(_path, PathType.CatmullRom, _mode, _persistent, _overlayed);
        }

        /// <summary>
        /// Shows a YES/RIGHT mark at the given world position
        /// </summary>
        /// <param name="_position">World position</param>
        /// <param name="_size">Size of the mark</param>
        public static void MarkYes(Vector3 _position, MarkSize _size = MarkSize.Normal)
        {
            Init();
            I.Pools.SpawnMarkYes(I.transform, _position, _size);
        }

        /// <summary>
        /// Shows a NO/WRONG mark at the given world position
        /// </summary>
        /// <param name="_position">World position</param>
        /// <param name="_size">Size of the mark</param>
        public static void MarkNo(Vector3 _position, MarkSize _size = MarkSize.Normal)
        {
            Init();
            I.Pools.SpawnMarkNo(I.transform, _position, _size);
        }

        #endregion

        #region Methods

        static void Init()
        {
            if (I != null)
            { return; }

            GameObject go = Instantiate(Resources.Load<GameObject>(ResourcePath));
            go.name = "[TutorialUI]";
        }

        TutorialUIAnimation DoDrawLine(Vector3[] _path, PathType _pathType, DrawLineMode _mode, bool _persistent, bool _overlayed)
        {
            bool hasFinger = _mode == DrawLineMode.Finger || _mode == DrawLineMode.FingerAndArrow;
            bool hasArrow = _mode == DrawLineMode.Arrow || _mode == DrawLineMode.FingerAndArrow;
            TutorialUIProp arrow = null;
            Vector3 startPos = _path[0];

            TutorialUILineGroup lr = null;
            TutorialUITrailGroup tr = null;
            if (_persistent)
            {
                lr = Pools.SpawnLineGroup(this.transform, startPos, _overlayed);
                currMovingTarget = lr.transform;
            }
            else
            {
                tr = Pools.SpawnTrailGroup(this.transform, startPos, _overlayed);
                currMovingTarget = tr.transform;
            }

            if (hasFinger)
            { Finger.Show(currMovingTarget, startPos); }
            if (hasArrow)
            { arrow = Pools.SpawnArrow(this.transform, startPos, _overlayed); }

            float actualDrawSpeed = DrawSpeed * GetCameraBasedScaleMultiplier(_path[0]);
            TweenParams parms = TweenParams.Params.SetSpeedBased().SetEase(Ease.OutSine).SetId(TweenId);

            Tween mainTween = currMovingTarget.DOPath(_path, actualDrawSpeed, _pathType).SetAs(parms);
            if (_persistent)
            {
                mainTween.OnUpdate(() => lr.AddPosition(lr.transform.position));
                mainTween.OnStepComplete(() =>
                {
                    if (hasFinger && lr.transform == currMovingTarget)
                    { Finger.Hide(); }
                });
            }
            else
            {
                mainTween.OnStepComplete(() =>
                {
                    if (hasFinger && tr.transform == currMovingTarget)
                    { Finger.Hide(); }
                });
            }

            if (hasArrow)
            {
                Tween t = arrow.transform.DOPath(_path, actualDrawSpeed, _pathType).SetLookAt(0.01f).SetAs(parms);
                if (!_persistent)
                {
                    t.OnComplete(() => { DOVirtual.DelayedCall(Mathf.Max(tr.Time - 0.2f, 0), () => arrow.Hide(), false).SetId(TweenId); });
                }
            }

            return new TutorialUIAnimation(mainTween);
        }

        #endregion

        #region Helpers

        public static float GetCameraBasedScaleMultiplier(Vector3 _position)
        {
            if (I.Cam.orthographic)
            {
                return I.Cam.orthographicSize / 5.0f;
            }

            var dist = Vector3.Distance(_position, I.CamT.position);
            dist = Mathf.Clamp(dist, 0, 300);
            return (dist / 20) * (I.Cam.fieldOfView / 45f);
        }

        #endregion
    }
}
