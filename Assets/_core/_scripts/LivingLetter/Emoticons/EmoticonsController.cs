using Antura.Rewards;
using UnityEngine;
using DG.Tweening;

namespace Antura.LivingLetters
{
    public class EmoticonsController : MonoBehaviour
    {
        const string EMOTICON_PREFS_PATH = "Prefabs/Emoticons/";

        [Header("Emoticon icon type")]
        public Emoticons Icon = Emoticons.vfx_emo_happy;

        [Header("Container internal color")]
        public PaletteColors Color1 = PaletteColors.white;

        public PaletteTone Tone1 = PaletteTone.light;

        [Header("Container external color")]
        public PaletteColors Color2 = PaletteColors.white;

        public PaletteTone Tone2 = PaletteTone.light;

        [Header("Container cinetic lines color")]
        public PaletteColors Color3 = PaletteColors.white;

        public PaletteTone Tone3 = PaletteTone.light;

        [Header("Components")]
        public Animator anim;

        public Transform EmoticonParentBone;

        public SkinnedMeshRenderer[] Internal, External, Cinetic;

        void Awake()
        {
            transform.DOScale(0, 0);
            SetEmoticon(Icon, false);
        }

        void CleanEmoticonIcons()
        {
            foreach (Transform child in EmoticonParentBone)
            {
                Destroy(child.gameObject);
            }
        }

        void scaleToOneAllChildren(Transform _parent)
        {
            foreach (Transform t in _parent.GetComponentsInChildren<Transform>())
            {
                t.localScale = Vector3.one;
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

        public void SetEmoticon(Emoticons _emoticons, bool _open = false)
        {
            GameObject Et;
            CleanEmoticonIcons();
            switch (_emoticons)
            {
                case Emoticons.vfx_emo_angry:
                case Emoticons.vfx_emo_exclamative:
                case Emoticons.vfx_emo_happy:
                case Emoticons.vfx_emo_interrogative:
                case Emoticons.vfx_emo_negative:
                case Emoticons.vfx_emo_positive:
                    Et = Instantiate(Resources.Load(EMOTICON_PREFS_PATH + _emoticons.ToString()), EmoticonParentBone, false) as GameObject;
                    Debug.Assert(Et != null, "Emoticon was not instanced");
                    break;
                default:
                    Debug.LogWarningFormat("Emoticons {0} not found!", _emoticons.ToString());
                    break;
            }

            if (_open)
            {
                Open(true);
            }
        }


        void Update()
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Open(true);
            }

            if (Input.GetKeyDown(KeyCode.R))
            {
                Open(false);
            }
        }

        #region API

        public void Open(bool _isOpen)
        {
            if (_isOpen)
            {
                transform.DOScale(1, 0.1f);
            }

            anim.SetBool("IsOpen", _isOpen);

            if (!_isOpen)
            {
                transform.DOScale(0, 0.1f);
            }
        }

        #endregion
    }
}
