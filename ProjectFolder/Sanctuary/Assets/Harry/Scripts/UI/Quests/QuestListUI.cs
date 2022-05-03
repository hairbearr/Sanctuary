using Sanctuary.Harry.Quests;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.UI.Quests
{
    public class QuestListUI : MonoBehaviour
    {
        [SerializeField] QuestItemUI questPrefab;
        [SerializeField] QuestList questList;


        // Start is called before the first frame update
        void Start()
        {
            questList = GameObject.FindGameObjectWithTag("Player").GetComponent<QuestList>();
            questList.onUpdate += Redraw;
            Redraw();
        }

        private void Redraw()
        {
            foreach (Transform item in transform)
            {
                Destroy(item.gameObject);
            }

            foreach (QuestStatus status in questList.GetStatuses())
            {
                QuestItemUI uiInstance = Instantiate<QuestItemUI>(questPrefab, transform);
                uiInstance.Setup(status);
            }
        }

    }
}
