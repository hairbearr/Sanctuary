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
        Vector3 targetPoint;
        float dmg = 0f;
        bool isHoming = true;
        GameObject instigator = null;

        private void Start()
        {
            transform.LookAt(GetAimLocation());
        }

        void Update()
        {
            if (tgt != null && isHoming && !tgt.IsDead()) { transform.LookAt(GetAimLocation()); }

            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, GameObject instigator,  float damage)
        {
            SetTarget(instigator, damage, target);
        }

        public void SetTarget(Vector3 targetPoint, GameObject instigator, float damage)
        {
            SetTarget(instigator, damage, null, targetPoint);
        }

        public void SetTarget(GameObject instigator, float damage, Health target = null, Vector3 targetPoint = default)
        {
            this.tgt = target;
            this.targetPoint = targetPoint;
            this.dmg = damage;
            this.instigator = instigator;

            Destroy(gameObject, maxLifeTime);
        }

        private Vector3 GetAimLocation()
        {
            if(tgt == null)
            {
                return targetPoint;
            }

            CapsuleCollider tgtCapsule = tgt.GetComponent<CapsuleCollider>();
            if(tgtCapsule == null)
            {
                return tgt.transform.position;
            }
            return tgt.transform.position + Vector3.up * tgtCapsule.height / 2;
        }

        private void OnTriggerEnter(Collider other)
        {

            Health health = other.GetComponent<Health>();
            if (tgt != null && health != tgt) return;

            if (health == null || health.IsDead()) return;

            if(other.gameObject == instigator) return;

            health.TakeDamage(instigator, dmg);

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
