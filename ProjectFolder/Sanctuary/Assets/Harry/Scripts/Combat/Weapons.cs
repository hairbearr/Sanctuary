using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Sanctuary.Harry.Combat
{
    public class Weapons : MonoBehaviour
    {
        public void OnHit()
        {
            print($"Weapon Hit by: {gameObject.name}");
        }
    }
}
