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

        QuestStatus status;

        public void Setup(QuestStatus status)
        {
            this.status = status;
            title.text = status.GetQuest().GetTitle();
            progress.text = $"{status.GetCompletedCount()}/{status.GetQuest().GetObjectiveCount()}";
        }

        public QuestStatus GetQuestStatus()
        {
            return status;
        }
    }
}
