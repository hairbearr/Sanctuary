using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Control;
using UnityEngine;

public class ConeEffectPrefabScript : MonoBehaviour
{
    GameObject instigator = null;
    [SerializeField] float healthChange, coneDamage;
    [SerializeField] bool floorObject, canHeal;

    bool playerTriggerEntered = false, enemyTriggerEntered = false;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if(this.transform.childCount <= 0) {Destroy(this.gameObject);}
    }

    
    public void SetInstigator(GameObject gameObject)
    {
        instigator = gameObject;
    }

    private IEnumerator OnTriggerEnter(Collider other)
    {

        if(floorObject == false)
        {
            if(other.gameObject.tag == "Enemy")
            {
                Health health = other.GetComponent<Health>();
                if(health == null || health.IsDead()) yield break;

                if(other.gameObject == instigator) yield break;

                health.TakeDamage(instigator,coneDamage);
            }
            
        }

        if(floorObject == true)
        {
            if(other.gameObject.tag == "Player")
            {
                if(canHeal == true)
                {
                    playerTriggerEntered = true;
                    Health playerHealth = other.GetComponent<Health>();
                    while(playerTriggerEntered)
                    {
                        playerHealth.Heal(healthChange);
                        yield return new WaitForSeconds(1);
                    }
                }
            }

            if(other.gameObject.tag == "Enemy")
            {
                enemyTriggerEntered = true;
                Health enemyHealth = other.GetComponent<Health>();
                while(enemyTriggerEntered)
                {
                    if(enemyHealth.IsDead())
                    {
                        yield break;
                    }
                    enemyHealth.TakeDamage(instigator, coneDamage);
                    yield return new WaitForSeconds(1);
                    
                }

            }
        }        
    }

    private void OnTriggerExit(Collider other)
    {
        if(floorObject == true)
        {
            if(other.gameObject.tag == "Player")
            {
                playerTriggerEntered = false;
            }

            if(other.gameObject.tag == "Enemy")
            {
                enemyTriggerEntered = false;
            }
        }
    }
}
