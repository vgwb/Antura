using System;
using UnityEngine;
using DG.Tweening;

namespace Antura.Minigames.Egg
{
    public class EggPiece : MonoBehaviour
    {
        public Action onPoofEnd;
        public MeshRenderer pieceRenderer;
        public Rigidbody eggRigidbody;
        public MeshCollider meshCollider;

        public GameObject poofPrefab;

        private Vector3 poofRight = new Vector3(10f, 10f);
        private Vector3 poofLeft = new Vector3(-10f, 10f);

        private bool landed = true;
        private const float landedTime = 1f;
        private float landedTimer = 0f;

        private bool poofed = false;

        private bool poofDirRight = true;

        private bool smoke = false;
        const float smokeTime = 2f;
        private float smokeTimer = 0f;

        private Tween shakeTween;

        public void Reset()
        {
            meshCollider.enabled = false;
            eggRigidbody.useGravity = false;
            eggRigidbody.velocity = Vector3.zero;
            eggRigidbody.isKinematic = true;

            gameObject.SetActive(true);

            transform.localPosition = Vector3.zero;
            transform.localEulerAngles = new Vector3(-90f, 180f);

            pieceRenderer.enabled = true;

            poofed = false;

            landed = true;
            landedTimer = 0f;

            smoke = false;
            smokeTimer = 0f;
        }

        public void Shake()
        {
            if (shakeTween != null)
            {
                shakeTween.Kill();
            }

            shakeTween = transform.DOShakePosition(0.2f, 0.02f, 20, 180f).OnComplete(delegate ()
            { transform.localPosition = Vector3.zero; });
        }

        public void Poof(bool poofDirRight)
        {
            if (!poofed)
            {
                if (shakeTween != null)
                {
                    shakeTween.Kill();
                }

                poofed = true;

                this.poofDirRight = poofDirRight;

                MoveAndPoof();
            }
        }

        void MoveAndPoof()
        {
            eggRigidbody.isKinematic = false;
            meshCollider.enabled = true;
            eggRigidbody.useGravity = true;

            eggRigidbody.velocity = poofDirRight ? poofRight : poofLeft;

            landed = false;
            landedTimer = landedTime;
        }

        void Update()
        {
            if (!landed)
            {
                landedTimer -= Time.deltaTime;

                if (landedTimer <= 0f)
                {
                    landed = true;

                    eggRigidbody.isKinematic = true;

                    pieceRenderer.enabled = false;

                    StartSmoke();
                }
            }

            if (smoke)
            {
                smokeTimer -= Time.deltaTime;

                if (smokeTimer <= 0f)
                {
                    smoke = false;
                    OnSmokeEnd();
                }
            }
        }

        void StartSmoke()
        {
            smoke = true;

            smokeTimer = smokeTime;

            var poofGo = Instantiate(poofPrefab);
            poofGo.AddComponent<AutoDestroy>().duration = smokeTime;

            ChengeGameObjectLayer(poofGo);

            poofGo.SetActive(true);

            poofGo.transform.SetParent(transform);
            poofGo.transform.localPosition = new Vector3(0f, -0.1f, 0f);
        }

        void ChengeGameObjectLayer(GameObject go)
        {
            go.layer = LayerMask.NameToLayer("Ball");

            int childCount = go.transform.childCount;

            if (childCount > 0)
            {
                for (int i = 0; i < childCount; i++)
                {
                    ChengeGameObjectLayer(go.transform.GetChild(i).gameObject);
                }
            }
        }

        void OnSmokeEnd()
        {
            if (onPoofEnd != null)
            {
                onPoofEnd();
            }
        }
    }
}
