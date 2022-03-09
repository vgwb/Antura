using UnityEngine;

namespace Antura.Minigames.Maze
{
    public class MazeLLId : MonoBehaviour
    {

        public string getLLId()
        {
            string name = gameObject.name;
            int cloneIndex = name.IndexOf("(Clone");

            if (cloneIndex != -1)
            {
                name = name.Substring(0, cloneIndex);
            }
            return name;
        }
    }
}
