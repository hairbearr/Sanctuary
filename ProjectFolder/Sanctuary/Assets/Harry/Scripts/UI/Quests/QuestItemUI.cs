using Sanctuary.Harry.Quests;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sanctuary.Harry.UI.Quests
{
    public class QuestItemUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, progress;


        public void Setup(Quest quest)
        {
            title.text = quest.GetTitle();
            progress.text = $"0/{quest.GetObjectiveCount()}";
        }
    }
}
