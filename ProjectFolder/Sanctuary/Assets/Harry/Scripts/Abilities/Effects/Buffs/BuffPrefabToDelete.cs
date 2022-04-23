using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using UnityEngine;

public class BuffPrefabToDelete : MonoBehaviour
{
    [SerializeField] Transform buffedIndividual;
    [SerializeField] GameObject buffPrefab;

    void Start()
    {
        buffedIndividual = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(buffedIndividual.GetComponent<Health>().GetShielded() == false && buffedIndividual !=null) Destroy(buffPrefab);
        
    }
}
