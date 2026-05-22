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
        if (_assetData != null && _assetData.ImageReference != null && _assetData.ImageReference.Asset)
        {
            _image.sprite = _assetData.ImageReference.Asset as Sprite;
        }
    }
}
