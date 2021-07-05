using UnityEngine;
using UnityEngine.Events;

namespace TowerDefense.Deck
{
    public class CardPointerEvent : MonoBehaviour
    {
        public UnityEvent<CardProperties> pointerEnterEvent;
        public UnityEvent<CardProperties> pointerExitEvent;

        public bool isSelectable = true;
    }
}
