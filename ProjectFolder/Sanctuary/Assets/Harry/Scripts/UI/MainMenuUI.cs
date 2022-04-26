using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using Sanctuary.Harry.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

namespace Sanctuary.Harry.UI
{
    public class MainMenuUI : MonoBehaviour
    {
        [SerializeField] TMP_InputField newGameNameField;
        [SerializeField] Button continueButton;
        LazyValue<SavingWrapper> savingWrapper;

        private void Awake()
        {
            savingWrapper = new LazyValue<SavingWrapper>(GetSavingWrapper);
            
        }

        private void Start()
        {
            continueButton.gameObject.SetActive(savingWrapper.value.GetSaveFileBool());
        }

        private void Update()
        {
            // continueButton.gameObject.SetActive(savingWrapper.value.GetSaveFileBool());
        }

        private SavingWrapper GetSavingWrapper()
        {
            return FindObjectOfType<SavingWrapper>();
        }

        public void ContinueGame()
        {
            savingWrapper.value.ContinueGame();
        }

        public void NewGame()
        {
            savingWrapper.value.NewGame(newGameNameField.text);
        }

        public void QuitGame()
        {
            #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
            #else
            Application.Quit();
            #endif
        }

        public void MailingList()
        {
            Application.OpenURL("https://mailchi.mp/db240f6d31e9/sanctuary");
        }

        public void FeedbackForm()
        {
            Application.OpenURL("https://forms.gle/ZzqYc7vN3nLgFZmYA");
        }
    }
}
