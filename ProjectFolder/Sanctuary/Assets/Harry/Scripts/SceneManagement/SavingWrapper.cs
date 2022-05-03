using GameDevTV.Saving;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Sanctuary.Harry.SceneManagement
{
    public class SavingWrapper : MonoBehaviour
    {
        private const string currentSaveKey = "currentSaveName";
        [SerializeField] float fadeInTime = 0.2f;
        [SerializeField] float fadeOutTime = 0.2f;
        [SerializeField] int firstLevelBuildIndex = 1;
        [SerializeField] int menuLevelBuildIndex = 0;

        [SerializeField] float autoSaveInterval = 30f;
        public float timer = 0;


        public void ContinueGame()
        {
            if(!PlayerPrefs.HasKey(currentSaveKey)) return;

            if(!GetSaveFileBool()) return;
            
            StartCoroutine(LoadLastScene());
        }

        public void LoadGame(string saveFile)
        {
            SetCurrentSave(saveFile);
            ContinueGame();
        }

        public void LoadMenu()
        {
            StartCoroutine(LoadMenuScene());
        }

        public bool GetSaveFileBool()
        {
            return GetComponent<SavingSystem>().SaveFileExists(GetCurrentSave());
        }

        public void NewGame(string saveFile)
        {
            if(String.IsNullOrEmpty(saveFile)) return;
            
            SetCurrentSave(saveFile);
            StartCoroutine(LoadFirstScene());
        }

        private string GetCurrentSave()
        {
            return PlayerPrefs.GetString(currentSaveKey);
        }

        private void SetCurrentSave(string saveFile)
        {
            PlayerPrefs.SetString(currentSaveKey, saveFile);
        }

        IEnumerator LoadFirstScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(firstLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadMenuScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            yield return SceneManager.LoadSceneAsync(menuLevelBuildIndex);
            yield return fader.FadeIn(fadeInTime);
        }

        IEnumerator LoadLastScene()
        {
            Fader fader = FindObjectOfType<Fader>();
            yield return fader.FadeOut(fadeOutTime);

            yield return GetComponent<SavingSystem>().LoadLastScene(GetCurrentSave());
            yield return fader.FadeIn(fadeInTime);
        }

        public void Save()
        {
            GetComponent<SavingSystem>().Save(GetCurrentSave());
        }

        public void Load()
        {
            GetComponent<SavingSystem>().Load(GetCurrentSave());
        }

        public void Delete()
        {
            GetComponent<SavingSystem>().Delete(GetCurrentSave());
        }

        public IEnumerable<string> ListSaves()
        {
            return GetComponent<SavingSystem>().ListSaves();
        }

        public void AutoSave()
        {
            InvokeRepeating("Save", 1f, autoSaveInterval);
        }
    }
}
