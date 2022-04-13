using GameDevTV.Inventories;
using GameDevTV.Saving;
using Sanctuary.Harry.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Quests
{
    public class QuestList : MonoBehaviour, ISaveable, IPredicateEvaluator
    {
        List<QuestStatus> statuses = new List<QuestStatus>();

        public event Action onUpdate;

        public void AddQuest(Quest quest)
        {
            if (HasQuest(quest)) { return; }

            QuestStatus newStatus = new QuestStatus(quest);
            statuses.Add(newStatus);
            if (onUpdate != null) { onUpdate(); }
            
        }

        public bool HasQuest(Quest quest)
        {
            return GetQuestStatus(quest) != null;
        }

        public IEnumerable<QuestStatus> GetStatuses()
        {
            return statuses;
        }

        public void CompleteObjective(Quest quest, string objective)
        {
            QuestStatus status = GetQuestStatus(quest);
            status.CompleteObjective(objective);
            if (status.IsComplete()) { GiveReward(quest); }
            if (onUpdate != null) { onUpdate(); }
        }

        

        private QuestStatus GetQuestStatus(Quest quest)
        {
            foreach (QuestStatus status in statuses)
            {
                if(status.GetQuest() == quest)
                {
                    return status;
                }
            }

            return null;
        }

        private void GiveReward(Quest quest)
        {
            foreach (Quest.Reward reward in quest.GetRewards())
            {
                //in case the reward is NOT stackable
                if (!reward.item.IsStackable())
                {
                    int given = 0;

                    //add all possible to empty slots

                    for (int i = 0; i < reward.number; i++)
                    {
                        bool isGiven = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, 1);
                        if (!isGiven) break;

                        given++;
                    }

                    //if entire reward was given, go to the next reward

                    if (given == reward.number) continue;

                    //if given less than in reward, drop the difference

                    for (int i = given; i < reward.number; i++)
                    {
                        GetComponent<ItemDropper>().DropItem(reward.item, 1);
                    }
                }

                //if stackable, drop/add several units
                else
                {
                    bool isGiven = GetComponent<Inventory>().AddToFirstEmptySlot(reward.item, reward.number);
                    if (!isGiven)
                    {
                        for (int i = 0; i < reward.number; i++)
                        {
                            GetComponent<ItemDropper>().DropItem(reward.item, reward.number);
                        }
                    }
                }


            }
        }

        public object CaptureState()
        {
            List<object> state = new List<object>();
            foreach (QuestStatus status in statuses)
            {
                state.Add(status.CaptureState());
            }
            return state;
        }

        public void RestoreState(object state)
        {
            List<object> stateList = state as List<object>;
            if (stateList == null) return;

            statuses.Clear();

            foreach (object objectState in stateList)
            {
                statuses.Add(new QuestStatus(objectState));
            }
        }

        public bool? Evaluate(string predicate, string[] parameters)
        {
            

            switch (predicate)
            {
                case "HasQuest":
                    Debug.Log($"HasQuest {parameters[0]} = {HasQuest(Quest.GetByName(parameters[0]))}");
                    return HasQuest(Quest.GetByName(parameters[0]));
                case "CompletedQuest":
                    Debug.Log($"CompletedQuest {parameters[0]} = {GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete()}");
                    return GetQuestStatus(Quest.GetByName(parameters[0])).IsComplete();
            }


            return null;
        }
    }
}
