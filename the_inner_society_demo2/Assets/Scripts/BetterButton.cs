using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

namespace farmingsim
{
    public class BetterButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
    {
        public UnityEvent onLeft;
        public UnityEvent onRight;
        public UnityEvent onMiddle;
        public UnityEvent onEnter;
        public UnityEvent onExit;

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                onLeft.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Right)
            {
                onRight.Invoke();
            }
            else if (eventData.button == PointerEventData.InputButton.Middle)
            {
                onMiddle.Invoke();
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            onEnter.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onExit.Invoke();
        }
    }
}