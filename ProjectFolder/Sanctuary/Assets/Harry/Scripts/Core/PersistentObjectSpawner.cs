using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Core
{
    public class PersistentObjectSpawner : MonoBehaviour
    {
        [SerializeField] GameObject persistentObjPrefab;

        static bool hasSpawned = false;

        private void Awake()
        {
            if (hasSpawned) return;

            SpawnPersistentObj();

            hasSpawned = true;
        }

        private void SpawnPersistentObj()
        {
            GameObject persistentObj = Instantiate(persistentObjPrefab);
            DontDestroyOnLoad(persistentObj);
        }
    }
}
