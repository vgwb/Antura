using UnityEngine;

public class SpriteRendererActivator : MonoBehaviour
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
