using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using GameDevTV.UI;
using Sanctuary.Harry.Attributes;
using Sanctuary.Harry.SceneManagement;
using UnityEngine;
using UnityEngine.AI;

namespace Sanctuary.Harry.Control
{
    public class Respawner : MonoBehaviour
    {
        [SerializeField] Transform respawnLocation;
        [SerializeField] float respawnDelay = 3, fadeTime = 0.2f, healthRegenPercentage = 20;
        [SerializeField] ShowHideUI deathMenu;

        // Start is called before the first frame update
        void Awake()
        {
            GetComponent<Health>().onDie.AddListener(Respawn);
            
        }

        private void Start()
        {
            if(GetComponent<Health>().IsDead()){ Respawn(); }
        }

        private void Respawn()
        {
            StartCoroutine(RespawnRoutine());
        }

        private IEnumerator RespawnRoutine()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();
            savingWrapper.Save();
            yield return new WaitForSeconds(respawnDelay);
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            ResetEnemies();
            RespawnPlayer();
            savingWrapper.Save();
            deathMenu.Open();
            yield return fader.FadeIn(fadeTime);
        }

        private void ResetEnemies()
        {
            foreach (AIController enemyControllers in FindObjectsOfType<AIController>())
            {
                Health health = enemyControllers.GetComponent<Health>();
                if(health && !health.IsDead())
                {
                    enemyControllers.Reset();
                    health.Heal(health.GetMaxHealthPoints() - health.GetHealthPoints());
                }
            }
        }

        private void RespawnPlayer()
        {
            
            Vector3 positionDelta = respawnLocation.position - transform.position;
            GetComponent<NavMeshAgent>().Warp(respawnLocation.position);
            Health health = GetComponent<Health>();
            health.Heal(health.GetMaxHealthPoints() * healthRegenPercentage / 100);
            ICinemachineCamera activeVirtualCamera = FindObjectOfType<CinemachineBrain>().ActiveVirtualCamera;
            if(activeVirtualCamera.Follow == transform)
            {
                activeVirtualCamera.OnTargetObjectWarped(transform, positionDelta);
            }
        }
    }
}
