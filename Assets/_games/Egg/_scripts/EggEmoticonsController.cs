using Antura.LivingLetters;
using UnityEngine;

namespace Antura.Minigames.Egg
{
    public class EggEmoticonsController
    {
        private EmoticonsController emoticonsController;
        private EggEmoticonsMaterials eggEmoticonsMaterials;

        private bool autoClose;

        private float emoticonsCloseTime = 2f;
        private float emoticonsCloseTimer = 0f;
        private bool emoticonsClosed;
        private Emoticons? currentEmoticon;

        private Material iconMaterial;
        private Material internalMaterial;
        private Material externalMaterial;
        private Material cineticMaterial;

        public EggEmoticonsController(Transform parent, GameObject emoticonsPrefab, EggEmoticonsMaterials eggEmoticonsMaterials)
        {
            emoticonsController = Object.Instantiate(emoticonsPrefab).GetComponent<EmoticonsController>();
            this.eggEmoticonsMaterials = eggEmoticonsMaterials;

            emoticonsController.transform.SetParent(parent);
            emoticonsController.transform.localPosition = new Vector3(0f, 3f);
            emoticonsController.SetEmoticon(Emoticons.vfx_emo_exclamative, true);

            parent.localScale = new Vector3(3f, 3f, 3f);

            CloseEmoticons();
        }

        public void Update(float delta)
        {
            if (!emoticonsClosed && autoClose)
            {
                emoticonsCloseTimer -= delta;

                if (emoticonsCloseTimer <= 0f)
                {
                    CloseEmoticons();
                }
            }
        }

        public void EmoticonHappy()
        {
            internalMaterial = eggEmoticonsMaterials.blue;
            iconMaterial = eggEmoticonsMaterials.purple;
            cineticMaterial = eggEmoticonsMaterials.purple;
            externalMaterial = eggEmoticonsMaterials.orange;

            OpenEmoticons(Emoticons.vfx_emo_happy);

            autoClose = true;
        }

        public void EmoticonPositive()
        {
            internalMaterial = eggEmoticonsMaterials.green;
            iconMaterial = eggEmoticonsMaterials.white;
            cineticMaterial = eggEmoticonsMaterials.yellow;
            externalMaterial = eggEmoticonsMaterials.orange;

            OpenEmoticons(Emoticons.vfx_emo_positive);

            autoClose = true;
        }

        public void EmoticonNegative()
        {
            internalMaterial = eggEmoticonsMaterials.red;
            iconMaterial = eggEmoticonsMaterials.white;
            cineticMaterial = eggEmoticonsMaterials.black;
            externalMaterial = eggEmoticonsMaterials.orange;

            OpenEmoticons(Emoticons.vfx_emo_negative);

            autoClose = true;
        }

        public void EmoticonInterrogative()
        {
            internalMaterial = eggEmoticonsMaterials.blue;
            iconMaterial = eggEmoticonsMaterials.white;
            cineticMaterial = eggEmoticonsMaterials.orange;
            externalMaterial = eggEmoticonsMaterials.yellowDark;

            OpenEmoticons(Emoticons.vfx_emo_interrogative);

            autoClose = false;
        }

        void OpenEmoticons(Emoticons icon)
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
            emoticonsController.Open(false);
            emoticonsCloseTimer = 0f;
            emoticonsClosed = true;
            currentEmoticon = null;
        }

        void UpdateEmoticonsColor()
        {
            changeMaterials(iconMaterial, getIconMeshRenderer());
            changeMaterials(internalMaterial, emoticonsController.Internal);
            changeMaterials(externalMaterial, emoticonsController.External);
            changeMaterials(cineticMaterial, emoticonsController.Cinetic);
        }

        MeshRenderer[] getIconMeshRenderer()
        {
            MeshRenderer[] meshRenderer = new MeshRenderer[emoticonsController.EmoticonParentBone.childCount];

            for (int i = 0; i < meshRenderer.Length; i++)
            {
                meshRenderer[i] = emoticonsController.EmoticonParentBone.GetChild(i).GetComponent<MeshRenderer>();
            }

            return meshRenderer;
        }

        void changeMaterials(Material _material, MeshRenderer[] _meshRenderer)
        {
            foreach (var item in _meshRenderer)
            {
                MeshRenderer m = item.gameObject.GetComponent<MeshRenderer>();
                m.materials = new Material[] { _material };
            }
        }

        void changeMaterials(Material _material, SkinnedMeshRenderer[] _meshRenderer)
        {
            foreach (var item in _meshRenderer)
            {
                SkinnedMeshRenderer m = item.gameObject.GetComponent<SkinnedMeshRenderer>();
                m.materials = new Material[] { _material };
            }
        }
    }
}
