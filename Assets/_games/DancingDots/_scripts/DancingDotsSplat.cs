using Antura.Audio;
using UnityEngine;

namespace Antura.Minigames.DancingDots
{
    public class DancingDotsSplat : MonoBehaviour
    {

        public Color[] colors;

        // Use this for initialization
        void Start()
        {
            GetComponent<SpriteRenderer>().color = colors[Random.Range(0, colors.Length)];
            AudioManager.I.PlaySound(Sfx.Splat);
        }

        public void CleanSplat()
        {
            Destroy(gameObject, 0.25f);
        }

        //		public float minAlpha = 0.25f;
        //		public float fadeDuration = 3f;
        //
        //		IEnumerator FadeTo(float aValue, float aTime)
        //		{
        //			float alpha = goRenderer.color.a;
        //			for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / aTime)
        //			{
        //				Color newColor = new Color(1, 1, 1, Mathf.Lerp(alpha,aValue,t));
        //				goRenderer.color = newColor;
        //				yield return null;
        //			}
        //		}
        //
        //		public float splatMaxSize = 5.0f;
        //		public float splatMaxY = -22;
        //		public float splatGrowFactor = 5f;
        //		public float splatSlideFactor = 10f;
        //		public float splatWaitTime = 1f;
        //
        //		IEnumerator AnimateSplat(Transform trans)
        //		{
        //
        //
        //			float timer = 0;
        //
        //			AudioManager.I.PlaySound(Sfx.Splat);
        //
        //			// Scale
        //			while(splatMaxSize > trans.localScale.x)
        //			{
        //				timer += Time.deltaTime;
        //				trans.localScale += Vector3.one * Time.deltaTime * splatGrowFactor;
        //				yield return null;
        //			}
        //
        //
        //			yield return new WaitForSeconds(splatWaitTime);
        //
        //			//Slide
        //			while(splatMaxY < trans.position.y)
        //			{
        //				timer += Time.deltaTime;
        //				trans.position -= new Vector3(0, Time.deltaTime * splatSlideFactor, 0);
        //				yield return null;
        //			}
        //
        //			Destroy(trans.gameObject);
        //
        //		}


    }
}
