using System.Collections;
using System.Collections.Generic;
using Sanctuary.Harry.Control;
using Sanctuary.Harry.SceneManagement;
using UnityEngine;

namespace Sanctuary.Harry.UI
{
    public class PauseMenuUI : MonoBehaviour
    {
        PlayerController playerController;
        [SerializeField] float playSpeed = 1f;

        private void Awake()
        {
            playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
        }

        private void OnEnable()
        {
            if(playerController == null) return;
            Time.timeScale = 0;
            playerController.SetEnable(false);
        }

        private void OnDisable()
        {
            if(playerController == null) return;
            Time.timeScale = playSpeed;
            playerController.SetEnable(true);
        }

        public void Save()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            savingWrapper.Save();
        }

        public void SaveAndQuit()
        {
            SavingWrapper savingWrapper = FindObjectOfType<SavingWrapper>();

            savingWrapper.Save();
            savingWrapper.LoadMenu();
        }

        public void BugReporting()
        {
            Application.OpenURL("https://forms.gle/ZzqYc7vN3nLgFZmYA");
        }
    }
}
