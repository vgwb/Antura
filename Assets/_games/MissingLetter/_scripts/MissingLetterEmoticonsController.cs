using Antura.LivingLetters;
using UnityEngine;
using UnityEngine.Assertions;

namespace Antura.Minigames.MissingLetter
{
    public class MissingLetterEmoticonsController
    {

        #region API
        public MissingLetterEmoticonsController(GameObject _EmoticonsController, MissingLetterEmoticonsMaterials missingLetterEmoticonsMaterials)
        {
            Assert.IsNotNull<EmoticonsController>(_EmoticonsController.GetComponent<EmoticonsController>(), "Please attach the EmoticonsController script to the prefab : " + _EmoticonsController.name);
            emoticonsController = _EmoticonsController.GetComponent<EmoticonsController>();
            this.missingLetterEmoticonsMaterials = missingLetterEmoticonsMaterials;
        }

        public void init(Transform parent)
        {
            emoticonsController.transform.SetParent(parent);
            emoticonsController.transform.localPosition = new Vector3(0, 8f);
            emoticonsController.transform.localScale = new Vector3(3f, 3f, 3f);
            emoticonsController.transform.forward = Vector3.back;

            CloseEmoticons();
        }

        public void EmoticonPositive()
        {
            internalMaterial = missingLetterEmoticonsMaterials.green;
            iconMaterial = missingLetterEmoticonsMaterials.white;
            cineticMaterial = missingLetterEmoticonsMaterials.yellow;
            externalMaterial = missingLetterEmoticonsMaterials.orange;

            OpenEmoticons(Emoticons.vfx_emo_positive);

            //autoClose = true;
        }

        public void EmoticonNegative()
        {
            internalMaterial = missingLetterEmoticonsMaterials.red;
            iconMaterial = missingLetterEmoticonsMaterials.white;
            cineticMaterial = missingLetterEmoticonsMaterials.black;
            externalMaterial = missingLetterEmoticonsMaterials.orange;

            OpenEmoticons(Emoticons.vfx_emo_negative);

            //autoClose = true;
        }

        public void CloseEmoticons()
        {
            emoticonsController.Open(false);

            //emoticonsClosed = true;
            currentEmoticon = null;
        }
        #endregion

        #region PRIVATE_FUNCTION
        void OpenEmoticons(Emoticons icon)
        {
            if (!currentEmoticon.HasValue || (currentEmoticon.HasValue && currentEmoticon.Value != icon))
            {
                currentEmoticon = icon;
                emoticonsController.Open(false);
                UpdateEmoticonsColor();
                emoticonsController.SetEmoticon(icon, true);
            }

            //emoticonsClosed = false;
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
        #endregion

        #region VARS
        EmoticonsController emoticonsController;
        MissingLetterEmoticonsMaterials missingLetterEmoticonsMaterials;

        //bool autoClose;
        //bool emoticonsClosed;
        Emoticons? currentEmoticon;

        Material iconMaterial;
        Material internalMaterial;
        Material externalMaterial;
        Material cineticMaterial;
        #endregion
    }
}
