using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Sanctuary.Harry.Movement;
using Sanctuary.Harry.Core;

namespace Sanctuary.Harry.Combat
{
    public class Fight : MonoBehaviour, IAction
    {
        [SerializeField] float wepRange = 2f, atkSpd = 1f, wepDmg = 5f;

        Health tgt;
        float timeSinceLastAtk = Mathf.Infinity;


        private void Update()
        {
            timeSinceLastAtk += Time.deltaTime;

            if (tgt == null)  return;

            if(tgt.IsDead()) return;

            if (!GetIsInRange()) { GetComponent<Move>().MoveTo(tgt.transform.position); }
            else
            {
                GetComponent<Move>().Cancel();
                AtkBehaviour();
            }
        }

        private void AtkBehaviour()
        {
            transform.LookAt(tgt.transform); //rotate towards enemy after you start attacking

            if(timeSinceLastAtk > atkSpd)
            {
                // This will trigger the Hit Event
                TriggerAtk();
                timeSinceLastAtk = 0;
            }

        }

        private void TriggerAtk()
        {
            GetComponent<Animator>().ResetTrigger("stopAtk");
            GetComponent<Animator>().SetTrigger("attack");
        }

        void Hit() //Animation Event
        {
            if (tgt == null) return;
            tgt.TakeDamage(wepDmg);
        }

        public bool CanAtk(GameObject fightTgt)
        {
            if(fightTgt == null) { return false; }

            Health trgtToTst = fightTgt.GetComponent<Health>();
            return trgtToTst != null && !trgtToTst.IsDead();
        }

        public void Attack(GameObject fightTgt)
        {
            GetComponent<ActionScheduler>().StartAction(this);
            tgt = fightTgt.GetComponent<Health>();
        }


        private bool GetIsInRange()
        {
            return Vector3.Distance(transform.position, tgt.transform.position) < wepRange;
        }

        public void Cancel()
        {
            StopAttack();
            tgt = null;
        }

        private void StopAttack()
        {
            GetComponent<Animator>().ResetTrigger("attack");
            GetComponent<Animator>().SetTrigger("stopAtk");
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.color = Color.green;
            Gizmos.DrawWireSphere(transform.position, wepRange);
        }

    }
}
