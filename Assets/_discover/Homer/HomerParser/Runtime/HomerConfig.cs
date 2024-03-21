using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Homer
{
    public class HomerConfig : MonoBehaviour
    {
        //THIS MUST BE CONFIGURED BY HAND: DRAG Homer/ProjectData/Homer(.json) HERE!!!
        public TextAsset Homer;
        
        public static HomerConfig I;
        
        void Awake()
        {
            if (I == null)
            {
                I = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Debug.Log("HomerConfig DUPLICATED!");
                Destroy(gameObject);
            }
        }
        
        void Start()
        {
        
        }

    }
}
