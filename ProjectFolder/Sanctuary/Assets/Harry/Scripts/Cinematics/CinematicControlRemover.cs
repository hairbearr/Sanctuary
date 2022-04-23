using Sanctuary.Harry.Control;
using Sanctuary.Harry.Core;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

namespace Sanctuary.Harry.Cinematics
{
    public class CinematicControlRemover : MonoBehaviour
    {
        GameObject player, inventoryCanvas, hud;
        

        private void Awake()
        {
            hud = GameObject.FindWithTag("HUD");
            inventoryCanvas = GameObject.FindWithTag("InventoryCanvas");
            player = GameObject.FindWithTag("Player");
        }

        private void OnEnable()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
        }

        private void OnDisable()
        {
            GetComponent<PlayableDirector>().played -= DisableControl;
            GetComponent<PlayableDirector>().stopped -= EnableControl;
        }

        void DisableControl(PlayableDirector director)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().SetEnable(false);
            inventoryCanvas.SetActive(false);
            hud.SetActive(false);
        }

        void EnableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().SetEnable(true);
            inventoryCanvas.SetActive(true);
            hud.SetActive(true);
        }
    }
}
