using RPG.Management;
using RPG.Management.Progression;
using RPG.Management.Progression.Editor;
using System.Collections.Generic;
using System.Linq;
using Unity.GraphToolkit.Editor;
using UnityEditor.AssetImporters;
using UnityEngine;

namespace RPG.Dialogue.Editor
{
    [ScriptedImporter(23, DialogueGraph.AssetExtension)]
    public class DialogueGraphImporter : ScriptedImporter
    {
        private static Queue<DialogueNode> _dialogueQueue = new Queue<DialogueNode>();
        private static Dictionary<DialogueNode, Dialogue> _processedDialogues = new Dictionary<DialogueNode, Dialogue>();
        private static Dialogue _previousDialogue;
        private static AssetImportContext context;

        public override void OnImportAsset(AssetImportContext ctx)
        {
            _dialogueQueue = new Queue<DialogueNode>();
            _processedDialogues = new Dictionary<DialogueNode, Dialogue>();
            _previousDialogue = null;
            context = ctx;

            var graph = GraphDatabase.LoadGraphForImporter<DialogueGraph>(ctx.assetPath);
            if (graph == null)
            {
                Debug.LogError("Failed to load Dialogue Graph asset: " + ctx.assetPath);
                return;
            }

            DialogueNode node = graph.GetNodes().OfType<DialogueEntryNode>().First();
            if (node == null)
            {
                Debug.LogError("Failed to find Entry Dialogue in asset: " + ctx.assetPath);
                return;
            }


            DialogueGraphRuntime runtimeAsset = ScriptableObject.CreateInstance<DialogueGraphRuntime>();
            _dialogueQueue.Enqueue(node);

            do
            {
                node = _dialogueQueue.Dequeue();
                BuildRuntimeGraph(node, runtimeAsset);
            } while (_dialogueQueue.Count > 0);

            ctx.AddObjectToAsset("RuntimeAsset", runtimeAsset);
            ctx.SetMainObject(runtimeAsset);
        }

        static void BuildRuntimeGraph(DialogueNode node, DialogueGraphRuntime runtimeAsset)
        {
            switch (node)
            {
                case DialogueEntryNode:
                    if (_previousDialogue != null) Debug.LogError("More than one Entry Node detected");

                    DialogueEntrySO dialogueEntry = ScriptableObject.CreateInstance<DialogueEntrySO>();
                    dialogueEntry.Dialogue = CreateDialogue(node);
                    context.AddObjectToAsset("DialogueEntryDialogue", dialogueEntry.Dialogue);

                    _previousDialogue = dialogueEntry.Dialogue;

                    Dialogue secondDialogue = GetNextDialogue(node, dialogueEntry.Dialogue, DialogueNode.OUTPUT_NEXT_DIALOGUE);
                    dialogueEntry.Dialogue.Next = secondDialogue;
                    context.AddObjectToAsset("DialogueEntry", dialogueEntry);
                    context.AddObjectToAsset("Dialogue" + _processedDialogues.Count, secondDialogue);
                    runtimeAsset.Entry = dialogueEntry;
                    break;
                case DialogueOptionNode:

                    _previousDialogue = CreateDialogue(node);
                    DialogueWithChoice dialogueWithChoice = _previousDialogue as DialogueWithChoice;

                    INodeOption optionsCountOption = ((DialogueOptionNode)node).GetNodeOptionByName(DialogueOptionNode.OPTIONS_COUNT);
                    optionsCountOption.TryGetValue<int>(out int optionsCount);
                    Dialogue[] choices = new Dialogue[optionsCount];

                    for (int i = 0; i < optionsCount; i++)
                    {
                        Dialogue choice = GetNextDialogue(node, dialogueWithChoice, DialogueOptionNode.OUTPUT_OPTION + i);
                        if(choice != null)
                            context.AddObjectToAsset("DialogueWithOption" + _processedDialogues.Count+"Option"+i, choice);
                        choices[i] = choice;
                    }

                    dialogueWithChoice.SetChoices(choices);

                    break;
                case DialogueNode:
                    _previousDialogue = CreateDialogue(node);
                    Dialogue nextDialogue = GetNextDialogue(node, _previousDialogue, DialogueNode.OUTPUT_NEXT_DIALOGUE);
                    _previousDialogue.Next = nextDialogue;

                    if(nextDialogue != null)
                        context.AddObjectToAsset("Dialogue"+_processedDialogues.Count, nextDialogue);
                    break;
            }
        }

