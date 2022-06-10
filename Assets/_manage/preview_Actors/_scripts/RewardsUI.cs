using Antura.Dog;
using Antura.Rewards;
using Antura.Tutorial;
using DG.Tweening;
using System.Collections.Generic;
using System.Linq;
using Antura.Core;
using UnityEngine;
using UnityEngine.UI;
using System;

namespace Antura
{

    // refactor: why is this in the Test scripts?s
    public class RewardsUI : MonoBehaviour
    {

        [Header("Rewards elements")]
        public GridLayoutGroup ElementContainer;
        public GameObject ElementPrefab;

        [HideInInspector]
        public string RewardTypeFilter = "";

        public GameObject AnturaPJ;
        private RewardSystemManager RewardSystem;

        /// <summary>
        /// The actual reward enabled for material modification.
        /// </summary>
        private RewardProp actualReward;
        //private RewardBase actualReward;

        private GameObject actualRewardGO;

        void Awake()
        {
            ElementContainer = GetComponentInChildren<GridLayoutGroup>();
        }

        void Start()
        {
            ClearList();
            AddListenersMatColor1();
            AddListenersMatColor2();
            SetMaterial1("white_dark");
            SetMaterial2("white_pure");

            RewardSystem = new RewardSystemManager();
            RewardSystem.Init();

            actualRewardGO = AnturaPJ;
        }

        void Update()
        {
            if (AnturaModelManager.I.transformParent != null) {
                Camera.main.transform.LookAt(AnturaModelManager.I.transformParent.position);
            } else {
                Camera.main.transform.LookAt(AnturaModelManager.I.transform.position);
            }
        }

        #region Rewards

        void ClearList()
        {
            foreach (Button b in ElementContainer.GetComponentsInChildren<Button>()) {
                b.onClick.RemoveAllListeners();
                Destroy(b.gameObject);
            }

        }

        void LoadRewardsList(string _position = "")
        {
            ClearList();
            IEnumerable<RewardBase> rewardBases;
            var allPropBases = AppManager.I.RewardSystemManager.GetRewardBasesOfType(RewardBaseType.Prop);
            if (_position != "") {
                rewardBases = allPropBases.Where(r => (r as RewardProp).BoneAttach == _position);
            } else {
                rewardBases = allPropBases;
            }

            foreach (RewardBase rewardBase in rewardBases)
            {
                RewardProp rewardProp = rewardBase as RewardProp;
                Button b = Instantiate<Button>(ElementPrefab.GetComponent<Button>(), ElementContainer.transform, false);
                b.GetComponentInChildren<Text>().text = rewardProp.RewardName;
                b.onClick.AddListener(delegate { OnClickButton(b.GetComponentInChildren<Text>().text); });
            }
        }

        /// <summary>
        /// Delegate function for button click.
        /// </summary>
        /// <param name="_name">The name.</param>
        void OnClickButton(string _name)
        {
            LoadRewardOnDog(_name);
        }

