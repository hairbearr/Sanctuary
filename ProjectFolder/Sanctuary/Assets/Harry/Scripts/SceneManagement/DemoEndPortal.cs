using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanctuary.Harry.SceneManagement
{
    public class DemoEndPortal : MonoBehaviour
    {
        [SerializeField] float fadeTime = 0.3f;

        private IEnumerator EndGameRoutine()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeTime);
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);
        }

        private void OnTriggerEnter(Collider other)
        {
            if(other.tag == "Player") { StartCoroutine(EndGameRoutine()); }
        }
    }
}
