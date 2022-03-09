using Antura.Minigames;
using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class SlingshotController : MonoBehaviour
    {
        public static SlingshotController instance;

        public SpringController spring;
        public BallController ball;
        public GameObject arc;
        public GameObject center;

        // The point of impact, calculated in real-time:
        private Vector3 pointOfImpact;

        // The force exerted on the ball at launch, calculated in real-time:
        private Vector3 launchForce;

        // The elasticity. The higher the value, the more the ball travels for a fixed tug at the slingshot.
        //private float elasticity = SROptions.Current.Elasticity;

        void Awake()
        {
            instance = this;
        }

        void Start()
        {
            Vector3 centerPosition = center.transform.position;
            float cameraDistance = Mathf.Abs(Camera.main.transform.position.z - centerPosition.z);
            cameraDistance = 26f;
            centerPosition.y = Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height / 3, cameraDistance)).y;
            center.transform.position = centerPosition;

            pointOfImpact.y = GroundController.instance.transform.position.y;

            arc.SetActive(false);


            IInputManager inputManager = ThrowBallsConfiguration.Instance.Context.GetInputManager();
            inputManager.onPointerDrag += OnPointerDrag;
            inputManager.onPointerUp += OnPointerUp;

            foreach (Collider collider in GetComponentsInChildren<Collider>())
            {
                collider.enabled = false;
            }
        }

        public Vector3 INITIAL_BALL_POSITION = new Vector3(0, 5.25f, -20f);
        //private float yzStretchRange = 3f;
        public bool dragging = false;
        private void OnPointerDrag()
        {
            if (ball.IsDragging())
            {
                dragging = true;
                Vector2 lastTouch = ThrowBallsConfiguration.Instance.Context.GetInputManager().LastPointerPosition;

                float xInput = ((Screen.width * 0.5f - lastTouch.x) / Screen.height) * 2;
                float yInput = ((lastTouch.y - Camera.main.WorldToScreenPoint(center.transform.position).y) / Screen.height) * 1.8f;

                if (!spring.Released)
                {
                    spring.angle = Mathf.Clamp(xInput * 90, -60, 60);
                    spring.t = Mathf.Clamp(yInput, -1, 0);
                }
            }
        }

        private void OnPointerUp()
        {
            dragging = false;
            if (ball.IsDragging() && IsMinimalInput())
            {
                Vector3 forceToApply = GetLaunchForce();
                ball.Launch(forceToApply);
                Catapult.instance.DisableCollider();
            }
            else
            {
                ball.CancelDragging();
            }
        }

        bool IsMinimalInput()
        {
            return spring.t < -0.085f;
        }

        void Update()
        {
            spring.Released = !(ball.IsAnchored() || ball.IsDragging());

            if (!spring.Released)
            {
                if (!dragging && ball.IsAnchored())
                    spring.t = Mathf.Lerp(spring.t, 0, 6 * Time.deltaTime);

                ball.transform.position = spring.Slot.position;

                if (IsMinimalInput())
                {
                    ArrowBodyController.instance.Enable();
                    ArrowHeadController.instance.Enable();
                }
                else
                {
                    ArrowBodyController.instance.Disable();
                    ArrowHeadController.instance.Disable();
                }
            }
            else
            {
                ArrowBodyController.instance.Disable();
                ArrowHeadController.instance.Disable();
            }
        }

        void FixedUpdate()
        {

            if (!BallController.instance.IsLaunched())
            {
                UpdateLaunchForce();
                UpdatePointOfImpact();
                UpdateArrow();
            }
        }

        private void UpdateArrow()
        {
            Vector3 projectedCenter = center.transform.position;
            projectedCenter.y = GroundController.instance.transform.position.y;

            Vector3 direction = pointOfImpact - projectedCenter;

            ArrowBodyController.instance.OnUpdateDistance(direction);
            ArrowHeadController.instance.OnUpdate();
        }

        private void UpdateLaunchForce()
        {
            Vector3 ballPosition = ball.transform.position;

            launchForce = center.transform.position - ballPosition;
            //launchForce *= SROptions.Current.Elasticity;
            // FIX updated with fixed value because we can't refer to SRdebugger options in runtime!
            launchForce *= 19f;
        }

        private void UpdatePointOfImpact()
        {
            Vector3 ballPosition = BallController.instance.transform.position;

            float yVelocity = launchForce.y;

            float velocityFactor = (-1 * yVelocity) - Mathf.Sqrt(Mathf.Pow(yVelocity, 2) - (2 * (ballPosition.y - pointOfImpact.y) * Constants.GRAVITY.y));
            velocityFactor = Mathf.Pow(velocityFactor, -1);
            velocityFactor *= Constants.GRAVITY.y;

            pointOfImpact.z = (launchForce.z / velocityFactor) + ballPosition.z;
            pointOfImpact.x = (launchForce.x / velocityFactor) + ballPosition.x;
        }

        public Vector3 GetSlingshotCenterPosition()
        {
            return center.transform.position;
        }

        private void UpdateArc()
        {
            if (BallController.instance.transform.position == BallController.instance.INITIAL_BALL_POSITION)
            {
                arc.SetActive(false);
            }
            else
            {
                arc.SetActive(true);

                Vector3 ballPosition = ball.transform.position;

                Vector3 hypotheticalPeakPosition = new Vector3();
                hypotheticalPeakPosition.y = -0.5f * Mathf.Pow(launchForce.y, 2) * Constants.GRAVITY_INVERSE.y + ballPosition.y;
                hypotheticalPeakPosition.z = (-launchForce.y * Constants.GRAVITY_INVERSE.y) * launchForce.z + ballPosition.z;
                hypotheticalPeakPosition.x = (-launchForce.y * Constants.GRAVITY_INVERSE.y) * launchForce.x + ballPosition.x;

                Vector3 hypotheticalArcStart = new Vector3();
                hypotheticalArcStart.y = pointOfImpact.y;
                hypotheticalArcStart.z = -pointOfImpact.z + 2 * hypotheticalPeakPosition.z;

                // To find the start X of the arc, we need to find the equation of the circle
                // passing through the 3 points: Ball position, peak, and point of impact.

                // We begin by determining the center of the arc. The center is the intersection
                // point of three planes: the bisector plane of a segment formed by two points,
                // the bisector plane of a segment formed by a different pair of points, and
                // the plane formed by the three points themselves.

                Vector3 plane1Normal = hypotheticalPeakPosition - ballPosition;
                float plane1CstFactor = plane1Normal.x * (hypotheticalPeakPosition.x + ballPosition.x) / 2
                                            + plane1Normal.y * (hypotheticalPeakPosition.y + ballPosition.y) / 2
                                                  + plane1Normal.z * (hypotheticalPeakPosition.z + ballPosition.z) / 2;

                Vector3 plane2Normal = hypotheticalPeakPosition - pointOfImpact;
                float plane2CstFactor = plane2Normal.x * (hypotheticalPeakPosition.x + pointOfImpact.x) / 2
                                            + plane2Normal.y * (hypotheticalPeakPosition.y + pointOfImpact.y) / 2
                                                  + plane2Normal.z * (hypotheticalPeakPosition.z + pointOfImpact.z) / 2;

                Vector3 plane3Normal = Vector3.Cross(hypotheticalPeakPosition - ballPosition, ballPosition - pointOfImpact);
                float plane3CstFactor = plane3Normal.x * (pointOfImpact.x)
                                            + plane3Normal.y * (pointOfImpact.y)
                                                  + plane3Normal.z * (pointOfImpact.z);

                Vector3 arcCenter = (plane1CstFactor * Vector3.Cross(plane2Normal, plane3Normal)
                                        + plane2CstFactor * Vector3.Cross(plane3Normal, plane1Normal)
                                            + plane3CstFactor * Vector3.Cross(plane1Normal, plane2Normal))
                    / (Vector3.Dot(plane1Normal, Vector3.Cross(plane2Normal, plane3Normal)));

                float radius = (arcCenter - pointOfImpact).magnitude;

                hypotheticalArcStart.x = -pointOfImpact.x + 2 * arcCenter.x;

                Vector3 arcDistance = new Vector3(hypotheticalArcStart.x - pointOfImpact.x, 0, hypotheticalArcStart.z - pointOfImpact.z);

                float xScale = arcDistance.magnitude * 0.5f;
                float yScale = 15;
                float zScale = hypotheticalPeakPosition.y;

                arc.transform.localScale = new Vector3(xScale, yScale, zScale);

                Vector3 ballToPointOfImpactDistance = new Vector3(ballPosition.x - pointOfImpact.x, 0, ballPosition.z - pointOfImpact.z);
                float yAngle = Vector3.Angle(ballToPointOfImpactDistance, Vector3.right);

                arc.transform.rotation = Quaternion.Euler(90, yAngle, 0);

                Vector3 arcPosition = arc.transform.position;
                arcPosition.x = (ballPosition.x + pointOfImpact.x) / 2;
                arcPosition.z = (ballPosition.z + pointOfImpact.z) / 2;
                arcPosition.y = pointOfImpact.y;
                arc.transform.position = arcPosition;
            }

        }

        public Vector3 GetLaunchForce()
        {
            return launchForce;
        }

        public void Enable()
        {
            gameObject.SetActive(true);
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }
    }
}
