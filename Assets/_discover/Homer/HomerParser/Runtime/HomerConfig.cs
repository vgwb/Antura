using System.Collections;
using System.Collections.Generic;
using System;
using System.Linq;
using UnityEngine;
using DG.DeInspektor.Attributes;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public void PermalinkList()
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
        public void PermalinkCheck()
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
            Debug.Log(notUsedPermalinks.Count() + " not used permalinks" + "\n\n" + output);

            // Check if all used NodePermalink in Interactable exist in the permalinks
            var permalinkSet = permalinks.ToHashSet();
            foreach (var interactable in interactables)
            {
                if (!string.IsNullOrEmpty(interactable.NodePermalink) && !permalinkSet.Contains(interactable.NodePermalink))
                {
                    Debug.LogError($"\nWRONG permalink: {interactable.NodePermalink} in {interactable.gameObject.name}");
                }
            }
        }

        /// <summary>
        /// Returns a dictionary of task metadata and a HashSet of task IDs for the selected flow.
        /// </summary>
        private void GetTaskMetaDictAndIds(out Dictionary<string, JObject> metaDict, out HashSet<string> taskIds)
        {
            metaDict = new Dictionary<string, JObject>();
            taskIds = new HashSet<string>();

            var project = JObject.Parse(Homer.text);

            // Build a dictionary of Objective metadata values by ID
            foreach (var meta in project["_metadata"] as JArray)
            {
                if ((string)meta["_uid"] == "TASK")
                {
                    foreach (var val in meta["_values"] as JArray)
                    {
                        metaDict[(string)val["_id"]] = (JObject)val;
                    }
                }
            }

            // Find the selected flow by slug
            var flows = project["_flows"] as JArray;
            var flow = flows?.FirstOrDefault(f => (string)f["_slug"] == SelectedFlow.ToString());
            if (flow == null)
            {
                Debug.LogWarning("Flow not found: " + SelectedFlow);
                return;
            }

            // Find all objective IDs in nodes
            foreach (var node in flow["_nodes"] as JArray)
            {
                var metadata = node["_metadata"] as JArray;
                if (metadata == null)
                    continue;
                foreach (var metaId in metadata)
                {
                    var id = (string)metaId;
                    if (id.StartsWith("MV-") && metaDict.ContainsKey(id))
                    {
                        taskIds.Add(id);
                    }
                }
            }
        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void TaskList()
        {
            GetTaskMetaDictAndIds(out var metaDict, out var tasks);

            // Output resolved objectives
            if (tasks.Count == 0)
            {
                Debug.Log("No Objectives found in flow: " + SelectedFlow);
            }
            else
            {
                var output = "TOTAL: " + tasks.Count() + " Objectives in Quest " + SelectedFlow + ":\n";
                foreach (var id in tasks)
                {
                    var meta = metaDict[id];
                    output += $"- {meta["_value"]}\n";// ({meta["_uid"]}) [{id}]\n";
                }
                Debug.Log(output);
            }
        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void TaskCheck()
        {
            GetTaskMetaDictAndIds(out var metaDict, out var tasks);

            // Find QuestManager in the scene
            var questManagers = FindObjectsByType<QuestManager>(FindObjectsSortMode.None);
            var questManager = questManagers.Length > 0 ? questManagers[0] : null;
            if (questManager == null || questManager.QuestTasks == null)
            {
                Debug.LogError("QuestManager or QuestTasks not found in the scene!");
                return;
            }

            var usedCodes = new HashSet<string>();
            var missingCodes = new List<string>();

            // Check for codes used but not defined
            foreach (var task in questManager.QuestTasks)
            {
                if (task == null || string.IsNullOrEmpty(task.Code))
                    continue;
                usedCodes.Add(task.Code);
                if (!metaDict.ContainsKey(task.Code))
                {
                    missingCodes.Add(task.Code);
                }
            }

            // Check for codes defined but not used
            var unusedCodes = new List<string>();
            foreach (var code in metaDict.Keys)
            {
                if (!usedCodes.Contains(code))
                {
                    unusedCodes.Add(code);
                }
            }

            // Output results
            if (missingCodes.Count == 0 && unusedCodes.Count == 0)
            {
                Debug.Log("All QuestTasks codes are valid and all defined codes are used.");
            }
            else
            {
                if (missingCodes.Count > 0)
                {
                    Debug.LogWarning("QuestTask codes used but NOT defined in JSON:\n" + string.Join("\n", missingCodes));
                }
                if (unusedCodes.Count > 0)
                {
                    Debug.LogWarning("Task codes defined in JSON but NOT used in QuestTasks:\n" + string.Join("\n", unusedCodes));
                }
            }
        }

        [DeMethodButton(mode = DeButtonMode.Default)]
        public void ActionsList()
        {
            var project = JObject.Parse(Homer.text);

            // Find the selected flow by slug
            var flows = project["_flows"] as JArray;
            var flow = flows?.FirstOrDefault(f => (string)f["_slug"] == SelectedFlow.ToString());
            if (flow == null)
            {
                Debug.LogWarning("Flow not found: " + SelectedFlow);
                return;
            }

            // Build a dictionary of Action metadata values by ID (ACTION and ACTION_POST)
            var actionDict = new Dictionary<string, JObject>();
            foreach (var meta in project["_metadata"] as JArray)
            {
                if ((string)meta["_uid"] == "ACTION" || (string)meta["_uid"] == "ACTION_POST")
                {
                    foreach (var val in meta["_values"] as JArray)
                    {
                        actionDict[(string)val["_id"]] = (JObject)val;
                    }
                }
            }

            // Find all action IDs in nodes
            var actions = new HashSet<string>();
            foreach (var node in flow["_nodes"] as JArray)
            {
                var metadata = node["_metadata"] as JArray;
                if (metadata == null)
                    continue;
                foreach (var metaId in metadata)
                {
                    var id = (string)metaId;
                    if ((id.StartsWith("MTV-") || id.StartsWith("MV-")) && actionDict.ContainsKey(id))
                    {
                        actions.Add(id);
                    }
                }
            }

            // Output resolved actions
            if (actions.Count == 0)
            {
                Debug.Log("No Actions found in flow: " + SelectedFlow);
            }
            else
            {
                var output = "TOTAL: " + actions.Count() + " Actions in Quest " + SelectedFlow + ":\n";

                foreach (var id in actions)
                {
                    var meta = actionDict[id];
                    output += $"- {meta["_value"]}\n"; // ({meta["_uid"]}) [{id}]\n";
                }
                Debug.Log(output);
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
