using UnityEngine;

namespace Antura.Minigames.ThrowBalls
{
    public class CratePileController : MonoBehaviour
    {
        public CrateController bottomCrate;
        public CrateController middleCrate;
        public CrateController topCrate;

        public LetterController letter;

        public bool hit = false;

        void Start()
        {
        }

        void Update()
        {
        }

        public void OnSwerveUpdate(CrateController crate, float rotateByAngle, Vector3 rotationPivot, Vector3 zVector)
        {
            if (crate == topCrate)
            {
                letter.transform.RotateAround(rotationPivot, zVector, rotateByAngle);
            }
        }

        public void OnCrateHit(CrateController crate)
        {
            if (!hit)
            {
                ThrowBallsConfiguration.Instance.Context.GetAudioManager().PlaySound(Sfx.CrateLandOnground);

                crate.Launch(new Vector3(0, 0, 1), 30);

                middleCrate.ApplyCustomGravity();
                middleCrate.SetIsKinematic(false);
                middleCrate.VanishAfterDelay(0.7f);
                if (middleCrate.IsSwerving())
                {
                    middleCrate.StopSwerving();
                }

                topCrate.ApplyCustomGravity();
                topCrate.SetIsKinematic(false);
                topCrate.VanishAfterDelay(0.9f);
                if (topCrate.IsSwerving())
                {
                    topCrate.StopSwerving();
                }

                bottomCrate.ApplyCustomGravity();
                bottomCrate.SetIsKinematic(false);
                bottomCrate.VanishAfterDelay(1.1f);
                if (bottomCrate.IsSwerving())
                {
                    bottomCrate.StopSwerving();
                }

                if (letter.IsJumping())
                {
                    letter.StopJumping();
                }

                letter.JumpOffOfCrate();

                BallController.instance.DampenVelocity();
                hit = true;
            }
        }

        public void SetSwerving()
        {
            Vector3 pivot = bottomCrate.transform.position;

            Vector3 leftPivot = bottomCrate.transform.position;
            leftPivot.x += -1.6f;
            leftPivot.y += -1.6f;

            Vector3 rightPivot = bottomCrate.transform.position;
            rightPivot.x += 1.6f;
            rightPivot.y += -1.6f;

            float swervePhase = Random.Range(-360f, 360f);

            topCrate.SetSwerving(leftPivot, rightPivot, 1.5f, swervePhase);
            middleCrate.SetSwerving(leftPivot, rightPivot, 1.25f, swervePhase);
            bottomCrate.SetSwerving(leftPivot, rightPivot, 1f, swervePhase);
        }

        public void Reset()
        {
            bottomCrate.Reset();
            middleCrate.Reset();
            topCrate.Reset();

            Vector3 letterPos = letter.transform.position;

            bottomCrate.transform.position = new Vector3(letterPos.x + Random.Range(-0.4f, 0.4f), letterPos.y - 8.05f, letterPos.z + Random.Range(-0.4f, 0.4f));
            middleCrate.transform.position = new Vector3(letterPos.x + Random.Range(-0.4f, 0.4f), letterPos.y - 4.85f, letterPos.z + Random.Range(-0.4f, 0.4f));
            topCrate.transform.position = new Vector3(letterPos.x + Random.Range(-0.4f, 0.4f), letterPos.y - 1.65f, letterPos.z + Random.Range(-0.4f, 0.4f));

            letterPos.x = topCrate.transform.position.x;
            letterPos.z = topCrate.transform.position.z;

            letter.transform.position = letterPos;

            hit = false;
        }

        public void Disable()
        {
            gameObject.SetActive(false);
        }

        public void Enable()
        {
            gameObject.SetActive(true);

            bottomCrate.Enable();
            middleCrate.Enable();
            topCrate.Enable();
        }
    }
}
