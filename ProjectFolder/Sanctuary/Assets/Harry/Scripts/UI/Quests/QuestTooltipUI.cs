using Sanctuary.Harry.Quests;
using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace Sanctuary.Harry.UI.Quests
{
    public class QuestTooltipUI : MonoBehaviour
    {
        [SerializeField] TextMeshProUGUI title, rewardText;
        [SerializeField] Transform objectiveContainer;
        [SerializeField] GameObject objectivePrefab, objectiveIncompletePrefab;



        public void Setup(QuestStatus status)
        {
            Quest quest = status.GetQuest();
            title.text = quest.GetTitle();
            foreach (Transform item in objectiveContainer)
            {
                Destroy(item.gameObject);
            }
            foreach (var objective in quest.GetObjectives())
            {
                GameObject prefab = objectiveIncompletePrefab;
                if (status.IsObjectiveComplete(objective.reference)){ prefab = objectivePrefab; }
                GameObject objectiveInstance = Instantiate(prefab, objectiveContainer);
                TextMeshProUGUI objectiveText = objectiveInstance.GetComponentInChildren<TextMeshProUGUI>();
                objectiveText.text = objective.description;
            }

            rewardText.text = GetRewardText(quest);
        }

        private string GetRewardText(Quest quest)
        {
            string rewardText = "";
            foreach (var reward in quest.GetRewards())
            {
                if(rewardText != "")
                {
                    rewardText += ", ";
                }
                if(reward.number > 1)
                {
                    rewardText+= " x" + reward.number + " ";
                }
                rewardText += reward.item.GetDisplayName();
            }

            if(rewardText == "") { rewardText = "No reward"; }

            rewardText += ".";
            return rewardText;
        }
    }

}