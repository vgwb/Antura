using UnityEngine;

namespace Antura.LivingLetters
{
    public class EmoticonsAnimator : MonoBehaviour
    {
        private EmoticonsController emoticonsController;

        public GameObject emoticonsPrefab;
        public Material black;
        public Material blue;
        public Material green;
        public Material orange;
        public Material pink;
        public Material purple;
        public Material red;
        public Material white;
        public Material yellow;
        public Material yellowDark;

        private bool autoClose;
        private bool initialized;

        private float emoticonsCloseTime = 2f;
        private float emoticonsCloseTimer = 0f;
        private bool emoticonsClosed;
        private Emoticons? currentEmoticon;

        private Material iconMaterial;
        private Material internalMaterial;
        private Material externalMaterial;
        private Material cineticMaterial;

        void Initialize()
        {
            if (initialized)
            {
                return;
            }

            initialized = true;

            var parent = new GameObject("EmoticonsParent");
            parent.transform.SetParent(transform);
            parent.transform.localPosition = Vector3.zero;

            emoticonsController = Object.Instantiate(emoticonsPrefab).GetComponent<EmoticonsController>();
            emoticonsController.transform.SetParent(parent.transform);
            emoticonsController.transform.localPosition = new Vector3(0f, 3f);

            parent.transform.localScale = new Vector3(3f, 3f, 3f);

            CloseEmoticons();
        }

        void Update()
        {
            if (!initialized)
            {
                return;
            }

            if (!emoticonsClosed && autoClose)
            {
                emoticonsCloseTimer -= Time.deltaTime;

                if (emoticonsCloseTimer <= 0f)
                {
                    CloseEmoticons();
                }
            }
        }

        public void DoHappy()
        {
            Initialize();

            internalMaterial = blue;
            iconMaterial = purple;
            cineticMaterial = purple;
            externalMaterial = orange;

            ShowEmoticons(Emoticons.vfx_emo_happy);

            autoClose = true;
        }

        public void DoCorrect()
        {
            Initialize();

            internalMaterial = green;
            iconMaterial = white;
            cineticMaterial = yellow;
            externalMaterial = orange;

            ShowEmoticons(Emoticons.vfx_emo_positive);

            autoClose = true;
        }

        public void DoWrong()
        {
            Initialize();

            internalMaterial = red;
            iconMaterial = white;
            cineticMaterial = black;
            externalMaterial = orange;

            ShowEmoticons(Emoticons.vfx_emo_negative);

            autoClose = true;
        }

        public void DoInterrogative()
        {
            Initialize();

            internalMaterial = blue;
            iconMaterial = white;
            cineticMaterial = orange;
            externalMaterial = yellowDark;

            ShowEmoticons(Emoticons.vfx_emo_interrogative);

            autoClose = false;
        }

        void ShowEmoticons(Emoticons icon)
        {
            if (!currentEmoticon.HasValue || (currentEmoticon.HasValue && currentEmoticon.Value != icon))
            {
                currentEmoticon = icon;
                emoticonsController.SetEmoticon(icon, true);
                UpdateEmoticonsColor();
            }

            emoticonsCloseTimer = emoticonsCloseTime;
            emoticonsClosed = false;
        }

        public void CloseEmoticons()
        {
            Initialize();

            emoticonsController.Open(false);
            emoticonsCloseTimer = 0f;
            emoticonsClosed = true;
            currentEmoticon = null;
        }

        void UpdateEmoticonsColor()
        {
            ChangeMaterials(iconMaterial, GetIconMeshRenderer());
            ChangeMaterials(internalMaterial, emoticonsController.Internal);
            ChangeMaterials(externalMaterial, emoticonsController.External);
            ChangeMaterials(cineticMaterial, emoticonsController.Cinetic);
        }

        MeshRenderer[] GetIconMeshRenderer()
        {
            MeshRenderer[] meshRenderer = new MeshRenderer[emoticonsController.EmoticonParentBone.childCount];

            for (var i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i] = emoticonsController.EmoticonParentBone.GetChild(i).GetComponent<MeshRenderer>();
            }

            return meshRenderer;
        }

        void ChangeMaterials(Material _material, MeshRenderer[] _meshRenderer)
        {
            foreach (var item in _meshRenderer)
            {
                var m = item.gameObject.GetComponent<MeshRenderer>();
                m.materials = new Material[] { _material };
            }
        }

        void ChangeMaterials(Material _material, SkinnedMeshRenderer[] _meshRenderer)
        {
            foreach (var item in _meshRenderer)
            {
                var m = item.gameObject.GetComponent<SkinnedMeshRenderer>();
                m.materials = new Material[] { _material };
            }
        }
    }
}
