using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace Sanctuary.Harry.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f, maxLifeTime = 10f, lifeAfterImpact = 2f;
        [SerializeField] GameObject hitEffect = null;
        [SerializeField] GameObject[] destroyOnHit = null;
        [SerializeField] UnityEvent onHit;

        Health tgt = null;
        float dmg = 0f;
        bool isHoming = true;
        GameObject instigator = null;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (tgt == null) return;

            if (isHoming && !tgt.IsDead()) { transform.LookAt(GetAimLocation()); }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator,  float damage)
        {
            this.tgt = target;
            this.dmg = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            CapsuleCollider tgtCapsule = tgt.GetComponent<CapsuleCollider>();
            if(tgtCapsule == null)
            {
                return tgt.transform.position;
            }
            return tgt.transform.position + Vector3.up * tgtCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.GetComponent<Health>() != tgt) return;
            if (tgt.IsDead()) { return; }
            tgt.TakeDamage(instigator, dmg);

            speed = 0;

            onHit.Invoke();

            if (hitEffect != null) { Instantiate(hitEffect, GetAimLocation(), transform.rotation); }

            foreach(GameObject toDestroy in destroyOnHit)
            {
                Destroy(toDestroy);
            }

            Destroy(gameObject, lifeAfterImpact);
        }
    }
}
