using System;
using System.Collections;
using System.Collections.Generic;
using Antura.Discover.Interaction;
using UnityEngine;
using Antura.UI;

namespace Antura.Discover
{
    public class InfoPoint : MonoBehaviour
    {
        [Header("References")]
        public TextRender Label;
        [Header("Content")]
        public string Text;

        void Start()
        {
            if (Text != "")
            {
                Label.SetText(Text);
            }
        }
    }
}
