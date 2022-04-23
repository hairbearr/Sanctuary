using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using UnityEngine;

public class DebuffPrefabToDelete : MonoBehaviour
{
    [SerializeField] Transform deBuffedIndividual;
    [SerializeField] GameObject deBuffPrefab;

    void Start()
    {
        deBuffedIndividual = this.transform.parent;
    }

    // Update is called once per frame
    void Update()
    {
        if(deBuffedIndividual.GetComponent<Health>().IsDead() == true && deBuffedIndividual !=null) Destroy(deBuffPrefab.gameObject);
    }
}
