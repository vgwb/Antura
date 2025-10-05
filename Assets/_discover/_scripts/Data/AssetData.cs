using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.Serialization;
using Cysharp.Threading.Tasks;
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEditor.AddressableAssets;
using UnityEditor.AddressableAssets.Settings;
using UnityEditor.AddressableAssets.Settings.GroupSchemas;
#endif

namespace Antura.Discover
{
    [CreateAssetMenu(fileName = "AssetData", menuName = "Antura/Discover Data/Asset")]
    public class AssetData : IdentifiedData
    {
        public Status Status = Status.Draft;
        public AssetType Type = AssetType.Image;
        public Countries Country = Countries.International;

        [Header("Asset Reference")]
        [SerializeField] private AssetReferenceSprite imageReference;
        [SerializeField] private AssetReferenceAudioClip audioReference;
        [SerializeField] private AssetReferenceGameObject modelReference;

        [SerializeField, HideInInspector, FormerlySerializedAs("Image")] private Sprite legacyImage;
        [SerializeField, HideInInspector, FormerlySerializedAs("Audio")] private AudioClip legacyAudio;
        [SerializeField, HideInInspector, FormerlySerializedAs("Model3D")] private GameObject legacyModel;

        [System.NonSerialized] private Sprite cachedImage;
        [System.NonSerialized] private AudioClip cachedAudio;
        [System.NonSerialized] private GameObject cachedModel;

        [System.NonSerialized] private AsyncOperationHandle<Sprite> imageHandle;
        [System.NonSerialized] private AsyncOperationHandle<AudioClip> audioHandle;
        [System.NonSerialized] private AsyncOperationHandle<GameObject> modelHandle;

        [Header("License Information")]
        public LicenseType License = LicenseType.CC0;
        public string Copyright; // e.g., "Stefano Cecere, 2025"
        public string SourceUrl; // Original source link

        [TextArea]
        public string LicenseNotes;    // For custom terms or clarifications

        [Header("Authoring Metadata")]
        public List<AuthorCredit> Credits;

        public AssetReferenceSprite ImageReference => imageReference;
        public AssetReferenceAudioClip AudioReference => audioReference;
        public AssetReferenceGameObject ModelReference => modelReference;

        public bool HasImageReference => imageReference != null && imageReference.RuntimeKeyIsValid();
        public bool HasAudioReference => audioReference != null && audioReference.RuntimeKeyIsValid();
        public bool HasModelReference => modelReference != null && modelReference.RuntimeKeyIsValid();

        public bool HasImageAsset => HasImageReference || legacyImage != null;
        public bool HasAudioAsset => HasAudioReference || legacyAudio != null;
        public bool HasModelAsset => HasModelReference || legacyModel != null;

        public Sprite Image
        {
            get => GetImage();
#if UNITY_EDITOR
            set
            {
                if (Application.isPlaying)
                {
                    cachedImage = value;
                    return;
                }
                AssignImageEditor(value, true);
            }
#endif
        }

        public AudioClip Audio
        {
            get => GetAudio();
#if UNITY_EDITOR
            set
            {
                if (Application.isPlaying)
                {
                    cachedAudio = value;
                    return;
                }
                AssignAudioEditor(value, true);
            }
#endif
        }

        public GameObject Model3D
        {
            get => GetModel();
#if UNITY_EDITOR
            set
            {
                if (Application.isPlaying)
                {
                    cachedModel = value;
                    return;
                }
                AssignModelEditor(value, true);
            }
#endif
        }

        public Sprite GetImage(bool forceReload = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                var editorAsset = imageReference != null ? imageReference.editorAsset as Sprite : null;
                if (editorAsset != null)
                    return editorAsset;
                return legacyImage;
            }
#endif
            if (!forceReload && cachedImage != null)
                return cachedImage;
            if (!forceReload && legacyImage != null)
            {
                cachedImage = legacyImage;
                return cachedImage;
            }
            if (!HasImageReference)
                return null;

            EnsureImageHandleLoaded(forceReload);
            return cachedImage;
        }

        public AudioClip GetAudio(bool forceReload = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                AudioClip editorAsset = audioReference != null ? audioReference.editorAsset : null;
                if (editorAsset != null)
                    return editorAsset;
                return legacyAudio;
            }
#endif
            if (!forceReload && cachedAudio != null)
                return cachedAudio;
            if (!forceReload && legacyAudio != null)
            {
                cachedAudio = legacyAudio;
                return cachedAudio;
            }
            if (!HasAudioReference)
                return null;

