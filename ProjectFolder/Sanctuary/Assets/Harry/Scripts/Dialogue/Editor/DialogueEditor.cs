using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.Callbacks;
using UnityEngine;

namespace Sanctuary.Harry.Dialogue.Editor
{
    public class DialogueEditor : EditorWindow
    {
        Dialogue selectedDialogue = null;
        [NonSerialized] GUIStyle npcOneNodeStyle = null, playerNodeStyle = null, npcTwoNodeStyle = null, npcThreeNodeStyle = null;
        [NonSerialized] DialogueNode draggingNode = null, creatingNode = null, deletingNode = null, linkingParentNode = null;
        [NonSerialized] Vector2 draggingOffset, draggingCanvasOffset;
        Vector2 scrollPosition;
        [NonSerialized] bool draggingCanvas = false;

        const float canvasSize = 4000, backgroundSize = 50;
        

        [MenuItem("Window/Dialogue Editor")]
        public static void ShowEditorWindow() { GetWindow(typeof(DialogueEditor), false, "Dialogue Editor"); }

        [OnOpenAsset(1)]
        public static bool OnOpenAsset(int instanceID, int line)
        {
            Dialogue dialogue = EditorUtility.InstanceIDToObject(instanceID) as Dialogue;
            if (dialogue != null) { ShowEditorWindow(); return true; }
            return false;
        }

        private void OnEnable()
        {
            Selection.selectionChanged += OnSelectionChanged;

            npcOneNodeStyle = new GUIStyle();
            npcOneNodeStyle.normal.background = EditorGUIUtility.Load("node0") as Texture2D;
            npcOneNodeStyle.normal.textColor = Color.white;
            npcOneNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            npcOneNodeStyle.border = new RectOffset(12, 12, 12, 12);

            playerNodeStyle = new GUIStyle();
            playerNodeStyle.normal.background = EditorGUIUtility.Load("node1") as Texture2D;
            playerNodeStyle.normal.textColor = Color.white;
            playerNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            playerNodeStyle.border = new RectOffset(12, 12, 12, 12);

            npcTwoNodeStyle = new GUIStyle();
            npcTwoNodeStyle.normal.background = EditorGUIUtility.Load("node2") as Texture2D;
            npcTwoNodeStyle.normal.textColor = Color.white;
            npcTwoNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            npcTwoNodeStyle.border = new RectOffset(12, 12, 12, 12);

            npcThreeNodeStyle = new GUIStyle();
            npcThreeNodeStyle.normal.background = EditorGUIUtility.Load("node3") as Texture2D;
            npcThreeNodeStyle.normal.textColor = Color.white;
            npcThreeNodeStyle.padding = new RectOffset(10, 10, 10, 10);
            npcThreeNodeStyle.border = new RectOffset(12, 12, 12, 12);
        }

        private void OnSelectionChanged()
        {
            Dialogue newDialogue = Selection.activeObject as Dialogue;
            if(newDialogue != null) { selectedDialogue = newDialogue; Repaint(); }
        }

        private void OnGUI()
        {
            if (selectedDialogue == null) { EditorGUILayout.LabelField("No Dialogue Selected."); }
            else
            {
                ProcessEvents();
                scrollPosition = EditorGUILayout.BeginScrollView(scrollPosition);
                


                Rect canvas = GUILayoutUtility.GetRect(canvasSize, canvasSize);
                Texture2D backgroundText = Resources.Load("background") as Texture2D;
                Rect texCoords = new Rect(0, 0, canvasSize/backgroundSize, canvasSize / backgroundSize);

                GUI.DrawTextureWithTexCoords(canvas, backgroundText, texCoords);

                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawConnections(node);
                }
                foreach (DialogueNode node in selectedDialogue.GetAllNodes())
                {
                    DrawNode(node);
                }
                if(creatingNode != null) { selectedDialogue.CreateNode(creatingNode); creatingNode = null; }

                EditorGUILayout.EndScrollView();

                if (deletingNode != null) { selectedDialogue.DeleteNode(deletingNode); deletingNode = null; }
            }

            
            
        }

