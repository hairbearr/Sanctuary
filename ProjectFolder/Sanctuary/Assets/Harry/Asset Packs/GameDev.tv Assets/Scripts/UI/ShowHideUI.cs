﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameDevTV.UI
{
    public class ShowHideUI : MonoBehaviour
    {
        [SerializeField] KeyCode toggleKey = KeyCode.Escape;
        [SerializeField] GameObject uiContainer = null;

        // Start is called before the first frame update
        void Start()
        {
            uiContainer.SetActive(false);
        }

        // Update is called once per frame
        void Update()
        {
            if (Input.GetKeyDown(toggleKey))
            {
                Toggle();
            }

            // if(Input.GetKeyDown(KeyCode.Escape))
            // {
            //     Close();
            // }
        }

        public void Toggle()
        {
            uiContainer.SetActive(!uiContainer.activeSelf);
        }

        public void Open()
        {
            if(uiContainer.activeSelf == true){ return; }
            uiContainer.SetActive(true);
        }

        public void Close()
        {
            if(uiContainer.activeSelf == false){ return; }
            uiContainer.SetActive(false);
        }

    }
}