using UnityEngine;
using System.Collections;

public class DancingDotsQuadManager : MonoBehaviour
{

    public GameObject[] quads;
    public float minTime = 0.1f;
    public float maxTime = 0.5f;

    // Use this for initialization
    void Start()
    {
        //StartCoroutine(CR_AnimateQuads());
    }

    void SwapQuads(GameObject Quad1, GameObject Quad2)
    {
        if (!Quad1 || !Quad2)
            return;

        Vector3 temp = Quad1.transform.localPosition;

        Quad1.transform.localPosition = Quad2.transform.localPosition;
        Quad2.transform.localPosition = temp;
    }

    void SwapColor(SpriteRenderer sprt1, SpriteRenderer sprt2)
    {
        if (!sprt1 || !sprt2)
            return;

        Color temp = sprt1.color;

        sprt1.color = sprt2.color;
        sprt2.color = temp;
    }

    IEnumerator CR_AnimateQuads()
    {
        do
        {
            yield return new WaitForSeconds(UnityEngine.Random.Range(minTime, maxTime));
            int Q1 = UnityEngine.Random.Range(0, quads.Length);
            int Q2 = -1;
            do
            {
                Q2 = UnityEngine.Random.Range(0, quads.Length);
            } while (Q1 == Q2);
            SwapQuads(quads[Q1], quads[Q2]);

        } while (true);

    }

    public void swap()
    {
        int Q1 = UnityEngine.Random.Range(0, quads.Length);
        int Q2 = -1;
        do
        {
            Q2 = UnityEngine.Random.Range(0, quads.Length);
        } while (Q1 == Q2);

        SwapQuads(quads[Q1], quads[Q2]);
    }

    public void swap(SpriteRenderer[] sprts)
    {
        int Q1, Q2 = -1;

        Q1 = UnityEngine.Random.Range(0, sprts.Length);

        do
        {
            Q2 = UnityEngine.Random.Range(0, sprts.Length);
        } while (Q1 == Q2 || sprts[Q1].color.Equals(sprts[Q2].color));

        SwapColor(sprts[Q1], sprts[Q2]);
    }
}
