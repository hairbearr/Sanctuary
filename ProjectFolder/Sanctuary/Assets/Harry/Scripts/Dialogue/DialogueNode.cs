using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using UnityEditor;
using UnityEngine;


public class DialogueNode : ScriptableObject
{
    [SerializeField] enum speaking { player, npcOne, npcTwo, npcThree }
    [SerializeField] speaking currentSpeaker;
    [SerializeField] private string  text;
    [SerializeField] private List<string> children = new List<string>();
    [SerializeField] private Rect rect = new Rect(0,0,200,100);
    [SerializeField] private string onEnterAction;
    [SerializeField] private string onExitAction;
    [SerializeField] Condition condition;


    public Rect GetRect()
    {
        return rect;
    }

    public string GetText()
    {
        return text;
    }
    public List<string> GetChildren()
    {
        return children;
    }

    public bool IsPlayerSpeaking()
    {
        if (currentSpeaker == speaking.player)
        {
            return true;
        }
        else return false;
    }

    public bool IsNPCOneSpeaking()
    {
        if (currentSpeaker == speaking.npcOne)
        {
            return true;
        }
        else return false;
    }

    public bool IsNPCTwoSpeaking()
    {
        if (currentSpeaker == speaking.npcTwo)
        {
            return true;
        }
        else return false;
    }

    public bool IsNPCThreeSpeaking()
    {
        if (currentSpeaker == speaking.npcThree)
        {
            return true;
        }
        else return false;
    }

    public string GetOnEnterAction()
    {
        return onEnterAction;
    }

    public string GetOnExitAction()
    {
        return onExitAction;
    }

    public bool CheckConditon(IEnumerable<IPredicateEvaluator> evaluators)
    {
        return condition.Check(evaluators);
    }

#if UNITY_EDITOR
    public void SetPosition(Vector2 newPosition)
    {
        Undo.RecordObject(this, "Move Dialogue Node");
        rect.position = newPosition;
        EditorUtility.SetDirty(this);
    }

    public void SetText(string newText)
    {
        if(newText != text) { Undo.RecordObject(this, "Update Dialogue Text"); text = newText; EditorUtility.SetDirty(this); }
    }

    public void AddChild(string childID)
    {
        Undo.RecordObject(this, "Add Dialogue Link");
        children.Add(childID);
        EditorUtility.SetDirty(this);
    }

    public void RemoveChild(string childID)
    {
        Undo.RecordObject(this, "Remove Dialogue Link");
        children.Remove(childID);
        EditorUtility.SetDirty(this);
    }

    
#endif
}
