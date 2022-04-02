using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Dialogue
{
    [CreateAssetMenu(fileName = "New Dialogue", menuName = "Sanctuary/Dialogue/Make New Dialogue", order =0)]
    public class Dialogue : ScriptableObject
    {
        [SerializeField] List<DialogueNode> nodes;

#if UNITY_EDITOR
        private void Awake()
        {
            if(nodes.Count == 0) { nodes.Add(new DialogueNode()); }
        }
#endif
        public IEnumerable<DialogueNode> GetAllNodes()
        {
            return nodes;
        }
    }
}