        private void ProcessEvents()
        {
            if (Event.current.type == EventType.MouseDown && draggingNode == null)
            { //mouse down
                draggingNode = GetNodeAtPoint(Event.current.mousePosition + scrollPosition);
                if (draggingNode != null)
                {
                    draggingOffset = draggingNode.GetRect().position - Event.current.mousePosition;
                    Selection.activeObject = draggingNode;
                }
                else
                {
                    draggingCanvas = true;
                    draggingCanvasOffset = Event.current.mousePosition + scrollPosition;
                    Selection.activeObject = selectedDialogue;
                }
            }
            else if (Event.current.type == EventType.MouseDrag && draggingNode != null)
            {//mouse drag
                Undo.RecordObject(selectedDialogue, "Move Dialogue Node");
                draggingNode.SetPosition(Event.current.mousePosition + draggingOffset);

                GUI.changed = true;
            }
            else if (Event.current.type == EventType.MouseDrag && draggingCanvas)
            {
                scrollPosition = draggingCanvasOffset - Event.current.mousePosition;

                GUI.changed = true;
            }

            else if (Event.current.type == EventType.MouseUp && draggingNode != null)
            {//mouse up
                draggingNode = null;
            }
            else if (Event.current.type == EventType.MouseUp && draggingCanvas)
            {
                draggingCanvas = false;
            }


        }

        private void DrawNode(DialogueNode node)
        {
            GUIStyle style = npcOneNodeStyle;
            if (node.IsPlayerSpeaking())
            {
                style = playerNodeStyle;
            }
            if (node.IsNPCOneSpeaking())
            {
                style = npcOneNodeStyle;
            }
            if (node.IsNPCTwoSpeaking())
            {
                style = npcTwoNodeStyle;
            }
            if (node.IsNPCThreeSpeaking())
            {
                style = npcThreeNodeStyle;
            }

            GUILayout.BeginArea(node.GetRect(), style);

            node.SetText(EditorGUILayout.TextField(node.GetText()));

            GUILayout.BeginHorizontal();

            if (GUILayout.Button("x")) { deletingNode = node; }
           
            DrawLinkButtons(node);

            if (GUILayout.Button("+")) { creatingNode = node; }

            GUILayout.EndHorizontal();



            GUILayout.EndArea();
        }

        private void DrawLinkButtons(DialogueNode node)
        {
            if (linkingParentNode == null)
            {
                if (GUILayout.Button("Link"))
                {
                    linkingParentNode = node;
                }
            }
            else if (linkingParentNode == node)
            {
                if (GUILayout.Button("Cancel"))
                {
                    linkingParentNode = null;
                }
            }
            else if (linkingParentNode.GetChildren().Contains(node.name))
            {
                if (GUILayout.Button("Unlink"))
                {
                    linkingParentNode.RemoveChild(node.name);
                    linkingParentNode = null;
                }
            }
            else
            {
                if (GUILayout.Button("Child"))
                {
                    linkingParentNode.AddChild(node.name);
                    linkingParentNode = null
                        ;
                }
            }
        }

        private void DrawConnections(DialogueNode node)
        {
            Vector3 startPosition = new Vector2(node.GetRect().xMax, node.GetRect().center.y);
            foreach (DialogueNode childNode in selectedDialogue.GetAllChildren(node))
            {
                Vector3 endPosition = new Vector2(childNode.GetRect().xMin, childNode.GetRect().center.y);
                Vector3 controlPointOffset = endPosition - startPosition;
                controlPointOffset.y = 0;
                controlPointOffset.x *= 0.8f;
                Handles.DrawBezier(startPosition, endPosition, startPosition + controlPointOffset, endPosition - controlPointOffset, Color.white, null, 4f);
            }
        }


        private DialogueNode GetNodeAtPoint(Vector2 point)
        {
            DialogueNode foundNode = null;
            foreach (DialogueNode node in selectedDialogue.GetAllNodes())
            {
                if (node.GetRect().Contains(point)) { foundNode = node; }
            }
            return foundNode;
        }
    }
}
