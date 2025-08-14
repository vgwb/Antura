using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Yarn;
using Yarn.Unity;

namespace Antura.Discover
{
    public class DiscoverDialoguePresenter : DialoguePresenterBase
    {
        string lastSeenLine;

        public override async YarnTask OnDialogueStartedAsync()
        {
            // Called by the Dialogue Runner to signal that dialogue has just
            // started up.
            //
            // You can use this method to prepare for presenting dialogue, like
            // changing the camera, fading up your on-screen UI, or other tasks.
            //
            // The Dialogue Runner will wait until every Dialogue View returns from
            // this method before delivering any content.

        }

        public override async YarnTask OnDialogueCompleteAsync()
        {
            // Called by the Dialogue Runner to signal that dialogue has ended.
            //
            // You can use this method to clean up after running dialogue, like
            // changing the camera back, fading away on-screen UI, or other tasks.
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

            QuestNode displayNode = new QuestNode
            {
                Type = NodeType.TEXT,
                Content = TextWithoutCharacterName.Text,
                ContentNative = "CICCIO NATIVO",
                AudioId = null,
                Image = ""  //"testimage"
            };

            UIManager.I.dialogues.ShowDialogueLine(displayNode);
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

        public override async YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
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
            YarnTaskCompletionSource<DialogueOption?> selectedOptionCompletionSource = new YarnTaskCompletionSource<DialogueOption?>();
            // A cancellation token source that becomes cancelled when any
            // option item is selected, or when this entire option view is
            // cancelled
            var completionCancellationSource = CancellationTokenSource.CreateLinkedTokenSource(cancellationToken);

            // var lineId = line.TextID;
            // var CharacterName = line.CharacterName;
            // var RawText = line.RawText;
            // var TextWithoutCharacterName = line.TextWithoutCharacterName;
            // var metadata = line.Metadata;
            //var image = line.Metadata[0];
            //Debug.Log($"RunLineAsync: {lineId} {CharacterName} {RawText} {TextWithoutCharacterName} {metadata.ToString()} ");

            QuestNode displayNode = new QuestNode
            {
                Type = NodeType.CHOICE,
                Content = lastSeenLine,
                ContentNative = "CICCIO NATIVO",
                AudioId = null,
                Image = "",
                Choices = new List<NodeChoice>()
            };

            for (int i = 0; i < dialogueOptions.Length; i++)
            {
                var option = dialogueOptions[i];
                NodeChoice newChoice = new NodeChoice
                {
                    YarnOption = option,
                    OnOptionSelected = selectedOptionCompletionSource,
                    completionToken = completionCancellationSource.Token,
                    Index = i,
                    Content = option.Line.TextWithoutCharacterName.Text,
                    ContentNative = option.Line.TextWithoutCharacterName.Text, // Assuming native text is the same for now
                    AudioId = "",
                    Image = "",
                    Highlight = false // Set highlight based on your logic
                };
                displayNode.Choices.Add(newChoice);
            }

            UIManager.I.dialogues.ShowDialogueLine(displayNode);

            var continueButton = true;

            var completedTask = await selectedOptionCompletionSource.Task;
            completionCancellationSource.Cancel();

            // if we are cancelled we still need to return but we don't want to have a selection, so we return no selected option
            if (cancellationToken.IsCancellationRequested)
            {
                return await DialogueRunner.NoOptionSelected;
            }

            // finally we return the selected option
            return completedTask;

        }
    }
}

