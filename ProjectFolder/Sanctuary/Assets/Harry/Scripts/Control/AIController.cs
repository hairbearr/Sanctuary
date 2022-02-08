using Sanctuary.Harry.Combat;
using Sanctuary.Harry.Core;
using Sanctuary.Harry.Movement;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Control
{
    public class AIController : MonoBehaviour
    {
        [SerializeField] float aggroRange = 5f, suspicionTime = 3f, waypointTolerance = 1f, waypointDwellTime = 3f;
        [Range(0,1)] [SerializeField] float patSpeedFract = 0.2f;
        [SerializeField] PatrolPath patrolPath = null;

        Fight fight;
        Health health;
        GameObject player;
        Move move;

        float timeSinceLastSawPlayer = Mathf.Infinity, timeSinceArrivedAtWaypoint = Mathf.Infinity;
        Vector3 startPos;
        int currentWaypointIndex = 0;

        private void Start()
        {
            fight = GetComponent<Fight>();
            health = GetComponent<Health>();
            player = GameObject.FindGameObjectWithTag("Player");
            move = GetComponent<Move>();

            startPos = transform.position;
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
        }

        private bool InAttackRangeofPlayer()
        {
            float distanceToPlayer = Vector3.Distance(player.transform.position, transform.position);
            return distanceToPlayer < aggroRange;
        }

        private void HandleStates()
        {
            if (InAttackRangeofPlayer() && fight.CanAtk(player))
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

        private void PatrolState()
        {
            Vector3 nextPos = startPos;

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
        }

        //Called by Unity
        private void OnDrawGizmosSelected ()
        {
            Gizmos.color = Color.red;
            Gizmos.DrawWireSphere(transform.position, aggroRange);
        }
    }
}
