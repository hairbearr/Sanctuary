using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DialogueNode
{
    public string uniqueID, text;
    public List<string> children = new List<string>();
    public Rect rect = new Rect(0,0,200,100);
}
