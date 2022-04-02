using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace Sanctuary.Harry.UI
{
    public class UIClickHandler : MonoBehaviour, IPointerClickHandler
    {
        public UnityEvent onLeftClick;
        public UnityEvent onRightClick;
        public UnityEvent onMiddleClick;

        public void OnPointerClick(PointerEventData eventData)
        {
            if(eventData.pointerId == -1) { onLeftClick.Invoke(); }
            if(eventData.pointerId == -2) { onRightClick.Invoke(); }
            if(eventData.pointerId == -3) { onMiddleClick.Invoke(); }
        }
    }
}
