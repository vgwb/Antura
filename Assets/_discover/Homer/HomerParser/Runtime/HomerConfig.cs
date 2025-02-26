using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using DG.DeInspektor.Attributes;
using Newtonsoft.Json;
using Antura.Minigames.DiscoverCountry;

namespace Homer
{
    public class HomerConfig : MonoBehaviour
    {
        //THIS MUST BE CONFIGURED BY HAND: DRAG Homer/ProjectData/Homer(.json) HERE!!!
        public TextAsset Homer;

        public HomerFlowSlugs.FlowSlug SelectedFlow;

        public static HomerConfig I;

        void Awake()
        {
            if (I == null)
            {
                I = this;
                //                DontDestroyOnLoad(gameObject);
            }
            else
            {
                //Debug.Log("HomerConfig DUPLICATED!");
                //                Destroy(gameObject);
            }
        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void ListAllPermalinks()
        {
            HomerProject project = JsonConvert.DeserializeObject<HomerProject>(Homer.text);
            var permalinks = GetPermalinksBySlug(project._flows, SelectedFlow.ToString());

            var output = "";
            foreach (string permalink in permalinks)
            {
                output += permalink + "\n";
            }
            Debug.Log("TOTAL: " + permalinks.Count() + " permalinks in Quest " + SelectedFlow + "\n\n" + output);

        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void CheckNotUsedPermalinks()
        {
            HomerProject project = JsonConvert.DeserializeObject<HomerProject>(Homer.text);
            var permalinks = GetPermalinksBySlug(project._flows, SelectedFlow.ToString());

            // Find all Interactable components in the scene
            Interactable[] interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);

            // Check if any NodePermalink in Interactable matches the permalinks
            var usedPermalinks = interactables.Where(i => !string.IsNullOrEmpty(i.NodePermalink)).Select(i => i.NodePermalink).ToHashSet();
            var notUsedPermalinks = permalinks.Where(p => !usedPermalinks.Contains(p)).ToArray();

            var output = "";
            foreach (var permalink in notUsedPermalinks)
            {
                output += permalink + "\n";
            }
            Debug.Log(notUsedPermalinks.Count() + " not used permalinks of Quest " + SelectedFlow + "\n\n" + output);
        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void VerifyUsedPermalinks()
        {
            Debug.Log("Verify Used Permalinks of Quest " + SelectedFlow);
            HomerProject project = JsonConvert.DeserializeObject<HomerProject>(Homer.text);
            var permalinks = GetPermalinksBySlug(project._flows, SelectedFlow.ToString());

            // Find all Interactable components in the scene
            Interactable[] interactables = FindObjectsByType<Interactable>(FindObjectsSortMode.None);

            // Check if all used NodePermalink in Interactable exist in the permalinks
            var permalinkSet = permalinks.ToHashSet();
            foreach (var interactable in interactables)
            {
                if (!string.IsNullOrEmpty(interactable.NodePermalink) && !permalinkSet.Contains(interactable.NodePermalink))
                {
                    Debug.LogError($"WRONG permalink: {interactable.NodePermalink} in {interactable.gameObject.name}");
                }
            }
        }

        private string[] GetPermalinksBySlug(HomerFlow[] _flows, string slug)
        {
            var flow = _flows.FirstOrDefault(f => f._slug == slug);
            if (flow == null)
            {
                return new string[0];
            }
            return flow._nodes
                .Where(node => !string.IsNullOrEmpty(node._permalink))
                .Select(node => node._permalink)
                .OrderBy(permalink => permalink)
                .ToArray();
        }

    }
}
