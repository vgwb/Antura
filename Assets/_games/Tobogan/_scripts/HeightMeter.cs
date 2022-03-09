using UnityEngine;
using System.Collections.Generic;

namespace Antura.Minigames.Tobogan
{
    public class HeightMeter : MonoBehaviour
    {
        public GameObject dotPrefab;
        public Material dotsMaterial;
        public GameObject heightBar;

        public float targetHeight;
        float height;
        float lastHeight = 0;

        public float dotsDistance = 1.0f;
        float lastDotsDistance = 0;

        float animationPosition = 0;
        public float targetAnimationSpeed = 5.0f;

        List<GameObject> dots = new List<GameObject>();

        void Awake()
        {
            // Instantiate a runtime material
            dotsMaterial = new Material(dotsMaterial);
        }

        void Update()
        {
            float animationSpeed = targetAnimationSpeed;

            if (Mathf.Abs(height - targetHeight) > 0.1f)
            {
                animationSpeed *= 3;

                height += animationSpeed * Time.deltaTime * Mathf.Sign(targetHeight - height) * Mathf.Min(1, Mathf.Abs(height - targetHeight));
            }

            heightBar.transform.localPosition = Vector3.up * height;

            if (height != lastHeight || dotsDistance != lastDotsDistance)
            {
                int neededDots = Mathf.RoundToInt(height / dotsDistance) + 2;

                if (neededDots < dots.Count)
                {
                    // Need to remove some
                    for (int i = dots.Count - 1; i > neededDots; --i)
                    {
                        Destroy(dots[i]);
                        dots.RemoveAt(i);
                    }
                }
                else if (neededDots > dots.Count)
                {
                    // Need to add some
                    for (int i = dots.Count; i <= neededDots; ++i)
                    {
                        var newDot = Instantiate(dotPrefab);
                        newDot.SetActive(true);
                        newDot.GetComponent<MeshRenderer>().material = dotsMaterial;
                        newDot.transform.SetParent(transform);
                        dots.Add(newDot);
                    }
                }

                dotsMaterial.SetFloat("_Base", transform.position.y);
                dotsMaterial.SetFloat("_Height", height);

                lastHeight = height;
                lastDotsDistance = dotsDistance;
            }

            // Animate
            for (int i = 0; i < dots.Count; ++i)
            {
                dots[i].transform.localPosition = Vector3.up * ((i - 1) * dotsDistance + animationPosition);
            }

            animationPosition = Mathf.Repeat(animationPosition + animationSpeed * Time.deltaTime, dotsDistance);
        }
    }
}
