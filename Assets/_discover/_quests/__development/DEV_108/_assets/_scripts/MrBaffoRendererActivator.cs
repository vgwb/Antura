using UnityEngine;

public class MrBaffoRendererActivator : MonoBehaviour
{
    private SpriteRenderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {
        _renderer.enabled = true;
    }
}
