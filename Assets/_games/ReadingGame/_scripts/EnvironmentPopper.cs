using UnityEngine;
using System.Collections.Generic;
using Antura.Minigames;

namespace Antura.Minigames.ReadingGame
{
    public class EnvironmentPopper : MonoBehaviour
    {
        public GameObject poofPrefab;
        public List<GameObject> toPop;
        public ReadingGameGame game;

        int popCount = 0;
        int popped = 0;

        void Start()
        {
            for (int i = 0; i < toPop.Count; ++i)
                toPop[i].SetActive(false);

            popCount = toPop.Count;
        }

        void Update()
        {
            float t = (game.CurrentScore / (float)game.GetStarsThreshold(3));

            int mustBePopped = Mathf.Min(Mathf.RoundToInt(t * popCount), popCount);

            for (int i = popped; i < mustBePopped; ++i)
            {
                toPop[i].SetActive(true);
                ++popped;

                var puffGo = GameObject.Instantiate(poofPrefab);
                puffGo.AddComponent<AutoDestroy>().duration = 2;
                puffGo.SetActive(true);
                var toPopTransform = toPop[i].transform;
                var position = toPopTransform.position + toPopTransform.up * 5 + toPopTransform.forward * 2;
                puffGo.transform.position = position;
                puffGo.transform.localScale *= 5f;
            }
        }
    }
}