            EnsureAudioHandleLoaded(forceReload);
            return cachedAudio;
        }

        public GameObject GetModel(bool forceReload = false)
        {
#if UNITY_EDITOR
            if (!Application.isPlaying)
            {
                GameObject editorAsset = modelReference != null ? modelReference.editorAsset : null;
                if (editorAsset != null)
                    return editorAsset;
                return legacyModel;
            }
#endif
            if (!forceReload && cachedModel != null)
                return cachedModel;
            if (!forceReload && legacyModel != null)
            {
                cachedModel = legacyModel;
                return cachedModel;
            }
            if (!HasModelReference)
                return null;

            EnsureModelHandleLoaded(forceReload);
            return cachedModel;
        }

        public async UniTask<Sprite> LoadImageAsync(CancellationToken cancellationToken = default)
        {
            if (cachedImage != null)
                return cachedImage;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return GetImage();
#endif
            if (legacyImage != null)
            {
                cachedImage = legacyImage;
                return cachedImage;
            }
            if (!HasImageReference)
                return null;

            if (!imageHandle.IsValid())
                imageHandle = imageReference.LoadAssetAsync<Sprite>();

            var result = await imageHandle.ToUniTask(cancellationToken: cancellationToken);

            cachedImage = result;
            return cachedImage;
        }

        public async UniTask<AudioClip> LoadAudioAsync(CancellationToken cancellationToken = default)
        {
            if (cachedAudio != null)
                return cachedAudio;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return GetAudio();
#endif
            if (legacyAudio != null)
            {
                cachedAudio = legacyAudio;
                return cachedAudio;
            }
            if (!HasAudioReference)
                return null;

            if (!audioHandle.IsValid())
                audioHandle = audioReference.LoadAssetAsync<AudioClip>();

            var result = await audioHandle.ToUniTask(cancellationToken: cancellationToken);

            cachedAudio = result;
            return cachedAudio;
        }

        public async UniTask<GameObject> LoadModelAsync(CancellationToken cancellationToken = default)
        {
            if (cachedModel != null)
                return cachedModel;
#if UNITY_EDITOR
            if (!Application.isPlaying)
                return GetModel();
#endif
            if (legacyModel != null)
            {
                cachedModel = legacyModel;
                return cachedModel;
            }
            if (!HasModelReference)
                return null;

            if (!modelHandle.IsValid())
                modelHandle = modelReference.LoadAssetAsync<GameObject>();

            var result = await modelHandle.ToUniTask(cancellationToken: cancellationToken);

            cachedModel = result;
            return cachedModel;
        }

        public void ReleaseCachedAssets()
        {
            ReleaseImage();
            ReleaseAudio();
            ReleaseModel();
        }

        public void ReleaseImage()
        {
            if (imageHandle.IsValid())
            {
                Addressables.Release(imageHandle);
                imageHandle = default;
            }
            cachedImage = null;
        }

        public void ReleaseAudio()
        {
            if (audioHandle.IsValid())
            {
                Addressables.Release(audioHandle);
                audioHandle = default;
            }
            cachedAudio = null;
        }

        public void ReleaseModel()
        {
            if (modelHandle.IsValid())
            {
                Addressables.Release(modelHandle);
                modelHandle = default;
            }
            cachedModel = null;
        }

        void OnDisable()
        {
            if (!Application.isPlaying)
                return;
            ReleaseCachedAssets();
        }

        private void EnsureImageHandleLoaded(bool forceReload = false)
        {
            if (!forceReload && cachedImage != null)
                return;
            if (!HasImageReference)
                return;
            if (forceReload || !imageHandle.IsValid())
                imageHandle = imageReference.LoadAssetAsync<Sprite>();
            cachedImage = imageHandle.WaitForCompletion();
        }

        private void EnsureAudioHandleLoaded(bool forceReload = false)
        {
            if (!forceReload && cachedAudio != null)
                return;
            if (!HasAudioReference)
                return;
            if (forceReload || !audioHandle.IsValid())
                audioHandle = audioReference.LoadAssetAsync<AudioClip>();
            cachedAudio = audioHandle.WaitForCompletion();
        }

        private void EnsureModelHandleLoaded(bool forceReload = false)
        {
            if (!forceReload && cachedModel != null)
                return;
            if (!HasModelReference)
                return;
            if (forceReload || !modelHandle.IsValid())
                modelHandle = modelReference.LoadAssetAsync<GameObject>();
            cachedModel = modelHandle.WaitForCompletion();
        }

