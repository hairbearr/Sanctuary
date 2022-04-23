using System.Collections;
using System.Collections.Generic;
using GameDevTV.Inventories;
using UnityEngine;

[RequireComponent(typeof(ActionStore))]
public class PreLoadAbilities : MonoBehaviour
{
    [SerializeField] private ActionItem[] abilitiesToPreload;
    private ActionStore actionStore;

    void Awake()
    {
        actionStore = GetComponent<ActionStore>();
        for (int i = 0; i < abilitiesToPreload.Length; i++)
        {
            actionStore.AddAction(abilitiesToPreload[i], i, 1);
        }
    }
}
