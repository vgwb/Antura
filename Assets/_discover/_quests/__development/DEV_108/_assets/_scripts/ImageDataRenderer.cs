using Antura.Discover;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class ImageDataRenderer : MonoBehaviour
{
    private Image _image;
    [SerializeField] private AssetData _assetData;

    private void Awake()
    {
        _image = GetComponent<Image>();
        if (_assetData != null && _assetData.Image != null)
        {
            _image.sprite = _assetData.Image;
        }
    }
}
