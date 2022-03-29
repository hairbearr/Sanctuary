using Sanctuary.Harry.Combat;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.Core;
using Sanctuary.Harry.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using GameDevTV.Utils;

namespace Sanctuary.Harry.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float aggroRange = 5f, suspicionTime = 3f, waypointTolerance = 1f, waypointDwellTime = 3f, aggroCDTime = 5f, shoutDistance = 5f;
        [Range(0,1)] [SerializeField] float patSpeedFract = 0.2f;
        [SerializeField] PatrolPath patrolPath = null;
        [SerializeField] bool hasShout = false;

        CombatController fight;
        Health health;
        GameObject player;
        MovementController move;

        float timeSinceLastSawPlayer = Mathf.Infinity, timeSinceArrivedAtWaypoint = Mathf.Infinity, timeSinceLastAggroed = Mathf.Infinity;
        LazyValue<Vector3> startPos;
        int currentWaypointIndex = 0;
        
        

        private void Awake()
        {
            fight = GetComponent<CombatController>();
            health = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player");
            move = GetComponent<MovementController>();
            startPos = new LazyValue<Vector3>(GetStartPos);
        }

        private Vector3 GetStartPos()
        {
            return transform.position;
        }

        private void Start()
        {
            startPos.ForceInit();
        }

        private void Update()
        {
            if (health.IsDead()) return;
            HandleStates();
            UpdateTimers();
        }

        private void UpdateTimers()
        {
            timeSinceLastSawPlayer += Time.deltaTime;
            timeSinceArrivedAtWaypoint += Time.deltaTime;
            timeSinceLastAggroed += Time.deltaTime;
        }

        private bool IsAggroed()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            
            return distanceToPlayer < aggroRange || timeSinceLastAggroed<aggroCDTime;
        }

        private void HandleStates()
        {
            if (IsAggroed() && fight.CanAtk(player))
            {
                AttackState();
            }
            else if (timeSinceLastSawPlayer < suspicionTime)
            {
                SuspicionState();
            }
            else
            {
                PatrolState();
            }
        }

        public void Aggro()
        {
            timeSinceLastAggroed = 0;
        }

        private void PatrolState()
        {
            Vector3 nextPos = startPos.value;

            if(patrolPath != null)
            {
                if (AtWaypoint())
                {
                    timeSinceArrivedAtWaypoint = 0;
                    CycleWaypoint();
                }
                nextPos = GetCurrentWaypoint();
            }

            if(timeSinceArrivedAtWaypoint > waypointDwellTime) { move.StartMoveAction(nextPos, patSpeedFract); }
        }

        private Vector3 GetCurrentWaypoint()
        {
            return patrolPath.GetWaypoint(currentWaypointIndex);
        }

        private void CycleWaypoint()
        {
            currentWaypointIndex = patrolPath.GetNextIndex(currentWaypointIndex);
        }

        private bool AtWaypoint()
        {
            float distanceToWaypoint = Vector3.Distance(transform.position, GetCurrentWaypoint());
            return distanceToWaypoint < waypointTolerance;
        }

        private void SuspicionState()
        {
            GetComponent<ActionScheduler>().CancelCurrentAction();
        }

        private void AttackState()
        {
            timeSinceLastSawPlayer = 0;
            fight.Attack(player);

            AggroNearbyEnemies();
            hasShout = false;
        }

        private void AggroNearbyEnemies()
        {
            RaycastHit[] hits = Physics.SphereCastAll(transform.position, shoutDistance, Vector3.up, 0);
            foreach (RaycastHit hit in hits)
            {
                AIController ai = hit.collider.GetComponent<AIController>();
                if (ai == null) continue;
                if (ai == this) continue;
                if(!hasShout) { ai.Aggro(); }
            }
        }

        //Called by Unity
        private void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }

    }
}
