using System;
using System.Collections;
using System.Collections.Generic;
using GameDevTV.Utils;
using Sanctuary.Harry.SceneManagement;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Sanctuary.Harry.UI
{
    public class EndGameUI : MonoBehaviour
    {

        void Start()
        {
            Fader fader = FindObjectOfType<Fader>();
            fader.FadeIn(0.5f);
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

        public void ReturnToMainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}

