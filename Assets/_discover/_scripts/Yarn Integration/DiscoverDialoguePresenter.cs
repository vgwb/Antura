using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;
using System.Linq;

namespace Antura.Discover
{
    public class DiscoverDialoguePresenter : DialoguePresenterBase
    {
        public YarnAnturaManager Manager { get; set; }

        private QuestNode previousQuestNode;
        private bool _hasPendingOptions;
        public bool HasPendingOptions => _hasPendingOptions;

        private YarnTaskCompletionSource<DialogueOption> _optionTCS;
        private CancellationTokenSource _optionCTS;
        private DialogueOption[] _currentOptions;

        private string _currentNodeName;

        public void SetCurrentNodeName(string nodeName)
        {
            _currentNodeName = nodeName;
        }

        /// <summary>
        /// Programmatically select an option by index (used by YarnAnturaManager.NextNode).
        /// </summary>
        public void SelectOption(int index)
        {
            if (!_hasPendingOptions || _optionTCS == null || _currentOptions == null || _currentOptions.Length == 0)
                return;

            index = Mathf.Clamp(index, 0, _currentOptions.Length - 1);
            _optionTCS.TrySetResult(_currentOptions[index]);
            _optionCTS?.Cancel();
            _hasPendingOptions = false;
        }

        public override YarnTask OnDialogueStartedAsync()
        {
            // Signal UI that a dialogue session started (open once)
            DiscoverNotifier.Game.OnStartDialogue.Dispatch();
            return YarnTask.CompletedTask;
        }

        public override YarnTask OnDialogueCompleteAsync()
        {
            // Close the dialogue UI once at the end
            UIManager.I?.dialogues?.CloseDialogue();
            return YarnTask.CompletedTask;
        }

        public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
        {
            // Called by the Dialogue Runner to signal that a line of dialogue
            // should be shown to the player.
            //
            // If your dialogue views handles lines, it should take the 'line'
            // parameter and use the information inside it to present the content to
            // the player, in whatever way makes sense.
            //
            // Some useful information:
            // - The 'Text' property in 'line' contains the parsed, localised text
            //   of the line, including attributes and text.
            // - The 'TextWithoutCharacterName' property contains all of the text
            //   after the character name in the line (if present), and the
            //   'CharacterName' contains the character name (if present).
            // - The 'Asset' property contains whatever object was associated with
            //   this line, as provided by your Dialogue Runner's Line Provider.
            //
            // The LineCancellationToken contains information on whether the
            // Dialogue Runner wants this Dialogue View to hurry up its
            // presentation, or to advance to the next line.
            //
            // - If 'token.IsHurryUpRequested' is true, that's a hint that your view
            //   should speed up its delivery of the line, if possible (for example,
            //   by displaying text faster).
            // - If 'token.IsNextLineRequested' is true, that's an instruction that
            //   your view must end its presentation of the line as fast as possible
            //   (even if that means ending the delivery early.)
            //
            // The Dialogue Runner will wait for all Dialogue Views to return from
            // this method before delivering new content.
            //
            // If your Dialogue View doesn't need to handle lines, simply return
            // from this method immediately.

            var nodeType = Manager.Runner.Dialogue.GetHeaderValue(_currentNodeName, "type");
            // Debug.Log($"Node {_currentNodeName} has type {nodeType}");

            NodeType type = NodeType.TEXT;
            if (nodeType == "panel_endgame")
                type = NodeType.PANEL_ENDGAME;
            else if (nodeType == "panel")
                type = NodeType.PANEL;

            var lineId = line.TextID;
            var CharacterName = line.CharacterName;
            var RawText = line.RawText;
            var TextNative = line.TextWithoutCharacterName;
            var TextLearning = (line as LocalizedLineDiscover)?.TextInLearningLang;
            var AudioNative = line.Asset as AudioClip;
            var AudioLearning = (line as LocalizedLineDiscover)?.AssetInLearningLang as AudioClip;
            var metadata = line.Metadata;

            // Debug.Log(metadata != null ? "LINE METADATA:" + string.Join(", ", metadata.ToArray()) : "no-metadata");

            //Debug.Log($"RunLineAsync: {lineId} {CharacterName} {RawText} {TextWithoutCharacterName} {metadata.ToString()} ");

            var questNode = new QuestNode
            {
                Type = type,
                Content = TextLearning,
                ContentNative = TextNative.Text,
                AudioLearning = AudioLearning,
                AudioNative = AudioNative,
                Image = GetLineTagValue(metadata, "card") ?? string.Empty,
                Permalink = _currentNodeName
            };

            // Parse node header tags first  from Yarn header "tags"
            var headerTags = GetHeaderTagsTokens(_currentNodeName);
            if (headerTags != null && headerTags.Length > 0)
            {
                ApplyTagsToQuestNode(questNode, headerTags);
            }

            // Parse inline tags/metadata (can refine/override)
            ApplyTagsToQuestNode(questNode, line.Metadata);

            UIManager.I.dialogues.ShowDialogueLine(questNode);
            Manager?.EmitQuestNode(questNode);

            // Store last seen node for potential repeat in choices
            previousQuestNode = questNode;

            var continueButton = true;
            if (continueButton)
            {
                // If the Dialogue Runner has a continue button, it will wait for
                // the player to press it before continuing.
                //
                // If your Dialogue View doesn't have a continue button, you can
                // simply return from this method immediately.
                await YarnTask.WaitUntilCanceled(token.NextLineToken).SuppressCancellationThrow();
            }
        }