        /// <summary>
        /// Loads the reward on dog.
        /// </summary>
        /// <param name="_name">The name.</param>
        void LoadRewardOnDog(string _name)
        {
            actualReward = null;

            //actualReward = RewardSystemManager.GetConfig().Rewards.Find(r => r.RewardName == _name); //OLD LINE
            //block01: replaces the previous non-working old line (ajzbrun 09-06-22)
            List<RewardBase> allRewards = RewardSystem.GetRewardBasesOfType(RewardBaseType.Prop).ToList();
            List<RewardProp> allRew = new List<RewardProp>();
            foreach(RewardBase rw in allRewards) //conversion from RewardBase to RewardProp, which actually has the rewardname
            {
                RewardProp rw_aux = rw as RewardProp;
                allRew.Add(rw_aux);
            }
            actualReward = allRew.Find(r => r.RewardName == _name);

            AnturaPJ.GetComponent<AnturaModelManager>().ClearLoadedRewardInCategory(actualReward.Category);
            //end block01

            //actualRewardGO = AnturaModelManager.Instance.LoadRewardOnAntura(actualReward.ID); //OLD LINE
            //block02: replaces the previous non-working old line (ajzbrun 09-06-22)
            RewardColor rw_color = new RewardColor();
            foreach (var color in actualRewardGO.GetComponentsInChildren<MeshRenderer>())
            {
                if (color.name == "color_1")
                {
                    //color.materials = new Material[] { MaterialManager.LoadMaterial(material1, (PaletteType)Enum.Parse(typeof(PaletteType), actualReward.Material1)) }; //OLD LINE
                    rw_color.Color1RGB = ActiveMaterial1Image.material.color.ToString();
                    rw_color.Color1Name = ActiveMaterial1Image.material.name;
                }
                else if (color.name == "color_2")
                {
                    //color.materials = new Material[] { MaterialManager.LoadMaterial(material2, (PaletteType)Enum.Parse(typeof(PaletteType), actualReward.Material2)) }; //OLD LINE
                    rw_color.Color2RGB = ActiveMaterial2Image.material.color.ToString();
                    rw_color.Color2Name = ActiveMaterial2Image.material.name;
                }
                rw_color.LocID = actualReward.LocID;
                rw_color.ID = actualReward.ID;
            }
            RewardPack rp = new RewardPack(RewardBaseType.Prop, allRewards.Find(r => r.ID == actualReward.ID), rw_color);
            actualRewardGO = AnturaPJ.GetComponent<AnturaModelManager>().LoadRewardPackOnAntura(rp);
            //block02
        }

        public void ClearAllRewards()
        {
            AnturaPJ.GetComponent<AnturaModelManager>().ClearLoadedRewardsWithoutDestroy();
        }

        #endregion

        #region Reward type filter

        public void SetRewardTypeFilter(string _filterString)
        {
            RewardTypeFilter = _filterString;
            LoadRewardsList(_filterString);

            switch (_filterString) {
                case "dog_head":
                    doMoveCamera(new Vector3(10.0f, 14.0f, -10.0f));
                    break;
                case "dog_spine01":
                    doMoveCamera(new Vector3(13.0f, 13.0f, -10.0f));
                    break;
                case "dog_jaw":
                    doMoveCamera(new Vector3(8.0f, 8.0f, -12.0f));
                    break;
                case "dog_Tail4":
                    doMoveCamera(new Vector3(-12.0f, 14.0f, 8.0f));
                    break;
                case "dog_R_ear04":
                    doMoveCamera(new Vector3(-8.0f, 9.0f, -8.0f));
                    break;
                case "dog_L_ear04":
                    doMoveCamera(new Vector3(8.0f, 9.0f, -8.0f));
                    break;
            }
        }

        #endregion

        #region Materials

        [Header("Material Lists")]
        public GridLayoutGroup MaterialGrid1;
        public GridLayoutGroup MaterialGrid2;

        public Image ActiveMaterial1Image;
        public Image ActiveMaterial2Image;

        public void SetMaterial1(string _materialName)
        {
            ActiveMaterial1Image.material = MaterialManager.LoadMaterial(_materialName, PaletteType.specular_saturated_2side);
            if (actualReward != null)
                LoadRewardOnDog(actualReward.ID);
        }

        public void SetMaterial2(string _materialName)
        {
            ActiveMaterial2Image.material = MaterialManager.LoadMaterial(_materialName, PaletteType.specular_saturated_2side);
            if (actualReward != null)
                LoadRewardOnDog(actualReward.ID);
        }

        void AddListenersMatColor1()
        {
            foreach (Button b in MaterialGrid1.GetComponentsInChildren<Button>()) {
                string selectedButtonName = b.GetComponent<Image>().material.name;
                b.name = selectedButtonName;
                b.onClick.AddListener(delegate {
                    SetMaterial1(selectedButtonName);
                });
            }
        }
        void AddListenersMatColor2()
        {
            foreach (Button b in MaterialGrid2.GetComponentsInChildren<Button>()) {
                string selectedButtonName = b.GetComponent<Image>().material.name;
                b.name = selectedButtonName;
                b.onClick.AddListener(delegate {
                    SetMaterial2(selectedButtonName);
                });
            }
        }

        #endregion

        #region Camera

        void doMoveCamera(Vector3 _position)
        {
            float duration = 2;
            Camera.main.transform.DOMove(_position, duration);

        }

        #endregion
    }
}
