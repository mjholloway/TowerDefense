using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace TowerDefense.Deck
{
    public interface IPlayable
    {
        bool targets { get; set; }

        void PlayCard();
    }
}
