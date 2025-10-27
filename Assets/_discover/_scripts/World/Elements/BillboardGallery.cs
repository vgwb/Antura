using System;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace Antura.Discover
{
    public enum BillboardSource
    {
        TopicData = 1,
        CardData = 2,
        AssetData = 3,
        ImageList = 4,
        DirectSprite = 5
    }

    public enum FitMode
    {
        None,               // show whole sprite rect
        Cover,              // fill both dimensions + crop whichever side is needed
        FillWidthCropTB,    // fill width + crop Top/Bottom
        FillHeightCropLR    // fill height + crop Left/Right
    }
    public enum HorizontalAnchor { Left, Center, Right }
    public enum VerticalAnchor { Bottom, Center, Top }

    /// <summary>
    /// Shows a Sprite from AssetData, CardData, TopicData or a list on a 3D billboard (quad/mesh) using UV cropping.
    /// Keeps the panel's world size constant and fits the image by adjusting _MainTex_ST (no stretching).
    /// </summary>
    public class BillboardGallery : MonoBehaviour
    {
        [Header("Source")]
        public BillboardSource Source = BillboardSource.AssetData;
        public TopicData Topic;
        public CardData Card;
        public AssetData Asset;
        public Sprite DirectSprite;

        [Header("Image List Source")]
        public List<AssetData> GalleryAssets = new List<AssetData>();
        public List<Sprite> GallerySprites = new List<Sprite>();

        [Header("Target Material Properties")]
        public Renderer TargetRenderer;
        public string TextureProperty = "_MainTex";
        public string TintProperty = "_Tint";
        public Color Tint = Color.white;

        [Header("Fitting & Cropping")]
        public FitMode Fit = FitMode.Cover;
        public HorizontalAnchor XAnchor = HorizontalAnchor.Center;
        public VerticalAnchor YAnchor = VerticalAnchor.Center;

        [Tooltip("Optional: if set > 0, enforces a specific panel aspect (width/height). Leave 0 to infer from localScale.")]
        public float OverridePanelAspect = 0f;
        [Tooltip("Optional: use this transform's localScale to compute panel aspect (useful when attaching on prefab root but rendering on a child panel). If null, uses TargetRenderer.transform if available, else this.transform.")]
        public Transform PanelTransform;

        [Header("Gallery")]
        [Tooltip("Cycle images if multiple are available (Card, Topic or ImageList sources).")]
        public bool EnableGallery = true;
        [Tooltip("Seconds each image is shown before a transition.")]
        public float GalleryInterval = 4f;
        [Tooltip("Fade duration in seconds (out+in). If Tint property unsupported, swap instantly.")]
        public float FadeDuration = 0.8f;
        [Tooltip("If true, fades the Tint color to Black and back instead of fading alpha (requires a Tint-like color property).")]
        public bool FadeToBlack = false;
        [Tooltip("Include Card.PreviewImage when building a Card/Topic gallery.")]
        public bool IncludePreviewImage = false;
        [Tooltip("Randomize gallery order on build.")]
        public bool ShuffleGallery = false;

        [Header("Card Title (CardData / TopicData)")]
        public GameObject PanelTitle;
        public TMP_Text TitleText;
        [Tooltip("Optional string.Format pattern for the title (e.g., '{0}').")] public string TitleFormat = "{0}";
        [Tooltip("Hide the title object if there's no card to show a title for.")] public bool HideTitleWhenNoCard = true;

        // Cache
        private Sprite _lastSprite;
        private Vector2 _lastScale;
        private MaterialPropertyBlock _mpb;
        private int _texId, _texStId, _tintId;

        // Gallery state
        private readonly List<Sprite> _gallery = new List<Sprite>();
        private int _galleryIndex = 0;
        private float _galleryTimer = 0f;
        private GalleryState _galleryState = GalleryState.Showing;
        private float _fadeTimer = 0f;
        private CardData _lastCard;
        private TopicData _lastTopic;
        private int _lastAssetsCount = -1, _lastSpritesCount = -1;
        private readonly Dictionary<Sprite, CardData> _spriteToCard = new Dictionary<Sprite, CardData>();
        private CardData _currentCard;

        private enum GalleryState { Showing, FadingOut, FadingIn }

#if UNITY_EDITOR
        // private void OnEnable()
        // {
        //     EditorApplication.projectChanged += EditorProjectChanged;
        //     Undo.undoRedoPerformed += RefreshNow;
        //     Init();
        //     RebuildGalleryIfNeeded();
        //     RefreshNow();
        // }
        // private void OnDisable()
        // {
        //     EditorApplication.projectChanged -= EditorProjectChanged;
        //     Undo.undoRedoPerformed -= RefreshNow;
        // }
        // private void EditorProjectChanged() => RefreshNow();
#else
        private void OnEnable()
        {
            Init();
            RebuildGalleryIfNeeded();
            RefreshNow();
        }
#endif

        private void Reset()
        {
            TargetRenderer = GetComponent<Renderer>();
        }

        private void Init()
        {
            if (TargetRenderer == null)
                TargetRenderer = GetComponent<Renderer>();
            if (_mpb == null)
                _mpb = new MaterialPropertyBlock();
            UpdatePropertyIDs();
        }

        private void UpdatePropertyIDs()
        {
            _texId = Shader.PropertyToID(TextureProperty);
            _texStId = Shader.PropertyToID(TextureProperty + "_ST");
            _tintId = ResolveTintPropertyId();
        }

        private int ResolveTintPropertyId()
        {
            // Try user-provided property, then common fallbacks
            string[] candidates = string.IsNullOrEmpty(TintProperty)
                ? new[] { "_Tint", "_Color", "_BaseColor" }
                : new[] { TintProperty, "_Tint", "_Color", "_BaseColor" };
            Material mat = TargetRenderer != null ? TargetRenderer.sharedMaterial : null;
            foreach (var name in candidates)
            {
                if (string.IsNullOrEmpty(name))
                    continue;
                if (mat == null || mat.HasProperty(name))
                    return Shader.PropertyToID(name);
            }
            return -1;
        }

        private void OnValidate()
        {
            Init();
            RebuildGalleryIfNeeded();
            RefreshNow();
        }

        private void Update()
        {
            // Cheap change detection in Editor and Play modes
            RebuildGalleryIfNeeded();
            HandleGallery(Time.deltaTime);
            var s = ResolveSprite();
            var sc = new Vector2(transform.localScale.x, transform.localScale.y);
            if (s != _lastSprite || sc != _lastScale)
            {
                RefreshNow();
            }
        }

        // --- Core ---

        private void RefreshNow()
        {
            if (TargetRenderer == null)
                return;

            Sprite sprite = ResolveSprite();
            _lastSprite = sprite;
            _lastScale = new Vector2(transform.localScale.x, transform.localScale.y);

            TargetRenderer.GetPropertyBlock(_mpb);

            if (sprite == null || sprite.texture == null)
            {
                // Clear texture, keep tint (or set white texture)
                _mpb.SetTexture(_texId, Texture2D.whiteTexture);
                if (_tintId != -1)
                    _mpb.SetColor(_tintId, Tint);
                _mpb.SetVector(_texStId, new Vector4(1, 1, 0, 0));
                TargetRenderer.SetPropertyBlock(_mpb);
                return;
            }

            // Base tiling/offset (atlas-aware: use sprite.textureRect)
            Rect r = sprite.textureRect;
            Vector2 texSize = new Vector2(sprite.texture.width, sprite.texture.height);
            Vector2 baseTiling = new Vector2(r.width / texSize.x, r.height / texSize.y);
            Vector2 baseOffset = new Vector2(r.x / texSize.x, r.y / texSize.y);

            // Panel & sprite aspect
            float panelAspect = OverridePanelAspect > 0f
                ? OverridePanelAspect
                : ComputePanelAspect();

            float spriteAspect = r.width / Mathf.Max(0.0001f, r.height);

            // Start from full sprite rect
            Vector2 tiling = baseTiling;
            Vector2 offset = baseOffset;

            // Anchor weights (0..1)
            float ax01 = (XAnchor == HorizontalAnchor.Left) ? 0f :
                         (XAnchor == HorizontalAnchor.Right) ? 1f : 0.5f;
            float ay01 = (YAnchor == VerticalAnchor.Bottom) ? 0f :
                         (YAnchor == VerticalAnchor.Top) ? 1f : 0.5f;

            switch (Fit)
            {
                case FitMode.Cover:
                    if (panelAspect > spriteAspect)
                    {
                        // Panel wider → crop vertically
                        float fracY = Mathf.Clamp01(spriteAspect / Mathf.Max(0.0001f, panelAspect)); // fraction of height to sample
                        tiling.y = baseTiling.y * fracY;
                        float unusedY = baseTiling.y - tiling.y;
                        offset.y = baseOffset.y + unusedY * ay01; // 0=bottom, .5=center, 1=top
                    }
                    else
                    {
                        // Panel taller/narrower → crop horizontally
                        float fracX = Mathf.Clamp01(panelAspect / Mathf.Max(0.0001f, spriteAspect)); // fraction of width to sample
                        tiling.x = baseTiling.x * fracX;
                        float unusedX = baseTiling.x - tiling.x;
                        offset.x = baseOffset.x + unusedX * ax01; // 0=left, .5=center, 1=right
                    }
                    break;

                case FitMode.FillWidthCropTB:
                {
                    float fracY = Mathf.Clamp01(spriteAspect / Mathf.Max(0.0001f, panelAspect));
                    tiling.y = baseTiling.y * fracY;
                    float unusedY = baseTiling.y - tiling.y;
                    offset.y = baseOffset.y + unusedY * ay01;
                }
                break;

                case FitMode.FillHeightCropLR:
                {
                    float fracX = Mathf.Clamp01(panelAspect / Mathf.Max(0.0001f, spriteAspect));
                    tiling.x = baseTiling.x * fracX;
                    float unusedX = baseTiling.x - tiling.x;
                    offset.x = baseOffset.x + unusedX * ax01;
                }
                break;

                case FitMode.None:
                default:
                    // Show full sprite rect; may leave letterboxing if panel aspect differs
                    break;
            }

            // Apply to material
            _mpb.SetTexture(_texId, sprite.texture);
            _mpb.SetVector(_texStId, new Vector4(tiling.x, tiling.y, offset.x, offset.y));
            if (_tintId != -1)
                _mpb.SetColor(_tintId, Tint);
            TargetRenderer.SetPropertyBlock(_mpb);

            // Update title if needed
            UpdateTitleForCurrentSprite();
        }

        private Sprite ResolveSprite()
        {
            switch (Source)
            {
                case BillboardSource.AssetData:
                    _currentCard = null;
                    return Asset != null ? Asset.Image : null;

                case BillboardSource.CardData:
                    if (EnableGallery && _gallery.Count > 0)
                    {
                        // Ensure index in range
                        if (_galleryIndex < 0 || _galleryIndex >= _gallery.Count)
                            _galleryIndex = 0;
                        var sp = _gallery[_galleryIndex];
                        _currentCard = ResolveCardForSprite(sp);
                        return sp;
                    }
                    _currentCard = Card;
                    return GetPrimarySpriteFromCard(Card);

                case BillboardSource.TopicData:
                    if (EnableGallery && _gallery.Count > 0)
                    {
                        if (_galleryIndex < 0 || _galleryIndex >= _gallery.Count)
                            _galleryIndex = 0;
                        var sp = _gallery[_galleryIndex];
                        _currentCard = ResolveCardForSprite(sp);
                        return sp;
                    }
                    _currentCard = GetFirstCardFromTopic(Topic);
                    return GetPrimarySpriteFromCard(_currentCard);

                case BillboardSource.ImageList:
                    if (EnableGallery && _gallery.Count > 0)
                    {
                        if (_galleryIndex < 0 || _galleryIndex >= _gallery.Count)
                            _galleryIndex = 0;
                        var sp = _gallery[_galleryIndex];
                        _currentCard = null;
                        return sp;
                    }
                    _currentCard = null;
                    return GetFirstSpriteFromImageList();

                case BillboardSource.DirectSprite:
                    _currentCard = null;
                    return DirectSprite;

                default:
                    return null;
            }
        }

        // --- Strongly-typed access & gallery helpers ---
        private void RebuildGalleryIfNeeded()
        {
            if (Source != BillboardSource.CardData && Source != BillboardSource.TopicData && Source != BillboardSource.ImageList)
                return;

            if ((Source == BillboardSource.CardData && _lastCard == Card && _gallery.Count > 0) ||
                (Source == BillboardSource.TopicData && _lastTopic == Topic && _gallery.Count > 0) ||
                (Source == BillboardSource.ImageList && GalleryUnchanged() && _gallery.Count > 0))
                return;

            _lastCard = Card;
            _lastTopic = Topic;
            _gallery.Clear();
            _spriteToCard.Clear();
            _galleryIndex = 0;
            _galleryTimer = 0f;
            _galleryState = GalleryState.Showing;
            _fadeTimer = 0f;

            if (Source == BillboardSource.CardData)
            {
                if (Card == null)
                    return;
                foreach (var s in Card.GetGallerySprites(IncludePreviewImage))
                {
                    _gallery.Add(s);
                    if (s != null)
                        _spriteToCard[s] = Card;
                }
            }
            else if (Source == BillboardSource.TopicData)
            {
                if (Topic == null)
                    return;
                var seen = new HashSet<Sprite>();
                if (Topic.CoreCard != null)
                {
                    foreach (var s in Topic.CoreCard.GetGallerySprites(IncludePreviewImage))
                        if (seen.Add(s))
                        {
                            _gallery.Add(s);
                            if (s != null)
                                _spriteToCard[s] = Topic.CoreCard;
                        }
                }
                foreach (var conn in Topic.Connections)
                {
                    if (conn != null && conn.ConnectedCard != null)
                    {
                        foreach (var s in conn.ConnectedCard.GetGallerySprites(IncludePreviewImage))
                            if (seen.Add(s))
                            {
                                _gallery.Add(s);
                                if (s != null)
                                    _spriteToCard[s] = conn.ConnectedCard;
                            }
                    }
                }
            }
            else if (Source == BillboardSource.ImageList)
            {
                var seen = new HashSet<Sprite>();
                _lastAssetsCount = GalleryAssets != null ? GalleryAssets.Count : 0;
                _lastSpritesCount = GallerySprites != null ? GallerySprites.Count : 0;
                if (GalleryAssets != null)
                {
                    foreach (var a in GalleryAssets)
                    {
                        if (a != null && a.Image != null && seen.Add(a.Image))
                            _gallery.Add(a.Image);
                    }
                }
                if (GallerySprites != null)
                {
                    foreach (var s in GallerySprites)
                    {
                        if (s != null && seen.Add(s))
                            _gallery.Add(s);
                    }
                }
            }

            if (ShuffleGallery)
                Shuffle(_gallery);
        }

        private Sprite GetPrimarySpriteFromCard(CardData card)
        {
            if (card == null)
                return null;
            if (card.ImageAsset != null && card.ImageAsset.Image != null)
                return card.ImageAsset.Image;
            if (card.PreviewImage != null)
                return card.PreviewImage;
            return null;
        }

        private CardData GetFirstCardFromTopic(TopicData topic)
        {
            if (topic == null)
                return null;
            if (topic.CoreCard != null)
            {
                var s = GetPrimarySpriteFromCard(topic.CoreCard);
                if (s != null)
                    return topic.CoreCard;
            }
            foreach (var c in topic.Connections)
            {
                if (c != null && c.ConnectedCard != null)
                {
                    var s = GetPrimarySpriteFromCard(c.ConnectedCard);
                    if (s != null)
                        return c.ConnectedCard;
                }
            }
            return null;
        }

        private Sprite GetFirstSpriteFromImageList()
        {
            if (GalleryAssets != null)
            {
                foreach (var a in GalleryAssets)
                {
                    if (a != null && a.Image != null)
                        return a.Image;
                }
            }
            if (GallerySprites != null)
            {
                foreach (var s in GallerySprites)
                {
                    if (s != null)
                        return s;
                }
            }
            return null;
        }

        private bool GalleryUnchanged()
        {
            int ac = GalleryAssets != null ? GalleryAssets.Count : 0;
            int sc = GallerySprites != null ? GallerySprites.Count : 0;
            return ac == _lastAssetsCount && sc == _lastSpritesCount;
        }

        private void HandleGallery(float dt)
        {
            if (!((Source == BillboardSource.CardData) || (Source == BillboardSource.TopicData) || (Source == BillboardSource.ImageList)) || !EnableGallery)
                return;
            if (_gallery.Count <= 1)
                return; // nothing to cycle

            bool canFade = _tintId != -1;

            switch (_galleryState)
            {
                case GalleryState.Showing:
                    _galleryTimer += dt;
                    if (_galleryTimer >= Mathf.Max(0.01f, GalleryInterval))
                    {
                        if (canFade && FadeDuration > 0.01f)
                        {
                            _galleryState = GalleryState.FadingOut;
                            _fadeTimer = 0f;
                        }
                        else
                        {
                            AdvanceGalleryIndex();
                            RefreshNow();
                            _galleryTimer = 0f;
                        }
                    }
                    break;

                case GalleryState.FadingOut:
                    _fadeTimer += dt;
                    float half = FadeDuration * 0.5f;
                    float tOut = Mathf.Clamp01(_fadeTimer / Mathf.Max(0.0001f, half));
                    if (FadeToBlack)
                        ApplyTintColor(Color.Lerp(Tint, Color.black, tOut));
                    else
                        ApplyTintAlpha(1f - tOut);
                    if (_fadeTimer >= half)
                    {
                        AdvanceGalleryIndex();
                        RefreshNow();
                        _galleryState = GalleryState.FadingIn;
                        _fadeTimer = 0f;
                    }
                    break;

                case GalleryState.FadingIn:
                    _fadeTimer += dt;
                    float tIn = Mathf.Clamp01(_fadeTimer / Mathf.Max(0.0001f, FadeDuration * 0.5f));
                    if (FadeToBlack)
                        ApplyTintColor(Color.Lerp(Color.black, Tint, tIn));
                    else
                        ApplyTintAlpha(tIn);
                    if (_fadeTimer >= FadeDuration * 0.5f)
                    {
                        _galleryState = GalleryState.Showing;
                        _galleryTimer = 0f;
                        if (FadeToBlack)
                            ApplyTintColor(Tint);
                        else
                            ApplyTintAlpha(1f);
                    }
                    break;

                default:
                    // No-op
                    break;
            }
        }

        private void AdvanceGalleryIndex()
        {
            if (_gallery.Count == 0)
                return;
            _galleryIndex = (_galleryIndex + 1) % _gallery.Count;
        }

        private void ApplyTintAlpha(float alpha)
        {
            if (_tintId == -1)
                return;
            TargetRenderer.GetPropertyBlock(_mpb);
            var c = Tint;
            c.a = Mathf.Clamp01(alpha);
            _mpb.SetColor(_tintId, c);
            TargetRenderer.SetPropertyBlock(_mpb);
        }

        private void ApplyTintColor(Color color)
        {
            if (_tintId == -1)
                return;
            TargetRenderer.GetPropertyBlock(_mpb);
            _mpb.SetColor(_tintId, color);
            TargetRenderer.SetPropertyBlock(_mpb);
        }

        private void Shuffle(List<Sprite> list)
        {
            int n = list.Count;
            for (int i = 0; i < n - 1; i++)
            {
                int j = UnityEngine.Random.Range(i, n);
                var tmp = list[i];
                list[i] = list[j];
                list[j] = tmp;
            }
        }

        private float ComputePanelAspect()
        {
            Transform t = PanelTransform != null ? PanelTransform : (TargetRenderer != null ? TargetRenderer.transform : transform);
            return Mathf.Abs(t.localScale.x / Mathf.Max(0.0001f, t.localScale.y));
        }

        private CardData ResolveCardForSprite(Sprite sp)
        {
            if (sp == null)
                return null;
            _spriteToCard.TryGetValue(sp, out var card);
            return card;
        }

        private void UpdateTitleForCurrentSprite()
        {
            if (TitleText == null)
                return;

            string title = null;
            var card = _currentCard;
            if (card != null)
            {
                try
                {
                    if (card.Title != null && !card.Title.IsEmpty)
                    {
                        title = card.Title.GetLocalizedString();
                    }
                }
                catch { }
                if (string.IsNullOrEmpty(title))
                    title = string.IsNullOrEmpty(card.TitleEn) ? card.name : card.TitleEn;
            }

            if (string.IsNullOrEmpty(title))
            {
                if (HideTitleWhenNoCard)
                {
                    PanelTitle.SetActive(false);
                    TitleText.gameObject.SetActive(false);
                }
                else
                {
                    PanelTitle.SetActive(true);
                    TitleText.gameObject.SetActive(true);
                    TitleText.text = string.Empty;
                }
            }
            else
            {
                PanelTitle.SetActive(true);
                TitleText.gameObject.SetActive(true);
                TitleText.text = string.IsNullOrEmpty(TitleFormat) ? title : string.Format(TitleFormat, title);
            }
        }
    }
}
