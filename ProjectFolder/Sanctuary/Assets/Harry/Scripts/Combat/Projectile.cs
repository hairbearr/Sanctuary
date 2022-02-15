using Sanctuary.Harry.Core;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Combat
{
    public class Projectile : MonoBehaviour
    {
        [SerializeField] float speed = 1f;

        Health tgt = null;
        float dmg = 0f;

        void Update()
        {
            if (tgt == null) return;

            transform.LookAt(GetAimLocation());
            transform.Translate(Vector3.forward * speed * Time.deltaTime);
        }

        public void SetTarget(Health target, float damage)
        {
            this.tgt = target;
            this.dmg = damage;
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

            tgt.TakeDamage(dmg);
            Destroy(gameObject);
        }
    }
}