        public override async YarnTask<DialogueOption> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
        {
            // Called by the Dialogue Runner to signal that options should be shown
            // to the player.
            //
            // If your Dialogue View handles options, it should present them to the
            // player and await a selection. Once a choice has been made, it should
            // return the appropriate element from dialogueOptions.
            //
            // The CancellationToken can be used to check to see if the Dialogue
            // Runner no longer needs this Dialogue View to make a choice. This
            // happens if a different Dialogue View made a selection, or if dialogue
            // has been cancelled. If the token is cancelled, it means that the
            // returned value from this method will not be used, and this method
            // should return null as soon as possible.
            //
            // The Dialogue Runner will wait for all Dialogue Views to return from
            // this method before delivering new content.
            //
            // If your Dialogue View doesn't need to handle options, simply return
            // null from this method to indicate that this Dialogue View didn't make
            // a selection.

            // A completion source that represents the selected option.
            _optionTCS = new YarnTaskCompletionSource<DialogueOption>();
            _optionCTS = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);
            _currentOptions = dialogueOptions;
            _hasPendingOptions = true;

            // var lineId = line.TextID;
            // var CharacterName = line.CharacterName;
            // var RawText = line.RawText;
            // var TextWithoutCharacterName = line.TextWithoutCharacterName;
            // var metadata = line.Metadata;
            //var image = line.Metadata[0];
            //Debug.Log($"RunLineAsync: {lineId} {CharacterName} {RawText} {TextWithoutCharacterName} {metadata.ToString()} ");

            // Parse node header tags first  from Yarn header "tags"
            var headerTags = GetHeaderTagsTokens(_currentNodeName);

            // Debug.Log(headerTags != null ? "LINE HEADERTAGS:" + string.Join(", ", headerTags.ToArray()) : "no-headerTags");

            // if we have a noRepeatLastLine tag, we don't repeat the last line
            // obviously if we don't have a previous line, we can't repeat it
            var repeatLastLineSeen = previousQuestNode != null;
            if (headerTags != null && headerTags.Contains("noRepeatLastLine"))
            {
                // Debug.Log("noRepeatLastLine TAG FOUND");
                repeatLastLineSeen = false;
            }

            var qn = new QuestNode
            {
                Type = NodeType.CHOICE,
                Content = repeatLastLineSeen ? previousQuestNode.Content : "",
                ContentNative = repeatLastLineSeen ? previousQuestNode.ContentNative : "",
                AudioLearning = repeatLastLineSeen ? previousQuestNode.AudioLearning : null,
                AudioNative = repeatLastLineSeen ? previousQuestNode.AudioNative : null,
                Image = "",
                Choices = new List<NodeChoice>()
            };

