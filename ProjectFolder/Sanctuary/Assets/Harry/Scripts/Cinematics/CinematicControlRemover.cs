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
        GameObject player;

        private void Start()
        {
            GetComponent<PlayableDirector>().played += DisableControl;
            GetComponent<PlayableDirector>().stopped += EnableControl;
            player = GameObject.FindWithTag("Player");
        }

        void DisableControl(PlayableDirector director)
        {
            player.GetComponent<ActionScheduler>().CancelCurrentAction();
            player.GetComponent<PlayerController>().SetEnable(false);
        }

        void EnableControl(PlayableDirector director)
        {
            player.GetComponent<PlayerController>().SetEnable(true);
        }
    }
}