#if UNITY_EDITOR
        public Sprite Editor_GetLegacyImage() => legacyImage;
        public AudioClip Editor_GetLegacyAudio() => legacyAudio;
        public GameObject Editor_GetLegacyModel() => legacyModel;

        private const string SpriteGroupName = "Discover Sprites";
        private const string AudioGroupName = "Discover Audio";
        private const string ModelGroupName = "Discover Models";
        private static readonly string[] CommonLabels = { "discover" };
        private static readonly string[] SpriteExtraLabels = { "discover-sprite" };
        private static readonly string[] AudioExtraLabels = { "discover-audio" };
        private static readonly string[] ModelExtraLabels = { "discover-model" };

        public void EditorAssignImage(Sprite sprite, bool markAddressable = true)
        {
            AssignImageEditor(sprite, markAddressable);
        }

        public void EditorAssignAudio(AudioClip clip, bool markAddressable = true)
        {
            AssignAudioEditor(clip, markAddressable);
        }

        public void EditorAssignModel(GameObject prefab, bool markAddressable = true)
        {
            AssignModelEditor(prefab, markAddressable);
        }

        public void EditorClearLegacy()
        {
            legacyImage = null;
            legacyAudio = null;
            legacyModel = null;
        }

        private void AssignImageEditor(Sprite sprite, bool markAddressable)
        {
            bool assigned = sprite == null || !markAddressable || UpdateAddressableEntry(sprite);
            if (sprite == null)
            {
                if (imageReference != null)
                {
                    imageReference.SetEditorAsset(null);
                    imageReference.SubObjectName = null;
                    imageReference = null;
                }
                legacyImage = null;
            }
            else
            {
                if (imageReference == null)
                    imageReference = CreateSpriteReference(sprite);

                if (assigned)
                {
                    imageReference?.SetEditorAsset(sprite);
                    if (imageReference != null)
                    {
                        imageReference.SubObjectName = AssetDatabase.IsSubAsset(sprite) ? sprite.name : null;
                    }
                    legacyImage = null;
                }
                else
                {
                    legacyImage = sprite;
                }
            }
            cachedImage = null;
            if (!Application.isPlaying)
                ReleaseImage();
            EditorUtility.SetDirty(this);
        }

        private void AssignAudioEditor(AudioClip clip, bool markAddressable)
        {
            bool assigned = clip == null || !markAddressable || UpdateAddressableEntry(clip);
            if (clip == null)
            {
                if (audioReference != null)
                {
                    audioReference.SetEditorAsset(null);
                    audioReference = null;
                }
                legacyAudio = null;
            }
            else
            {
                if (audioReference == null)
                    audioReference = CreateAudioReference(clip);

                if (assigned)
                {
                    audioReference?.SetEditorAsset(clip);
                    legacyAudio = null;
                }
                else
                {
                    legacyAudio = clip;
                }
            }
            cachedAudio = null;
            if (!Application.isPlaying)
                ReleaseAudio();
            EditorUtility.SetDirty(this);
        }

        private void AssignModelEditor(GameObject prefab, bool markAddressable)
        {
            bool assigned = prefab == null || !markAddressable || UpdateAddressableEntry(prefab);
            if (prefab == null)
            {
                if (modelReference != null)
                {
                    modelReference.SetEditorAsset(null);
                    modelReference = null;
                }
                legacyModel = null;
            }
            else
            {
                if (modelReference == null)
                    modelReference = CreateModelReference(prefab);

                if (assigned)
                {
                    modelReference?.SetEditorAsset(prefab);
                    legacyModel = null;
                }
                else
                {
                    legacyModel = prefab;
                }
            }
            cachedModel = null;
            if (!Application.isPlaying)
                ReleaseModel();
            EditorUtility.SetDirty(this);
        }

        private static AssetReferenceSprite CreateSpriteReference(Sprite sprite)
        {
            if (sprite == null)
                return new AssetReferenceSprite(string.Empty);
            var guid = AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(sprite));
            var reference = new AssetReferenceSprite(guid);
            if (AssetDatabase.IsSubAsset(sprite))
            {
                reference.SubObjectName = sprite.name;
            }
            return reference;
        }

        private static AssetReferenceAudioClip CreateAudioReference(AudioClip clip)
        {
            var guid = clip == null ? string.Empty : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(clip));
            return new AssetReferenceAudioClip(guid);
        }

        private static AssetReferenceGameObject CreateModelReference(GameObject prefab)
        {
            var guid = prefab == null ? string.Empty : AssetDatabase.AssetPathToGUID(AssetDatabase.GetAssetPath(prefab));
            return new AssetReferenceGameObject(guid);
        }

        private bool UpdateAddressableEntry(Object asset)
        {
            if (asset == null)
                return false;

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
            if (settings == null)
            {
                Debug.LogWarning("[AssetData] Addressable settings not found. Cannot mark asset as addressable.", asset);
                return false;
            }

            string assetPath = AssetDatabase.GetAssetPath(asset);
            var guid = AssetDatabase.AssetPathToGUID(assetPath);
            if (string.IsNullOrEmpty(guid))
                return false;

            var entry = settings.FindAssetEntry(guid);
            if (entry == null)
            {
                var group = settings.DefaultGroup ?? settings.groups.FirstOrDefault();
                if (group == null)
                {
                    group = settings.CreateGroup("Discover Assets", false, false, false, new List<AddressableAssetGroupSchema>());
                }
                entry = settings.CreateOrMoveEntry(guid, group, readOnly: false, postEvent: false);
            }

            if (!entry.labels.Contains("discover"))
                entry.SetLabel("discover", true, true, true);
            return true;
        }

        private void OnValidate()
        {
            if (Application.isPlaying)
                return;

            // Auto-upgrade legacy fields if someone drags assets into hidden slots via debug inspector
            if (legacyImage && !HasImageReference)
                EditorAssignImage(legacyImage, true);
            if (legacyAudio && !HasAudioReference)
                EditorAssignAudio(legacyAudio, true);
            if (legacyModel && !HasModelReference)
                EditorAssignModel(legacyModel, true);

            if (imageReference != null && imageReference.editorAsset is Sprite assignedSprite && AssetDatabase.IsSubAsset(assignedSprite))
            {
                imageReference.SubObjectName = assignedSprite.name;
            }
            else if (imageReference != null && string.IsNullOrEmpty(imageReference.SubObjectName))
            {
                imageReference.SubObjectName = null;
            }

            // Group & labeling pass
            TryEnsureGrouping(imageReference, SpriteGroupName, SpriteExtraLabels);
            TryEnsureGrouping(audioReference, AudioGroupName, AudioExtraLabels);
            TryEnsureGrouping(modelReference, ModelGroupName, ModelExtraLabels);
        }

        private void TryEnsureGrouping(AssetReference reference, string groupName, string[] extraLabels)
        {
            if (reference == null || !reference.RuntimeKeyIsValid())
                return;

            var settings = AddressableAssetSettingsDefaultObject.GetSettings(false);
            if (settings == null)
                return;

            var guid = reference.AssetGUID;
            if (string.IsNullOrEmpty(guid))
                return;

            var entry = settings.FindAssetEntry(guid);
            if (entry == null)
            {
                var group = EnsureGroup(settings, groupName);
                entry = settings.CreateOrMoveEntry(guid, group, false, false);
            }
            else if (entry.parentGroup != null && entry.parentGroup.name != groupName)
            {
                var targetGroup = EnsureGroup(settings, groupName);
                entry = settings.CreateOrMoveEntry(guid, targetGroup, false, false);
            }

            // Labels
            foreach (var lbl in CommonLabels)
            {
                if (!entry.labels.Contains(lbl))
                    entry.SetLabel(lbl, true, true, true);
            }
            if (extraLabels != null)
            {
                foreach (var lbl in extraLabels)
                {
                    if (!entry.labels.Contains(lbl))
                        entry.SetLabel(lbl, true, true, true);
                }
            }
        }

        private AddressableAssetGroup EnsureGroup(AddressableAssetSettings settings, string name)
        {
            var group = settings.groups.FirstOrDefault(g => g != null && g.Name == name);
            if (group != null)
                return group;
            group = settings.CreateGroup(name, false, false, false, new List<AddressableAssetGroupSchema>()
            {
                settings.DefaultGroup?.GetSchema<BundledAssetGroupSchema>() != null ? ScriptableObject.CreateInstance<BundledAssetGroupSchema>() : null,
                ScriptableObject.CreateInstance<ContentUpdateGroupSchema>()
            }.Where(s => s != null).ToList(), typeof(BundledAssetGroupSchema), typeof(ContentUpdateGroupSchema));
            return group;
        }
#endif
    }
}
