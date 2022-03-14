using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

namespace Antura.Minigames.TakeMeHome
{
    public class TakeMeHomeSpwanTube : MonoBehaviour
    {


        public List<GameObject> tubes;

        void Start()
        {

        }

        public void show(tubeColor tColor)
        {
            StartCoroutine(coShow(tColor));
        }
        public IEnumerator coShow(tubeColor tColor)
        {
            string c = tColor.ToString().ToLower();

            GameObject t = tubes.Find(x => { return x.gameObject.name.ToLower().Contains(c); });

            t.SetActive(true);
            transform.DOMoveY(5.18f, 0.5f);

            yield return new WaitForSeconds(1);

            transform.DOMoveY(9.64f, 0.5f).OnComplete(() =>
            {
                t.SetActive(false);
            });
        }
    }
}
