using Antura.UI;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using Yarn.Unity;
using UnityEngine.Localization.SmartFormat.Utilities;

namespace Antura.Discover
{
    public class DialogUIBridge : DialoguePresenterBase
    {

        void Awake()
        {

        }

        public override YarnTask OnDialogueStartedAsync()
        {
            // show your UI



            return YarnTask.CompletedTask; // nothing async to wait for
        }

        public override YarnTask OnDialogueCompleteAsync()
        {
            // hide your UI and clear


            return YarnTask.CompletedTask;
        }

        public override async YarnTask RunLineAsync(LocalizedLine line, LineCancellationToken token)
        {
            var lineId = line.TextID;
            var CharacterName = line.CharacterName;
            var RawText = line.RawText;
            var TextWithoutCharacterName = line.TextWithoutCharacterName;
            var metadata = line.Metadata;
            //var image = line.Metadata[0];
            Debug.Log($"RunLineAsync: {lineId} {CharacterName} {RawText} {TextWithoutCharacterName} {metadata.ToString()} ");

            QuestNode displayNode = new QuestNode
            {
                Type = NodeType.TEXT,
                Content = line.TextWithoutCharacterName.Text,
                ContentNative = "CICCIO NATIVO",
                AudioId = null,
                Image = null
            };

            //InteractionManager.I.DisplayNode(displayNode);
            UIManager.I.dialogues.StartDialogue(displayNode);
            return; // nothing async to wait for
        }

        public override YarnTask<DialogueOption?> RunOptionsAsync(DialogueOption[] dialogueOptions, CancellationToken cancellationToken)
        {
            // QuestNode displayNode = new QuestNode
            // {
            //     Type = NodeType.TEXT,
            //     Content = line.TextWithoutCharacterName.Text,
            //     ContentNative = line.TextWithoutCharacterName.Text,
            //     AudioId = null,
            //     Image = null
            // };

            // //InteractionManager.I.DisplayNode(displayNode);
            // UIManager.I.dialogues.StartDialogue(displayNode);

            return YarnTask.FromResult<DialogueOption?>(null);
        }
    }
}
