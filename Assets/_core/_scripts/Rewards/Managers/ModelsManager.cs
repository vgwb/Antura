using System.Linq;
using Antura.Core;
using Antura.Dog;
using UnityEngine;

namespace Antura.Rewards
{
    public static class ModelsManager
    {
        public const string ANTURA_REWARDS_PREFABS_PATH = "Rewards/";

        #region API

        public static GameObject MountModel(AnturaPetType petType, string _id, Transform _parent, ModelType _type = ModelType.AnturaForniture, bool checkExisting = false)
        {
            //Debug.Log("Mounting model " + _id + " on " + _parent.name);
            string resourceToLoadPath;
            switch (_type)
            {
                case ModelType.AnturaForniture:
                    resourceToLoadPath = ANTURA_REWARDS_PREFABS_PATH;
                    break;
                default:
                    return null;
            }

            var prefab = Resources.Load($"{petType}/{resourceToLoadPath}{_id}");

            if (_parent.childCount > 0 && checkExisting)
            {
                Transform spawnedChild = null;
                foreach (Transform childTr in _parent.transform)
                {
                    if (childTr == _parent) continue;
                    if (!childTr.gameObject.name.Contains("(Clone)")) continue;
                    spawnedChild = childTr;
                    break;
                }

                if (spawnedChild != null)
                {
                    if (spawnedChild.name.Contains(prefab.name))
                    {
                        // Already loaded
                        return spawnedChild.gameObject;
                    }
                    else
                    {
                        // Wrong one, destroy it
                        GameObject.Destroy(spawnedChild.gameObject);
                    }
                }
            }


            var rewardModel = GameObject.Instantiate(prefab) as GameObject;
            rewardModel.transform.SetParent(_parent, false);
            return rewardModel;
        }

        public static GameObject MountModel(AnturaPetType petType, string _id, Transform _parent, MaterialPair _materialPair,
            ModelType _type = ModelType.AnturaForniture)
        {
            CleanTranformChildren(_parent);
            GameObject returnObject = MountModel(petType, _id, _parent, _type);
            SwitchMaterial(returnObject, _materialPair);
            return returnObject;
        }

        public static MaterialPair SwitchMaterial(GameObject _gameObject, MaterialPair _materialPair)
        {
            if (_materialPair.Material1 == null || _materialPair.Material2 == null)
            { return _materialPair; }
            foreach (var color in _gameObject.GetComponentsInChildren<MeshRenderer>())
            {
                if (color.name == "color_1")
                {
                    color.materials = new Material[] { _materialPair.Material1 };
                }
                else if (color.name == "color_2")
                {
                    color.materials = new Material[] { _materialPair.Material2 };
                }
            }
            foreach (var color in _gameObject.GetComponentsInChildren<SkinnedMeshRenderer>())
            {
                if (color.name == "color_1")
                {
                    color.materials = new Material[] { _materialPair.Material1 };
                }
                else if (color.name == "color_2")
                {
                    color.materials = new Material[] { _materialPair.Material2 };
                }
            }
            return _materialPair;
        }

        public static void CleanTranformChildren(Transform _parent)
        {
            for (int i = _parent.childCount - 1; i >= 0; --i)
            {
                var child = _parent.GetChild(i).gameObject;
                GameObject.Destroy(child);
            }
        }

        #endregion


        public enum ModelType
        {
            AnturaForniture,
        }
    }
}