        static Dialogue CreateDialogue(DialogueNode node)
        {
            if (_processedDialogues.ContainsKey(node)) return _processedDialogues[node];

            switch(node)
            {
                case DialogueOptionNode:
                    return CreateChoiceDialogue((DialogueOptionNode)node);
                default:
                    return CreateRegularDialogue(node);
            }
            
        }

        static Dialogue CreateRegularDialogue(DialogueNode node)
        {
            IPort dialogueKeyInput = node.GetInputPortByName(DialogueNode.INPUT_DIALOGUE_KEY);
            dialogueKeyInput.TryGetValue<string>(out string dialogueKey);

            IPort dialogueDisplayInfoInput = node.GetInputPortByName(DialogueNode.INPUT_DIALOGUE_DISPLAY_INFO);
            dialogueDisplayInfoInput.TryGetValue<EntityScriptable>(out EntityScriptable entityScriptable);
            DialogueDisplayInfo dialogueDisplayInfo = ScriptableObject.CreateInstance<DialogueDisplayInfo>();

            if (entityScriptable != null)
            {
                dialogueDisplayInfo.Title = entityScriptable.CharacterName;
                dialogueDisplayInfo.Sprite = entityScriptable.Icon;
            }

            context.AddObjectToAsset("Dialogue" + _processedDialogues.Count + "DisplayInfo", dialogueDisplayInfo);

            Dialogue dialogue = ScriptableObject.CreateInstance<Dialogue>();
            dialogue.Init(dialogueKey, dialogueDisplayInfo);


            _processedDialogues.Add(node, dialogue);


            return dialogue;
        }

        static DialogueWithChoice CreateChoiceDialogue(DialogueOptionNode node)
        {
            IPort dialogueKeyInput = node.GetInputPortByName(DialogueNode.INPUT_DIALOGUE_KEY);
            dialogueKeyInput.TryGetValue<string>(out string dialogueKey);

            IPort dialogueDisplayInfoInput = node.GetInputPortByName(DialogueNode.INPUT_DIALOGUE_DISPLAY_INFO);
            dialogueDisplayInfoInput.TryGetValue<EntityScriptable>(out EntityScriptable entityScriptable);
            DialogueDisplayInfo dialogueDisplayInfo = ScriptableObject.CreateInstance<DialogueDisplayInfo>();

            if (entityScriptable != null)
            {
                dialogueDisplayInfo.Title = entityScriptable.CharacterName;
                dialogueDisplayInfo.Sprite = entityScriptable.Icon;
            }

            context.AddObjectToAsset("Dialogue" + _processedDialogues.Count + "DisplayInfo", dialogueDisplayInfo);

            DialogueWithChoice dialogue = ScriptableObject.CreateInstance<DialogueWithChoice>();
            dialogue.Init(dialogueKey, dialogueDisplayInfo);

            _processedDialogues.Add(node, dialogue);


            return dialogue;
        }

        static Dialogue GetNextDialogue(DialogueNode node, Dialogue dialogue, string nextDialogueOutputName)
        {
            IPort nextDialogueOutput = node.GetOutputPortByName(nextDialogueOutputName);
            DialogueNode nextNode = nextDialogueOutput.firstConnectedPort?.GetNode() as DialogueNode;
            if (nextNode == null) return null;

            _dialogueQueue.Enqueue(nextNode);
            return CreateDialogue(nextNode);
        }
    }
}
