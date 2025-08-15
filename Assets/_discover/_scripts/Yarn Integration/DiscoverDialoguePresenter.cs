using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Antura.Discover
{
    public class DiscoverDialoguePresenter : DialoguePresenterBase
    {
        public YarnAnturaManager Manager { get; set; }

        private string lastSeenLine;
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
            // Called by the Dialogue Runner to signal that dialogue has just
            // started up.
            //
            // You can use this method to prepare for presenting dialogue, like
            // changing the camera, fading up your on-screen UI, or other tasks.
            //
            // The Dialogue Runner will wait until every Dialogue View returns from
            // this method before delivering any content.
            return YarnTask.CompletedTask;
        }

        public override YarnTask OnDialogueCompleteAsync()
        {
            // Called by the Dialogue Runner to signal that dialogue has ended.
            //
            // You can use this method to clean up after running dialogue, like
            // changing the camera back, fading away on-screen UI, or other tasks.
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

            var lineId = line.TextID;
            var CharacterName = line.CharacterName;
            var RawText = line.RawText;
            var TextWithoutCharacterName = line.TextWithoutCharacterName;
            var metadata = line.Metadata;
            //var image = line.Metadata[0];
            //Debug.Log($"RunLineAsync: {lineId} {CharacterName} {RawText} {TextWithoutCharacterName} {metadata.ToString()} ");

            var qn = new QuestNode
            {
                Type = NodeType.TEXT,
                Content = TextWithoutCharacterName.Text,
                ContentNative = TextWithoutCharacterName.Text, // TODO: provide native via localization
                AudioId = null,
                Image = "",
                Permalink = _currentNodeName
            };

            // Parse tags/metadata into QuestNode fields
            ApplyTagsToQuestNode(qn, line.Metadata);

            UIManager.I.dialogues.ShowDialogueLine(qn);
            Manager?.EmitQuestNode(qn);
            lastSeenLine = TextWithoutCharacterName.Text;
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

            var qn = new QuestNode
            {
                Type = NodeType.CHOICE,
                Content = lastSeenLine,
                ContentNative = lastSeenLine,
                AudioId = null,
                Image = "",
                Choices = new List<NodeChoice>()
            };

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
                    Content = option.Line.TextWithoutCharacterName.Text,
                    ContentNative = option.Line.TextWithoutCharacterName.Text, // Assuming native text is the same for now
                    AudioId = "",
                    Image = "",
                    Highlight = false // Set highlight based on your logic
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

            // finally we return the selected option
            return completedTask;

        }

        private void ApplyTagsToQuestNode(QuestNode node, string[] attributes)
        {
            if (attributes == null || attributes.Length == 0)
                return;

            // Expect tag strings like: action=OpenShop, task=Collect, balloon=panel, mood=happy, image=SpriteName, nexttarget=Foo
            foreach (var raw in attributes)
            {
                if (string.IsNullOrWhiteSpace(raw))
                    continue;
                var parts = raw.Split(new[] { '=' }, 2);
                var name = parts[0].Trim().ToLowerInvariant();
                var value = parts.Length > 1 ? parts[1].Trim() : string.Empty;

                switch (name)
                {
                    case "action":
                        node.Action = value;
                        break;
                    case "action_post":
                        node.ActionPost = value;
                        break;
                    case "task":
                        node.Task = value; // optionally parse sub-args via attr.Properties
                        break;
                    case "balloon":
                    case "balloon_type":
                        node.BalloonType = value;
                        if (value == "quiz")
                            node.Type = NodeType.QUIZ;
                        if (value == "panel")
                            node.Type = NodeType.PANEL;
                        break;
                    case "mood":
                        node.Mood = value;
                        break;
                    case "image":
                        node.Image = value;
                        break;
                    case "nexttarget":
                        node.NextTarget = value;
                        break;
                    default:
                        break;
                }
            }
        }
    }
}