            if (headerTags != null && headerTags.Length > 0)
            {
                ApplyTagsToQuestNode(qn, headerTags);
            }

            // Use the first option's metadata (if any) to populate high-level tags; or prefer current node tags when available
            if (dialogueOptions.Length > 0)
            {
                ApplyTagsToQuestNode(qn, dialogueOptions[0].Line.Metadata);
            }

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var option = dialogueOptions[i];
                NodeChoice newChoice = new NodeChoice
                {
                    YarnOption = option,
                    OnOptionSelected = _optionTCS,
                    completionToken = _optionCTS.Token,
                    Index = i,
                    Content = (option.Line as LocalizedLineDiscover)?.TextInLearningLang,
                    ContentNative = option.Line.TextWithoutCharacterName.Text,
                    AudioLearning = (option.Line as LocalizedLineDiscover)?.AssetInLearningLang as AudioClip,
                    AudioNative = (option.Line as LocalizedLineDiscover)?.Asset as AudioClip,
                    Image = GetLineTagValue(option.Line.Metadata, "card") ?? string.Empty,
                    Highlight = GetLineTagExists(option.Line.Metadata, "highlight"),
                };
                qn.Choices.Add(newChoice);
            }

            UIManager.I.dialogues.ShowDialogueLine(qn);
            Manager?.EmitOptionsNode(qn);

            var completedTask = await _optionTCS.Task;
            _optionCTS.Cancel();
            _hasPendingOptions = false;

            // if we are cancelled we still need to return but we don't want to have a selection, so we return no selected option
            if (cancellationToken.IsCancellationRequested)
            {
                return await DialogueRunner.NoOptionSelected;
            }

            // return the selected option
            return completedTask;
        }

        /// <summary>
        /// Extract a tag value from line metadata. Supports formats like "card:my_id" or "card=my_id".
        /// </summary>
        private string GetLineTagValue(string[] metadata, string tagname)
        {
            if (metadata == null || metadata.Length == 0)
                return null;
            for (int i = 0; i < metadata.Length; i++)
            {
                var raw = metadata[i];
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                var parts = raw.Split(new[] { ':', '=' }, 2);
                if (parts.Length < 2)
                    continue;
                var key = parts[0].Trim().ToLowerInvariant();
                var val = parts[1].Trim();
                if (key == tagname && !string.IsNullOrEmpty(val))
                    return val;
            }
            return null;
        }

        /// <summary>
        /// Extract if a tag exists in line metadata.
        /// </summary>
        private bool GetLineTagExists(string[] metadata, string tagname)
        {
            if (metadata == null || metadata.Length == 0)
                return false;
            for (int i = 0; i < metadata.Length; i++)
            {
                var raw = metadata[i];
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                if (raw.Trim().ToLowerInvariant() == tagname)
                    return true;
            }
            return false;
        }

        private void ApplyTagsToQuestNode(QuestNode node, string[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
                return;

            // Expect tag strings like: type=panel
            foreach (var raw in attributes)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                var parts = raw.Split(new[] { '=' }, 2);
                var name = parts[0].Trim().ToLowerInvariant();
                var value = parts.Length > 1 ? parts[1].Trim().Trim(',') : string.Empty;

                switch (name)
                {
                    case "type":
                        node.BalloonType = value;
                        if (value == "quiz")
                            node.Type = NodeType.QUIZ;
                        if (value == "panel")
                            node.Type = NodeType.PANEL;
                        break;
                    default:
                        break;
                }
            }
        }

        private string[] GetHeaderTagsTokens(string nodeName)
        {
            try
            {
                if (string.IsNullOrEmpty(nodeName))
                    return null;
                var tags = Manager.Runner.Dialogue.GetHeaderValue(nodeName, "tags");
                if (string.IsNullOrEmpty(tags))
                    return null;
                return tags.Split(' ');
            }
            catch
            {
                return null;
            }
        }
    }
}
